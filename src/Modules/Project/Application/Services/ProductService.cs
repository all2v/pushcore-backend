using Astrum.Projects.Aggregates;
using Astrum.Projects.Repositories;
using Astrum.Projects.Specifications.Customer;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace Astrum.Projects.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IMemberRepository _memberRepository;
    private readonly IProjectService projectService;

    public ProductService(IProductRepository productRepository, IMapper mapper
        , IFileStorage fileStorage, IMemberRepository memberRepository, IProjectService projectService)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _memberRepository = memberRepository;
        this.projectService = projectService;
    }

    #region IProductService Members

    public async Task<Result<ProductView>> Create(ProductRequest product)
    {
        var validationResult = Validate(product.Projects, product.StartDate.Date, product.EndDate?.Date, product.Name);
        if (validationResult.Failed)
            return Result.Error(validationResult.MessageWithErrors);

        var newProduct = _mapper.Map<Product>(product);
        var res = await _fileStorage.UploadFile(product.CoverImage);
        if (res != null)
        {
            newProduct.CoverImageId = (Guid) res.UploadedFileId;
        }
        await _productRepository.AddAsync(newProduct);
        try
        {
            await _productRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании продукта.");
        }
        return await Get(newProduct.Id);
    }

    public async Task<Result<List<ProductView>>> GetAll()
    {
        var spec = new GetProductsSpec();
        var products = await _productRepository.ListAsync(spec);
        var result = _mapper.Map<List<ProductView>>(products);
        return Result.Success(result);
    }

    public async Task<Result<ProductPaginationView>> GetAllPagination(int count, int startIndex, string? predicate)
    {
        if (startIndex < 0) startIndex = 0;
        if (count < 0) count = 5;
        var spec = string.IsNullOrEmpty(predicate)? new GetProductsPaginateSpec(count, startIndex)
            : new GetProductsPaginateSpec(count, startIndex, predicate);
        var products = await _productRepository.ListAsync(spec);
        if (products.Count == 0)
            return Result.Success(new ProductPaginationView
            {
                Products = new(),
                Index = 0,
                NextExist = false
            });
        var lastIndex = products[products.Count - 1].Index;
        var nextExistspec = new CheckNextSpec(lastIndex);
        var nextExist = await _productRepository.AnyAsync(nextExistspec);
        var productsView = new List<ProductView>();
        foreach (var product in products)
        {
            var productView = _mapper.Map<ProductView>(product);
            if (product.CoverImageId != null)
                productView.CoverUrl = await _fileStorage.GetFileUrl(product.CoverImageId);
            productsView.Add(productView);
        }
        var result = new ProductPaginationView
        {
            Products = productsView,
            Index = lastIndex,
            NextExist = nextExist
        };

        return Result.Success(result);
    }

    public async Task<Result<ProductView>> Get(Guid id)
    {
        var spec = new GetProductByIdSpec(id);

        var product = await _productRepository.FirstOrDefaultAsync(spec);
        if (product is null)
            return Result.NotFound("Проект не найден.");
        var result = _mapper.Map<ProductView>(product);
        if (product.CoverImageId != null)
            result.CoverUrl = await _fileStorage.GetFileUrl(product.CoverImageId);
        return Result.Success(result);
    }

    public async Task<Result<ProductView>> Delete(Guid id)
    {
        var spec = new GetProductByIdSpec(id);
        var product = await _productRepository.FirstOrDefaultAsync(spec);
        await _productRepository.DeleteAsync(product);
        await _productRepository.UnitOfWork.SaveChangesAsync();
        return Result.Success(_mapper.Map<ProductView>(product));
    }

    public async Task<Result<ProductView>> Update(Guid id, ProductUpdateDto productDto)
    {
        var validationResult = Validate(productDto.Projects, productDto.StartDate?.Date, productDto.EndDate?.Date, productDto.Name);
        if (validationResult.Failed)
            return Result.Error(validationResult.MessageWithErrors);

        var spec = new GetProductByIdSpec(id);
        var product = await _productRepository.FirstOrDefaultAsync(spec);
        if (product is null)
            return Result.Error("Project was not found");
        var baseProjects = product.Projects.ToList();
        _mapper.Map(productDto, product);
        var newProjects = product.Projects;
        foreach (var baseProject in baseProjects)
        {
            var prj = newProjects.FirstOrDefault(p => p.Id == baseProject.Id);
            if (prj == null)
                continue;
            newProjects.Remove(prj);
            _mapper.Map(prj, baseProject);
            newProjects.Add(baseProject);
        }
        if (productDto.CoverImage != null)
        {
            var res = await _fileStorage.UploadFile(productDto.CoverImage);
            if (!res.Success)
                return Result.Error("Ошибка во время сохранения изображения");
            if (res != null)
            {
                product.CoverImageId = (Guid) res.UploadedFileId;
            }
        }
        await _productRepository.UnitOfWork.SaveChangesAsync();
        var result = _mapper.Map<ProductView>(product);
        result.CoverUrl = await _fileStorage.GetFileUrl(product.CoverImageId);
        return Result.Success(result);
    }

    #endregion

    private static Result Validate(List<ProjectRequest> projects, DateTime? startDate, DateTime? endDate, string name)
    {
        if (!projects.Any())
        {
            return Result.Error("Отсутствуют проекты в продукте");
        }

        if (endDate != null && startDate != null && startDate > endDate)
        {
            return Result.Error("Дата начала не может быть позже даты окончания");
        }

        if (startDate != null && startDate?.Year < 2009)
        {
            return Result.Error("Некорректная дата начала");
        }

        if (name.Length > 30)
            return Result.Error("Слишком длинное название продукта");

        return Result.Success();
    }
}
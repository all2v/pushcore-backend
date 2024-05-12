using System.Text.RegularExpressions;
using Astrum.Projects.Aggregates;
using Astrum.Projects.Repositories;
using Astrum.Projects.Specifications.Customer;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Projects.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    #region ICustomerService Members

    public async Task<Result<CustomerView>> Create(CustomerRequest customer)
    {
        var validationResult = await Validate(customer);
        if (validationResult.Failed)
            return Result.Error(validationResult.MessageWithErrors);

        var newCustomer = _mapper.Map<Customer>(customer);
        await _customerRepository.AddAsync(newCustomer);
        try
        {
            await _customerRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании покупателя.");
        }
        return Result.Success(_mapper.Map<CustomerView>(newCustomer));
    }

    public async Task<Result<List<CustomerView>>> GetAll()
    {
        var result = await _customerRepository.ListAsync();
        return Result.Success(_mapper.Map<List<CustomerView>>(result));
    }

    public async Task<Result<CustomerView>> Update(CustomerView request)
    {
        var spec = new GetCustomerByIdSpec(request.Id);

        var customer = await _customerRepository.FirstOrDefaultAsync(spec);
        if (customer == null)
            return Result.NotFound("Покупатель не найден.");

        customer.Name = request.Name;
        await _customerRepository.UpdateAsync(customer);
        try
        {
            await _customerRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании покупателя.");
        }
        return Result.Success(_mapper.Map<CustomerView>(customer));
    }

    public async Task<Result<CustomerView>> Delete(Guid id)
    {
        var spec = new GetCustomerByIdSpec(id);
        var customer = await _customerRepository.FirstOrDefaultAsync(spec);
        if (customer == null)
            return Result.NotFound("Покупатель не найден.");
        try
        {
            await _customerRepository.DeleteAsync(customer);
            await _customerRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении покупателя.");
        }
        return Result.Success(_mapper.Map<CustomerView>(customer));
    }

    public async Task<SharedLib.Common.Results.Result<CustomerView>> Get(Guid id)
    {
        var spec = new GetCustomerByIdSpec(id);

        var result = await _customerRepository.FirstOrDefaultAsync(spec);
        if (result == null)
            return Result.NotFound("Покупатель не найден.");

        var customerView = _mapper.Map<CustomerView>(result);
        return Result.Success(customerView);
    }

    public async Task<bool> Exist(string name)
    {
        var spec = new GetCustomerByNameSpec(name);
        var result = await _customerRepository.FirstOrDefaultAsync(spec);
        return result != null;
    }

    #endregion

    private async Task<Result> Validate(CustomerRequest customer)
    {
        var name = customer.Name.Trim();
        var spec = new GetCustomerByNameSpec(customer.Name);
        var customerExist = await _customerRepository.AnyAsync(spec);
        if (customerExist)
            return Result.Error("Заказчик с таким именем уже существует");

        name = Regex.Replace(name, @"\s+", " ");
        if(string.IsNullOrEmpty(name) || name.Length > 50)
            return Result.Error("Недопустимая длина имени");

        return Result.Success();
    }
}
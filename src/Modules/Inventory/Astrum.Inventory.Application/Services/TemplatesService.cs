using System.Reflection;
using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.Inventory.Domain.Aggregates;
using Astrum.Inventory.Domain.Specifications;
using Astrum.Inventory.DomainServices.Repositories;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.Extensions;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using FilterItem = Astrum.SharedLib.Application.Models.Filters.FilterItem;

namespace Astrum.Inventory.Application.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly ITemplatesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;

        public TemplatesService(ITemplatesRepository repository, IMapper mapper, IFileStorage fileStorage)
        {
            _repository = repository;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        public async Task<Result<List<TemplateView>>> GetTemplates(CancellationToken cancellationToken = default)
        {
            var spec = new GetTemplatesSpec();
            var templates = await _repository.ListAsync(spec, cancellationToken);
            var templatesView = _mapper.Map<List<TemplateView>>(templates);
            foreach (var template in templatesView.Where(item => item.PictureId != null))
            {
                template.LinkImage = await _fileStorage.GetFileUrl(template.PictureId.Value);
            }

            return Result.Success(templatesView);
        }

        public async Task<Result<TemplateView>> GetTemplateById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetTemplateById(id);
            var template = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (template == null)
            {
                return Result.NotFound("Категория не найдена.");
            }
            var templateView = _mapper.Map<TemplateView>(template);
            if (templateView.PictureId.HasValue)
            {
                templateView.LinkImage = await _fileStorage.GetFileUrl(templateView.PictureId.Value);
            }
            return Result.Success(templateView);
        }

        public async Task<Result<TemplateView>> CreateTemplate(TemplateCreateRequest templateCreate, CancellationToken cancellationToken = default)
        {
            var template = _mapper.Map<Template>(templateCreate);

            if (templateCreate.Image != null)
            {
                var res = await _fileStorage.UploadFile(templateCreate.Image, cancellationToken);
                if (res != null && res.Success)
                {
                    template.PictureId = res.UploadedFileId;
                }
            }
            template = await _repository.AddAsync(template, cancellationToken);
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании категории.");
            }

            var templateView = _mapper.Map<TemplateView>(template);
            if (template.PictureId.HasValue)
            {
                templateView.LinkImage = await _fileStorage.GetFileUrl(template.PictureId.Value);
            }
            return Result.Success(templateView);
        }
        public async Task<Result<TemplateView>> UpdateTemplate(Guid id,TemplateUpdateRequest templateUpdate, CancellationToken cancellationToken = default)
        {
            var template = _mapper.Map<Template>(templateUpdate);
            var spec = new GetTemplateById(id);
            var dbTemplate = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (dbTemplate == null)
            {
                return Result.NotFound("Категория не найдена.");
            }

            _mapper.Map(templateUpdate, dbTemplate);
            if (templateUpdate.Image != null)
            {
                var res = await _fileStorage.UploadFile(templateUpdate.Image);
                if (res != null && res.Success)
                {
                    dbTemplate.PictureId = res.UploadedFileId;
                }
            }
            try
            {
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении категории.");
            }

            var templateView = _mapper.Map<TemplateView>(dbTemplate);
            if (dbTemplate.PictureId.HasValue)
            {
                templateView.LinkImage = await _fileStorage.GetFileUrl(dbTemplate.PictureId.Value);
            }
            return Result.Success(templateView);
        }
        public async Task<Result<TemplateView>> DeleteTemplate(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetTemplateById(id);
            var template = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            if (template == null)
            {
                return Result.NotFound("Категория не найдена.");
            }
            try
            {
                await _repository.DeleteAsync(template, cancellationToken);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении категории");
            }

            var templateView = _mapper.Map<TemplateView>(template);
            return Result.Success(templateView);
        }

        public async Task<Result<FilterInfo>> GetFilterInfo(CancellationToken cancellationToken = default)
        {
            var spec = new GetTemplatesSpec();
            var templates = await _repository.ListAsync(spec, cancellationToken);
            var templatesView = _mapper.Map<List<TemplateView>>(templates);
            var statuses = Enum.GetValues<Status>().ToList();
            return Result.Success(new FilterInfo
            {
                Templates = templatesView,
                Statuses = statuses
            });
        }

        public async Task<Result<FilterResponse>> GetFilters(CancellationToken cancellationToken = default)
        {
            var spec = new GetTemplatesSpec();
            var templates = (await _repository.ListAsync(spec, cancellationToken))
            .Select(t => new FilterItem
            {
                Title = t.Title,
                Value = t.Title,
                Count = null,
            }).ToList();
            var templatesBlock = new FilterBlock()
            {
                Name = "templates",
                Title = "По типу",
                FilterItems = templates
            };
            var statuses = Enum.GetValues<Status>().Select(t => new FilterItem
            {
                Title = t.GetDisplayName(),
                Value = ((int)t).ToString(),
                Count = null,
            }).ToList();
            var statuserBlock = new FilterBlock()
            {
                Name = "statuses",
                Title = "По статусу",
                FilterItems = statuses
            };

            return Result.Success(new FilterResponse 
            {
                Blocks = new List<FilterBlock>
                {
                    templatesBlock,
                    statuserBlock
                }
            });
        }

        public async Task<bool> TemplateAlreadyExists(string title, CancellationToken cancellationToken = default)
        {
            var templates = await _repository.ListAsync(cancellationToken);
            return templates.Any(template => template.Title.ToLower().Trim() == title.ToLower().Trim());
        }

        public async Task<bool> TemplateAlreadyExists(string title, Guid templateId, CancellationToken cancellationToken = default)
        {
            var templates = await _repository.ListAsync(cancellationToken);
            var spec = new GetTemplateById(templateId);
            var templateDb = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
            return templates.Any(template => template.Title.ToLower().Trim() == title.ToLower().Trim() && templateDb.Title.ToLower().Trim() != title.ToLower().Trim());
        }
    }
}

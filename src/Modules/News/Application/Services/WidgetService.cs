using Astrum.News.Aggregates;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;
using AutoMapper;

namespace Astrum.News.Services
{
    public class WidgetService : IWidgetService
    {
        private readonly IWidgetRepository _widgetRepository;
        private readonly IFileStorage _fileStorage;
        private readonly IMapper _mapper;

        public WidgetService(IWidgetRepository widgetRepository, IMapper mapper, IFileStorage fileStorage)
        {
            _widgetRepository = widgetRepository;
            _mapper = mapper;
            _fileStorage = fileStorage;
        }

        #region IBannersService Members

        public async Task<SharedLib.Common.Results.Result<List<WidgetForm>>> GetWidgets(CancellationToken cancellationToken = default)
        {
            var widgets = await _widgetRepository.ListAsync(cancellationToken);

            var response = _mapper.Map<List<WidgetForm>>(widgets);
            foreach (var widget in response)
            {
                if (widget.PictureId.HasValue)
                    widget.PictureS3Link = await _fileStorage.GetFileUrl(widget.PictureId.Value);
            }

            return Result.Success(response);
        }

        public async Task<SharedLib.Common.Results.Result<List<WidgetForm>>> GetActiveWidgets(CancellationToken cancellationToken = default)
        {
            var spec = new GetActiveWidgetsSpec();
            var widgets = await _widgetRepository.ListAsync(spec);

            var response = _mapper.Map<List<WidgetForm>>(widgets);
            foreach (var widget in response)
            {
                if (widget.PictureId.HasValue)
                    widget.PictureS3Link = await _fileStorage.GetFileUrl(widget.PictureId.Value);
            }

            return Result.Success(response);
        }

        public async Task<SharedLib.Common.Results.Result<WidgetForm>> GetWidgetById(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetWidgetByIdSpec(id);
            var widget = await _widgetRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (widget == null)
            {
                return Result.NotFound("Виджет не найден.");
            }
            return Result.Success(_mapper.Map<WidgetForm>(widget));
        }

        public async Task<SharedLib.Common.Results.Result<WidgetForm>> UpdateWidget(Guid id, WidgetForm widgetForm,
            CancellationToken cancellationToken = default)
        {
            var widget = _mapper.Map<Widget>(widgetForm);
            var spec = new GetWidgetByIdSpec(id);
            var dbWidget = await _widgetRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (dbWidget == null)
            {
                return Result.NotFound("Виджет не найден.");
            }
            dbWidget.Title = widget.Title;
            //dbWidget.PictureS3Link = widget.PictureS3Link;
            dbWidget.IsActive = widget.IsActive;
            //dbBanner.Version++;
            try
            {
                await _widgetRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении виджета.");
            }
            return Result.Success(_mapper.Map<WidgetForm>(dbWidget));
        }

        public async Task<SharedLib.Common.Results.Result<WidgetForm>> CreateWidget(WidgetForm widgetForm, FileForm image, CancellationToken cancellationToken = default)
        {
            var widget = _mapper.Map<Widget>(widgetForm);

            if (image != null)
            {
                var uploadResult = await _fileStorage.UploadFile(image);
                widget.PictureId = uploadResult.UploadedFileId;
            }

            widget = await _widgetRepository.AddAsync(widget, cancellationToken);
            try
            {
                await _widgetRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании виджета.");
            }

            var response = _mapper.Map<WidgetForm>(widget);
            if (widget.PictureId.HasValue)
                response.PictureS3Link = await _fileStorage.GetFileUrl(widget.PictureId.Value);
            return Result.Success(response);
        }

        public async Task<SharedLib.Common.Results.Result<WidgetForm>> DeleteWidget(Guid id, CancellationToken cancellationToken = default)
        {
            var spec = new GetWidgetByIdSpec(id);
            var widget = await _widgetRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (widget == null)
            {
                return Result.NotFound("Виджет не найден.");
            }
            try
            {
                await _widgetRepository.DeleteAsync(widget, cancellationToken);
                await _widgetRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении виджета");
            }
            return Result.Success(_mapper.Map<WidgetForm>(widget));
        }

        #endregion
    }
}

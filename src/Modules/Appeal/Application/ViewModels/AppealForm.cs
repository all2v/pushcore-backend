using Astrum.Appeal.Enums;
using Astrum.Storage.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Appeal.ViewModels;

public class AppealForm
{
    [FromRoute]
    public Guid Id { get; set; }

    [FromBody]
    public AppealFormData Body { get; set; }
}

public class AppealFormResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Request { get; set; }
    public List<AppealCategoryForm> Categories { get; set; }
    public Guid From { get; set; }
    public string? FromName { get; set; }
    public Guid To { get; set; }
    public string? ToName { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public AppealStatus? AppealStatus { get; set; }
    public string? Answer { get; set; }
    public DateTime? Closed { get; set; }
    public string? CoverUrl { get; set; }
    public Guid? CoverImageId { get; set; }
}

public class AppealFormData
{
    public string Title { get; set; }
    public string Request { get; set; }
    public List<AppealCategoryForm> Categories { get; set; }
    public Guid From { get; set; }
    public string? FromName { get; set; }
    public Guid To { get; set; }
    public string? ToName { get; set; }
    public DateTimeOffset? DateCreated { get; set; }
    public AppealStatus? AppealStatus { get; set; }
    public string? Answer { get; set; }
    public DateTime? Closed { get; set; }
    public MinimizedFileForm? Image { get; set; }
}
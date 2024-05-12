using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Astrum.Api.Validators.LocalizedDataAnnotationsValidator;

public class LocalizedDataAnnotationsValidator : ComponentBase
{
    [Inject]
    private ILocalizationService Localizer { get; set; }

    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        CurrentEditContext.AddLocalizedDataAnnotationsValidation(Localizer);
    }
}
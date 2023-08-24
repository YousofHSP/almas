using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages.Insurer;

public class InsurersBase: ComponentBase
{
    [Inject] private IInsurerService Service { get; set; } = null!;
    public IEnumerable<InsurerResDto>? Insurers { get; set; }


    protected override async Task OnInitializedAsync()
    {
        Insurers = await Service.Get();
    }
}
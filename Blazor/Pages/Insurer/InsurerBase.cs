using Blazor.Services.Contracts;
using DTO;
using Microsoft.AspNetCore.Components;

namespace Blazor.Pages.Insurer;

public class InsurerBase: ComponentBase
{
    [Inject] private IInsurerService Service { get; set; } = null!;
    [Parameter] public required int Id { get; set; }
    public InsurerResDto? Insurer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Insurer = await Service.Get(Id);
    }
}
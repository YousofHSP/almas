using Microsoft.AspNetCore.Components;

namespace Blazor.Pages;

public class ShopBase : ComponentBase
{
    [Parameter] public int Id { get; set; }
}
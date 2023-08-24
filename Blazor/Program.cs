using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor;
using Blazor.Services;
using Blazor.Services.Admin;
using Blazor.Services.Admin.Contracts;
using Blazor.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44317/api/v1/") });

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IInsurerService, InsurerService>();

// admin services
builder.Services.AddScoped<IAdminShopService, AdminShopService>();

await builder.Build().RunAsync();
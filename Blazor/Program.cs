using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor;
using Blazor.Services;
using Blazor.Services.Admin;
using Blazor.Services.Admin.Contracts;
using Blazor.Services.Contracts;
using CurrieTechnologies.Razor.SweetAlert2;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44317/api/v1/") });
builder.Services.AddSweetAlert2();

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IInsurerService, InsurerService>();

// admin services
builder.Services.AddScoped<IAdminShopService, AdminShopService>();
builder.Services.AddScoped<IAdminCourseService, AdminCourseService>();
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminLessonService, AdminLessonService>();
builder.Services.AddScoped<IAdminInsurerService, AdminInsurerService>();
builder.Services.AddScoped<IAdminPackageService, AdminPackageService>();

await builder.Build().RunAsync();
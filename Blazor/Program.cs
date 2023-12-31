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

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://almasapi.chbk.run/api/v1/") });
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44317/api/v1/") });
builder.Services.AddSweetAlert2();

builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddScoped<IInsurerService, InsurerService>();
builder.Services.AddScoped<ISlideService, SlideService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<ICourseService, CourseService>();

// admin services
builder.Services.AddScoped(typeof(IAdminService<,,>), typeof(AdminService<,,>));
builder.Services.AddScoped<IAdminUserService, AdminUserService>();
builder.Services.AddScoped<IAdminLessonService, AdminLessonService>();

await builder.Build().RunAsync();
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DependencyResolvers.AutoFac;
using Core.DependencyResolver;
using Core.Entities;
using Core.Extensions;
using Core.Utilities.IoC;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacBusinessModule());
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IDto, FiyatBilgisiDto>();

// IMusteriService gönderilirse ne verileceðini ve IMusteriDal gönderilirse ne verileceðini belirtiyoruz.
builder.Services.AddSingleton<IMusteriService, MusteriManager>();
builder.Services.AddSingleton<IMusteriDal, EfMusteriDal>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
{
    x.LoginPath = "/Login/Login";
    //x.ExpireTimeSpan = TimeSpan.FromSeconds(10);
    x.SlidingExpiration = true;

    //x.AccessDeniedPath = "/Forbidden/";
});

builder.Services.AddDependencyResolvers(new ICoreModule[] {
    new CoreModule()
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=home}/{action=index}/{id?}");


});
    app.Run();

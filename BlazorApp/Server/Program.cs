using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlazorApp.Server.Global;
using BlazorApp.Server.Hubs;
using BlazorApp.Server.Jobs;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//禁止不可为空的引用类型和必须属性
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddJobServiceStep();
builder.Services.AddControllers(o =>
{
    o.Filters.Add(typeof(GlobalExceptionsFilter));
    o.Filters.Add(typeof(GlobalResponseFilter));
    o.Filters.Add(typeof(GlobalModelStateValidationFilter));
});
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//覆盖用于创建服务提供者的工厂
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>//依赖注入
{
    var basePath = AppDomain.CurrentDomain.BaseDirectory;
    var servicesDllFile = Path.Combine(basePath, "Application.dll");//需要依赖注入的项目生成的dll文件名称
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    containerBuilder.RegisterAssemblyTypes(assemblysServices)
        .Where(x => x.FullName != null && x.FullName.EndsWith("Service"))//对比名称最后是否相同然后注入
              .AsImplementedInterfaces()
              .InstancePerDependency();
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<FileHub>("/fileHub");
});

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

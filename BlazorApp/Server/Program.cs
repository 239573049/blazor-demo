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

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);//��ֹ����Ϊ�յ��������ͺͱ�������
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
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//�������ڴ��������ṩ�ߵĹ���
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>//����ע��
{
    var basePath = AppDomain.CurrentDomain.BaseDirectory;
    var servicesDllFile = Path.Combine(basePath, "Application.dll");//��Ҫ����ע�����Ŀ���ɵ�dll�ļ�����
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    containerBuilder.RegisterAssemblyTypes(assemblysServices)
        .Where(x => x.FullName != null && x.FullName.EndsWith("Service"))//�Ա���������Ƿ���ͬȻ��ע��
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

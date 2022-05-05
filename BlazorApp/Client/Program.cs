using AntDesign;
using BlazorApp.Client;
using Blazored.SessionStorage;
using BlazorHelper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
builder.Services.AddBlazoredSessionStorage(c =>
{
    c.JsonSerializerOptions.WriteIndented = true;

});
var message = new MessageService();
HubConnection? hubConnecton;

hubConnecton = new HubConnectionBuilder()
    .WithUrl(new Uri(builder.HostEnvironment.BaseAddress + "fileHub"), a =>
    {
        a.Headers.Add("Id",Guid.NewGuid().ToString("N"));
    }).Build();
builder.Services.AddSingleton(sp=> message);
builder.Services.AddSingleton(hubConnecton);
var http = HttpClientFactory.Create();
http.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
var httpHelper=new HttpHelper(http);
httpHelper.AddRequestHandlin(a =>
{
    Console.WriteLine("发送处理时间："+ DateTime.Now.ToFileTimeUtc().ToString());
});
httpHelper.AddResponseBodyHandling(a =>
{
    var now = DateTime.Now;
    Console.WriteLine("接收处理时间：" + DateTime.Now.ToFileTimeUtc().ToString());
    
});
builder.Services.AddHttpClientHelperExtensions(httpHelper);

//httpClient.AddRequestBodyHandling(async response =>
//{
//    if (response.StatusCode == System.Net.HttpStatusCode.OK)
//    {
//        var data = JsonConvert.DeserializeObject<ModelStateResult>(await response.Content.ReadAsStringAsync());
//        if (data.StatusCode != 200)
//        {
//            await message.Error(data.Message);
//        }
//    }
//});
var basePath = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddInjectSingletonExtensions(Path.Combine(basePath, "BlazorApp.Client.dll"), a => a.Name.EndsWith("Api"));
await builder.Build().RunAsync();

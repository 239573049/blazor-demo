using AntDesign;
using BlazorApp.Client;
using BlazorHelper;
using Entitys.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Diagnostics;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
var message = new MessageService();
HubConnection? hubConnecton;

hubConnecton = new HubConnectionBuilder()
    .WithUrl(new Uri(builder.HostEnvironment.BaseAddress + "fileHub"), a =>
    {
        a.Headers.Add("Id",Guid.NewGuid().ToString("N"));
    }).Build();
builder.Services.AddSingleton(sp=> message);
builder.Services.AddSingleton(hubConnecton);
var httpClient =new HttpHelper(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClientHelperExtensions(httpClient);
httpClient.AddRequestBodyHandling(async response =>
{
    if (response.StatusCode == System.Net.HttpStatusCode.OK)
    {
        var data = JsonConvert.DeserializeObject<ModelStateResult>(await response.Content.ReadAsStringAsync());
        if (data.StatusCode != 200)
        {
            await message.Error(data.Message);
        }
    }
});
var basePath = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddInjectSingletonExtensions(Path.Combine(basePath, "BlazorApp.Client.dll"), a => a.Name.EndsWith("Api"));
await builder.Build().RunAsync();

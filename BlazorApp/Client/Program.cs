using AntDesign;
using BlazorApp.Client;
using BlazorHelper;
using Entitys.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
var message = new MessageService();
HubConnection? hubConnecton;

hubConnecton = new HubConnectionBuilder()
    .WithUrl(new Uri(builder.HostEnvironment.BaseAddress + "fileHub")).Build();
builder.Services.AddSingleton(sp=> message);
builder.Services.AddSingleton(hubConnecton);
builder.Services.AddHttpClientHelper(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }, async a =>
{
    if (a.StatusCode == HttpStatusCode.OK)
    {
        var content = JsonConvert.DeserializeObject<ModelStateResult<object>>(await a.Content.ReadAsStringAsync());
        if (content.StatusCode != 200)
        {
            await message.Error(content.Message);
        }
    }
});
var basePath = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddInjectSingleton(Path.Combine(basePath, "BlazorApp.Client.dll"), a => a.Name.EndsWith("Api"));
await builder.Build().RunAsync();

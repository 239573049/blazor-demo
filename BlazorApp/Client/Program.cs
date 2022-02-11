using AntDesign;
using BlazorApp.Client;
using BlazorHelper;
using Entitys.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Newtonsoft.Json;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
var message = new MessageService();
builder.Services.AddSingleton(sp=> message);
builder.Services.AddHttpClientHelper(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }, async a =>
{
    if (a.StatusCode == HttpStatusCode.OK)
    {
        var content = JsonConvert.DeserializeObject<ModelStateResult<object>>(await a.Content.ReadAsStringAsync());
        if (content.StatusCode != 200)
        {
            message.Error(content.Message);
        }
    }
});
var basePath = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddInjectSingleton(Path.Combine(basePath, "BlazorApp.Client.dll"), a => a.Name.EndsWith("Api"));
await builder.Build().RunAsync();

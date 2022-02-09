using BlazorApp.Client;
using BlazorApp.Client.Api;
using BlazorHelper;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
builder.Services.AddHttpClientHelper(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
var basePath = AppDomain.CurrentDomain.BaseDirectory;
builder.Services.AddInjectSingleton(Path.Combine(basePath, "BlazorApp.Client.dll"), a => a.Name.EndsWith("Api"));
Request.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
await builder.Build().RunAsync();

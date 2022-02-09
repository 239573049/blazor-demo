using Autofac;
using BlazorApp.Client;
using BlazorApp.Client.Api;
using BlazorHttpUtil;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddAntDesign();
var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
builder.Services.AddSingleton(sp => httpClient);
builder.Services.AddSingleton<IHttpHelp,HttpHelp>();
builder.Services.AddSingleton(typeof(FileAdminApi));
Request.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
await builder.Build().RunAsync();

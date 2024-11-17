using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PerfomanceProject.Client;
using PerfomanceProject.Client.Services;
using PerformanceProject.Client.Services.Generated;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddScoped(sp =>
    new Client("https://localhost:44365", new HttpClient { BaseAddress = new Uri("https://localhost:44365") }));

builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();

await builder.Build().RunAsync();

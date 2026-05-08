using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorUtils.EasyApi;
using BlazorUtils.EasyApi.Client;
using InvoiceSoftware.Client.Services;
using InvoiceSoftware.Shared.Api.Clients;
using InvoiceSoftware.Shared.Services.Modal;

namespace InvoiceSoftware.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Register HttpClient for API calls
        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
        });

        // Register EasyApi client with contracts from shared assembly
        builder.Services.AddEasyApi()
            .WithContract(typeof(GetClients).Assembly)
            .WithClient();

        // Register Modal Service
        builder.Services.AddScoped<IModalService, ModalService>();

        // Register Layout Service for JS interop
        builder.Services.AddScoped<ILayoutService, LayoutService>();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddAuthenticationStateDeserialization();

        await builder.Build().RunAsync();
    }
}

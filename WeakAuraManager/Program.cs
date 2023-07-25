using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace WeakAuraManager 
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IWeakAuraManagerRunner, WeakAuraManagerRunner>();
                    services.AddSingleton<IFileGenerator, FileGenerator>();
                    services.AddSingleton<IParser, Parser>();
                    services.AddSingleton<IFileBackerUpper, FileBackerUpper>();
                    services.AddSingleton<IGroupBuilder, GroupBuilder>();
                })
                .Build();

            await host.Services.GetRequiredService<IWeakAuraManagerRunner>().Run();
        }
    }
}
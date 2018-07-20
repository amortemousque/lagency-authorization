﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.AspNetCore;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "IdentityServer4";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"identityserver4_log.txt")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("hosting.json", optional: true)
             .AddCommandLine(args)
             .Build();
            
            return WebHost.CreateDefaultBuilder(args)
                    .UseConfiguration(config)
                    .UseStartup<Startup>()
                    .ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddSerilog();
                    })
                    .Build();
        }            
    }
}
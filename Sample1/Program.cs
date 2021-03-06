﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using Chromely.CefGlue;
using Chromely.Core;
using Chromely.Core.Host;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;

namespace Sample1
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Console.WriteLine("Sample showing stonehenge on Chromely");
            Console.WriteLine();

            Console.WriteLine($"Running on {RuntimeEnvironment.GetRuntimeDirectory()}, CLR {RuntimeEnvironment.GetSystemVersion()}");
            Console.WriteLine();

            // ensure CEF runtime files are present
            Console.WriteLine("Check CEF framework is installed in the correct version");
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(path);

            // Starting stonehenge backend
            Console.WriteLine("Starting stonehenge backend");
            var provider = StonehengeResourceLoader
                .CreateDefaultLoader(new VueResourceProvider());
            var options = new StonehengeHostOptions
            {
                Title = "Sample 1",
                StartPage = "information",
                ServerPushMode = ServerPushModes.LongPolling,
                PollIntervalMs = 1000
            };
            var host = new KestrelHost(provider, options);
            if (!host.Start("localhost", 32000))
            {
                Console.WriteLine("Failed to start stonehenge server");
            }

            // Starting chromely frontend
            Console.WriteLine("Starting chromely frontend");
            var startUrl = host.BaseUrl;

            var config = ChromelyConfiguration
                .Create()
                .WithLoadingCefBinariesIfNotFound(true)
                .WithSilentCefBinariesLoading(true)
                // ReSharper disable once RedundantArgumentDefaultValue
                .WithHostMode(WindowState.Normal, true)
                .WithHostTitle(options.Title)
                .WithHostIconFile("stonehenge-chromely.ico")
                .WithAppArgs(args)
                .WithHostSize(2000, 1200)
                .RegisterCustomerUrlScheme("http", "localhost")
                .WithStartUrl(startUrl);

            using (var window = ChromelyWindow.Create(config))
            {
                var exitCode = window.Run(args);
                if (exitCode != 0)
                {
                    Console.WriteLine("Failed to start chromely frontend: code " + exitCode);
                }
            }
            
            Console.WriteLine("Demo done.");
        }
    }
}

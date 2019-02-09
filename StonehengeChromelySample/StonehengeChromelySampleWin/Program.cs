using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Chromely.CefGlue.Winapi.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.SimpleHttp;
using IctBaden.Stonehenge3.Vue;
using Xilium.CefGlue;

namespace StonehengeChromelySample
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

            Console.WriteLine("Starting stonehenge backend");
            var options = new StonehengeHostOptions
            {
                Title = "Demo"
            };
            var provider = StonehengeResourceLoader
                .CreateDefaultLoader(new VueResourceProvider());
            var host = new KestrelHost(provider, options);
            if (!host.Start(options.Title, false, "localhost", 8888))
            {
                Console.WriteLine("Failed to start stonehenge server");
            }

            // ensure CEF runtime files are present
            Console.WriteLine("Check CEF framework is installed in the correct version");
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? ".";
            Directory.SetCurrentDirectory(path);
            try
            {
                CefRuntime.Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to load runtime: " + ex.Message);
                Console.WriteLine("Installing CEF runtime from " + CefLoader.CefBuildsDownloadUrl);
                CefLoader.Load();
            }
            
            Console.WriteLine("Starting chromely frontend");
            var startUrl = host.BaseUrl;

            var config = ChromelyConfiguration
                .Create()
                .WithHostMode(WindowState.Normal, true)
                .WithHostTitle(options.Title)
                .WithHostIconFile("stonehenge-chromely.ico")
                .WithAppArgs(args)
                .WithHostSize(1000, 600)
                .RegisterCustomrUrlScheme("http", "localhost")
                .WithStartUrl(startUrl);

            using (var window = new CefGlueBrowserWindow(config))
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

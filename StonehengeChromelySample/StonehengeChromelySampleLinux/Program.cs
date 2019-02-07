using System;
using System.IO;
using System.Reflection;
using Chromely.CefGlue.Gtk.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;
using Xilium.CefGlue;

namespace StonehengeChromelySample
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
        [STAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("Sample showing stonehenge on Chromely");

            // stonehenge backend
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
            
            // chromely frontend
            var startUrl = host.BaseUrl;

            var config = ChromelyConfiguration
                .Create()
                .WithHostMode(WindowState.Normal, true)
                .WithHostTitle(options.Title)
                //.WithHostIconFile("chromely.ico")
                .WithAppArgs(args)

                // Linux specific
                .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, false)
                .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                .WithCustomSetting(CefSettingKeys.NoSandbox, true)
                
                .WithCommandLineArg("disable-extensions", "1")
                .WithCommandLineArg("disable-gpu", "1")
                .WithCommandLineArg("disable-gpu-compositing", "1")
                .WithCommandLineArg("disable-smooth-scrolling", "1")
                .WithCommandLineArg("no-sandbox", "1")

                .WithHostSize(1000, 600)
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

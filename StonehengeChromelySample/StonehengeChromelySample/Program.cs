using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Chromely.CefGlue.Loader;
#if LINUX
using Chromely.CefGlue.Gtk.BrowserWindow;
#else
using Chromely.CefGlue.Winapi.BrowserWindow;
#endif
using Chromely.Core;
// ReSharper disable once RedundantUsingDirective
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
        private static string ChromiumVersion => 
            Assembly.GetAssembly(typeof(CefGlueBrowserWindow))
                .GetName().Version.ToString();

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Console.WriteLine("Sample showing stonehenge on Chromely");
            Console.WriteLine();

            Console.WriteLine($"Running on {RuntimeEnvironment.GetRuntimeDirectory()}, CLR {RuntimeEnvironment.GetSystemVersion()}");
            Console.WriteLine($"Chromium version {ChromiumVersion}");
            Console.WriteLine();

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
                CefLoader.Load();
            }

            // Starting stonehenge backend
            Console.WriteLine("Starting stonehenge backend");
            var options = new StonehengeHostOptions
            {
                Title = "Demo",
                StartPage = "start",
                ServerPushMode = ServerPushModes.LongPolling,
                PollIntervalMs = 1000
            };
            var provider = StonehengeResourceLoader
                .CreateDefaultLoader(new VueResourceProvider());
            var host = new KestrelHost(provider, options);
            if (!host.Start("localhost", 8888))
            {
                Console.WriteLine("Failed to start stonehenge server");
            }

            // Starting chromely frontend
            Console.WriteLine("Starting chromely frontend");
            var startUrl = host.BaseUrl;

            var config = ChromelyConfiguration
                .Create()
                // ReSharper disable once RedundantArgumentDefaultValue
                .WithHostMode(WindowState.Normal, true)
                .WithHostTitle(options.Title)
                .WithHostIconFile("stonehenge-chromely.ico")
                .WithAppArgs(args)
#if LINUX
                //TODO: Can be removed using Chromely v0.9.3
                .WithCustomSetting(CefSettingKeys.MultiThreadedMessageLoop, false)
                .WithCustomSetting(CefSettingKeys.SingleProcess, true)
                .WithCustomSetting(CefSettingKeys.NoSandbox, true)

                .WithCommandLineArg("disable-extensions", "1")
                .WithCommandLineArg("disable-gpu", "1")
                .WithCommandLineArg("disable-gpu-compositing", "1")
                .WithCommandLineArg("disable-smooth-scrolling", "1")
                .WithCommandLineArg("no-sandbox", "1")
                .WithCommandLineArg("no-zygote", "1")
#endif
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

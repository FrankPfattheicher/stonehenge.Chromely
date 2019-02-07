using System;
using Chromely.CefGlue.Gtk.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Host;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.Kestrel;
using IctBaden.Stonehenge3.Resources;
using IctBaden.Stonehenge3.Vue;

namespace StonehengeChromelySample
{
    internal static class Program
    {
        // ReSharper disable once UnusedParameter.Local
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

            // chromely frontend
            var startUrl = "https://google.com";

            var config = ChromelyConfiguration
                .Create()
                .WithHostMode(WindowState.Normal, true)
                .WithHostTitle("chromely")
                .WithHostIconFile("chromely.ico")
                .WithAppArgs(args)
                .WithHostSize(1000, 600)
                .WithStartUrl(startUrl);

            using (var window = new CefGlueBrowserWindow(config))
            {
                return window.Run(args);
            }
            
            
            Console.ReadLine();
        }
    }
}
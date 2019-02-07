using System;
using System.Reflection;
using Chromely.CefGlue.Gtk.BrowserWindow;
using IctBaden.Stonehenge3.Hosting;

namespace StonehengeChromelySample.ViewModels
{
    public class StartVm
    {
        public string TimeStamp => DateTime.Now.ToLongTimeString();
        
        public string StonehengeVersion => 
            Assembly.GetAssembly(typeof(StonehengeHostOptions))
            .GetName().Version.ToString();
        
        public string ChromelyVersion  => 
            Assembly.GetAssembly(typeof(CefGlueBrowserWindow))
                .GetName().Version.ToString();

        
    }
}
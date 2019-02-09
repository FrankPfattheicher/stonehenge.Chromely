using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Chromely.CefGlue.Gtk.BrowserWindow;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.ViewModel;
// ReSharper disable UnusedMember.Global

namespace StonehengeChromelySample.ViewModels
{
    public class StartVm : ActiveViewModel, IDisposable
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public string TimeStamp => DateTime.Now.ToLongTimeString();

        public string StonehengeVersion =>
            Assembly.GetAssembly(typeof(StonehengeHostOptions))
            .GetName().Version.ToString();

        public string ChromelyVersion =>
            Assembly.GetAssembly(typeof(CefGlueBrowserWindow))
                .GetName().Version.ToString();

        public string DemoAppVersion =>
            Assembly.GetAssembly(GetType())
                .GetName().Version.ToString();
        
        public string RuntimeDirectory => RuntimeEnvironment.GetRuntimeDirectory();

        public string ClrVersion => RuntimeEnvironment.GetSystemVersion();


        private readonly Task _updater;
        private readonly CancellationTokenSource _cancelUpdate;

        public StartVm(AppSession session)
            : base(session)
        {
            _cancelUpdate = new CancellationTokenSource();
            _updater = new Task(
                () =>
                {
                    while ((_updater != null) && !_cancelUpdate.IsCancellationRequested)
                    {
                        try
                        {
                            Task.Delay(1000, _cancelUpdate.Token).Wait();
                        }
                        catch (TaskCanceledException)
                        {
                            break;
                        }
                        NotifyPropertyChanged(nameof(TimeStamp));
                    }
                    // ReSharper disable once FunctionNeverReturns
                }, _cancelUpdate.Token);
            _updater.Start();
        }

        public void Dispose()
        {
            _cancelUpdate.Cancel();
        }

    }
}
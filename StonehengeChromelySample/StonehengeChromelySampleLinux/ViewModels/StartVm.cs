using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Chromely.CefGlue.Gtk.BrowserWindow;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.ViewModel;

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
        
        public string ChromelyVersion  => 
            Assembly.GetAssembly(typeof(CefGlueBrowserWindow))
                .GetName().Version.ToString();

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
                        Task.Delay(1000, _cancelUpdate.Token).Wait();
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
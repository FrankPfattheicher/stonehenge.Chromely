using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Chromely.Core;
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
            FileVersionInfo.GetVersionInfo(Assembly.GetAssembly(typeof(StonehengeHostOptions)).Location).ProductVersion;
        
        public string ChromelyVersion  => 
            Assembly.GetAssembly(typeof(ChromelyConfiguration))
                .GetName().Version.ToString();

        public string DemoAppVersion =>
            Assembly.GetAssembly(GetType())
                .GetName().Version.ToString();

        
        public string RuntimeDirectory => RuntimeEnvironment.GetRuntimeDirectory();
        public bool IsSelfHosted { get; private set; }
        public bool IsNetCore { get; private set; }

        public string ClrVersion => RuntimeEnvironment.GetSystemVersion();


        private readonly Task _updater;
        private readonly CancellationTokenSource _cancelUpdate;
        
        public StartVm(AppSession session) 
            : base(session)
        {
            _cancelUpdate = new CancellationTokenSource();
            _updater = new Task(UpdateView, _cancelUpdate.Token);
            _updater.Start();
            
            var runtimePath = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "."));
            var applicationPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "."));
            IsSelfHosted = runtimePath == applicationPath;

            IsNetCore = RuntimeInformation.FrameworkDescription.Contains("Core");
        }

        private void UpdateView()
        {
            while ((_updater != null) && !_cancelUpdate.IsCancellationRequested)
            {
                try
                {
                    NotifyPropertyChanged(nameof(TimeStamp));
                    Task.Delay(1000, _cancelUpdate.Token).Wait();
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        public void Dispose()
        {
            _cancelUpdate.Cancel();
            GC.SuppressFinalize(this);
        }
        
    }
}
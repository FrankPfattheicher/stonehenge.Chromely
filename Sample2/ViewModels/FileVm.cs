using System.Collections.Generic;
using Chromely.Dialogs;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.ViewModel;

namespace Sample2.ViewModels
{
    class FileVm : ActiveViewModel
    {
        public string Text
        {
            get { return Program.FileText; }
            set { Program.FileText = value; }
        }

        public FileVm(AppSession session) : base(session)
        {
            Text = "Initial text";
        }

        public void FileLoaded()
        {
            NotifyPropertyChanged(nameof(Text));
        }
    }
}



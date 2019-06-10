using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.Hosting;
using IctBaden.Stonehenge3.ViewModel;

namespace Sample2.ViewModels
{
    internal class FileVm : ActiveViewModel, IStonehengeAppCommands
    {
        public string Text
        {
            get => Program.FileText;
            set => Program.FileText = value;
        }

        public FileVm(AppSession session) : base(session)
        {
        }

        [ActionMethod]
        public void TextChanged()
        {
        }

        public void FileLoaded()
        {
            NotifyPropertyChanged(nameof(Text));
        }

    }
}



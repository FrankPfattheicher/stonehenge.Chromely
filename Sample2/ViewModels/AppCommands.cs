using System.Collections.Generic;
using System.IO;
using Chromely.Dialogs;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.Hosting;

namespace Sample2.ViewModels
{
    class AppCommands : IStonehengeAppCommands
    {
        public void FileOpen(AppSession session)
        {
            var options = new FileDialogOptions
            {
                Title = "Open",
                Filters = new List<FileFilter>
                {
                    new FileFilter { Extension = "txt", Name = "Text files" }
                },
                MustExist = true
            };

            var openResult = ChromelyDialogs.FileOpen("Open Text Tile", options);

            if (!openResult.IsCanceled)
            {
                Program.FileText = File.ReadAllText(openResult.Value.ToString());
                if (session.ViewModel is FileVm vm)
                {
                    vm.FileLoaded();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Chromely.Dialogs;
using IctBaden.Stonehenge3.Core;
using IctBaden.Stonehenge3.Hosting;

namespace Sample2.ViewModels
{
    // ReSharper disable once UnusedMember.Global
    internal class AppCommands : IStonehengeAppCommands
    {
        // ReSharper disable once UnusedMember.Global
        public void FileNew(AppSession session)
        {
            Program.FileText = string.Empty;
            if (session.ViewModel is FileVm vm)
            {
                vm.FileLoaded();
            }
        }

        // ReSharper disable once UnusedMember.Global
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

            var openResult = ChromelyDialogs.FileOpen("Open Text File", options);

            if (!openResult.IsCanceled)
            {
                try
                {
                    var fn = openResult.Value.ToString();
                    Program.FileText = File.ReadAllText(fn);
                    if (session.ViewModel is FileVm vm)
                    {
                        vm.FileLoaded();
                    }
                    Program.FileName = fn;
                }
                catch (Exception ex)
                {
                    ChromelyDialogs.MessageBox(ex.Message, new DialogOptions { Icon = DialogIcon.Error });
                }
                
            }
        }

        // ReSharper disable once UnusedMember.Global
        public void FileSave(AppSession session)
        {
            var options = new FileDialogOptions
            {
                Title = "Save",
                Filters = new List<FileFilter>
                {
                    new FileFilter { Extension = "txt", Name = "Text files" }
                },
                MustExist = true
            };

            var openResult = ChromelyDialogs.FileSave("Save My Changes", Program.FileName, options);

            if (!openResult.IsCanceled)
            {
                try
                {
                    File.WriteAllText(openResult.Value.ToString(), Program.FileText);
                }
                catch (Exception ex)
                {
                    ChromelyDialogs.MessageBox(ex.Message, new DialogOptions {Icon = DialogIcon.Error});
                }
            }
        }
    }
}

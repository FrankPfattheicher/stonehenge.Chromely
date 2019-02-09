using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Tar;
using Xilium.CefGlue;

namespace StonehengeChromelySample
{
    //TODO: Move this class to Chromely project
    
    /// <summary>
    /// Loads the necessary CEF runtime files from opensource.spotify.com
    /// Inherits detailed version information from cefbuilds/index page.
    ///
    /// Note:
    /// Keep this class in a separate nuget package
    /// due to additional reference to ICSharpCode.SharpZipLib.
    /// Not everyone will be glad about this. 
    /// </summary>
    public static class CefLoader
    {
        public const string CefBuildsDownloadUrl = "http://opensource.spotify.com/cefbuilds";
        private static string CefBuildsDownloadIndex(string platform) => $"http://opensource.spotify.com/cefbuilds/index.html#{platform}_builds";
        private static string CefDownloadUrl(string name) => $"http://opensource.spotify.com/cefbuilds/{name}";

        
        // http://opensource.spotify.com/cefbuilds/cef_binary_3.3440.1805.gbe070f9_linux64_minimal.tar.bz2

        /// <summary>
        /// Load CEF runtime files.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void Load()
        {
            var arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString().ToLower();
            var platform = CefRuntime.Platform.ToString().ToLower() + arch.Replace("x", "");
            var version = string.Join(".", CefRuntime.ChromeVersion.Split('.')[2]);
            
            var indexUrl = CefBuildsDownloadIndex(platform);
            var binaryNamePattern = $@"""((cef_binary_[0-9]+\.{version.Replace(".", @"\.")}\.[0-9]+\.(.*)_{platform}_minimal).tar.bz2)""";

            var tempBz2File = Path.GetTempFileName();
            var tempTarFile = Path.GetTempFileName();
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

            try
            {
                using (var client = new WebClient())
                {
                    var index = client.DownloadString(indexUrl);
                    var found = new Regex(binaryNamePattern).Match(index);
                    if (!found.Success)
                    {
                        throw new Exception($"CEF for chrome version {CefRuntime.ChromeVersion} not found.");
                    }

                    var archiveName = found.Groups[1].Value;
                    var folderName = found.Groups[2].Value;
                    var downloadUrl = CefDownloadUrl(archiveName);
                    client.DownloadFile(downloadUrl, tempBz2File);
                    
                    using(var inStream = new FileStream(tempBz2File, FileMode.Open))
                    using (var outStream = new FileStream(tempTarFile, FileMode.Create))
                    {
                        BZip2.Decompress(inStream, outStream, true);                        
                    }
                    using(var tarStream = new FileStream(tempTarFile, FileMode.Open))
                    {
                        var tar = TarArchive.CreateInputTarArchive(tarStream);
                        tar.ProgressMessageEvent += (archive, entry, message) => Console.WriteLine("Extracting " + entry.Name);

                        Directory.CreateDirectory(tempDirectory);
                        tar.ExtractContents(tempDirectory);
                    }
                    
                    // now we have all files in the temporary directory
                    // we have to copy the 'Release' and 'Resources' folder to the project directory
                    var srcPathRelease = Path.Combine(tempDirectory, folderName, "Release");
                    var srcPathResources = Path.Combine(tempDirectory, folderName, "Resources");
                    var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    
                    CopyDirectory(srcPathRelease, appDirectory);
                    CopyDirectory(srcPathResources, appDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                File.Delete(tempBz2File);
                File.Delete(tempTarFile);
                if (Directory.Exists(tempDirectory))
                {
                    Directory.Delete(tempDirectory, true);
                }
            }
        }

        private static void CopyDirectory(string srcPath, string dstPath)
        {
            // Create all of the sub-directories
            foreach (var dirPath in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(srcPath, dstPath));
            }

            // Copy all the files & replaces any files with the same name
            foreach (var newPath in Directory.GetFiles(srcPath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(srcPath, dstPath), true);
            }
        }
        
    }
}
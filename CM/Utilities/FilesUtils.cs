using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CM.Utilities
{
    static class FileUtils
    {
        public static bool ShowPickFolderDialog(string description, string initialPath, out string selectedPath)
        {
            selectedPath = null;
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = description;
                if (!string.IsNullOrWhiteSpace(initialPath) && Directory.Exists(initialPath))
                    dialog.SelectedPath = initialPath;
                dialog.ShowNewFolderButton = true;
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = dialog.SelectedPath;
                    return true;
                }
                return false;
            }
        }

        public static bool ShowPickFolderDialog(string description, string initialPath, out string selectedPath, string userName, string password)
        {
            if (!string.IsNullOrEmpty(initialPath) && !Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(initialPath, new NetworkCredential(userName, password)))
                {
                    return ShowPickFolderDialog(description, initialPath, out selectedPath);
                }
            }
            else
            {
                return ShowPickFolderDialog(description, initialPath, out selectedPath);
            }
        }

        public static void OpenFolderInExplorer(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
                Process.Start("explorer.exe", path);
        }

        public static void OpenFolderInExplorer(string path, string userName, string password)
        {
            if (!Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(path, new NetworkCredential(userName, password)))
                {
                    OpenFolderInExplorer(path);
                }
            }
            else
            {
                OpenFolderInExplorer(path);
            }
        }

        public static void OpenResource(string resource)
        {
            if (!string.IsNullOrWhiteSpace(resource))
                Process.Start(resource);
        }

        public static IEnumerable<FileInfo> FindFiles(string path, Func<FileInfo, bool> discriminator)
        {
            if (Directory.Exists(path))
            {
                FileInfo info;
                foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    info = new FileInfo(file);
                    if (discriminator(info))
                    {
                        yield return info;
                    }
                }
            }
        }

        public static IEnumerable<FileInfo> FindFiles(string path, Func<FileInfo, bool> discriminator, string userName, string password)
        {
            if (!Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(path, new NetworkCredential(userName, password)))
                {
                    return FindFiles(path, discriminator);
                }
            }
            else
            {
                return FindFiles(path, discriminator);
            }
        }

        public static byte[] GetFileBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public static byte[] GetFileBytes(string fileDirectory, string file, string userName, string password)
        {
            if (!Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(fileDirectory, new NetworkCredential(userName, password)))
                {
                    return GetFileBytes(file);
                }
            }
            else
            {
                return GetFileBytes(file);
            }
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public static void WriteAllBytes(string fileDirectory, string path, byte[] bytes, string userName, string password)
        {
            if (!Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(fileDirectory, new NetworkCredential(userName, password)))
                {
                    WriteAllBytes(path, bytes);
                }
            }
            else
            {
                WriteAllBytes(path, bytes);
            }
        }

        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public static void DeleteFile(string fileDirectory, string path, string userName, string password)
        {
            if (!Extenssions.IsAnyNullOrWhiteSpace(userName, password))
            {
                using (new NetworkConnection(fileDirectory, new NetworkCredential(userName, password)))
                {
                    DeleteFile(path);
                }
            }
            else
            {
                DeleteFile(path);
            }
        }

        public static async Task SaveFile(string filePath, string title, byte[] bytes)
        {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = filePath;
            dialog.Title = title;
            dialog.DefaultExt = System.IO.Path.GetExtension(filePath);
            dialog.AddExtension = true;
            dialog.Filter = string.Format("*{0}|*{0}", System.IO.Path.GetExtension(filePath));
            if (dialog.ShowDialog() == true)
            {
                Stream file = dialog.OpenFile();
                await file.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace _30ViewModel
{
    public static class GetAndCopyImage
    {
        public static string CopyImage()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Пользовательские файлы (*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png) |*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png";
            if (true == OpenFileDialog.ShowDialog())
            {
                string filePath = OpenFileDialog.FileName;
                string path = "data";
                string extension = Path.GetExtension(filePath);
                var hashSet = new HashSet<string>()
                {
                    ".jfif", ".pjpeg", ".jpeg", ".pjp", ".tiff", ".bmp", ".jpg", ".png"
                };
                if (hashSet.Contains(extension))
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string[] paths = { path, Path.GetRandomFileName() + extension };
                    string newPath = Path.Combine(paths);
                    File.Copy(filePath, newPath);
                    return newPath;
                }
            }
            return null;
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _30ViewModel
{
    public class ImageCopy
    {
        public static void CopyringImg()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Пользовательские файлы (*.jpg; *.png) |*.jpg; *.png";
            if (true == OpenFileDialog.ShowDialog())
            {
                string filePath = OpenFileDialog.FileName;
                string newFilePath;
                string extension = Path.GetExtension(filePath);
                if (extension == ".jpg")
                {
                    newFilePath = Path.GetRandomFileName() + ".jpg";
                }
                else if (extension == ".png")
                {
                    newFilePath = Path.GetRandomFileName() + ".png";
                }
                else
                {
                    throw new NotImplementedException();
                }
                File.Copy(filePath, newFilePath);
            }
        }

    }
}

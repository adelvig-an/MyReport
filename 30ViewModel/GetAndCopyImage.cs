using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace _30ViewModel
{
    public static class GetAndCopyImage
    {
        public static void AddInsurancePolicieImage()
        {
            OpenFileDialog OpenFileDialog = new OpenFileDialog();
            OpenFileDialog.Filter = "Пользовательские файлы (*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png) |*.jfif; *.pjpeg; *.jpeg; *.pjp; *.tiff; *.bmp; *.jpg; *.png";
            if (true == OpenFileDialog.ShowDialog())
            {
                string filePath = OpenFileDialog.FileName;
                string newFileName;
                string extension = Path.GetExtension(filePath);
                if (extension == ".jfif")
                {
                    newFileName = Path.GetRandomFileName() + ".jfif";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);
                }
                else if (extension == ".pjpeg")
                {
                    newFileName = Path.GetRandomFileName() + ".pjpeg";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".jpeg")
                {
                    newFileName = Path.GetRandomFileName() + ".jpeg";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".pjp")
                {
                    newFileName = Path.GetRandomFileName() + ".pjp";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".tiff")
                {
                    newFileName = Path.GetRandomFileName() + ".tiff";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".bmp")
                {
                    newFileName = Path.GetRandomFileName() + ".bmp";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".jpg")
                {
                    newFileName = Path.GetRandomFileName() + ".jpg";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);

                }
                else if (extension == ".png")
                {
                    newFileName = Path.GetRandomFileName() + ".png";
                    File.Copy(filePath, newFileName);
                    //PathInsurancePolicieCollection.Add(newFileName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

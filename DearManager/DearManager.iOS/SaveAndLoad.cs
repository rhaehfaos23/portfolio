using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using DearManager.Extension;
using System.IO;
using Xamarin.Forms;
using DearManager.iOS;

[assembly: Dependency(typeof(SaveAndLoad))]

namespace DearManager.iOS
{
    public class SaveAndLoad : ISaveAndLoad
    {
        public byte[] LoadImage(string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                return null;
            }
        }

        public void SaveImage(string fileName, byte[] image)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, image);
        }

        public void DeleteImage(string fileName)
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            File.Delete(filePath);
        }
    }
}
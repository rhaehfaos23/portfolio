using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DearManager.Droid;
using DearManager.Extension;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveAndLoad))]

namespace DearManager.Droid
{
    class SaveAndLoad : ISaveAndLoad
    {
        public void DeleteImage(string fileName)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            File.Delete(filePath);
        }

        public byte[] LoadImage(string fileName)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
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
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllBytes(filePath, image);
        }
    }
}
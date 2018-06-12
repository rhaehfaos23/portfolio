using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class CropImageViewModel: BaseViewModel
    {
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set => SetValue(ref _image, value);
        }
        public CropImageViewModel()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, App.tempImage);
            Image = ImageSource.FromFile(filePath);
        }
    }
}

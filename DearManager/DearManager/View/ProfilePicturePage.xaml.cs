using DearManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePicturePage : ContentPage
	{
        private uint userId;

		public ProfilePicturePage (uint userId)
		{
            InitializeComponent();
            this.userId = userId;
            
            ImageSetDefaultOrSaved();
        }

        private async void ImageSetDefaultOrSaved()
        {
            ImageSource image = await PageService.Default.GetProfileImage(userId);
            profileImage.Source = image;
        }
    }
}
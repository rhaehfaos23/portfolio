using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TutorialPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        private double width;
        private double height;

        public TutorialPage ()
		{
			InitializeComponent ();
		}

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                }
                else
                {
                }
            }
        }

        private async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PopPopupAsync();
            
            // 첫 번째 튜토리얼 확인용 프로퍼티 True로 변경
            Application.Current.Properties[App.confirmTutorialFirst] = true.ToString();
            await Application.Current.SavePropertiesAsync();
        }
    }
}
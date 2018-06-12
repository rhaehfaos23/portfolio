using DearManager.Extension;
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
	public partial class InputUserPage : ContentPage
	{
		public InputUserPage ()
		{
			InitializeComponent ();
            BindingContext = new ViewModel.InputUserViewModel();
        }

        // 가입 안된 사람은 자동으로 꺼지게 만듦 (안드로이드 플랫폼용)
        protected override bool OnBackButtonPressed()
        {
            if (Application.Current.Properties.ContainsKey(App.isFirst))
            {
                string first = (string)Application.Current.Properties[App.isFirst];
                bool boolFirst = Convert.ToBoolean(first);
                if (!boolFirst)
                {
                    return base.OnBackButtonPressed();
                }
            }

            if (Device.RuntimePlatform == Device.Android)
                DependencyService.Get<IAndroidMethods>().KillApp();

            return base.OnBackButtonPressed();
        }
    }
}
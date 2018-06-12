using DearManager.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DearManager.Model;
using Rg.Plugins.Popup.Services;

namespace DearManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GroupReceiptPage : ContentPage
	{
        private uint _member = 0;
        private uint _detail = 0;
        private ImageSource[] images = new ImageSource[2] { ImageSource.FromFile("ic_folding_menu_downside.png"), ImageSource.FromFile("ic_folding_menu_upside.png") };

        private GroupReceiptPageViewModel ViewModel
        {
            get { return (GroupReceiptPageViewModel)BindingContext; }
            set { BindingContext = value; }
        }

		public GroupReceiptPage (Group group)
		{
			InitializeComponent ();
            Title = group.Name;
            
            ViewModel = new GroupReceiptPageViewModel(group);
        }

        void OnClickMemberBtn(object sender, EventArgs e)
        {
            Animation ani = ViewModel.MemberFold();
            ani.Commit(this, "MemberListAnimation");
            (sender as Button).Image = (FileImageSource)images[++_member % 2];
        }
        void OnClickDetailBtn(object sender, EventArgs e)
        {
            Animation ani = ViewModel.DetailFold();
            ani.Commit(this, "DetailListAnimation");
            (sender as Button).Image = (FileImageSource)images[++_detail % 2];
        }

        void GoRepaymentPage(object sender, EventArgs e)
        {
            ViewModel?.CGoRepaymentPage.Execute(null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.UpdateData();
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count >= 1)
            {
                PopupNavigation.Instance.PopAsync(true);
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}
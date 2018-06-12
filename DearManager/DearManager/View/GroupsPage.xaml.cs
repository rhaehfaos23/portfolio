using DearManager.ViewModel;
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
	public partial class GroupsPage : ContentPage
	{
        private ListView GroupsView
        {
            get => GroupsList;
        }

        private GroupsPageViewModel ViewModel
        {
            get => BindingContext as GroupsPageViewModel;
            set => BindingContext = value;
        }

		public GroupsPage ()
		{
			InitializeComponent ();
            BindingContext = new GroupsPageViewModel();
		}

        /// <summary>
        /// 리스트 뷰에 있는 아이템을 클릭시 상세 영수증 페이지로 이동
        /// </summary>
        /// <param name="sender">안녕</param>
        /// <param name="e">하세요</param>
        void Handle_ItemSelected(object sender, EventArgs e)
        {
            ViewModel?.GoReceiptView.Execute(null);
        }

        void Handle_TextChanged(object sender, TextChangedEventArgs e)
        {
            //GetViewModel()?.SearchItem.Execute(GroupsView);
            ViewModel?.OnSearchBarTextChanged(GroupsView);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.PullGroupList();
        }
    }
}
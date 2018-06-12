using DearManager.Model;
using DearManager.ViewModel;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
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
	public partial class SearchFriendPage : PopupPage
	{
        private SearchFriendViewModel ViewModel
        {
            get => BindingContext as SearchFriendViewModel;
            set => BindingContext = value;
        }

		public SearchFriendPage (List<Person> people)
		{
			InitializeComponent ();
            ViewModel = new SearchFriendViewModel(people);
		}

        void FriendSearchStart(object sender, EventArgs e)
        {
            ViewModel.SearchFrined();
        }

        void ClosePage(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
        }
    }
}
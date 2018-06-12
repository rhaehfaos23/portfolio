using DearManager.Model;
using DearManager.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChoiceFriendsPage : ContentPage
	{
        ChoiceFriendsPageViewModel ViewModel
        {
            get => BindingContext as ChoiceFriendsPageViewModel;
            set => BindingContext = value;
        }

		public ChoiceFriendsPage ()
		{
			InitializeComponent ();
        }

        public ChoiceFriendsPage(Group group): this()
        {
            BindingContext = new ChoiceFriendsPageViewModel(group: group);
        }

        public ChoiceFriendsPage(Person person): this()
        {
            BindingContext = new ChoiceFriendsPageViewModel(person: person);
        }

        void OnSelectItem(object sender, EventArgs e)
        {
            ViewModel?.CSelectItem?.Execute(null);
        }
    }
}
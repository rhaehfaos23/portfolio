using DearManager.Model;
using System.Collections.Generic;
using System.Linq;
using DearManager.Custom;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using DearManager.ViewModel;
using System.Collections.ObjectModel;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FriendsPage : ContentPage
	{
        private FriendsPageViewModel ViewModel
        {
            get { return BindingContext as FriendsPageViewModel; }
            set { BindingContext = value; }
        }

        public FriendsPage ()
		{
			InitializeComponent ();
            ViewModel = new FriendsPageViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<FriendsPageViewModel, uint>(this, MessangerKey.FriendRequestRemove, FriendRequestRemove);

            await ViewModel?.UpdateData();

            while (FriendRequestList.Children.Count() >= 2)
            {
                FriendRequestList.Children.RemoveAt(1);
            }

            foreach (var sender in ViewModel.Requests)
            {
                FriendRequestList.Children.Add(new FriendRequest(sender.Id, sender.Name, ViewModel.CCommit, ViewModel.CReject));
            }

            lblNumberOfRequest.Text = AppResources.txtFriendsPageRequestCheckBefore + (FriendRequestList.Children.Count() - 1).ToString() +
                AppResources.txtFriendsPageRequestCheckAfter;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<FriendsPageViewModel, uint>(this, MessangerKey.FriendRequestRemove);
        }

        private void FriendRequestRemove(FriendsPageViewModel sender, uint id)
        {
            var friendRequest = FriendRequestList.Children.OfType<FriendRequest>().Where(FR => FR.SenderId == id).First();
            FriendRequestList.Children.Remove(friendRequest);
            lblNumberOfRequest.Text = AppResources.txtFriendsPageRequestCheckBefore + (FriendRequestList.Children.Count() - 1).ToString() +
                AppResources.txtFriendsPageRequestCheckAfter;
        }

        void GotoFriendProfifle(object sender, EventArgs e)
        {
            ViewModel?.CGotoFriendProfile?.Execute(null);
        }
    }
}
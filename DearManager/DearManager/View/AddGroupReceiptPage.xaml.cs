using DearManager.Model;
using DearManager.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddGroupReceiptPage : ContentPage
	{
		public AddGroupReceiptPage (Group group)
		{
			InitializeComponent ();
            BindingContext = new AddGroupReceiptPageViewModel(group);
            MessagingCenter.Subscribe<AddGroupReceiptPageViewModel, List<ViewCell>>(this, MessangerKey.AddTableSection, AddTableSectionChild);
		}

        void AddTableSectionChild(AddGroupReceiptPageViewModel sender, List<ViewCell> Cells)
        {
            memberSection.Clear();
            foreach (var cell in Cells)
            {
                memberSection.Add(cell);
            }
            Button button = new Button { Text = AppResources.txtAddGroupReceiptMemberAddButton, BackgroundColor = Color.Transparent };
            button.SetBinding(Button.CommandProperty, nameof(AddGroupReceiptPageViewModel.CGoChoiceFriendsPage));
            ViewCell view = new ViewCell()
            {
                View = button
            };
            memberSection.Add(view);
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
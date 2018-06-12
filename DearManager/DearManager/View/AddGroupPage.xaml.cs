using DearManager.Custom;
using DearManager.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DearManager.Model;
using System.Threading.Tasks;

namespace DearManager.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddGroupPage : ContentPage
	{
		public AddGroupPage ()
		{
			InitializeComponent ();
            BindingContext = new AddGroupPageViewModel();
        }


        /// <summary>
        /// 멤버 추가
        /// </summary>
        /// <param name="sender"></param>
        private async void AddMemberSection(AddGroupPageViewModel sender)
        {
            MessagingCenter.Unsubscribe<AddGroupPageViewModel>(this, MessangerKey.PeopleListChanged);
            var people = (BindingContext as AddGroupPageViewModel)?.People;

            memberSection.Clear();
            List<Task> tasks = new List<Task>();
            foreach (var person in people)
            {
                tasks.Add(Task.Run(async () =>
                {
                    person.ProfileImage = await PageService.Default.GetProfileImage(person.Id);
                }));
            }

            await Task.WhenAll(tasks);

            foreach (var person in people)
            {
                StackLayout stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10, 3)
                };
                stack.Children.Add(
                    new CircleImage
                    { Source =  person.ProfileImage,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start});

                stack.Children.Add(new Label { Text = person.NickName, VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand });

                Button removeBtn = new Button { Image = (FileImageSource)FileImageSource.FromFile("ic_remove.png"),
                    BackgroundColor = Color.Transparent, WidthRequest = 24, HeightRequest = 24,
                    Command = (BindingContext as AddGroupPageViewModel).CRemoveFriend, CommandParameter = person.Id };

                removeBtn.Clicked += RemoveBtn_Clicked;

                stack.Children.Add(removeBtn);

                ViewCell viewCell = new ViewCell
                {
                    View = stack,
                    ClassId = person.Id.ToString()
                };

                memberSection.Add(viewCell);
            }

            Button addBtn = new Button { Text = "추가", BackgroundColor = Color.Transparent };
            addBtn.SetBinding(Button.CommandProperty, nameof(AddGroupPageViewModel.CGotoChoiceFriendsPage));
            addBtn.Clicked += SubscribeMessaging;
            memberSection.Add(new ViewCell { View = addBtn });
        }

        private void RemoveBtn_Clicked(object sender, EventArgs e)
        {
            var id = (sender as Button).CommandParameter.ToString();
            foreach (var cell in memberSection.ToList())
            {
                if (cell.ClassId == id)
                {
                    memberSection.Remove(cell);
                }
            }
        }

        private void SubscribeMessaging(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<AddGroupPageViewModel>(this, MessangerKey.PeopleListChanged, AddMemberSection);
        }
    }
}
using DearManager.Model;
using DearManager.View;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class AddGroupReceiptPageViewModel: BaseViewModel
    {
        Group _group;
        string _amount;
        List<Person> selectedFriends;
        List<Entry> entries = new List<Entry>();
        bool isToggle = true;
        decimal AllDutchAmount;
        public string Amount
        {
            get => _amount;
            set
            {
                SetValue(ref _amount, value);
            }
        }
        public string Tag { get; set; }
        
        public ICommand CGoChoiceFriendsPage { get; private set; }
        public ICommand CLetsDutch { get; private set; }
        public ICommand CCancel { get; private set; }
        public ICommand CCommit { get; private set; }
        public ICommand CRemoveFriend { get; private set; }

        public AddGroupReceiptPageViewModel(Group group)
        {
            _group = group;
            Amount = "";
            selectedFriends = new List<Person>();
            CGoChoiceFriendsPage = new Command(GoChoiceFriendspage);
            CLetsDutch = new Command<TableSection>(LetsDutch);
            CCancel = new Command(OnClickCancelBtn);
            CCommit = new Command(OnClickCommitBtn);
            CRemoveFriend = new Command<uint>(RemoveFriend);
        }

        private void RemoveFriend(uint id)
        {
            selectedFriends.RemoveAll(f => f.Id == id);

            MessagingCenter.Send(this, MessangerKey.AddTableSection, GenerateCell(selectedFriends));
        }

        /// <summary>
        /// 취소 버튼 클릭시 뒤로가기
        /// </summary>
        private async void OnClickCancelBtn()
        {
            //현재 페이지 종료
            await PageService.Default.PopAsync();
        }

        /// <summary>
        /// 확인 버튼 클릭시 영수증을 저장
        /// </summary>
        private async void OnClickCommitBtn()
        {
            await PopupNavigation.Instance.PushAsync(new ProcessPage(), false);
            
            //건명이나 금액에 빈공간 발견시 경고창을 띄움
            if (string.IsNullOrEmpty(Tag) || string.IsNullOrEmpty(Amount))
            {
                await PageService.Default.DisplayAlert(AppResources.txtAddGroupReceiptPageVMBlankTitle,
                    AppResources.txtAddGroupReceiptPageVMBlankCaption, AppResources.txtButtonOK);

                await PopupNavigation.Instance.PopAsync(false);
                return;
            }

            //금액란에 입력되지 않은 사람이 존재하면 경고창을 띄움
            if (entries.Where(cell => string.IsNullOrEmpty(cell.Text)).Count() > 0)
            {
                await PageService.Default.DisplayAlert(AppResources.txtAddGroupReceiptPageVMBlankMoneyTitle,
                    AppResources.txtAddGroupReceiptPageVMBlankMoneyCaption, AppResources.txtButtonOK);

                await PopupNavigation.Instance.PopAsync(false);
                return;
            }

            var total = entries.Sum(c => Convert.ToDecimal(c.Text));
            //전체 금액과 친구들 금액을 합산한 금액이 같지 않으면 경고창을 띄움
            if (total == decimal.Parse(Amount) || total == AllDutchAmount)
            {
                await SaveReceipt();
            }
            else
            {
                var result = await PageService.Default.DisplayAlert(AppResources.txtAddGroupReceiptPageVMPriceErrorTitle, 
                    AppResources.txtAddGroupReceiptPageVMPriceErrorCaption, AppResources.txtButtonOK, AppResources.txtButtonCancel);
                if (result == true)
                {
                    await SaveReceipt();
                }
                else
                {
                    await PopupNavigation.Instance.PopAsync(false);
                    return;
                }
            }
        }

        private async Task SaveReceipt()
        {
            HttpGroupsReceiptsAccess receiptsAccess = new HttpGroupsReceiptsAccess(_group.Id, 0, 0);
            //영수증을 DB에 저장
            List<Task> tasks = new List<Task>();
            foreach (Entry cell in entries.Where(e => e.Text != "0"))
            {
                var personId = Convert.ToUInt32(cell.ClassId);
                var tempMoney = Convert.ToDecimal(cell.Text);
                var temp = new { person_id = personId, group_id = _group.Id, money = tempMoney, tag = Tag };
                var data = JsonConvert.SerializeObject(temp);
                tasks.Add(receiptsAccess.HttpPostAsync(data));
            }

            await Task.WhenAll(tasks);

            await PopupNavigation.Instance.PopAsync(false);

            //현재 페이지 종료
            await PageService.Default.PopAsync();
        }

        /// <summary>
        /// 더치페이 계산 적용
        /// </summary>
        /// <param name="members"></param>
        private void LetsDutch(TableSection members)
        {
            if (decimal.TryParse(Amount, out decimal result))
            {
                var dutchEntry = entries.Where(e => e.Text != "0").ToList();
                if (dutchEntry.Count() == 0)
                {
                    PageService.Default.DisplayAlert(AppResources.txtAddGroupReceiptPageVMDividedZeroTitle, 
                        AppResources.txtAddGroupReceiptPageVMDividedZeroCaption, AppResources.txtButtonOK);
                    return;
                }
                string dutchAmount;
                //더치 금액이 얼마인지 계산
                if (isToggle)
                {
                    dutchAmount = Math.Floor(result / entries.Count()).ToString();
                    isToggle = false;
                }
                else
                {
                    dutchAmount = (result / entries.Count()).ToString("0.#");
                    isToggle = true;
                }

                foreach (var entry in dutchEntry)
                {
                    //각 사람마다 더치 금액 적용
                    entry.Text = dutchAmount;
                }

                var remainder = result - (decimal.Parse(dutchAmount) * entries.Count());
                Random rnd = new Random();
                int rndNum = rnd.Next(0, dutchEntry.Count());
                dutchEntry[rndNum].Text = (decimal.Parse(dutchEntry[rndNum].Text) + remainder).ToString();
                AllDutchAmount = dutchEntry.Sum(e => decimal.Parse(e.Text));
            }
        }

        /// <summary>
        /// 친구 선택 페이지로 이동
        /// </summary>
        private async void GoChoiceFriendspage()
        {
            MessagingCenter.Subscribe<ChoiceFriendsPageViewModel, List<Person>>(this, MessangerKey.FriendAdds, FriendsAddCallback);
            await PageService.Default.PushAsync(new ChoiceFriendsPage(_group));
        }

        /// <summary>
        /// 친구 선택 페이지에서 돌아오면 실행되는 콜백
        /// </summary>
        /// <param name="sender">친구 선택페이지</param>
        /// <param name="friends">친구 선택페이지에서 선택된 친구들</param>
        private async void FriendsAddCallback(ChoiceFriendsPageViewModel sender, List<Person> friends)
        {
            MessagingCenter.Unsubscribe<ChoiceFriendsPageViewModel, List<Person>>(this, MessangerKey.FriendAdds);

            //친구 선택페이지에서 선택된 친구들 목록 추가
            foreach (Person friend in friends)
            {
                friend.ProfileImage = await PageService.Default.GetProfileImage(friend.Id);

                selectedFriends.Add(friend);
            }

            //겹치는 친구들 삭제
            selectedFriends = selectedFriends.Distinct(new PersonEqualityComparer()).ToList();

            //ViewCell 생성 및 View로 전달
            MessagingCenter.Send(this, MessangerKey.AddTableSection, GenerateCell(selectedFriends));
        }

        /// <summary>
        /// ViewCell을 생성과 저장
        /// </summary>
        /// <param name="people">친구들 정보</param>
        /// <returns>생성된 ViewCell 목록</returns>
        private List<ViewCell> GenerateCell(List<Person> people)
        {
            entries.Clear();
            var cells = new List<ViewCell>();
            foreach (var person in people)
            {
                var stack = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    Padding = new Thickness(10, 3)
                };

                stack.Children.Add(new Custom.CircleImage
                {
                    Source = person.ProfileImage,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    HeightRequest = 40,
                    WidthRequest = 40
                });

                stack.Children.Add(new Label
                {
                    Text = person.NickName,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    TextColor = Color.Black,
                    FontAttributes = FontAttributes.Bold
                });

                Entry entry = new Entry
                {
                    Placeholder = AppResources.txtAddGroupReceiptPageVMMoneyPlaceholder,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Keyboard = Keyboard.Numeric,
                    ClassId = person.Id.ToString()
                };

                entries.Add(entry);
                stack.Children.Add(entry);

                Button removeBtn = new Button
                {
                    Image = (FileImageSource)FileImageSource.FromFile("ic_remove.png"),
                    CommandParameter = person.Id,
                    BackgroundColor = Color.Transparent,
                    WidthRequest = 24,
                    HeightRequest = 24
                };

                removeBtn.SetBinding(Button.CommandProperty, nameof(AddGroupReceiptPageViewModel.CRemoveFriend));

                stack.Children.Add(removeBtn);

                ViewCell cell = new ViewCell { View = stack, Height = 50 };
                cells.Add(cell);
            }
            return cells;
        }
    }
}

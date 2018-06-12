using System.Windows.Input;
using Xamarin.Forms;
using DearManager.Model;
using Newtonsoft.Json;

namespace DearManager.ViewModel
{
    class ProfileViewModel: BaseViewModel
    {
        string _userId;
        string _userName;
        string _userNickName;
        Person _friend;

        public ICommand CRemoveFriend { get; private set; }
        public string UserId
        {
            get => _userId;
            set => SetValue(ref _userId, value);
        }
        public string UserName
        {
            get => _userName;
            set => SetValue(ref _userName, value);
        }
        public string UserNickName
        {
            get => _userNickName;
            set => SetValue(ref _userNickName, value);
        }

        //자신의 프로필을 보려는 경우
        public ProfileViewModel()
        {
            CRemoveFriend = new Command<uint>(RemoveFriendAsync);
        }

        //친구 프로필을 보려는 경우
        public ProfileViewModel(Person friend): this()
        {
            _friend = friend;
        }

        private async void RemoveFriendAsync(uint id)
        {
            bool accept = await PageService.Default.DisplayAlert(AppResources.txtProfileVMDialogTitle, AppResources.txtProfileVMDialogCaption, 
                AppResources.txtProfileVMDialogYesButton, AppResources.txtProfileVMDialogNoButton);
            
            if (accept)
            {
                uint myId = (uint)Application.Current.Properties[App.userId];
                HttpUsersFriendsAccess friendsHttp = new HttpUsersFriendsAccess(myId);
                var temp = new { my_id = myId, friend_id = id };
                var data = JsonConvert.SerializeObject(temp);
                await friendsHttp.HttpDeleteAsync(data);
                await PageService.Default.PopAsync();
            }
        }

        //유저 정보 불러오기
        public void GetUserInfo()
        {
            if (_friend != null)
            {
                SetUserInfo(_friend.Id.ToString(), _friend.Name, _friend.NickName);
            }
            else if (Application.Current.Properties.ContainsKey(App.userInfo))
            {
                SetUserInfo(
                    userId: ((uint)Application.Current.Properties[App.userId]).ToString(),
                    userName: (string)Application.Current.Properties[App.userName],
                    userNickName: (string)Application.Current.Properties[App.userNickName]);
            }
            else
            {
                SetUserInfo("UserId", "UserName", "UserNickName");
            }
        }

        //화면에 표시되야될 정보 입력
        private void SetUserInfo(string userId, string userName, string userNickName)
        {
            UserId = userId;
            UserName = userName;
            UserNickName = userNickName;
        }
        
    }
}

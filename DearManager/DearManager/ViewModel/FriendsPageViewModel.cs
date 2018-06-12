using DearManager.Model;
using DearManager.View;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class FriendsPageViewModel:BaseViewModel
    {
        private double _listHeight;
        private object _selected;
        private HttpUsersRequestAccess requestAccess;
        private HttpUsersFriendsAccess friendsAccess;
        bool _loadingVisible;
        bool _contentVisible;

        public bool LoadingVisible
        {
            get => _loadingVisible;
            set
            {
                SetValue(ref _loadingVisible, value);
            }
        }
        public bool ContentVisible
        {
            get => _contentVisible;
            set
            {
                SetValue(ref _contentVisible, value);
            }
        }
        public object SelectedFriend
        {
            get => _selected;
            set => SetValue(ref _selected, value);
        }
        public double ListHeight
        {
            get => _listHeight;
            set => SetValue(ref _listHeight, value);
        }
        
        public ObservableCollection<Person> Friends { get; set; } = new ObservableCollection<Person>();
        public List<Person> Requests { get; set; } = new List<Person>();

        public ICommand CCommit { get; private set; }
        public ICommand CReject { get; private set; }
        public ICommand CAddFriends { get; private set; }
        public ICommand CGotoFriendProfile { get; private set; }

        public FriendsPageViewModel()
        {
            CCommit = new Command<uint>(Commit);
            CReject = new Command<uint>(Reject);
            CAddFriends = new Command(AddFriend);
            CGotoFriendProfile = new Command(GotoFriendProfile);
            Friends.CollectionChanged += OnCollectionChanged;
            ListHeight = Friends.Count() * 50.0;
        }

        /// <summary>
        /// 친구 프로필 페이지로 이동
        /// </summary>
        private async void GotoFriendProfile()
        {
            (SelectedFriend as Person).ProfileImage = await PageService.Default.GetProfileImage((SelectedFriend as Person).Id);
            await PageService.Default.PushAsync(new ProfilePage(SelectedFriend as Person));
            SelectedFriend = null;
        }

        /// <summary>
        /// 친구 목록에 변화가 생겼을때 실행되는 함수/
        /// 친구 목록의 사람수 많큼 리스트의 높이 조절
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ListHeight = (sender as ObservableCollection<Person>).Count() * 50.0;
        }

        /// <summary>
        /// 친구 추가 버튼
        /// </summary>
        /// <param name="obj"></param>
        private void AddFriend(object obj)
        {
            PopupNavigation.Instance.PushAsync(new SearchFriendPage(Friends.ToList()));
        }

        /// <summary>
        /// 친구수락 거절 버튼 누를시 실행되는 함수
        /// </summary>
        /// <param name="obj">친구 요청을 보낸 사람의 id값</param>
        private async void Reject(uint obj)
        {
            var temp = new { my_id = (uint)Application.Current.Properties[App.userId], friend_id = obj };
            var data = JsonConvert.SerializeObject(temp);
            await requestAccess.HttpDeleteAsync(data);
            MessagingCenter.Send(this, MessangerKey.FriendRequestRemove, obj);
        }

        /// <summary>
        /// 친구수락 승낙 버튼 누를시 실행되는 함수
        /// </summary>
        /// <param name="obj">친구 요청을 보낸 사람의 id값</param>
        private async void Commit(uint obj)
        {
            var temp = new { my_id = (uint)Application.Current.Properties[App.userId], friend_id = obj};
            var data = JsonConvert.SerializeObject(temp);
            await friendsAccess.HttpPostAsync(data);
            await requestAccess.HttpDeleteAsync(data);
            await UpdateFriends();
            MessagingCenter.Send(this, MessangerKey.FriendRequestRemove, obj);
        }

        private async Task UpdateFriends()
        {
            if (!Application.Current.Properties.ContainsKey(App.userId))
                return;
            friendsAccess = new HttpUsersFriendsAccess((uint)Application.Current.Properties[App.userId]);
            Friends.Clear();
            var getFriends = await friendsAccess.HttpGetAsync();

            List<Task> tasks = new List<Task>();
            foreach (var person in getFriends)
            {
                tasks.Add(Task.Run(async () =>
                {
                    person.ProfileImage = await PageService.Default.GetProfileImage(person.Id);
                }));
            }
            await Task.WhenAll(tasks);

            foreach (Person friend in getFriends.OrderBy(p => p.Name))
            {
                Friends.Add(friend);
            }
        }

        private async Task UpdateRequests()
        {
            if (!Application.Current.Properties.ContainsKey(App.userId))
                return;
            requestAccess = new HttpUsersRequestAccess((uint)Application.Current.Properties[App.userId]);
            Requests.Clear();
            var getRequest = await requestAccess.HttpGetAsync();
            foreach (Person requestSender in getRequest.OrderBy(s => s.Name))
            {
                Requests.Add(requestSender);
            }
        }

        public async Task UpdateData()
        {
            // 로딩 시작
            LoadingVisible = true;
            ContentVisible = false;

            var t = UpdateFriends();
            var t2 = UpdateRequests();
            await Task.WhenAll(t, t2);
            
            // 로딩 끝
            LoadingVisible = false;
            ContentVisible = true;
        }
    }
}

using DearManager.Model;
using DearManager.View;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class AddGroupPageViewModel
    {
        public List<Person> People { get; set; }
        public string GroupName { get; set; }
        public ICommand CGotoChoiceFriendsPage { get; private set; }
        public ICommand CRemoveFriend { get; private set; }
        public ICommand CCommit { get; private set; }
        public ICommand CCancel { get; private set; }

        public AddGroupPageViewModel()
        {
            People = new List<Person>();
            CGotoChoiceFriendsPage = new Command(GotoChoiceFriendsPage);
            CCommit = new Command(Commit);
            CCancel = new Command(Cancel);
            CRemoveFriend = new Command<uint>(RemoveFriend);
        }

        private void RemoveFriend(uint id)
        {
            People.RemoveAll(p => p.Id == id);
        }

        /// <summary>
        /// 친구 선택 페이지로 이동
        /// </summary>
        private async void GotoChoiceFriendsPage()
        {
            MessagingCenter.Subscribe<ChoiceFriendsPageViewModel, List<Person>>(this, MessangerKey.FriendAdds, ChoiceFriendsCallback);
            await PageService.Default.PushAsync(new ChoiceFriendsPage(JsonConvert.DeserializeObject<Person>((string)Application.Current.Properties[App.userInfo])));
        }

        /// <summary>
        /// 친구 선택 페이지에서 확인을 누르면 실행되는 콜백함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="people"></param>
        void ChoiceFriendsCallback(ChoiceFriendsPageViewModel sender, List<Person> people)
        {
            MessagingCenter.Unsubscribe<ChoiceFriendsPageViewModel, List<Person>>(this, MessangerKey.FriendAdds);

            // 선택된 친구들 추가
            foreach (var person in people)
            {
                People.Add(person);
            }
            //겹치는 목록 삭제
            People = People.Distinct(new PersonEqualityComparer()).ToList();

            foreach (var person in people)
            {
                System.Diagnostics.Debug.WriteLine(person.Name);
            }
            
            MessagingCenter.Send(this, MessangerKey.PeopleListChanged);
        }
        
        /// <summary>
        /// 확인 버튼을 누르면 db에 그룹 목록 추가
        /// </summary>
        private async void Commit()
        {
            await PopupNavigation.Instance.PushAsync(new ProcessPage(), false);
            //그룹 추가
            HttpGroupsAccess groupsAccess = new HttpGroupsAccess();
            var temp = new { name = GroupName, admin_id = (uint)Application.Current.Properties[App.userId] };
            var data = JsonConvert.SerializeObject(temp);
            var currentGroup = await groupsAccess.HttpPostAsync(data);
            HttpGroupsMemberAccess memberAccess = new HttpGroupsMemberAccess(currentGroup.Id);

            List<Task> t = new List<Task>(); // 태스크 저장소
            //그룹에 멤버 추가

            //나 자신 멤버에 추가
            var person = new { person_id = (uint)Application.Current.Properties[App.userId], group_id = currentGroup.Id };
            var tempData = JsonConvert.SerializeObject(person);
            t.Add(memberAccess.HttpPostAsync(tempData));

            //선택된 친구들 멤버에 추가
            foreach (Person p in People)
            {
                //Json으로 직렬화 하기위한 임시 데이터
                var tempFriend = new { person_id = p.Id, group_id = currentGroup.Id };
                var jsonFriend = JsonConvert.SerializeObject(tempFriend);
                t.Add(memberAccess.HttpPostAsync(jsonFriend));
            }

            await Task.WhenAll(t);
            await PopupNavigation.Instance.PopAsync(false);
            await PageService.Default.PopAsync();
        }

        /// <summary>
        /// 취소 버튼을 누르면 페이지 닫기
        /// </summary>
        private async void Cancel()
        {
            await PageService.Default.PopAsync();
        }
    }
}

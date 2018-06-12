using DearManager.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class ChoiceFriendsPageViewModel: BaseViewModel
    {
        Group _group;
        Person _person;
        Person _selectedItem;
        List<Person> selectedPeople = new List<Person>();

        public ObservableCollection<Person> Friends { get; set; }
        public ICommand CSelectItem { get; private set; }
        public ICommand CAddButtonClick { get; private set; }
        public Person SelectedItem
        {
            get => _selectedItem;
            set => SetValue(ref _selectedItem, value);
        }

        public ChoiceFriendsPageViewModel()
        {
            Friends = new ObservableCollection<Person>();

            CSelectItem = new Command(CSelectItemMethod);
            CAddButtonClick = new Command(CAddButtonClickMethod);
        }

        /// <summary>
        /// 그룹 멤버를 불러올때의 생성자
        /// </summary>
        /// <param name="group"></param>
        public ChoiceFriendsPageViewModel(Group group): this()
        {
            _group = group;
            _person = null;

            UpdateGroupMember();
        }

        /// <summary>
        /// 친구 목록을 불러올때의 생성자
        /// </summary>
        /// <param name="person"></param>
        public ChoiceFriendsPageViewModel(Person person) : this()
        {
            _person = person;
            _group = null;

            UpdateFriendsList();
        }

        /// <summary>
        /// 셀을 선택하면 실행되는 함수
        /// </summary>
        private void CSelectItemMethod()
        {
            //선택된 셀을 가져옴
            var item = SelectedItem;
            if (item == null)
                return;
            if (selectedPeople.Contains(item)) //체크를 풀면 현재 리스트에서 삭제
                selectedPeople.Remove(item);
            else //체크를 하면 리스트에 추가
                selectedPeople.Add(item);
            SelectedItem = null; //선택된 셀 없는 상태로 되돌리기
        }

        /// <summary>
        /// 툴바 확인 버튼을 누르면 실행되는 함수
        /// </summary>
        private async void CAddButtonClickMethod()
        {
            MessagingCenter.Send(this, MessangerKey.FriendAdds, selectedPeople);
            await PageService.Default.PopAsync();
            selectedPeople = null;
        }

        /// <summary>
        /// 친구목록 불러오기
        /// </summary>
        private async void UpdateFriendsList()
        {
            Friends.Clear();
            HttpUsersFriendsAccess friendsAccess = new HttpUsersFriendsAccess(_person.Id);
            var getFriends = await friendsAccess.HttpGetAsync();
            foreach (Person firend in getFriends.OrderBy(p => p.Name))
            {
                Friends.Add(firend);
            }
        }


        /// <summary>
        /// 멤버 목록 불러오기
        /// </summary>
        private async void UpdateGroupMember()
        {
            Friends.Clear();
            HttpGroupsMemberAccess memberAccess = new HttpGroupsMemberAccess(_group.Id);
            var getMember = await memberAccess.HttpGetAsync();
            foreach (Person firend in getMember.OrderBy(p => p.Name))
            {
                Friends.Add(firend);
            }
        }
    }

    
}

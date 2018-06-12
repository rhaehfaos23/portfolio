using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DearManager.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class SearchFriendViewModel: BaseViewModel
    {
        private List<Person> _friends;
        public string FriendId { get; set; }
        
        public SearchFriendViewModel(List<Person> friends)
        {
            _friends = friends;
        }


        /// <summary>
        /// 친구 찾기
        /// 友達を探す
        /// </summary>
        public async void SearchFrined()
        {
            //숫자가 입력되있는지 확인
            //入力が正しいのか確認
            if (uint.TryParse(FriendId, out uint userId))
            {
                //입력된 아이디가 자기 자신인지 확인
                //入力されたIDが自分自身なのか確認
                if (userId == (uint)Application.Current.Properties[App.userId])
                {
                    await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMErrorTitle, 
                        AppResources.txtSearchFriendVMMeCaption, AppResources.txtButtonOK);
                    return;
                }

                //현재 추가되있는 친구인지 확인
                //現在友たちリストにあるのか確認
                if (_friends.Where(f => f.Id == userId).Count() > 0)
                {
                    await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMErrorTitle, 
                        AppResources.txtSearchFriendVMAlreadyCaption, AppResources.txtButtonOK);
                    return;
                }


                HttpUsersInfoAccesser usersinfo = new HttpUsersInfoAccesser(userId);
                var friend = await usersinfo.HttpGetAsync();

                //친구 정보가 존재하지 않을 경우
                //探す友達の情報が存在しない場合
                if (string.IsNullOrEmpty(friend.Name))
                {
                    await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMErrorTitle, 
                        AppResources.txtSearchFriendVMWrongCaption, AppResources.txtButtonOK);
                    return;
                }

                //추가하려는 친구가 맞는지 확인
                //追加したい人があっているのか確認
                bool accept = await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMCheckTitle,
                    AppResources.txtSearchFriendVMCheckCaption1 + friend.Name + AppResources.txtSearchFriendVMCheckCaption2, 
                    AppResources.txtButtonYes, AppResources.txtButtonNo);
                if (accept == true)
                {
                    uint myId = (uint)Application.Current.Properties[App.userId];
                    HttpUsersRequestAccess request = new HttpUsersRequestAccess(myId);

                    var tempData = new { my_id = myId, friend_id = userId };
                    var jsonData = JsonConvert.SerializeObject(tempData);
                    await request.HttpPostAsync(jsonData);
                    await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMSuccessTitle,
                        AppResources.txtSearchFriendVMSuccessCaption, AppResources.txtButtonOK);
                    await PopupNavigation.Instance.PopAsync();
                }
            }
            else
            {
                await PageService.Default.DisplayAlert(AppResources.txtSearchFriendVMInputTitle,
                    AppResources.txtSearchFriendVMInputCaption, AppResources.txtButtonOK);
            }
        }
    }
}

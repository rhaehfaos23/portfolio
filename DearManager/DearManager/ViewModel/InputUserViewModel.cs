using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class InputUserViewModel
    {
        public string UserName { get; set; }
        public string UserNickName { get; set; }

        public ICommand CCommit { get; private set; }

        public InputUserViewModel()
        {
            CCommit = new Command(OnClickCommitButton);
        }

        private async void OnClickCommitButton()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserNickName))
            {
                await PageService.Default.DisplayAlert(AppResources.txtInputUserVMBlankTitle, 
                    AppResources.txtInputUserVMBlankCaption, AppResources.txtButtonOK);
                return;
            }
            var userHttp = new Model.HttpUsersInfoAccesser(nickName: UserNickName.ToString());
            var isAlready = await userHttp.HttpGetIsAlready(UserNickName.ToString());
            if (isAlready)
            {
                await PageService.Default.DisplayAlert(AppResources.txtInputUserVMDuplicatedTitle, 
                    AppResources.txtInputUserVMDuplicatedCaption, AppResources.txtButtonOK);
                return;
            }
            await SaveUserInfo();
            await PageService.Default.PopModalAsync();
            await PopupNavigation.Instance.PushAsync(new View.TutorialPage());
        }

        private async Task SaveUserInfo()
        {
            Model.Person user = new Model.Person { Name = UserName, NickName = UserNickName };
            var userJson = JsonConvert.SerializeObject(user);
            var userhttp = new Model.HttpUsersAccess();
            await userhttp.HttpPostAsync(userJson);
            Application.Current.Properties[App.isFirst] = false.ToString();
            await Application.Current.SavePropertiesAsync();
        }
    }
}

using DearManager.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using DearManager.Model;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using System.ComponentModel;

namespace DearManager.ViewModel
{
    class GroupsPageViewModel: BaseViewModel
    {
        Group _selectedItem;
        string _searchText;
        bool _loadingVisible;
        bool _contentVisible;

        public ICommand GoReceiptView { get; private set; }
        public ICommand SearchItem { get; private set; }
        public ICommand AddGroups { get; private set; }
        public ObservableCollection<Group> Groups { get; private set; }
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
            set {
                SetValue(ref _contentVisible, value);
            }
        }
        public string SearchText
        {
            get => _searchText;
            set => SetValue(ref _searchText, value);
        }
        public Group SelectedItem
        {
            get => _selectedItem; 
            set => SetValue(ref _selectedItem, value);
        }
        
        public GroupsPageViewModel()
        {

            Groups = new ObservableCollection<Group>();
            GoReceiptView = new Command(OnClickItem);
            AddGroups = new Command(GotoAddGroupPage);
        }
        
        /// <summary>
        /// 영수증 페이지로 이동
        /// </summary>
        async void OnClickItem()
        {
            await PageService.Default.PushAsync(new GroupReceiptPage(SelectedItem));
            
            SelectedItem = null;
        }

        /// <summary>
        /// 그룹 추가 페이지 이동
        /// </summary>
        async void GotoAddGroupPage()
        {
            await PageService.Default.PushAsync(new AddGroupPage());
        }
        
        /// <summary>
        /// 검색창 텍스트 변경시 그룹 목록 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listView"></param>
        public void OnSearchBarTextChanged(ListView listView)
        {
            listView.BeginRefresh();
            if (string.IsNullOrEmpty(SearchText))
            {
                listView.ItemsSource = Groups;
            }
            else
            {
                listView.ItemsSource = 
                    Groups.Where(group => group.Name.ToLower().StartsWith(SearchText.ToLower())).ToList();
            }
            listView.EndRefresh();
        }

        public async void PullGroupList()
        {
            // 로딩 시작
            LoadingVisible = true;
            ContentVisible = false;

            if (!Application.Current.Properties.ContainsKey(App.userId))
                return;
            HttpUsersGroupsAccess groupsAccess = new HttpUsersGroupsAccess((uint)Application.Current.Properties[App.userId]);

            var result = await groupsAccess.HttpGetAsync();
            Groups.Clear();
            foreach (var group in result.OrderBy(group => group.Name))
            {
                Groups.Add(group);
            }

            // 로딩 끝
            LoadingVisible = false;
            ContentVisible = true;
        }
    }
}

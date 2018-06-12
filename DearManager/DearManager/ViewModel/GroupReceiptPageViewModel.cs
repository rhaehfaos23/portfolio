using DearManager.Model;
using DearManager.View;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace DearManager.ViewModel
{
    class GroupReceiptPageViewModel: BaseViewModel
    {
        #region private Field
        private bool isMemberListFolded = false;
        private bool isDetailListFolded = false;
        private double _memberHeight;
        private double _detailHeight;
        private Group _group;
        private BalanceInfo _balanceInfo;
        private int page = 0;
        #endregion

        #region private Property
        private double MemberInitHeight
        {
            get => BalanceInfos.Count() * MemberRowHeight;
        }
        private double DetailInitHeight
        {
            get => (Receipts.Count() * DetailRowHeight) + (ReceiptGroups.Count() * DetailHeaderHeight);
        }
        #endregion

        #region Public Property
        public BalanceInfo SelectedBalanceInfo
        {
            get => _balanceInfo;
            set => SetValue(ref _balanceInfo, value);
        }
        public double MemberHeight
        {
            get => _memberHeight;
            set => SetValue(ref _memberHeight, value);
        }

        public double DetailHeight
        {
            get => _detailHeight;
            set => SetValue(ref _detailHeight, value);
        }
        
        public double MemberRowHeight { get => 60; }
        public double DetailRowHeight { get => 30; }
        public double DetailHeaderHeight { get => 30; }
        public ObservableCollection<BalanceInfo> BalanceInfos { get; private set; }
        public ObservableCollection<ReceiptGroup> ReceiptGroups { get; set; }
        public ObservableCollection<Receipt> Receipts { get; set; }
        public ICommand CGoAddGroupReceiptPage { get; private set; }
        public ICommand CGoRepaymentPage { get; private set; }
        public ICommand CAppendReceipts { get; private set; }
        #endregion

        public GroupReceiptPageViewModel(Group group)
        {
            _group = group;
            BalanceInfos = new ObservableCollection<BalanceInfo>();
            Receipts = new ObservableCollection<Receipt>();
            ReceiptGroups = new ObservableCollection<ReceiptGroup>();

            CGoAddGroupReceiptPage = new Command(async () =>
            {
                if (_group.AdminId == (uint)Application.Current.Properties[App.userId])
                    await PageService.Default.PushAsync(new AddGroupReceiptPage(_group));
                else
                    await PageService.Default.DisplayAlert(AppResources.txtGroupReceiptPageVMAdminTitle,
                        AppResources.txtGroupReceiptPageVMAdminCaption, AppResources.txtButtonOK);
            });
            CGoRepaymentPage = new Command(async () =>
            {
                if (_group.AdminId != (uint)Application.Current.Properties[App.userId])
                {
                    await PageService.Default.DisplayAlert(AppResources.txtGroupReceiptPageVMAdminTitle,
                        AppResources.txtGroupReceiptPageVMAdminCaption, AppResources.txtButtonOK);
                    return;
                }
                    
                MessagingCenter.Subscribe<RepaymentPageViewModel>(this, MessangerKey.RepaymentCommit, RepaymentCommitCallback);
                await PopupNavigation.Instance.PushAsync(new RepaymentPage(_group, SelectedBalanceInfo));
                SelectedBalanceInfo = null;
            });
            CAppendReceipts = new Command(AppendReceipts);
        }

        #region Animation
        public Animation DetailFold()
        {
            Animation animation;
            if (isDetailListFolded)
                animation = new Animation(h => DetailHeight = h, 0, DetailInitHeight);
            else
                animation = new Animation(h => DetailHeight = h, DetailInitHeight, 0);
            isDetailListFolded = isDetailListFolded == true ? false : true;
            return animation;
        }

        public Animation MemberFold()
        {
            Animation animation;
            if (isMemberListFolded)
                animation = new Animation(h => MemberHeight = h, 0, MemberInitHeight);
            else
                animation = new Animation(h => MemberHeight = h, MemberInitHeight, 0);
            isMemberListFolded = isMemberListFolded == true ? false : true;
            return animation;
        }
        #endregion

        /// <summary>
        /// 상환 창이 닫히면 데이터 업데이트
        /// </summary>
        /// <param name="obj"></param>
        private void RepaymentCommitCallback(RepaymentPageViewModel obj)
        {
            MessagingCenter.Unsubscribe<RepaymentPageViewModel>(this, MessangerKey.RepaymentCommit);
            UpdateData();
        }

        /// <summary>
        /// 추가 영수증 불러오기
        /// </summary>
        private async void AppendReceipts()
        {
            page++;
            HttpGroupsReceiptsAccess receiptsAccess = new HttpGroupsReceiptsAccess(_group.Id, page, 20);
            var getResult = await receiptsAccess.HttpGetAsync();
            foreach (var receipt in getResult.OrderByDescending(r => r.DateTime))
            {
                Receipts.Add(receipt);
            }
        }

        /// <summary>
        /// 데이터 업데이트
        /// </summary>
        public async void UpdateData()
        {
            await UpdateBalanceInfo();
            await UpdateReceiptsInfo();
        }

        /// <summary>
        /// 영수증 정보 업데이트
        /// </summary>
        private async Task UpdateReceiptsInfo()
        {
            Receipts.Clear();
            ReceiptGroups.Clear();
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ko-KR", false);
            HttpGroupsReceiptsAccess receiptsAccess = new HttpGroupsReceiptsAccess(_group.Id, page, 20);
            var getResult = await receiptsAccess.HttpGetAsync();
            var grouped = getResult.GroupBy(r => new { Title = r.Tag, Date = r.DateTime }).OrderByDescending(x => x.Key.Date);
            foreach (var group in grouped)
            {
                var groupReceipt = new ReceiptGroup(group.Key.Title, group.Key.Date);
                foreach (var receipt in group)
                {
                    groupReceipt.Add(receipt);
                }
                ReceiptGroups.Add(groupReceipt);
            }

            foreach (var receipt in getResult.OrderByDescending(r => r.DateTime))
            {
                Receipts.Add(receipt);
            }
            DetailHeight = DetailInitHeight;
        }

        /// <summary>
        /// 멤버 잔액 정보 업데이트
        /// </summary>
        private async Task UpdateBalanceInfo()
        {
            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ko-KR", false);
            BalanceInfos.Clear();
            HttpGroupsBalanceAccess balanceAccess = new HttpGroupsBalanceAccess(_group.Id);
            var getResult = await balanceAccess.HttpGetAsync();
            List<Task> tasks = new List<Task>();
            foreach (var balance in getResult)
            {
                tasks.Add(Task.Run(async () =>
                {
                    balance.ProfileImage = await PageService.Default.GetProfileImage(balance.Id);
                }));
            }
            await Task.WhenAll(tasks);
            foreach (var balance in getResult.OrderBy(b => b.Name))
            {
                BalanceInfos.Add(balance);
            }
            MemberHeight = MemberInitHeight;
        }
    }
}

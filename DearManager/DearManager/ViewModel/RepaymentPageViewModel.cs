using DearManager.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    class RepaymentPageViewModel:BaseViewModel
    {
        private string _userName;
        private string _remainAmount;
        private ImageSource _profileImage;
        private decimal realRepayment;
        private bool _isall;
        private FileImageSource _partitionBtnImg;
        private FileImageSource _allBtnImg;
        private FileImageSource[] fImgs = new FileImageSource[] { (FileImageSource)ImageSource.FromFile("ic_checkbox_checked.png"), (FileImageSource)ImageSource.FromFile("ic_checkbox_unchecked.png") };

        private bool IsAll
        {
            get => _isall;
            set
            {
                if (_isall == value)
                    return;
                _isall = value;
                PartitionBtnImg = !_isall ? fImgs[0] : fImgs[1];
                AllBtnImg = _isall ? fImgs[0] : fImgs[1];
            }
        }

        public FileImageSource PartitionBtnImg
        {
            get => _partitionBtnImg;
            set => SetValue(ref _partitionBtnImg, value);
        }

        public FileImageSource AllBtnImg
        {
            get => _allBtnImg;
            set => SetValue(ref _allBtnImg, value);
        }

        public string UserName
        {
            get => _userName;
            set => SetValue(ref _userName, value);
        }
        
        public string Repayment { get; set; }

        public string RemainAmount
        {
            get => _remainAmount;
            set => SetValue(ref _remainAmount, value);
        }
        public ImageSource ProfileImage
        {
            get => _profileImage;
            set => SetValue(ref _profileImage, value);
        }

        public ICommand OnCancelBtn { get; private set; }
        public ICommand OnRepaymentBtn { get; private set; }
        public ICommand OnClickPartitionBtn { get; private set; }
        public ICommand OnClickAllBtn { get; private set; }


        public RepaymentPageViewModel(Group group, BalanceInfo balance)
        {
            UserName = balance.NickName;
            RemainAmount = balance.Balance.ToString();
            ProfileImage = balance.ProfileImage;
            IsAll = true;

            Task.Run(async () =>
            {
                ProfileImage = await PageService.Default.GetProfileImage(balance.Id);
            });

            OnClickAllBtn = new Command(OnClickAllRepayment);

            OnClickPartitionBtn = new Command(OnClickPartitionRepayment);

            OnCancelBtn = new Command(() =>
            {
                PopupNavigation.Instance.PopAsync(false);
            });


            //상환 버튼 클릭
            //返済確認ボタンをクリックした場合
            OnRepaymentBtn = new Command(async () =>
            {
                //전체상환인지 부분상환인지 판단
                //一括返済または一部返済なのか確認
                if (IsAll)
                    realRepayment = decimal.Parse(RemainAmount);
                else
                {
                    if (decimal.TryParse(Repayment, out decimal result))
                    {
                        //남아 있는 금액보다 높은 금액이 입력된 경우
                        //残った金額より高い金額を返済しようとする場合
                        if (result > decimal.Parse(RemainAmount))
                        {
                            var accept = await PageService.Default.DisplayAlert(AppResources.txtRepaymentPageVMOverTitle,
                                AppResources.txtRepaymentPageVMOverCaption, 
                                AppResources.txtButtonOK, AppResources.txtButtonCancel);
                            if (!accept)
                            {
                                return;
                            }
                        }
                        realRepayment = result;
                    }
                    else
                    {
                        await PageService.Default.DisplayAlert(AppResources.txtRepaymentPageVMNoMoneyTitle, 
                            AppResources.txtRepaymentPageVMNoMoneyCaption, AppResources.txtButtonOK);
                        return;
                    }
                }

                //상환 금액이 맞는지 확인
                //返済する金額が正しいのか確認
                var confirm = await PageService.Default.DisplayAlert(AppResources.txtRepaymentPageVMNoMoneyCaption,
                    $"{realRepayment:C}" + AppResources.txtRepaymentPageVMConfirmCaption, AppResources.txtButtonOK, AppResources.txtButtonCancel);
                if (confirm)
                {
                    HttpGroupsReceiptsAccess receiptsAccess = new HttpGroupsReceiptsAccess(group.Id);
                    var temp =
                    new { person_id = balance.Id, group_id = group.Id, money = -Convert.ToDecimal(realRepayment), tag = AppResources.txtRepaymentPageVMRepaymentText };
                    var data = JsonConvert.SerializeObject(temp);
                    await receiptsAccess.HttpPostAsync(data);
                    await PopupNavigation.Instance.PopAsync(false);
                    MessagingCenter.Send(this, MessangerKey.RepaymentCommit);
                }
            });
        }

        private void OnClickPartitionRepayment()
        {
            IsAll = false;
        }

        private void OnClickAllRepayment()
        {
            IsAll = true;
        }
    }
}

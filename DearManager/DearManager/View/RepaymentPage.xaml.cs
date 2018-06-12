using DearManager.Model;
using DearManager.ViewModel;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RepaymentPage : Rg.Plugins.Popup.Pages.PopupPage
	{
        public RepaymentPage(Group group, BalanceInfo balance)
		{
			InitializeComponent ();

            BindingContext = new RepaymentPageViewModel(group, balance);
		}
    }
}
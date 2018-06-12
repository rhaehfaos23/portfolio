using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DearManager.ViewModel
{
    interface IServicePage
    {
        Task PushAsync(Page page);
        Task PopAsync();
        Task DisplayAlert(string title, string message, string cancel);
    }
}

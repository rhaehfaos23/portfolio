using DearManager.Extension;
using DearManager.Model;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            InitializeComponent();

            this.CurrentPage = Children[1];
            this.Title = this.CurrentPage.Title;
            this.CurrentPageChanged += OnCurrentPageChanged;            
        }

        void OnCurrentPageChanged(object sender, EventArgs e)
        {
            this.Title = this.CurrentPage.Title;
        }

    }
}
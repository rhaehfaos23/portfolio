using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.Custom
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FriendsCell : ViewCell
	{
        private bool isChecked = false;
        public static readonly BindableProperty NameProperty = 
            BindableProperty.Create(nameof(Name), typeof(string), typeof(FriendsCell), default(string), propertyChanged: NamePropertyChanged);

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
                
        public FriendsCell ()
		{
			InitializeComponent ();
            CheckedImage.Source = (FileImageSource)ImageSource.FromFile("ic_checkbox_unchecked.png");
            this.Tapped += OnCellTapped;
		}


        /// <summary>
        /// 체크박스 이미지 바꾸는 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCellTapped(object sender, EventArgs e)
        {
            
            if (isChecked)
            {
                //체크 안되게 표시
                CheckedImage.Source = (FileImageSource)ImageSource.FromFile("ic_checkbox_unchecked.png");
            }
            else
            {
                //체크 되게 표시
                CheckedImage.Source = (FileImageSource)ImageSource.FromFile("ic_checkbox_checked.png");
            }

            isChecked = isChecked == true ? false : true;
        }

        /// <summary>
        /// NameProperty가 변경되었다 !
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void NamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var source = bindable as FriendsCell;
            source.lblName.Text = (string)newValue;
        }
    }
}
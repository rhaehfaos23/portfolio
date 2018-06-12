using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DearManager.Custom
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FriendRequest : StackLayout
	{
        public uint SenderId { get; set; }
		public FriendRequest (uint senderId, string name, ICommand commit = null, ICommand reject = null)
		{
			InitializeComponent ();
            lblName.Text = name;
            SenderId = senderId;
            btnCommit.Command = commit;
            btnCommit.CommandParameter = senderId;
            btnReject.Command = reject;
            btnReject.CommandParameter = senderId;
		}
	}
}
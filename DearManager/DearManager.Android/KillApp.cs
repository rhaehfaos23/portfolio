using Android.OS;
using DearManager.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(CloseApp))]
namespace DearManager.Droid
{
    public class CloseApp : DearManager.Extension.IAndroidMethods
    {
        public void KillApp()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}
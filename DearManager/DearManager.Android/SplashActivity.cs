using Android.App;
using Android.OS;
using Android.Content;
using Android.Content.PM;

namespace DearManager.Droid
{
    [Activity(Label = "@string/app_name", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnStart()
        {
            base.OnStart();

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            Finish();
            return;
        }
    }
}


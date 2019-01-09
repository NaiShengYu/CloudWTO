using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using CN.Jpush.Android.Api;

namespace CloudWTO.Droid
{
    [Activity(Label = "CloudWTO.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            App.ScreenHeight = (int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density);
            App.ScreenWidth = (int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density);

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            InitJPush();
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
        /// <summary>
        /// init JPush
        /// </summary>
        private void InitJPush()
        {
            JPushInterface.SetDebugMode(true);
            JPushInterface.Init(Application.Context);
            JPushInterface.SetAlias(Application.Context, 0, "cloudwto_alias_test");

            BasicPushNotificationBuilder builder = new BasicPushNotificationBuilder(this);
            builder.StatusBarDrawable = Resource.Drawable.jpush_notification_icon;
            JPushInterface.SetPushNotificationBuilder(new Java.Lang.Integer(1), builder);
        }


        protected override void OnResume()
        {
            base.OnResume();

            JPushInterface.OnResume(this);
        }

        protected override void OnPause()
        {
            base.OnPause();

            JPushInterface.OnPause(this);
        }

    }
}

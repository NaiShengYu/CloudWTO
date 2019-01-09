using System;
using Android.App;
using CloudWTO.Droid.Servers;
using CloudWTO.Services;
using CN.Jpush.Android.Api;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseService))]
namespace CloudWTO.Droid.Servers
{
    class DatabaseService : IJpushSetAlias
    {
        public void setAliasWithName(string name)
        {
            JPushInterface.SetAlias(Application.Context, 0, name);
        }
    }
}

using System;
using CloudWTO.iOS.Servers;
using CloudWTO.Services;
using IOSNativeLibrary;

[assembly: Xamarin.Forms.Dependency(typeof(DatabaseService))]
namespace CloudWTO.iOS.Servers

{
    class DatabaseService: IJpushSetAlias
    {
        public void setAliasWithName(string name){
            JPUSHService.SetAlias("", (arg0, arg1, arg2) => { }, new nint());
        }
    }
}

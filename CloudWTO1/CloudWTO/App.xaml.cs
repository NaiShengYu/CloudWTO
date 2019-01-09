using System;

using Xamarin.Forms;
using CloudWTO.Views;
using Newtonsoft.Json;
using CloudWTO.Services;

namespace CloudWTO
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";
        public static double pid = 3.14;
        //public static string BaseURL = "http://dev.azuratech.com:20001/api/";
        public static string BaseURL = "https://www.cloudWTO.com.cn:446/api/";
        public static string token = "";
        public static string appName = "cloudWTO";

        public static string deviceToken = "";
        public App()
        {
            InitializeComponent();

            //if (UseMockDataStore)
            //    DependencyService.Register<MockDataStore>();
            //else
            //DependencyService.Register<CloudDataStore>();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new NavigationPage(new LoginPage());

            else
                MainPage = new NavigationPage(new LoginPage());




        }
         
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }


        //给后传递deviceToken
        public static async void pushDeviceTokenToServie()
        {
            string url = App.BaseURL + "AppAlarm/StartedApptoken";
            Console.WriteLine("url ==" + url);
            string parama = "apptoken=" + App.deviceToken + "&" + "apptype=" + 0;
            Console.WriteLine("parama ==" + parama);
            string result = await EasyWebRequest.sendAwaitePOSTHttpWebRequest(url, parama, "");
            Console.WriteLine("result ==" + result);
        }


    }
}

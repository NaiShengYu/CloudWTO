using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Linq;
using CloudWTO.Services;
using Xamarin.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using CloudWTO.Models;
using Xamarin.Auth;
using Plugin.Hud;
using System.ComponentModel;
#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

namespace CloudWTO.Views
{
    public partial class LoginPage : ContentPage
    {
        bool isSave = false;
        string result;

        void changeSave(object sender, System.EventArgs e)
        {
            isSave = !isSave;
            if(isSave ==true)
                saveBut.Image = "select_on";
            else
                saveBut.Image = "select_off";
        }


        void Handle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e);

        }


        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetBackButtonTitle(this, "");

            var account = AccountStore.Create().FindAccountsForService(App.appName).LastOrDefault();
            if (account != null)
            {
                name.Text = account.Username;
                password.Text = account.Properties["pwd"];
                isSave = true;
                saveBut.Image = "select_on";
            }

        }

        void Login(object sender, System.EventArgs e)
        {

            var account = name.Text;
            var pwd = password.Text;
            if (account == null || account.Length == 0)
            {
                DependencyService.Get<Sample.IToast>().ShortAlert("账号不能为空");
            }
            else if (pwd == null || pwd.Length == 0)
            {
                DependencyService.Get<Sample.IToast>().ShortAlert("密码不能为空");
            }
            else
            {
                //判断是否保存密码
                if (isSave == true)
                {
                    Account count = new Account
                    {
                        Username = account
                    };
                    count.Properties.Add("pwd", pwd);
                    AccountStore.Create().Save(count, App.appName);
                }
                else
                {
                    //循环删除所存的数据
                    IEnumerable<Account> outs = AccountStore.Create().FindAccountsForService(App.appName);
                    for (int i = 0; i < outs.Count(); i++)
                    {
                        AccountStore.Create().Delete(outs.ElementAt(i), App.appName);
                    }
                }
                try
                {
                    //请求登陆
                    reqLogin(account, pwd);

                }
                catch (Exception ex)
                {
                    var hud = CrossHud.Current;
                        hud.ShowError(message: ex.Message, timeout: new TimeSpan(0, 0, 2));
                }
            }

        }

        private void reqLogin(String account, String pwd)
        {
            CrossHud.Current.Show("登陆中。。。");
            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender1, e1) =>
            {
                string uri = "https://www.cloudWTO.com.cn/token";

                //string uri = "http://dev.azuratech.com:20000/token";
                string parama = "Password=" + pwd + "&" + "UserName=" + account + "&" + "grant_type=password";
                result = EasyWebRequest.sendPOSTHttpWebRequest(uri, parama);
            };
            wrk.RunWorkerCompleted += (sender1, e1) =>
            {
                if (result.Contains("400"))
                {
                    CrossHud.Current.Dismiss();
                    DependencyService.Get<Sample.IToast>().ShortAlert("账号密码有误");
                } else if(result.Contains("access_token")){
                    haveToken model = JsonConvert.DeserializeObject<haveToken>(result);
                    App.token = "Bearer" + " " + model.access_token;
                    //CrossHud.Current.Dismiss();
                    if (App.token != null)
                    {
                        try
                        {
                            reqUserInfo();
                            DependencyService.Get<IJpushSetAlias>().setAliasWithName(account);
                        }
                        catch (Exception ex)
                        {
                            DisplayAlert("错误提示", ex.Message, "OK");
                        }
                    }
                }
                else
                {
                    CrossHud.Current.Dismiss();
                    DependencyService.Get<Sample.IToast>().ShortAlert("网络异常");
                    //Navigation.PushAsync(new EnterpriseListPage());
                }
            };
            wrk.RunWorkerAsync();

        }
        private void reqUserInfo()
        {
            string userinfoResult = "";
            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender1, e1) =>
            {
                string url = App.BaseURL + "appflow/GetUserInfo";
                Console.WriteLine(url);
                userinfoResult = EasyWebRequest.sendGetHttpWebRequest(url);
            };
            wrk.RunWorkerCompleted += (sender1, e1) =>
            {
                userInfo userinfo = JsonConvert.DeserializeObject<userInfo>(userinfoResult);

                if(userinfo.userInf_roleid ==null){

                    return;
                }
                if (userinfo.userInf_roleid == "4")
                {//企业管理员直接进入企业
                    try
                    {
                        reqGoEnt();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("错误提示", ex.Message, "OK");
                    }

                }
                else
                {
                    CrossHud.Current.Dismiss();
                    Navigation.PushAsync(new EnterpriseListPage());
                    pushDeviceTokenToServie();
                }


            };
            wrk.RunWorkerAsync();
        }


        private void reqGoEnt()
        {
            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender1, e1) =>
            {
                string url2 = App.BaseURL + "AppEnterprise/GeteEnterpriseByKey?searchKey=" + "&pageIndex=0" + "&pageSize=30";
                result = EasyWebRequest.sendGetHttpWebRequest(url2);
            };
            wrk.RunWorkerCompleted += (sender1, e1) =>
            {
                var retv = JsonConvert.DeserializeObject<entlisttype>(result);
                var list = retv.Items;
                var busine = new BusinessDetailsPage(list[0],0);
                CrossHud.Current.Dismiss();
                Navigation.PushAsync(busine); 
                pushDeviceTokenToServie();

            };
            wrk.RunWorkerAsync();
        }
       
        private async void pushDeviceTokenToServie()
        {
            string url = App.BaseURL + "AppAlarm/LoginApptoken";
            Console.WriteLine("url ==" + url);
            string parama = "apptoken=" + App.deviceToken + "&" + "apptype=" + 0;
            Console.WriteLine("parama ==" + parama);
            string result = await EasyWebRequest.sendAwaitePOSTHttpWebRequest(url, parama,App.token);
            Console.WriteLine("result ==" + result);
        }  



        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(App.ScreenWidth == 320){
                juli.Height = 0;
            }
            aaa.ScrollToAsync(0, 65, false);

            //var a = App.ScreenWidth;
            //var h = a * 1435 / 2048;
//#if __IOS__  
//            row.Height = new GridLength(h);

//#endif
//#if __ANDROID__
//            row.Height = h;
//#endif
        }

        internal class haveToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string refresh_token { get; set; }
            public string userName { get; set; }
        }

        internal class userInfo
        {
            public string userInf_id { get; set; }
            public string userInf_userid { get; set; }
            public string userInf_username { get; set; }
            public string userInf_telephone { get; set; }
            public string userInf_email { get; set; }
            public string userInf_roleid { get; set; }
            public string userInf_rangeid { get; set; }
        }

        internal class entlisttype
        {
            public int Totals { get; set; }
            public List<Enttype> Items { get; set; }
        }

    }
}

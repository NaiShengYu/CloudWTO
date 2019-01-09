using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CloudWTO.Models;

using CloudWTO.Services;
using System.ComponentModel;

#if __IOS__
using UIKit;
using Foundation;
#endif
#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class AlertInfoPage : ContentPage
    {
        void OnParamTapped1(object sender, EventArgs args)
        {
            Console.WriteLine("传感参数");
            Navigation.PushAsync(new ParameterIconPage(_alertInfo.operId, businessNameLab.Text, _alertInfo.flowName, _alertInfo.operName,_alertInfo.unit));
        }
        void OnParamTapped2(object sender, EventArgs args)
        {
            Console.WriteLine("处理预案");
            Navigation.PushAsync(new PlanPage(_alertInfo.alertplan));
        }
        void OnParamTapped4(object sender, EventArgs args)
        {
            Console.WriteLine("推送记录");
            Navigation.PushAsync(new PushRecordListPage(businessNameLab.Text, _alertInfo.alarmid));

        }
        void OnParamTapped3(object sender, EventArgs args)
        {
            Console.WriteLine("拨打电话");

#if __IOS__
            if (UIApplication.SharedApplication.CanOpenUrl(NSUrl.FromString("tel://" + _alertInfo.phone)))
            {
                UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + _alertInfo.phone));
            }
#endif

        }



        AlertAndAlarmt _alert = null;
        AlertInfo _alertInfo = null;
        public AlertInfoPage(AlertAndAlarmt alert, string businessName)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _alert = alert;
            businessNameLab.Text = businessName;
            this.Title = "报警记录";


            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) =>
            {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) =>
            {
                this.BindingContext = list;
                _alertInfo = list;
                flowName.Text = _alertInfo.flowName + " " + _alertInfo.equipmentName + " " + _alertInfo.operName;
            };
            wrk.RunWorkerAsync();

        }

        AlertInfo list = null;
        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppAlarm/GetParamByAlarm?id=" + _alert.alarmid;
                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                list = JsonConvert.DeserializeObject<AlertInfo>(result);


            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}
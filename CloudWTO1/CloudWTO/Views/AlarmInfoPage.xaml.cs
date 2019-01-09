using System;
using System.Collections.Generic;
using CloudWTO.Models;
using Xamarin.Forms;
using System.ComponentModel;

using CloudWTO.Services;
#if __IOS__
using Foundation;
using UIKit;
#endif

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class AlarmInfoPage : ContentPage
    {
        void OnParamTapped1(object sender, EventArgs args)
        {
            Console.WriteLine("传感参数");
            Navigation.PushAsync(new ParameterIconPage(_alarmInfo.operId, businessNameLab.Text, _alarmInfo.flowName, _alarmInfo.operName,_alarmInfo.unit));
        }
        void OnParamTapped2(object sender, EventArgs args)
        {
            Console.WriteLine("处理预案");
            Navigation.PushAsync(new PlanPage(_alarmInfo.alarmplan));
        }
        void OnParamTapped4(object sender, EventArgs args)
        {
            Console.WriteLine("推送记录");
            Navigation.PushAsync(new PushRecordListPage(businessNameLab.Text, _alarm.alarmid));

        }
        void OnParamTapped3(object sender, EventArgs args)
        {
            Console.WriteLine("拨打电话");

#if __IOS__
            if (UIApplication.SharedApplication.CanOpenUrl(NSUrl.FromString("tel://" + _alarmInfo.phone)))
            {
                UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + _alarmInfo.phone));
            }
#endif

        }



        AlarmInfo _alarmInfo = null;
        AlertAndAlarmt _alarm = null;
        public AlarmInfoPage(AlertAndAlarmt alarm, string businessName)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _alarm = alarm;
            this.Title = "预警记录";
            businessNameLab.Text = businessName;

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                this.BindingContext = list;
                _alarmInfo = list;
                flowName.Text = _alarmInfo.flowName + " " + _alarmInfo.equipmentName + " " + _alarmInfo.operName;
            };
            wrk.RunWorkerAsync();


        }

        AlarmInfo list = null;
        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppAlarm/GetParamByWarn?id=" + _alarm.alarmid;
                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                list = JsonConvert.DeserializeObject<AlarmInfo>(result);

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }

    }
}

using System;
using System.Collections.Generic;

using Xamarin.Forms;
using CloudWTO.Models;
using CloudWTO.Services;
using System.ComponentModel;

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
    public partial class PushRecordListPage : ContentPage
    {
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            PushRecordType record = e.SelectedItem as PushRecordType;

            if (record == null)
            {
                return;
            }


#if __IOS__
            if (UIApplication.SharedApplication.CanOpenUrl(NSUrl.FromString("tel://" + record.pushPhone)))
            {
                UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + record.pushPhone));
            }
#endif

            //if (Device.RuntimePlatform == Device.iOS)
            //{
            //if (UIApplication.SharedApplication.CanOpenUrl(NSUrl.FromString("tel://" + record.pushPhone)))
            //{
            //    UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel://" + record.pushPhone));
            //}

            //}

            listView.SelectedItem = null;

        }

        string _alertOrAlarmId = null;
        public PushRecordListPage(string businessName, string alertOrAlarmId)
        {
            InitializeComponent();
            businessNameLab.Text = businessName;
            this.Title = "推送记录";
            _alertOrAlarmId = alertOrAlarmId;



            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                if (list !=null)
                {
                    if(list.Count > 0){
                        listView.ItemsSource = list;
                    }
                }
            };
            wrk.RunWorkerAsync();


        }

        List<PushRecordType> list = null;
        void makeData()
        {
            try
            {
                string url = App.BaseURL+"AppPushAlarm/GetPush?id=" + _alertOrAlarmId;
                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                list = JsonConvert.DeserializeObject<List<PushRecordType>>(result);

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
    }
}

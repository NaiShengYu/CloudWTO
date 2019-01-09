using System;
using System.Collections.Generic;
using CloudWTO.Models;
using Xamarin.Forms;
using CloudWTO.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif


namespace CloudWTO.Views
{
    public partial class ClassificationPage : ContentPage
    {

        //综合信息
        void Handle_Clicked1(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProcessInformationPage(_flow, _ent.name));

        }
        //RO运行参数
        void Handle_Clicked2(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ROEquipmentListPage(_flow, _ent.name));
        }

        //指标监控
        void Handle_Clicked3(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DeviceListPage(_flow, _ent.name));
        }
        //进入预警报警
        void Handle_Clicked4(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AlertListPage(_ent.id, _ent.name));
        }

        Enttype _ent = null;
        BusinessDetailFlow _flow = null;
        DateTime? lastBackKeyDownTime;

        public ClassificationPage(Enttype ent,BusinessDetailFlow flow)
        {
            InitializeComponent();
            zonghe.WidthRequest = (App.ScreenWidth - (App.ScreenWidth / 10 + 10)) / 2 - 10;
            zonghe.HeightRequest = zonghe.WidthRequest * 273 / 326;
            ro.WidthRequest = (App.ScreenWidth - (App.ScreenWidth / 10 + 10)) / 2 - 10;
            ro.HeightRequest = zonghe.WidthRequest * 273 / 326;
            zhibiao.WidthRequest = (App.ScreenWidth - (App.ScreenWidth / 10 + 10)) / 2 - 10;
            zhibiao.HeightRequest = zonghe.WidthRequest * 273 / 326;
            yujing.WidthRequest = (App.ScreenWidth - (App.ScreenWidth / 10 + 10)) / 2 - 10;
            yujing.HeightRequest = zonghe.WidthRequest * 273 / 326;

            yujing1.Margin = new Thickness(0, 5,App.ScreenWidth/2-yujing.WidthRequest , 0);
            baojing.Margin = new Thickness(0, 40, App.ScreenWidth / 2 - yujing.WidthRequest , 0);
            yujing11.Margin = new Thickness(0, 5, App.ScreenWidth / 2 - yujing.WidthRequest, 0);
            baojing1.Margin = new Thickness(0, 40, App.ScreenWidth / 2 - yujing.WidthRequest, 0);



            NavigationPage.SetBackButtonTitle(this, "");
          
            this.Title = ent.name;
            _ent = ent;
            _flow = flow;


            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                if(aaaa !=null){
                    for (int i = 0; i < aaaa.Count;i ++){
                        resutlDic dic = aaaa[i];

                        //报警
                        if(dic.counttype =="1"){
                            baojing1.Text = dic.count;
                        }
                        //预警
                        if (dic.counttype == "2")
                        {
                            yujing11.Text = dic.count;
                        }



                    }



                }


            };
            wrk.RunWorkerAsync();


        }

        List<resutlDic> aaaa = null;
        void makeData()
        {
            try
            {

                DateTime end = DateTime.Now;
                DateTime start = end.AddHours(-6);

                var endStr = end.ToString("yyyy-MM-dd HH:mm:ss");
                var startStr = start.ToString("yyyy-MM-dd");
                startStr = startStr + " 00:00:00";
                string url = App.BaseURL + "AppAlarm/GetAlarmCountByFlowid?id=" + _flow.flowid +"&sdt="+startStr +"&edt=" +endStr;
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                aaaa = JsonConvert.DeserializeObject<List<resutlDic>>(result);
              
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }




		protected override bool OnBackButtonPressed()
        {
            if (!lastBackKeyDownTime.HasValue || DateTime.Now - lastBackKeyDownTime.Value > new TimeSpan(0, 0, 2))
            {
                DependencyService.Get<Sample.IToast>().ShortAlert("再按一次退出程序");
                lastBackKeyDownTime = DateTime.Now;
            }
            else
            {
                System.Environment.Exit(0);
            }
            return true;
        }


        internal class resutlDic
        {
            public string counttype { get; set; }
            public string count { get; set; }
        }



    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using CloudWTO.Models;
using CloudWTO.Services;
using System.ComponentModel;
using Plugin.Hud;
#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class ROEquipmentInfoPage : ContentPage
    {

        BusinessDetailFlow _flow = null;
        ROEquipmentModel _equipment = null;
        public ROEquipmentInfoPage(BusinessDetailFlow flow,ROEquipmentModel equipment,string businness)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");

            _flow = flow;
            _equipment = equipment;
            this.Title = equipment.nameName + " - RO运行参数";
            businessNameLab.Text = businness;
            gongyiliucheng.Text = flow.flowname + "(" + flow.flowTypeName + ")";

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                CrossHud.Current.Show("请求中。。。");
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                listV.ItemsSource = dataList;
            };
            wrk.RunWorkerAsync();
        }

        ObservableCollection<FlowDesInfo> dataList = new ObservableCollection<FlowDesInfo>();

        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppFlow/GetFlowOparamter?id=" + _flow.flowid +"&equipmentid=" +_equipment.id;
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果前：" + result);
                result = result.Replace("null", "0");
                Console.WriteLine("请求结果后：" + result);

                dataList = JsonConvert.DeserializeObject<ObservableCollection<FlowDesInfo>>(result);
                CrossHud.Current.Dismiss();

            }
            catch (Exception ex)
            {
                CrossHud.Current.Dismiss();
                CrossHud.Current.ShowError(message: "网络错误", timeout: new TimeSpan(0, 0, 4));

            }
        }

    }
}

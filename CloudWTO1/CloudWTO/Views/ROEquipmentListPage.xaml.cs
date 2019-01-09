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
    public partial class ROEquipmentListPage : ContentPage
    {

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var device = e.SelectedItem as ROEquipmentModel;
            if (device == null)
                return;
         
            Navigation.PushAsync(new ROEquipmentInfoPage(_flow, device,_businness));

            listV.SelectedItem = null;
        }



        BusinessDetailFlow _flow = null;
        string _businness = "";
        public ROEquipmentListPage(BusinessDetailFlow flow, string businness)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            _businness = businness;
            _flow = flow;
            this.Title = flow.flowname + " - 设备列表";
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

        ObservableCollection<ROEquipmentModel> dataList = new ObservableCollection<ROEquipmentModel>();
        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppFlow/GetAllROEquipment?id=" + _flow.flowid;
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                dataList = JsonConvert.DeserializeObject<ObservableCollection<ROEquipmentModel>>(result);
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

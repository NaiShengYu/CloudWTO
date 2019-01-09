using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using CloudWTO.Models;
using CloudWTO.Services;
using System.ComponentModel;

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class DeviceListPage : ContentPage
    {


        bool haveMole = true;
        int pageIndex = 0;
        void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            DeviesModel item = e.Item as DeviesModel;
            if (item == dataList[dataList.Count - 1] && haveMole == true && item != null)
            {
                pageIndex += 1;
                addMoreData();
            }

        }
        //加载更多数据
        void addMoreData()
        {
            makeData();
        }



        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new AllParameterListsPage(_flow, businessNameLab.Text), false);

        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var device = e.SelectedItem as DeviesModel;
            if (device == null)
                return;

            if (device.type == "a")//阵列
                Navigation.PushAsync(new DeviceInfoPage(_flow, businessNameLab.Text, gongyiliucheng.Text, device.id, device.name));

            if (device.type == "e")//设备
                Navigation.PushAsync(new EquipmentParametersPage(_flow, businessNameLab.Text, gongyiliucheng.Text, device.id, device.name));

            if (device.type == "o")//参数
                Navigation.PushAsync(new ParameterIconPage(device.id, businessNameLab.Text, gongyiliucheng.Text, device.name,""));

            listV.SelectedItem = null;
        }

        BusinessDetailFlow _flow = null;
        public DeviceListPage(BusinessDetailFlow flow, string businness)
        {
            InitializeComponent();

            NavigationPage.SetBackButtonTitle(this, "");
            _flow = flow;
            this.Title = flow.flowname + " - 设备列表";
            businessNameLab.Text = businness;
            gongyiliucheng.Text = flow.flowname + "(" + flow.flowTypeName + ")";

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                listV.ItemsSource = dataList;

            };
            wrk.RunWorkerAsync();

        }


        ObservableCollection<DeviesModel> dataList = new ObservableCollection<DeviesModel>();
        void makeData()
        {

            try
            {
                string url = App.BaseURL + "AppFlow/GetAllById?id=" + _flow.flowid + "&pageIndex="
                                                                                                + pageIndex +
                                                                                                "&pageSize=20";
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                devisResult dev = JsonConvert.DeserializeObject<devisResult>(result);
                if (pageIndex == 0)
                {
                    dataList.Clear();
                }
                if (dev.Items.Count < 20)
                {
                    haveMole = false;
                }

                for (int i = 0; i < dev.Items.Count; i++)
                {
                    DeviesModel item = dev.Items[i];

                    dataList.Add(item);
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }






       



        internal class devisResult
        {
            public int Totals { get; set; }
            public List<DeviesModel> Items { get; set; }
        }




    }
}

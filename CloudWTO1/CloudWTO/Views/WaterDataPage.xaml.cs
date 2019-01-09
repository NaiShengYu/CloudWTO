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
    public partial class WaterDataPage : ContentPage
    {


    
      
        ObservableCollection<WaterDataModel> dataList = new ObservableCollection<WaterDataModel>();

     

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var parameter = e.SelectedItem as WaterDataModel;
            if (parameter == null)
                return;

            Navigation.PushAsync(new ParameterIconPage(parameter.id, businessNameLab.Text, gongyiliucheng.Text, parameter.name,parameter.unit));

            listV.SelectedItem = null;
        }
        BusinessDetailFlow _flow = null;
        public WaterDataPage(BusinessDetailFlow flow, string businness)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _flow = flow;
            this.Title = "原水水质参数";
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



        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppFlow/GetFlowOperById?id=" + _flow.flowid ;

                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                dataList = JsonConvert.DeserializeObject<ObservableCollection<WaterDataModel>>(result);
             


            }
            catch (Exception ex)
            {
                DisplayAlert("错误提示", ex.Message, "OK");
            }


        }

        internal class entlisttype
        {
            public int Totals { get; set; }
            public List<Parameter> Items { get; set; }
        }


    }
}

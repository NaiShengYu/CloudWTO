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
    public partial class AllParameterListsPage : ContentPage
    {


        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

            Console.WriteLine(searchB.Text);
            if (searchB.Text == null || searchB.Text.Length == 0)
            {
                pageIndex = 0;
                haveMole = true;
                this.makeData();
            }
        }

        bool haveMole = true;
        int pageIndex = 0;
        ObservableCollection<Parameter> dataList = new ObservableCollection<Parameter>();

        void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            Parameter item = e.Item as Parameter;

            if (item == dataList[dataList.Count - 1] && haveMole == true && item != null)
            {
                pageIndex += 1;

                BackgroundWorker wrk = new BackgroundWorker();
                wrk.DoWork += (a, ee) => {
                    makeData();
                };

                wrk.RunWorkerAsync();
            }

        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            pageIndex = 0;
            this.makeData();
        }
        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PopAsync(false);
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var parameter = e.SelectedItem as Parameter;
            if (parameter == null)
                return;

            Navigation.PushAsync(new ParameterIconPage(parameter.operparamId, businessNameLab.Text, gongyiliucheng.Text, parameter.operparamName,parameter.operparamUnit));

            listV.SelectedItem = null;
        }
        BusinessDetailFlow _flow = null;
        public AllParameterListsPage(BusinessDetailFlow flow, string businness)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _flow = flow;
            this.Title = "运行参数列表";
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
                string url = App.BaseURL + "AppFlow/GetDataByFlow?id=" + _flow.flowid + "&searchKey=" + searchB.Text + "&pageIndex=" + pageIndex + "&pageSize=20";

                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result );

                var list = JsonConvert.DeserializeObject<entlisttype>(result);

                if (pageIndex == 0)
                    dataList.Clear();

                for (int i = 0; i < list.Items.Count; i++)
                {
                    Parameter item = list.Items[i];
                    dataList.Add(item);
                }
                if (dataList.Count >= list.Totals)
                {
                    haveMole = false;
                }
                else
                {
                    haveMole = true;
                }


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

using System;
using System.Collections.Generic;

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
    public partial class EquipmentParametersPage : ContentPage
    {


        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var param = e.SelectedItem as Parameter;
            if (param == null)
                return;
            Navigation.PushAsync(new ParameterIconPage(param.operparamId, businessNameLab.Text, gongyiliucheng.Text, param.operparamName,param.operparamUnit));

            listV.SelectedItem = null;

        }

        BusinessDetailFlow _details = null;
        string _eqId = null;
        public EquipmentParametersPage(BusinessDetailFlow details, string businness, string gylc, string EID, string title)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _eqId = EID;
            _details = details;
            this.Title = title;
            businessNameLab.Text = businness;
            gongyiliucheng.Text = gylc;


            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {

                listV.ItemsSource = list;
            };
            wrk.RunWorkerAsync();
        }

        List<Parameter> list = null;
        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppOperParam/GetAllOper?id=" + _eqId + "&pageIndex=0&pageSize=30";
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);
                entlisttype ENT = JsonConvert.DeserializeObject<entlisttype>(result);
                list = ENT.Items;
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

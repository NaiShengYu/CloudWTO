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
    public partial class ProcessInformationPage : ContentPage
    {

        void Test_Clicked(object sender, System.EventArgs e)
        {
            Button but = sender as Button;
            Device.BeginInvokeOnMainThread(() => {
                if (but == startDateBut)
                    startDatePicker.Focus();
                if (but == endDateBut)
                    endDatePicker.Focus();
                if (but == startTimeBut)
                    starTimePicker.Focus();
                if (but == endTimeBut)
                    endTimePicker.Focus();
            });

        }

        //点击查询按钮加载
        void AddData(object sender, System.EventArgs e)
        {

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender1, e1) => {
                calculatedData();
            };
            wrk.RunWorkerCompleted += (sender1, e1) => {

                lab4.Text = calculated.inValue.Value.ToString("f2") +"吨";
                lab5.Text = calculated.outValue.Value.ToString("f2")+ "吨";

                if(calculated.outValue.Value !=0.0){
                    lab6.Text = (calculated.outValue.Value / calculated.inValue.Value * 100 ).ToString("f2")+ "%";
                }


            };
            wrk.RunWorkerAsync();



        }
        //点击查询按钮加载
        void WaterData(object sender, System.EventArgs e)
        {

            Navigation.PushAsync(new WaterDataPage(_flow,_businness));


        }

        BusinessDetailFlow _flow = null;
        string _businness = "";
        public ProcessInformationPage(BusinessDetailFlow flow, string businness)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _flow = flow;
            _businness = businness;
            this.Title = flow.flowname + " - 综合信息";
            businessNameLab.Text = businness;
            gongyiliucheng.Text = flow.flowname + "(" + flow.flowTypeName + ")";

            DateTime end = DateTime.Now;
            DateTime start = end.AddHours(-6);

            startDatePicker.Date = start.Date;
            endDatePicker.Date = end.Date;

            startTime = start.TimeOfDay;
            endTime = end.TimeOfDay;
            endTimePicker.BindingContext = this;
            starTimePicker.BindingContext = this;
            startTimeBut.BindingContext = this;
            endTimeBut.BindingContext = this;

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                CrossHud.Current.Show("请求中。。。");
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                CrossHud.Current.Dismiss();
                if (list !=null){
                    for (int i = 0; i < list.Count; i ++){
                        FlowDesInfo info = list[i];
                        if (info.opname.Contains("提效前吨水成本")){
                            lab1.Text = info.value.Value.ToString("f2") + info.unit;
                        }
                        if (info.opname.Contains("提效后吨水成本"))
                        {
                            lab2.Text = info.value.Value.ToString("f2") + info.unit;
                        }
                        if (info.opname.Contains("提效前综合利用率"))
                        {
                            lab3.Text = info.value.Value.ToString("f2") + info.unit;
                        }
                        if (info.opname.Contains("总进水累计流量K1"))
                        {
                            entry1.Text = info.value.Value.ToString("f2");
                        }
                        if (info.opname.Contains("总产水累计流量K2"))
                        {
                            entry2.Text = info.value.Value.ToString("f2");
                        }
                    }
                }



            };
            wrk.RunWorkerAsync();

        }
        //获取提效前吨水成本、提效后吨水成本、提效前综合利用率
        List<FlowDesInfo> list = null;
        void makeData(){
            try
            {
                string url = App.BaseURL + "AppFlow/GetFlowDesInfo?id=" + _flow.flowid;
                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);
                result = result.Replace("null", "0");
                list = JsonConvert.DeserializeObject<List<FlowDesInfo>>(result);
            }
            catch (Exception ex)
            {
            
            }

        }

        //计算提效后综合利用率
        calculatedResult calculated = null;
        void calculatedData(){
            try
            {
                DateTime sd = startDatePicker.Date + startTime;
                DateTime ed = endDatePicker.Date + endTime;

                if(DateTime.Compare(sd,ed) >=0){
                    CrossHud.Current.ShowError(message: "开始时间必须早于结束时间", timeout: new TimeSpan(0, 0, 4));
                    return;
                }

                string url = App.BaseURL + "AppFlow/ComputedFlowAllWater";
                string parama = "flowid=" + _flow.flowid + "&" + "sdt=" + sd.ToString("yyyy-MM-dd HH:mm") + "&" + "edt="+ ed.ToString("yyyy-MM-dd HH:mm")+ "&" +"para1="+entry1.Text + "&"+"para2="+entry2.Text;

                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendOtherPOSTHttpWebRequest(url,parama);
                Console.WriteLine("请求结果：" + result);
                calculated = JsonConvert.DeserializeObject<calculatedResult>(result);
            }
            catch (Exception ex)
            {

            }

        }

        internal class calculatedResult {

            public float? inValue { get; set; }
            public float? outValue { get; set; }
        }


        private TimeSpan _st = new TimeSpan();

        public TimeSpan startTime
        {
            get { return _st; }
            set
            {
                _st = value;
                OnPropertyChanged("StartTimeString");
            }
        }

        public string StartTimeString
        {
            get
            {
                return _st.ToString(@"hh\:mm");
            }
        }


        private TimeSpan _et = new TimeSpan();

        public TimeSpan endTime
        {
            get { return _et; }
            set
            {
                _et = value;
                OnPropertyChanged("EndTimeString");


            }
        }
        public string EndTimeString
        {
            get
            {
                return _et.ToString(@"hh\:mm");
            }
        }

    }
}

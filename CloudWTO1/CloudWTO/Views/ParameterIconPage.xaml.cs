using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CloudWTO.Models;
using Xamarin.Forms;
using CloudWTO.Services;
using System.ComponentModel;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


using OxyPlot.Xamarin.Forms;
#endif

namespace CloudWTO.Views
{
    public partial class ParameterIconPage : ContentPage
    {





        void Handle_Clicked(object sender, System.EventArgs e)
        {
            
            var but = sender as Button;
            if (but == addMoreBut)
            {

                Console.WriteLine("加载更多" + startDatePicker.Date);

                //this.HaveParametDatas();

                BackgroundWorker wrk = new BackgroundWorker();
                wrk.DoWork += (a, b) => {
                    makeData();
                };
                wrk.RunWorkerCompleted += (a, b) => {
                    ProcessingData();
                };
                wrk.RunWorkerAsync();


            }



        }

        void Handle_Clicked1(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ParameterIconInfoPage(_businness,_gylc,_title,resultData.items,_unit));

        }

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

        string _parametId = null;
        string _businness = "";
        string _gylc = "";
        string _title = "";
        string _unit = "";
        public ParameterIconPage(string ParametId, string businness, string gylc, string title,string unit)
        {
            InitializeComponent();

            _businness = businness;
            _gylc = gylc;
            _title = title;
            _parametId = ParametId;
            _unit = unit;

            this.Title = title;
            businessNameLab.Text = businness;
            gongyiliucheng.Text = gylc;

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
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                ProcessingData();
            };
            wrk.RunWorkerAsync();

        }
        //处理数据
        void ProcessingData()
        {
            DateTime sd = startDatePicker.Date + startTime;
            DateTime ed = endDatePicker.Date + endTime;

            LineSeries s = new LineSeries();
            //s.DataFieldX = "X轴";
            //s.DataFieldY = "y轴";
            //s.Title = "这是参数标题";//折线图表示意义
            //s.IsVisible = false;//是否隐藏折线图
            s.Background = OxyColor.FromRgb(200, 200, 200);
            //s.XAxisKey = "X轴关键字";//多个X轴的关键字
            //s.YAxisKey = "Y轴关键字";//多个Y轴的关键字
            DateTimeAxis ax = new DateTimeAxis();
            ax.IsPanEnabled = true;
            ax.IsZoomEnabled = true;
            ax.StringFormat = "yyyy-MM-dd HH:mm";
            //ax.MinorStep = 200;
            //ax.MinimumMinorStep = 400;
            //ax.FilterMinValue = 400;
            //ax.MinimumMajorStep = 0;
            //ax.MinimumPadding = 400;
            ////ax.MinimumMajorStep = 400;
            ////ax.AbsoluteMinimum = 400;


            //ax.Position = AxisPosition.Right;//设置X轴的位置
            //ax.MinimumRange = sd.ToOADate();
            //ax.MaximumRange = ed.ToOADate();
            ax.Minimum = DateTimeAxis.ToDouble(sd);
            ax.Maximum = DateTimeAxis.ToDouble(ed);
            ax.MajorGridlineStyle = LineStyle.Solid;


            float min = float.MaxValue;
            float max = float.MinValue;
            if (resultData != null)
            {
                for (int i = 0; i < resultData.items.Count; i++)
                {
                    var para = resultData.items[i];

                    if (para.value.HasValue)
                    {
                        min = Math.Min(para.value.Value, min);
                        max = Math.Max(para.value.Value, max);
                        s.Points.Add(new DataPoint(DateTimeAxis.ToDouble(para.time), para.value.Value));
                    }
                }

                PlotModel pm = new PlotModel();
                pm.Axes.Add(ax);
                pm.Series.Add(s);


                LinearAxis ya = new LinearAxis();
                ya.Position = AxisPosition.Left;

                ya.Minimum = min - (max - min) * 0.1;//
                ya.Maximum = max + (max - min) * 0.1;

                ////下限值
                //ya.Minimum = resultData.lowerbound.Value;
                ////上限值
                //ya.Maximum = resultData.upperbound.Value;


                ya.IsPanEnabled = false;

                //设置上限、下限值
                ya.ExtraGridlines = new double[2] { resultData.lowerbound.Value, resultData.upperbound.Value };
                ya.ExtraGridlineStyle = LineStyle.LongDash;
                ya.ExtraGridlineColor = OxyColor.FromRgb(255,0,0);
                ya.IsZoomEnabled = false;
                ya.MajorGridlineStyle = LineStyle.Solid;//主网格线
                ya.MinorGridlineStyle = LineStyle.Solid;//子网格线

                pm.Axes.Add(ya);
                pView.Model = pm;

                pView.VerticalOptions = LayoutOptions.Fill;
                pView.HorizontalOptions = LayoutOptions.Fill;
            }

        }

        resultParama resultData = null;
        void makeData()
        {
            try
            {
                DateTime sd = startDatePicker.Date + startTime;
                DateTime ed = endDatePicker.Date + endTime;
                string url = App.BaseURL + "AppOperParam/GetDataByTime?id=" + _parametId + "&startTime=" + sd.ToString("yyyy-MM-dd HH:mm") + "&endTime=" + ed.ToString("yyyy-MM-dd HH:mm");

                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);
                result = result.Replace("null", "0");
                resultData = JsonConvert.DeserializeObject<resultParama>(result);
            }
            catch (Exception ex)
            {
                DisplayAlert("错误提示", ex.Message, "OK");
            }

        }



        public class resultParama
        {
            public List<prama> items { set; get; }
            public float? lowerbound { set; get; }
            public float? upperbound { set; get; }
        }


        public class prama
        {
            public DateTime time { get; set; }
            public float? value { get; set; }
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

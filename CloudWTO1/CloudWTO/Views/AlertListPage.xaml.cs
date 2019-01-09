using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CloudWTO.Models;
using Xamarin.Forms;
using CloudWTO.Services;
using System.ComponentModel;

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class AlertListPage : ContentPage //, INotifyPropertyChanged 
    {

        bool haveMole = true;
        int pageIndex = 0;
        void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            AlertAndAlarmt item = e.Item as AlertAndAlarmt;
            if (item == dataList[dataList.Count - 1] && haveMole == true && item != null)
            {
                pageIndex += 1;
                BackgroundWorker wrk = new BackgroundWorker();
                wrk.DoWork += (a, ee) => {
                    haveAlertOrAlarm();
                };

                wrk.RunWorkerAsync();

            }

        }





        //点击加载按钮加载
        void AddData(object sender, System.EventArgs e)
        {
            pageIndex = 0;
            this.haveAlertOrAlarm();

        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var but = sender as Button;
            if (but == alarmBut)
            {
                if (isselectAlert == false)
                {
                    return;
                }
                isSelectAlarm = !isSelectAlarm;
            }
            if (but == alertBut)
            {
                if (isSelectAlarm == false)
                {
                    return;
                }
                isselectAlert = !isselectAlert;
            }

            if (isSelectAlarm == true)
            {
                alarmBut.Image = "tick.png";

            }
            else
            {
                alarmBut.Image = null;
            }
            if (isselectAlert == true)
            {
                alertBut.Image = "tick.png";
            }
            else
            {
                alertBut.Image = null;
            }

            if (isselectAlert == true && isSelectAlarm == true)
            {
                _flag = 0;//全部

            }
            else
            {
                if (isSelectAlarm == true)
                {
                    _flag = 2;
                }
                if (isselectAlert == true)
                {
                    _flag = 1;
                }
            }
            pageIndex = 0;
            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (a, ee) => {
                haveAlertOrAlarm();
            };
            wrk.RunWorkerAsync();
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


        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var alertModel = e.SelectedItem as AlertAndAlarmt;
            if (alertModel == null)
                return;

            if (alertModel.type == "2")
                Navigation.PushAsync(new AlarmInfoPage(alertModel, this.businessNameLab.Text));

            if (alertModel.type == "1")
                Navigation.PushAsync(new AlertInfoPage(alertModel, this.businessNameLab.Text));

            listV.SelectedItem = null;


        }


        bool isSelectAlarm = true;
        bool isselectAlert = true;
        string _businessId = null;
        int _flag = 0;
        public AlertListPage(string businessId, string businessName)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            _businessId = businessId;
            this.Title = "预警、报警";

            this.businessNameLab.Text = businessName;

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
                haveAlertOrAlarm();
            };
            wrk.RunWorkerCompleted += (sender, e) => {
                if (dataList != null)
                    listV.ItemsSource = dataList;

            };
            wrk.RunWorkerAsync();



        }


        //List<AlertAndAlarmt> dataList = new List<AlertAndAlarmt>();
        ObservableCollection<AlertAndAlarmt> dataList = new ObservableCollection<AlertAndAlarmt>();
        void haveAlertOrAlarm()
        {

            try
            {
                DateTime sd = startDatePicker.Date + startTime;
                DateTime ed = endDatePicker.Date + endTime;
                string url = App.BaseURL + "AppAlarm/GetAllAlarm?id=" + _businessId + "&startTime=" + sd.ToString("yyyy-MM-dd HH:mm") + "&endTime=" + ed.ToString("yyyy-MM-dd HH:mm") + "&pageIndex=" + pageIndex +
                                                                                                                         "&pageSize=20" + "&flag=" + _flag;
                Console.WriteLine("请求接口：" + url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                resutlDic resultList = JsonConvert.DeserializeObject<resutlDic>(result);
                if (pageIndex == 0)
                {
                    dataList.Clear();
                }
                for (int i = 0; i < resultList.Items.Count; i++)
                {
                    AlertAndAlarmt item = resultList.Items[i];

                    dataList.Add(item);
                }

                if (resultList.Totals <= dataList.Count)
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
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }


        internal class resutlDic
        {
            public int Totals { get; set; }
            public List<AlertAndAlarmt> Items { get; set; }
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

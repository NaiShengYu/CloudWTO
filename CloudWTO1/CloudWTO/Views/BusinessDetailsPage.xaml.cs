using System;
using System.Collections.Generic;
using CloudWTO.Models;
using Xamarin.Forms;
using CloudWTO.Services;
using System.ComponentModel;
using Plugin.Hud;
#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif
namespace CloudWTO.Views
{
    public partial class BusinessDetailsPage : ContentPage
    {
        void BottomBut_Clicked(object sender, System.EventArgs e)
        {
            Button but = sender as Button;
            if (lastBut == but)
            {
                return;
            }
            gongyishuBut.Image = null;
            xunhuanshuiBut.Image = null;
            gongyefeishuiBut.Image = null;
            if (but == gongyishuBut)
            {
                gongyishuBut.Image = "tick.png";
                listV.ItemsSource = _GYSList;
                titleLab.Text = "工艺流程(工艺水)";

            }

            if (but == xunhuanshuiBut)
            {
                xunhuanshuiBut.Image = "tick.png";
                listV.ItemsSource = _XHSList;
                titleLab.Text = "工艺流程(循环水)";

            }

            if (but == gongyefeishuiBut)
            {
                gongyefeishuiBut.Image = "tick.png";
                listV.ItemsSource = _GYFSList;
                titleLab.Text = "工艺流程(工业废水)";
            }
            lastBut = but;

        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var details = e.SelectedItem as BusinessDetailFlow;
            if (details == null)
                return;

            Navigation.PushAsync(new ClassificationPage(_ent,details));

            //if(_type ==0)//综合信息
            //    Navigation.PushAsync(new ProcessInformationPage(details, _ent.name));
            //else if (_type ==1)//RO运行参数
            //    Navigation.PushAsync(new ROEquipmentListPage(details, _ent.name));
            //else//指标监控
                //Navigation.PushAsync(new DeviceListPage(details, _ent.name));

            listV.SelectedItem = null;

        }



        void Handle_Clicked(object sender, System.EventArgs e)
        {
            
            Navigation.PushAsync(new AlertListPage(_ent.id, _ent.name));
        }

        Enttype _ent = null;

        List<BusinessDetailFlow> _GYSList = new List<BusinessDetailFlow>();
        List<BusinessDetailFlow> _XHSList = new List<BusinessDetailFlow>();
        List<BusinessDetailFlow> _GYFSList = new List<BusinessDetailFlow>();
        Button lastBut = null;
        public BusinessDetailsPage(Enttype ent, int type)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
            this.Title = ent.name;
            _ent = ent;
            if (type == 0)
            {
                NavigationPage.SetHasBackButton(this, false);
            }
            //BusinessName.Text = ent.name;
            //alarmBut.IsVisible = (bool)ent.alarm;
            //if (ent.alert == true)
                //alertBut.IsVisible = (bool)ent.alert;
            lastBut = gongyishuBut;

            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) => {
                CrossHud.Current.Show("请求中。。。");
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) => {

                //desalination.Text = details.desalination;
                //recovery.Text = details.recovery;
                //avgrecovery.Text = details.avgrecovery;
                //infoLab.Text = _ent.addr + details.remark;
                //listV.ItemsSource = details.flow;
                for (int i = 0; i < details.flow.Count; i++)
                {
                    BusinessDetailFlow flow = details.flow[i];
                    if (flow.flowtype == "0")
                        _GYSList.Add(flow);

                    if (flow.flowtype == "1")
                        _XHSList.Add(flow);

                    if (flow.flowtype == "2")
                        _GYFSList.Add(flow);
                }
                listV.ItemsSource = _GYSList;

                CrossHud.Current.Dismiss();
            };
            wrk.RunWorkerAsync();
        }

        BusinessDetails details = null;
        void makeData()
        {
            try
            {
                string url = App.BaseURL + "AppEnterprise/GetEnterpriseDetail?id=" + _ent.id;
                Console.WriteLine(url);
                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                var list = JsonConvert.DeserializeObject<List<BusinessDetails>>(result);

                if (list.Count > 0)
                {
                    details = list[0];
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");
            }
        }
       
    }
}

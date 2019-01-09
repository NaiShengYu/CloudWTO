using System;
using System.Collections.Generic;
using CloudWTO.Models;
using Xamarin.Forms;
using CloudWTO.Services;
using System.ComponentModel;
using System.Collections.ObjectModel;

#if __MOBILE__
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif

namespace CloudWTO.Views
{
    public partial class EnterpriseListPage : ContentPage
    {
        DateTime? lastBackKeyDownTime;
        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {


            Console.WriteLine(searchB.Text);
            if (searchB.Text == null || searchB.Text.Length == 0)
            {
                pageIndex = 0;
                haveMole = true;
                BackgroundWorker wrk = new BackgroundWorker();
                wrk.DoWork += (sender1, e1) =>
                {
                    makeData();
                };
                wrk.RunWorkerAsync();
            }
        }

        ObservableCollection<Enttype> dataList = new ObservableCollection<Enttype>();
        bool haveMole = true;
        int pageIndex = 0;
        //item刚出现      
        void Handle_ItemAppearing(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            Enttype item = e.Item as Enttype;
            if (item == dataList[dataList.Count - 1] && haveMole == true && item != null)
            {
                pageIndex += 1;
                BackgroundWorker wrk = new BackgroundWorker();
                wrk.DoWork += (sender1, e1) =>
                {
                    makeData();
                };
                wrk.RunWorkerAsync();
            }
        }

        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            Console.WriteLine("搜索按钮");
            pageIndex = 0;
            haveMole = true;
            this.makeData();
        }


        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            Enttype ent = e.SelectedItem as Enttype;

            if (ent == null)
            {
                return;
            }
            //var busine = new BusinessDetailsPage(ent,1);
            Navigation.PushAsync(new BusinessDetailsPage(ent,1));
            lv.SelectedItem = null;

        }


        //下拉刷新
        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            pageIndex = 0;
            haveMole = true;
            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender1, e1) =>
            {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender2, e2) => {
                lv.EndRefresh();
            };
            wrk.RunWorkerAsync();
        }

        public EnterpriseListPage()
        {
            Console.WriteLine("test");
            InitializeComponent();
            Console.WriteLine("a");

            NavigationPage.SetBackButtonTitle(this, "");
            NavigationPage.SetHasBackButton(this, false);
            Console.WriteLine("b");

            this.Title = "企业列表";
            Console.WriteLine("c");

            //给ListView添加cell
            //ListView.ItemsSource = sourse;
            //设置cell高度
            lv.RowHeight = 60;
            Console.WriteLine("1");


            BackgroundWorker wrk = new BackgroundWorker();
            wrk.DoWork += (sender, e) =>
            {
                makeData();
            };
            wrk.RunWorkerCompleted += (sender, e) =>
            {
                lv.EndRefresh();
                Console.WriteLine("3");
                if (dataList != null)
                    lv.ItemsSource = dataList;

            };
            wrk.RunWorkerAsync();

        }

        void makeData()
        {
            try
            {

                string url = App.BaseURL + "AppEnterprise/GeteEnterpriseByKey?searchKey=" + searchB.Text + "&pageIndex=" + pageIndex + "&pageSize=30";
                Console.WriteLine("请求url：" + url);

                string result = EasyWebRequest.sendGetHttpWebRequest(url);
                Console.WriteLine("请求结果：" + result);

                var retv = JsonConvert.DeserializeObject<entlisttype>(result);
                if (retv == null)
                {
                    DisplayAlert("retv null", result, "OK");
                }
                if (pageIndex == 0)
                    dataList.Clear();
                for (int i = 0; i < retv.Items.Count; i++)
                {
                    Enttype item = retv.Items[i];
                    dataList.Add(item);
                }

                if (retv.Totals <= dataList.Count)
                    haveMole = false;
                else
                    haveMole = true;

            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", ex.Message, "OK");

            }
        }
        protected override bool OnBackButtonPressed()
        {
            if (!lastBackKeyDownTime.HasValue || DateTime.Now - lastBackKeyDownTime.Value > new TimeSpan(0, 0, 2))
            {
                DependencyService.Get<Sample.IToast>().ShortAlert("再按一次退出程序");
                lastBackKeyDownTime = DateTime.Now;
            }
            else
            {
                System.Environment.Exit(0);
            }
            return true;
        }
    }




    internal class entlisttype
    {
        public int Totals { get; set; }
        public ObservableCollection<Enttype> Items { get; set; }
    }


}

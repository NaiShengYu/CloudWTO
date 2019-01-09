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
    public partial class ParameterIconInfoPage : ContentPage
    {
        public ParameterIconInfoPage(string businness, string gylc, string title, List<ParameterIconPage.prama> dataList,string unit)
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            this.Title = title;
            businessNameLab.Text = businness;
            gongyiliucheng.Text = gylc;
            dataList.Reverse();
            listV.ItemsSource = dataList;
            unitLab.Text = "指标值"+"(" + unit +")";
        }

      
    }
}


using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CloudWTO.Views
{
    public partial class PlanPage : ContentPage
    {
        public PlanPage(string plan)
        {
            InitializeComponent();
            infoLab.Text = plan;
            this.Title = "处理预案";

        }
    }
}

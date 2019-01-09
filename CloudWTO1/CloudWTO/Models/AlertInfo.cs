using System;
namespace CloudWTO.Models
{
    public class AlertInfo
    {
        public DateTime time { get; set; }
        public string alarmid { get; set; }
        public string flowName { get; set; }
        public string equipmentName { get; set; }
        public string operName { get; set; }
        public string state { get; set; }
        public string operId { get; set; }
        public string forecastTime { get; set; }
        public string contact { get; set; }
        public string phone { get; set; }
        public string plan { get; set; }
        public string alertplan { get; set; }
        public string alarmplan { get; set; }
        public decimal alertupperbound { get; set; }
        public decimal alertlowerbound { get; set; }
        public decimal? value { get; set; }
        public string handle { get; set; }
        public string unit { get; set; }

        public string valueAndUnit
        {
            get
            {
                try
                {
                    return value.Value.ToString("f2") + " " + unit;
                }
                catch
                {
                    return "0" + " " + unit;
                }          
            }
            set { }
        }

        public string upperAndUnit
        {
            get
            {
                try
                {
                    return alertupperbound.ToString("f2") + " " + unit;
                }
                catch
                {
                    return "0" + " " + unit;
                }
            }
            set { }
        }
        public string lowerAndUnit
        {
            get
            {
                try
                {
                    return alertlowerbound.ToString("f2") + " " + unit;
                }
                catch
                {
                    return "0" + " " + unit;
                }
            }
            set { }
        }



    }
}

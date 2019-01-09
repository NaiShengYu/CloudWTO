
using System;
namespace CloudWTO.Models
{
    public class WaterDataModel
    {
        public string id { set; get; }
        public string name { set; get; }
        public string model { set; get; }
        public float? value { set; get; }
        public string Date { set; get; }
        public string type { set; get; }
        public string unit { set; get; }
    }
}

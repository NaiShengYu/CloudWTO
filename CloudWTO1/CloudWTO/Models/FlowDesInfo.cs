using System;
namespace CloudWTO.Models
{
    public class FlowDesInfo
    {
        public string id { get; set; }
        public string opname { get; set; }
        public string eqname { get; set; }
        public string unit { get; set; }
        public string parentid { get; set; }
        public string type { get; set; }
        public string index { get; set; }
        public float? value { get; set; }
        public string date { get; set; }

        public string valueUnit { 
            get{
                return value.Value.ToString("f2")+ " " + unit;
            } 
            set{}
        }

    }
}

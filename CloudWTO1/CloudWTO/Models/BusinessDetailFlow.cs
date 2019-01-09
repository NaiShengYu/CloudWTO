using System;
namespace CloudWTO.Models
{
    public class BusinessDetailFlow
    {
        public string flowid { get; set; }
        public string flowname { get; set; }
        public string flowtype { get; set; }

        public string flowTypeName{ 
            get{
                if (flowtype == "0")
                {
                    return "工艺水";
                }
                if (flowtype == "1")
                {
                    return "循环水";
                }
                if (flowtype == "2")
                {
                    return "工业废水";
                }
                return null;
            }
            set{}
        }
    }
}

using System;
namespace CloudWTO.Models
{
    public class Parameter
    {
     
        public string operparamId { get; set; }
        public string operparamName { get; set; }
        public string operparamUnit { get; set; }
        public string equipmentName { get; set; }
        public string equipmentId { get; set; }
        public string operCollectfrequency { get; set; }
        public string operLong { get; set; }
        public string time { get; set; }
        public decimal? value { get; set; }

        public string lastValue { 
            get {
                try {
                    return value.Value.ToString("f2");
                }catch{
                    return "0";
                }
            }
            set { }
        }

        public string NameAndName{
            get {
                return operparamName + "(" + equipmentName + ")";

            }

            set{}
        }



    }
}



using System;
namespace CloudWTO.Models
{
    public class ROEquipmentModel
    {
        public string id { get; set; }
        public string subname { get; set; }
        public string name { get; set; }

        public string nameName {
            get{
                return subname + name;
            }
            set{}
        }
    }
}

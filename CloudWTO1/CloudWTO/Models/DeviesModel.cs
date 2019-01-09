using System;
namespace CloudWTO.Models
{
    public class DeviesModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string model { get; set; }

        public string TypeImage
        {
            get
            {
                if (type == "o")
                {      
                    return "param.png";
                }
                if (type == "e")
                {
                    return "eq.png";
                }
                if (type == "a")
                {
                    return "array.png";
                }
                return null;
            }
            set { }

        }

    }
}

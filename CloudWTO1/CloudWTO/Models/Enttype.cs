using System;
using CloudWTO.Models;
namespace CloudWTO.Models
{
    public class Enttype
    {
        
        public string id { get; set; }
        public string name { get; set; }
        public string addr { get; set; }
        public bool? alarm { get; set; }
        public bool? alert { get; set; }
    }
}

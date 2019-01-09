using System;
namespace CloudWTO.Models
{
    public class EnterpriseListModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public bool Alarm { get; set; }
        public bool Alert { get; set; }
    }
}

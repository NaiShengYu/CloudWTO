using System;
namespace CloudWTO.Models
{
    public class AlertAndAlarmt
    {
      
        public string alarmid { get; set; }
        public string type { get; set; }
        public string flowName { get; set; }
        public string equipmentName { get; set; }
        public string operName { get; set; }
        public DateTime? time { get; set; }
        public string state { get; set; }
        public string handle { get; set; }

    }
}

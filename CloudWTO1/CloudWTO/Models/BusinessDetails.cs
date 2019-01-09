using System;
using System.Collections.Generic;

namespace CloudWTO.Models
{
    public class BusinessDetails
    {
        public string remark { get; set; }
        public string desalination { get; set; }
        public string recovery { get; set; }
        public string avgrecovery { get; set; }
        public List<BusinessDetailFlow> flow { get; set; }

    }
}

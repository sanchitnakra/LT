using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class SubArea
    {
        public int SubAreaID { get; set; }
        public string SubAreaName { get; set; }

        public List<SubArea> SubAreaList { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class CustomerManager
    {
        public int CustomerManagerID { get; set; }
        public string CustomerManagerName { get; set; }

        public List<CustomerManager> CustomerManagerList { get; set; }
    }
}
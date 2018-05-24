using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class Supervisor
    {
        public int SupervisorID { get; set; }
        public string SupervisorName { get; set; }

        public List<Supervisor> SupervisorList { get; set; }
    }
}
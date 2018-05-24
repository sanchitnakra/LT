using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }

        public List<Location> LocationList { get; set; }

    }
}
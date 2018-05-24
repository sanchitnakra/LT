using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
        public List<Project> ProjectNameList { get; set; }
    }
}
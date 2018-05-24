using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sampleMVC.Models
{
    public class AdprojectDetails
    {
        public int ProjectID { get; set; }
        [Display(Name = "Enter Project Name")]
        public string ProjectName { get; set; }

    }

    public class AdlocationDetails
    {
        public int LocationID { get; set; }
        [Display(Name = "Enter Location Name")]
        public string LocationName { get; set; }
        public int ProjectID { get; set; }
        [Display(Name = "Enter Project Name")]
        public string ProjectName { get; set; }

        public int BranchID { get; set; }
        [Display(Name = "Enter Branch Name")]
        public string BranchName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InformaticaAPIs.Models
{
    public class WorkflowLogItem
    {
        [Display(Name = "Workflow Run ID")]
        public string WorkflowRunID { get; set; }

        [Display(Name = "Workflow Name")]
        public string WorkflowName { get; set; }

        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        public string _ErrorCount { get; set; }
        public IEnumerable<WFEntityDetails> EntityDetails { get; set; }
    }

    public class WFEntityDetails
    {
        public WFEntityDetails() { }
        public string WorkflowRunID { get; set; }
        public string SourceRows { get; set; }
        public string TargetRows { get; set; }
        public string MappingName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformaticaAPIs.Models
{
    public class WorkFlowErrorModel
    {
        public string WorkFlowRunID { get; set; }

        public string WorkFlowName { get; set; }

        public string MappingName { get; set; }

        public string ErrorType { get; set; }

        public string ErrorMSG { get; set; }

        public string TransactionName { get; set; }

        public string FolderName { get; set; }

        public string TransactionRowID { get; set; }

        public string SourceData { get; set; }

    }

    public class WorkFlowErrorAnalysis
    {
        public string MappingName { get; set; }

        public string ErrorCount { get; set; }

        public string ErrorType { get; set; }
    }

    public class WorkFlowErrorAnalysis_FINAL
    {
        public string[] Mappings { get; set; }

        public string[] ErrorTypes { get; set; }
        public IEnumerable<WorkFlowErrorAnalysis> ErrorCount { get; set; }
    }
}
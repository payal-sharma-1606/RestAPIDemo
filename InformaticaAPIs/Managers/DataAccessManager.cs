using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using InformaticaAPIs.Helpers;
using InformaticaAPIs.Models;
using Newtonsoft.Json;

namespace InformaticaAPIs.Managers
{
    public class DataAccessManager
    {
        public string ProcessResponse(string _apiIdentifier, long WorkFlowRunID)
        {
            string _responseContent = string.Empty;
            switch (_apiIdentifier)
            {
                case "wf":
                    _responseContent = ProcessApiData_WF();
                    break;
                case "wfe":
                    _responseContent = ProcessApiData_WFE(WorkFlowRunID);
                    break;
                case "wfea":
                    _responseContent = ProcessApiData_WFEA(WorkFlowRunID);
                    break;
            }

            return _responseContent;
        }

        private static DataSet GetDatafromDB(string spName, Dictionary<string, object> _params)
        {
            DataSet _ds = new DataSet();
            SqlDbHelper.ExecuteStoreProcedure(spName, _params, _ds);
            return _ds;
        }
        private string ProcessApiData_WF()
        {
            DataSet _data = GetDatafromDB("IFUI_sqGetWorkFlowLogs", new Dictionary<string, object>());
            return this.ProcessRequest_WF(_data);
        }
        private string ProcessRequest_WF(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0) return string.Empty;

            List<WorkflowLogItem> filteredModel = new List<WorkflowLogItem>();
            //
            var _distinctWFData = ds.Tables[0].DefaultView.ToTable(true, "WorkflowRunID");

            foreach (DataRow _item in _distinctWFData.Rows)
            {
                WorkflowLogItem workflowLogItem = new WorkflowLogItem();

                DataRow _firstRow = ds.Tables[0].Select("WorkflowRunID =" + Convert.ToInt64(_item["WorkflowRunID"]), "StartTime ASC").FirstOrDefault();
                DataRow _lastWFRow = ds.Tables[0].Select("WorkflowRunID = " + Convert.ToInt64(_item["WorkflowRunID"]), "EndTime DESC").FirstOrDefault();

                workflowLogItem.WorkflowRunID = Convert.ToString(_item["WorkflowRunID"]);
                workflowLogItem.WorkflowName = Convert.ToString(_firstRow["WorkflowName"]);
                workflowLogItem._ErrorCount = Convert.ToString(_firstRow["_ErrorCount"]);
                workflowLogItem.StartTime = Convert.ToString(_firstRow["StartTime"]);
                workflowLogItem.EndTime = Convert.ToString(_lastWFRow["EndTime"]);
                workflowLogItem.EntityDetails = getEntityData(ds.Tables[0].Select("WorkflowRunID =" + Convert.ToInt64(_item["WorkflowRunID"]), "MappingName ASC").CopyToDataTable());
                filteredModel.Add(workflowLogItem);
            }
            return JsonConvert.SerializeObject(filteredModel);
        }
        private static List<WFEntityDetails> getEntityData(DataTable _data)
        {
            List<WFEntityDetails> _entityD = new List<WFEntityDetails>();

            foreach (DataRow _item in _data.Rows)
            {
                _entityD.Add(new WFEntityDetails
                {
                    WorkflowRunID = Convert.ToString(_item["WorkflowRunID"]),
                    MappingName = Convert.ToString(_item["MappingName"]),
                    SourceRows = Convert.ToString(_item["SourceRows"]),
                    TargetRows = Convert.ToString(_item["TargetRows"]),
                    StartTime = Convert.ToString(_item["StartTime"]),
                    EndTime = Convert.ToString(_item["EndTime"])
                });
            }
            return _entityD;

        }


        private string ProcessApiData_WFE(long WorkFlowRunID)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("WorkFlowRunID", WorkFlowRunID);
            //
            DataSet _data = GetDatafromDB("IFUI_sqGetWorkFlowErrorLogs", _params);

            return this.ProcessRequest_WFE(_data);
        }

        private string ProcessRequest_WFE(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0) return string.Empty;

            List<WorkFlowErrorModel> filteredModel = new List<WorkFlowErrorModel>();

            foreach (DataRow _item in ds.Tables[0].Rows)
            {
                WorkFlowErrorModel _workFlowErrorModel = new WorkFlowErrorModel();

                _workFlowErrorModel.WorkFlowRunID = Convert.ToString(_item["WorkflowRunID"]);
                _workFlowErrorModel.WorkFlowName = Convert.ToString(_item["WorkflowName"]);
                _workFlowErrorModel.MappingName = Convert.ToString(_item["MappingName"]);
                _workFlowErrorModel.SourceData = Convert.ToString(_item["SourceData"]);
                _workFlowErrorModel.TransactionName = Convert.ToString(_item["TransactionName"]);
                _workFlowErrorModel.TransactionRowID = Convert.ToString(_item["TransactionRowID"]);
                _workFlowErrorModel.FolderName = Convert.ToString(_item["FolderName"]);
                _workFlowErrorModel.ErrorType = Convert.ToString(_item["ErrorType"]);
                _workFlowErrorModel.ErrorMSG = Convert.ToString(_item["ErrorMSG"]);
                filteredModel.Add(_workFlowErrorModel);
            }
            return JsonConvert.SerializeObject(filteredModel);
        }

        private string ProcessApiData_WFEA(long WorkFlowRunID)
        {
            Dictionary<string, object> _params = new Dictionary<string, object>();
            _params.Add("WorkFlowRunID", WorkFlowRunID);
            _params.Add("isAnalysisCall", true);
            //
            DataSet _data = GetDatafromDB("IFUI_sqGetWorkFlowErrorLogs", _params);

            return this.ProcessRequest_WFEA(_data);
        }

        private string ProcessRequest_WFEA(DataSet ds)
        {
            if (ds == null || ds.Tables == null || ds.Tables.Count == 0 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count == 0) return string.Empty;

            List<WorkFlowErrorAnalysis> filteredModel = new List<WorkFlowErrorAnalysis>();
            //
            WorkFlowErrorAnalysis_FINAL _finalModel = new WorkFlowErrorAnalysis_FINAL();
            _finalModel.Mappings = ds.Tables[0].AsEnumerable().Select(x => x.Field<string>("MappingName")).ToArray();
            _finalModel.ErrorTypes = ds.Tables[1].AsEnumerable().Select(x => x.Field<string>("ErrorType")).ToArray();

            foreach (DataRow _item in ds.Tables[2].Rows)
            {
                WorkFlowErrorAnalysis _workFlowErrorAnalysis = new WorkFlowErrorAnalysis();
                //
                _workFlowErrorAnalysis.ErrorType = Convert.ToString(_item["ErrorType"]);
                _workFlowErrorAnalysis.ErrorCount = Convert.ToString(_item["ErrorCount"]);
                _workFlowErrorAnalysis.MappingName = Convert.ToString(_item["MappingName"]);
                filteredModel.Add(_workFlowErrorAnalysis);
            }
            _finalModel.ErrorCount = filteredModel;
            //
            return JsonConvert.SerializeObject(_finalModel);
        }
    }
}
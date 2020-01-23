#region Directives

using System;
using System.Data;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using InformaticaAPIs.Models;
using InformaticaAPIs.Managers;

#endregion

namespace InformaticaAPIs.Controllers
{
    public class DataAccessController : ApiController
    {
        #region Declarations

        DataAccessManager _manager;

        #endregion

        #region Http Methods

        [HttpPost, ActionName("QueryWFLogs")]
        public HttpResponseMessage QueryWFLogs(DataRequest request)
        {
            HttpResponseMessage _response = null as HttpResponseMessage;
            try
            {
                BeginRequest(ref request);
                BeginResponse(ref _response, "wf", 0);
                EndRequest(ref _response, ref request);
            }
            catch (Exception ex)
            {
                _response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return _response;
        }

        [HttpPost, ActionName("QueryWFELogs")]
        public HttpResponseMessage QueryWFELogs(DataRequest request, long WorkFlowRunID, bool isAnalysis)
        {
            HttpResponseMessage _response = null as HttpResponseMessage;
            string _identifier = isAnalysis ? "wfea" : "wfe";
            try
            {
                BeginRequest(ref request);
                BeginResponse(ref _response, _identifier, WorkFlowRunID);
                EndRequest(ref _response, ref request);
            }
            catch (Exception ex)
            {
                _response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return _response;
        }

        #endregion

        #region Helper Methods
        private HttpResponseMessage BeginResponse(ref HttpResponseMessage _response, string _apiIdentifier, long WorkFlowRunID)
        {
            _manager = new DataAccessManager();
            _response = Request.CreateResponse(HttpStatusCode.OK);
            _response.Content = new StringContent(_manager.ProcessResponse(_apiIdentifier, WorkFlowRunID), Encoding.UTF8, "application/json");
            return _response;
        }
        private void BeginRequest(ref DataRequest request)
        {
            request = new DataRequest();
            request.RequestStartTime = DateTime.UtcNow;
        }

        private void EndRequest(ref HttpResponseMessage _response, ref DataRequest request)
        {
            request.RequestEndTime = DateTime.UtcNow;
            _response.RequestMessage = request;
        }

        private void EndRequest(ref HttpResponseMessage _response, ref DataRequest request,Exception ex)
        {
            request.RequestEndTime = DateTime.UtcNow;
            _response.RequestMessage = request;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace InformaticaAPIs.Models
{
    public class DataRequest : HttpRequestMessage
    {
        public DateTime? RequestStartTime { get; set; }
        public DateTime? RequestEndTime { get; set; }

    }
}
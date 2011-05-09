using System.Web;
using System.Net;
using System.Collections.Specialized;

namespace OmnitureBeacon
{
    public class AnalyticsHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string queryString = context.Server.UrlDecode(context.Request.QueryString.ToString());
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "text/plain";
            HelperData dataHelper = new HelperData();
            NameValueCollection queryData = dataHelper.ParseQueryStringData(queryString);
            dataHelper.WriteValues(queryData);

        }
    }
}

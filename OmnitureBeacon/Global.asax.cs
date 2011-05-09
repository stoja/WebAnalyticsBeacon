using System;
using System.Net;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace OmnitureBeacon
{
    public class Global : HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            HttpException httpex = ex as HttpException;
            if (httpex != null && httpex.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                Server.ClearError();
                Response.Clear();
                IHttpHandler handler = new AnalyticsHandler();
                handler.ProcessRequest(Context);
            }
            else
            {
                //Logger.Write(ex.Message);
            }
        }
    }
}
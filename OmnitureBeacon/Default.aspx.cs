﻿using System;
using System.Collections.Specialized;
using System.Web;

namespace OmnitureBeacon
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IHttpHandler handler = new AnalyticsHandler();
            handler.ProcessRequest(this.Context);
        }

    }
}

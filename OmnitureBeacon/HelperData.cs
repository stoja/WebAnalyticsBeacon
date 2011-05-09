using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace OmnitureBeacon
{
    public class HelperData
    {
        private WebAnalyticsDataContext dataContext;
        private string[] exclusionList;
        private string[] urlPatternLibrary;

        public HelperData()
        {
            this.dataContext = new WebAnalyticsDataContext();
            this.ConfigureExclusions();
            this.ConfigureUrlStripParams();

        }

        /// <summary>
        /// Configures a list of omiture variable exclusions.
        /// </summary>
        private void ConfigureExclusions()
        {
            this.exclusionList = new string[]
                                     {
                                         "bw", "bh", 
                                         "c", "t", 
                                         "s", "p", 
                                         "j", "v", 
                                         "k", "ct", 
                                         "hp", "p", 
                                         "pid", "pidt",
                                         "oid", "ot",
                                         "oi", "r",
                                         "g", "c50"
                                     };

        }

        /// <summary>
        /// Configure query string blacklist data.
        /// </summary>
        public void ConfigureUrlStripParams()
        {
            this.urlPatternLibrary = new string[] { "?", 
                                                    "LoginID", 
                                                    "ID", 
                                                    "DateRange", 
                                                    "Sequence", 
                                                    "cid", 
                                                    "page", 
                                                    "sProfile", 
                                                    "catlocation", 
                                                    "savedID", 
                                                    "DJDKEY",
                                                    "JobID"
                                                  };
        }

        /// <summary>
        /// Parses the query string data into a name/value collection.
        /// </summary>
        /// <param name="url">Full url</param>
        /// <returns>Namevalue collection with all querystring data.</returns>
        public NameValueCollection ParseQueryStringData(string url)
        {
            try
            {
                NameValueCollection valueCollection = HttpUtility.ParseQueryString(url);
                return valueCollection;
            }
            catch (Exception e)
            {
                Logger.Write(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Strips any redundant query string data based on the a url parameter blacklist.
        /// </summary>
        /// <param name="inputUrl">The full input URL</param>
        /// <returns></returns>
        private string QueryPatternStrip(string inputUrl)
        {
            string cleanQuery = "";
            bool isModified = false;
            Uri url = null;
            if (!string.IsNullOrEmpty(inputUrl))
            {
                url = new Uri(inputUrl);
                NameValueCollection valueCollection = HttpUtility.ParseQueryString(url.Query);
                for (int i = 0; i < valueCollection.Count; i++)
                {
                    string value = valueCollection.GetKey(i);
                    if (!string.IsNullOrEmpty(value))
                    {
                        foreach (string blackListValue in this.urlPatternLibrary)
                        {
                            if (value.Equals(blackListValue))
                            {
                                valueCollection.Remove(blackListValue);
                                isModified = true;
                            }
                        }
                    }
                }
            }
            if (isModified)
            {
                return (url.Host + url.AbsolutePath + "?" + cleanQuery);
            }
            return inputUrl;
        }

        /// <summary>
        /// Write definition and result data into the specified tables in the database.
        /// </summary>
        /// <param name="inputData">A namevalue collection containing all data in the query string</param>
        public void WriteValues(NameValueCollection inputData)
        {
            try
            {
                if (inputData.Count > 0)
                {
                    // Selenium identifier string for checking the referrer field and stripping automation related referrer strings.
                    string seleniumIdent = "RemoteRunner";

                    string pageName = inputData["c7"];
                    string host = inputData["server"];
                    string url = this.QueryPatternStrip(inputData["g"]);
                    string referrer = this.QueryPatternStrip(inputData["r"]);
                    if (!string.IsNullOrEmpty(host) || !pageName.Equals("untaggedpage"))
                    {

                        if (string.IsNullOrEmpty(referrer) || referrer.Contains(seleniumIdent))
                        {
                            referrer = "undefined";
                        }
                        foreach (string key in inputData.AllKeys)
                        {
                            if ((key != null) && !this.exclusionList.Contains<string>(key))
                            {
                                int existingDefID = this.dataContext.GetDefinitionID(host, referrer, pageName, key);
                                if (existingDefID != 0)
                                {
                                    if (this.dataContext.DoesVarResultAlreadyExist(new int?(existingDefID), inputData[key]))
                                    {
                                        this.dataContext.UpdateResultTimeStamp(existingDefID, inputData[key]);
                                    }
                                    else
                                    {
                                        this.dataContext.InsertResult(existingDefID, inputData[key]);
                                    }
                                }
                                else
                                {
                                    this.dataContext.InsertNewDefinition(host, referrer, url, pageName, key);
                                    existingDefID = this.dataContext.GetDefinitionID(host, referrer, pageName, key);
                                    this.dataContext.InsertResult(existingDefID, inputData[key]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Write(e.Message);
            }

        }

    }
}

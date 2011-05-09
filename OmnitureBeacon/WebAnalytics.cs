using System.Collections.Generic;
using System.Data.Linq;
using System;
using System.Linq;

namespace OmnitureBeacon
{

    partial class WebAnalyticsDataContext
    {

        private static Func<WebAnalyticsDataContext, List<string>, IQueryable<int>> _getDefIdCompiledQuery;

        /// <summary>
        /// Returns the definition id of an existing definition if it exists - returns 0 if it doesn't.
        /// </summary>
        /// <param name="host">Name of the host</param>
        /// <param name="pageName">Omniture name of the page</param>
        /// <param name="varName">Name of the variable</param>
        /// <returns>An int value representing the definition id</returns>
        internal int GetDefinitionID(string host, string referrer, string pageName, string varName)
        {
            int? definitionID = 0;
            this.GetDefinitionID(host, referrer, pageName, varName, ref definitionID);

            return definitionID.GetValueOrDefault(0);
        }

        /// <summary>
        /// Inserts a new defintion into the Defninition table.
        /// </summary>
        /// <param name="host">Name of the host</param>
        /// <param name="url">String containing the calling page URL</param>
        /// <param name="pageName">Omniture name of the page</param>
        /// <param name="varName">Value of the Omniture variable</param>
        internal void InsertNewDefinition(string host, string referrer, string url, string pageName, string varName)
        {
            Definitions.InsertOnSubmit(new Definition()
            {
                host = host,
                referrer = referrer,
                url = url,
                page_name = pageName,
                var_name = varName
            });

            this.SubmitChanges();
        }

        /// <summary>
        /// Returns a boolean indicating whether a result already exists for the specified definition / result value.
        /// </summary>
        /// <param name="definitionID">Definition id</param>
        /// <param name="value">Omniture variable value</param>
        /// <returns></returns>
        internal bool DoesVarResultAlreadyExist(int? defID, string value)
        {
            int? definitionID = null;
            this.DoesVarResultAlreadyExist(defID, value, ref definitionID);

            return definitionID.HasValue;
        }

        /// <summary>
        /// Inserts a result into the Result table.
        /// </summary>
        /// <param name="definitionID">Definition id</param>
        /// <param name="value">Omniture variable value</param>
        internal void InsertResult(int definitionID, string value)
        {
            Results.InsertOnSubmit(new Result()
            {
                definition_id = definitionID,
                actual_result = value,
                time_stamp = DateTime.Now
            });

            this.SubmitChanges();
        }


        /// <summary>
        /// Updates the timestamp on an existing result value in the Results table.
        /// </summary>
        /// <param name="definitionID">Definition id</param>
        /// <param name="value">Omniture result value</param>
        internal void UpdateResultTimeStamp(int definitionID, string value)
        {
            var tblResult = this.Results;
            var resList = from results in tblResult
                          where results.definition_id.Equals(definitionID) && results.actual_result.Equals(value)
                          select results;

            foreach (Result res in resList)
            {
                res.time_stamp = DateTime.Now;
            }

            this.SubmitChanges();

        }

        /// <summary>
        /// Inserts a new baseline value into the Baseline table.
        /// </summary>
        /// <param name="definitionID">Definition id</param>
        /// <param name="value">Omniture variable value</param>
        internal void InsertBaselineValue(int definitionID, string value)
        {
            Baselines.InsertOnSubmit(new Baseline()
            {
                definition_id = definitionID,
                expected_value = value,
            });

            this.SubmitChanges();
        }
    }
}

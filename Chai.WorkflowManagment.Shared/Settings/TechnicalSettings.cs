using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Shared.Settings
{
    /// <summary>
    /// Technical parameters.
    /// </summary
    public static class TechnicalSettings
    {
        private const string VERSION   = "1.0.0.*";
        private const string COMPANY   = "CHAI Ethiopia";
        private const string PRODUCT   = "POC";
        private const string COPYRIGHT = "CHAI 2012";

        public static string SoftwareVersion
        {
            get { return string.Format("v{0}", VERSION); }
        }

        public static string Copyright
        {
            get { return COPYRIGHT; }
        }

        public static string Product
        {
            get { return PRODUCT; }
        }

        public static string Company
        {
            get { return COMPANY; }
        }

        public static string ConnectionString
        {
            get { return TechnicalConfig.GetConfiguration()["ConnectionString"]; }
        }
    }

}

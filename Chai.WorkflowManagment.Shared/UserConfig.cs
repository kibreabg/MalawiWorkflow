using System;
using System.Configuration;
using System.Xml;
using System.Collections.Specialized;

namespace Chai.WorkflowManagment.Shared
{
    /// <summary>
    /// Summary description for Config.
    /// </summary>
    public class UserConfig
    {
        private UserConfig()
        {
        }

        public static NameValueCollection GetConfiguration()
        {
            return (NameValueCollection)ConfigurationManager.GetSection("ChaiTechnicalSettings");
        }
    }

    public class ChaiUserSectionHandler : NameValueSectionHandler
    {
        protected override string KeyAttributeName
        {
            get { return "setting"; }
        }

        protected override string ValueAttributeName
        {
            get { return base.ValueAttributeName; }
        }
    }
}

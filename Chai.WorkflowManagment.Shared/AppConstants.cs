using System;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Shared
{

    public static class AppConstants
    {
        public const string TABID =  "tabid";
        public const string NODEID = "nodeid";
        public const string USERID = "userid";
        public const string ROLEID = "roleid";
        public const string MODULEID = "moduleid";
        public const string TASKPANID = "tpanid";
        public const string GDNOTEID = "gdnid";
        public const string GRNOTEID = "grnid";
        public const string PAGENAME = "pname";

        public const string PROVINCEID = "provinceid";
        public const string DISTRICTID = "districtid";
        public const string FACILITYID = "facilityid";

        public const string PRODUCTID = "proid";
        public const string TESTID = "testid";
        public static string DeleteConfirmation()
        {
            return "javascript:return confirm('Are you sure you want to delete it?');";
        }
    }
}

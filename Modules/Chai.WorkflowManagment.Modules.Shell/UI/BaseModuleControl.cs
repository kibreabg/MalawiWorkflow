using System;
using System.Collections.Generic;
using System.Text;
using Chai.WorkflowManagment.CoreDomain;

namespace Chai.WorkflowManagment.Modules.Shell
{
    public class BaseModuleControl : Microsoft.Practices.CompositeWeb.Web.UI.UserControl
    {
        private BaseMaster _baseMaster;
        private string _title;
        private bool _editAllowed = false;

        public BaseModuleControl()
        {
                 
        }
                 
        public BaseMaster BaseMaster
        {
            get { return _baseMaster; }
            set { _baseMaster = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public bool EditAllowed
        {
            get { return _editAllowed; }
            set { _editAllowed = value; }
        }
        
    }
}

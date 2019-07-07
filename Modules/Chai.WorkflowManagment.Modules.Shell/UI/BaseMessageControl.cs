using System;
using System.Collections.Generic;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Shell
{
    public class BaseMessageControl : Microsoft.Practices.CompositeWeb.Web.UI.UserControl
    {
        private AppMessage _message;

        public AppMessage Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public BaseMessageControl()
        {
        }
    }
}

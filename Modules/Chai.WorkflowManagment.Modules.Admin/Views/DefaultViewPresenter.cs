using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Collections;


namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public class DefaultViewPresenter : Presenter<IDefaultView>
    {
        private AdminController _controller;

        public DefaultViewPresenter([CreateNew] AdminController controller)
        {
            this._controller = controller;
        }
   
    }
}

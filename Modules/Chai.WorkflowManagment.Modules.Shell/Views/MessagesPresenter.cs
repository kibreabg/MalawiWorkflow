﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public class MessagesPresenter : Presenter<IMessagesView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IChai.WorkflowManagment.Modules.ShellController _controller;
        // public MessagesPresenter([CreateNew] IChai.WorkflowManagment.Modules.ShellController controller)
        // {
        // 		_controller = controller;
        // }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        // TODO: Handle other view events and set state in the view
    }
}





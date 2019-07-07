using System;
using System.Collections.Generic;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Shared.Events
{
    public class MessageEventArgs : EventArgs
    {
        private AppMessage _message;
        public MessageEventArgs(AppMessage message)
        {
            _message = message;
        }

        public MessageEventArgs(string msg, RMessageType mtype)
        {
            _message = new AppMessage(msg, mtype);
        }

        public AppMessage Message
        {
            get { return _message; }
        }
    }
}

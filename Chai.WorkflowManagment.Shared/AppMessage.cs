using System;
using System.Collections.Generic;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.Settings;

namespace Chai.WorkflowManagment.Shared
{
    public class AppMessage
    {
        private string _messageText;
        private RMessageType _messageType;

        public AppMessage(string message, RMessageType mtype)
        {
            _messageText = message;
            _messageType = mtype;
        }

        private AppMessage()
        {
        }

        public string MessageText
        {
            get { return _messageText; }
        }

        public RMessageType MessageType
        {
            get { return _messageType; }
        }

        public string IconFileName
        {
            get
            {
                return String.Format(UserSettings.GetMessageIcon, _messageType);
            }
        }
    }
}

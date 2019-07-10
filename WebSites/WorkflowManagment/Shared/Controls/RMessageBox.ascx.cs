using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Modules.Shell;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class RMessageBox : BaseMessageControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindMessage();
        }

        public void BindMessage()
        {
           // this.imgIcon.ImageUrl = Message.IconFileName;
            this.ltrMessage.Text = Message.MessageText;

            if (Message.MessageType == RMessageType.Info)
                this.panMessage.Attributes.Add("class", "alert alert-success fade in");
            else
                this.panMessage.Attributes.Add("class", "alert alert-danger fade in");
        }
    }
}


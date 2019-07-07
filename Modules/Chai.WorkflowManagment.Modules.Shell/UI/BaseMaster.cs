using System;
using System.Web.UI;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb.Web;

using Chai.WorkflowManagment.Shared.Events;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Modules.Shell.MasterPages;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Shell
{
    public class BaseMaster : Microsoft.Practices.CompositeWeb.Web.UI.MasterPage, IBaseMasterView
    {
        public delegate void MessageEventHandler(object sender, MessageEventArgs e);
        public event MessageEventHandler Message;
        private BaseMasterPresenter _presenter;
        public AppUser CurrentUser;
        public BaseMaster()
        {
            
           
            //if (Presenter.CurrentUser == null)
                //Presenter.Navigate("~/UserLogin.aspx");
            //}
            //else if (!Presenter.UserIsAuthenticated)
            //{
                //Presenter.Navigate("~/UserLogin.aspx");
            //}
        }
        
        [CreateNew]
        public BaseMasterPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessage(MessageEventArgs e)
        {
            if (Message != null)
            {
                Message(this, e);
            }
        }
       
        public string TabId
        {
            get { return Request.QueryString[AppConstants.TABID]; }
        }
        public string NodeId
        {
            get { return Request.QueryString[AppConstants.NODEID]; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>     
        public void ShowMessage(Chai.WorkflowManagment.Shared.AppMessage message)
        {
            MessageEventArgs e = new MessageEventArgs(message);
            OnMessage(e);
        }

        public void TransferMessage(Chai.WorkflowManagment.Shared.AppMessage message)
        {
            this.GetRMessaage = message;
        }
        
        protected void CheckTransferdMessage()
        {
            object msgObject =  GetRMessaage;
            if (msgObject != null && (msgObject is Chai.WorkflowManagment.Shared.AppMessage))
            {
                ShowMessage((Chai.WorkflowManagment.Shared.AppMessage)msgObject);
                this.GetRMessaage = null;
            }
        }

        private object GetRMessaage
        {
            get
            {
                return Presenter.CurrentContext.Session["RMESSAGE"];
            }
            set
            {
                Presenter.CurrentContext.Session["RMESSAGE"] = value;
            }
 
        }



        AppUser IBaseMasterView.CurrentUser
        {
            get
            {
                return CurrentUser;
            }
            set
            {
                CurrentUser = value;
            }
        }
    }
    
}

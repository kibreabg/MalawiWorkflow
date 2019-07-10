using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Services;

using Chai.WorkflowManagment.CoreDomain.Users;
using System.Web.UI;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class UserInfopage : Microsoft.Practices.CompositeWeb.Web.UI.UserControl, IUserInfoView
    {
        private UserInfoPresenter _presenter;
        AppUser _user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
            GetUserInfo();
        }

        [CreateNew]
        public UserInfoPresenter Presenter
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

        void GetUserInfo()
        {

            lblUser.Text = "|  " + _user.UserName;
        }
        protected void lnkChangepassword_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/ChangePassword.aspx?{0}=1", AppConstants.TABID));
        }
        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html


        public AppUser user
        {
            set { _user = value; }
        }
    }
}


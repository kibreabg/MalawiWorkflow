using System;
using System.Web;
using System.Web.UI;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public partial class AdminMaster : BaseMaster
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                base.Presenter.OnViewInitialized();
            }
            base.CheckTransferdMessage();
            base.Presenter.OnViewLoaded();
            UserApprover();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Message += new MessageEventHandler(AdminMaster_Message);
        }
        private void UserApprover()
        {
            foreach (AppUserRole userroles in CurrentUser.AppUserRoles)
            {

                if (userroles.Role.Name.Contains("Approver") || userroles.Role.Name.Contains("Reviewer") || userroles.Role.Name.Contains("Preparer") || userroles.Role.Name.Contains("Authorizer") || userroles.Role.Name.Contains("Payer"))
                {
                    lnkassign.Visible = true;
                    break;
                }
                else
                {
                    lnkassign.Visible = false;
                }
            }

        }
        protected void lnkassign_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/Admin/AssignJob.aspx?{0}=0", Chai.WorkflowManagment.Shared.AppConstants.TABID));
        }
        void AdminMaster_Message(object sender, Chai.WorkflowManagment.Shared.Events.MessageEventArgs e)
        {
            BaseMessageControl ctr = (BaseMessageControl)Page.LoadControl("~/Shared/Controls/RMessageBox.ascx");
            ctr.Message = e.Message;
            this.plhMessage.Controls.Add(ctr);   
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                EmailSender.SendEmails(txtFrom.Text, txtTo.Text, txtSubject.Text, txtMessage.Text);
                lblMessage.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
                clearControls();
            }
            catch (Exception ex)
            {

            }
        }
        private void clearControls()
        {
            txtFrom.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";

        }
    }
}

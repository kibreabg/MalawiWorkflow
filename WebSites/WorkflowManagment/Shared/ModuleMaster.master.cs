using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.CoreDomain;
using System.Web.UI;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Shell.MasterPages
{
    public partial class ModuleMaster : BaseMaster
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                base.Presenter.OnViewInitialized();
            }

            base.CheckTransferdMessage();
            base.Presenter.OnViewLoaded();
            UserRole();
            UserApprover();
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Message += new MessageEventHandler(ModuleMaster_Message);
        }
        private void UserRole()
        {
            foreach (AppUserRole userroles in CurrentUser.AppUserRoles)
            {
                if (userroles.Role.Name.Contains("Administrator"))
                {
                    lnkAdmin.Visible = true;
                    break;
                }
                else
                {
                    lnkAdmin.Visible = false;
                }

            }

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
        protected void lnkAdmin_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/Admin/Default.aspx?{0}=0", Chai.WorkflowManagment.Shared.AppConstants.TABID));
        }
        protected void lnkassign_Click(object sender, EventArgs e)
        {
            this.Page.Response.Redirect(string.Format("~/Admin/AssignJob.aspx?{0}=0", Chai.WorkflowManagment.Shared.AppConstants.TABID));
        }
        protected void ModuleMaster_Message(object sender, Chai.WorkflowManagment.Shared.Events.MessageEventArgs e)
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

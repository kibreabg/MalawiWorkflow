using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.Settings;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class AssignJob : Microsoft.Practices.CompositeWeb.Web.UI.Page, IAssignJobView
    {
        private AssignJobPresenter _presenter;
        private bool status = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                PopUser();
                BindUserControls();

            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentAssignJob.Status == true)
            {
                btnUnassign.Enabled = true;
                btnSave.Enabled = false;
            }
            else
            {
                btnUnassign.Enabled = false;
                btnSave.Enabled = true;
            }
        }

        [CreateNew]
        public AssignJobPresenter Presenter
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
        private void PopUser()
        {
            ddlAssignTo.Items.Add(new ListItem("None", "0"));
            ddlAssignTo.DataSource = _presenter.GetApprovers();
            ddlAssignTo.DataBind();
        }
        private void BindUserControls()
        {

            this.ddlAssignTo.SelectedValue = _presenter.CurrentAssignJob.AssignedTo.ToString();

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    _presenter.SaveOrUpdateAssignJob();
                    _presenter.SaveorUpdateUser(true);
                    SendEmail(true);
                    Master.ShowMessage(new AppMessage("Assigned successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));


                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SendEmail(bool value)
        {
            if (value)
            {
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentAssignJob.AssignedTo).Email, "You are assigned as approver by '" + _presenter.CurrentAssignJob.AppUser.FullName + "'", "Assigned Job");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentAssignJob.AssignedTo).Email, "You are Unassigned from your approver by '" + _presenter.CurrentAssignJob.AppUser.FullName + "'", "Unassigned Job");
            }




        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        public int GetAssisnTo
        {
            get { return Convert.ToInt32(ddlAssignTo.SelectedValue); }
        }
        public bool Getstatus
        {
            get { return status; }
        }
        public int GetId
        {
            get
            {
                if (_presenter.GetAssignedJobbycurrentuser() != null)
                    return _presenter.GetAssignedJobbycurrentuser().Id;
                else
                    return 0;

            }


        }
        protected void btnUnassign_Click(object sender, EventArgs e)
        {
            try
            {
                 status = false;
                _presenter.SaveOrUpdateAssignJob();
                _presenter.SaveorUpdateUser(false);
                SendEmail(false);

                Master.ShowMessage(new AppMessage("Unassigned successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));


            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
    }
}



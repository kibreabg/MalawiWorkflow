using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Setting;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmLeaveApproval : POCBasePage, ILeaveApprovalView
    {
        private LeaveApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private LeaveRequest _leaverequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
            }

            //BindJS();
            this._presenter.OnViewLoaded();


            BindSearchLeaveRequestGrid();
            if (_presenter.CurrentLeaveRequest.Id != 0)
            {
                BindLeaveRequestforprint();
            }
            //btnPrint.Attributes.Add("onclick", "javascript:Clickheretoprint('divprint'); return false;");

        }
        //protected void BindJS()
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "workflowscripts", String.Format("<script language=\"JavaScript\" src=\"http://localhost/WorkflowManagment/WorkflowManagment.js\"></script>\n"));
        //}
        [CreateNew]
        public LeaveApprovalPresenter Presenter
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
        public override string PageID
        {

            get
            {
                return "{282224A8-DCCA-4FED-AAB1-BEB6A5AA0653}";
            }
        }
        public CoreDomain.Request.LeaveRequest LeaveRequest
        {
            get
            {
                return _leaverequest;
            }
            set
            {
                _leaverequest = value;
            }
        }
        public string RequestNo
        {
            get { return txtRequestNosearch.Text; }
        }
        public string RequestDate
        {
            get { return txtRequestDatesearch.Text; }
        }
        public int LeaveRequestId
        {
            get
            {
                if (grvLeaveRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvLeaveRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        private void PopApprovalStatus()
        {
            ddlApprovalStatus.Items.Clear();
            ddlApprovalStatus.Items.Add(new ListItem("Select Status", "0"));
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 3) == s[i].Substring(0, 3))
                {
                    ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                }

            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.Leave_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if ((AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager") && _presenter.CurrentLeaveRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                else if (_presenter.GetUser(_presenter.CurrentLeaveRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }


        }
        private void BindLeaveRequestStatus()
        {
            foreach (LeaveRequestStatus LRS in _presenter.CurrentLeaveRequest.LeaveRequestStatuses)
            {
                if (LRS.WorkflowLevel == _presenter.CurrentLeaveRequest.CurrentLevel && _presenter.CurrentLeaveRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    ddlApprovalStatus.SelectedValue = LRS.ApprovalStatus;
                    txtRejectedReason.Text = LRS.RejectedReason;
                    btnApprove.Enabled = true;

                }

            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentLeaveRequest.CurrentLevel == _presenter.CurrentLeaveRequest.LeaveRequestStatuses.Count && _presenter.CurrentLeaveRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                SendEmailToRequester();

            }

        }
        private void SendEmail(LeaveRequestStatus LRS)
        {
            if (_presenter.GetUser(LRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(LRS.Approver).Email, "Leave Request", (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + " Requests for Leave with Leave Request No. - '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(LRS.Approver).AssignedTo).Email, "Leave Request", (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + " Requests for Leave with Leave Request No. - '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "'");
            }




        }
        private void SendEmailRejected(LeaveRequestStatus LRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).Email, "Leave Request Rejection", "Your Leave Request with Leave Request No. " + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (LRS.RejectedReason).ToUpper() + "'");

            if (LRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < LRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentLeaveRequest.LeaveRequestStatuses[i].Approver).Email, "Leave Request Rejection", "Leave Request with Leave Request No. - " + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (LRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).Email, "Leave Request ", "Your Leave Request with Leave Request No. - '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "' was Completed.");
        }
        private void GetNextApprover()
        {
            foreach (LeaveRequestStatus LRS in _presenter.CurrentLeaveRequest.LeaveRequestStatuses)
            {
                if (LRS.ApprovalStatus == null)
                {
                    SendEmail(LRS);
                    _presenter.CurrentLeaveRequest.CurrentApprover = LRS.Approver;
                    _presenter.CurrentLeaveRequest.CurrentLevel = LRS.WorkflowLevel;
                    _presenter.CurrentLeaveRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    _presenter.CurrentLeaveRequest.CurrentStatus = LRS.ApprovalStatus;
                    break;

                }
            }
        }
        private void SaveLeaveRequestStatus()
        {
            foreach (LeaveRequestStatus LRS in _presenter.CurrentLeaveRequest.LeaveRequestStatuses)
            {
                if ((LRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(LRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(LRS.Approver).AssignedTo : 0)) && LRS.WorkflowLevel == _presenter.CurrentLeaveRequest.CurrentLevel)
                {
                    LRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    LRS.RejectedReason = txtRejectedReason.Text;
                    LRS.ApprovalDate = DateTime.Today.Date;
                    LRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(LRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(LRS.Approver).AppUser.FullName : "";
                    if (LRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {

                        if (_presenter.CurrentLeaveRequest.CurrentLevel == _presenter.CurrentLeaveRequest.LeaveRequestStatuses.Count)
                        {
                            _presenter.CurrentLeaveRequest.CurrentApprover = LRS.Approver;
                            _presenter.CurrentLeaveRequest.CurrentLevel = LRS.WorkflowLevel;
                            _presenter.CurrentLeaveRequest.CurrentStatus = LRS.ApprovalStatus;
                            _presenter.CurrentLeaveRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        LRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(LRS.Approver).FullName + " has " + LRS.ApprovalStatus + " Leave Request made by " + _presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName);
                    }
                    else
                    {
                        _presenter.CurrentLeaveRequest.CurrentApprover = LRS.Approver;
                        _presenter.CurrentLeaveRequest.CurrentLevel = LRS.WorkflowLevel;
                        _presenter.CurrentLeaveRequest.CurrentStatus = LRS.ApprovalStatus;
                        _presenter.CurrentLeaveRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        LRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(LRS);
                        Log.Info(_presenter.GetUser(LRS.Approver).FullName + " has " + LRS.ApprovalStatus + " Leave Request made by " + _presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName);
                    }
                    break;
                }

            }

        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {

                if (_presenter.CurrentLeaveRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveLeaveRequestStatus();
                    _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);
                    CalculateLeavetaken();
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Leave Approval Processed ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Leave Approval Rejected ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }

                    btnApprove.Enabled = false;
                    BindSearchLeaveRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                BindLeaveRequestforprint();
            }
            catch (Exception ex)
            {

            }


        }
        private decimal CalculateLeave(EmployeeLeave empleave)
        {
            decimal workingdays = Convert.ToDecimal((DateTime.Today.Date - empleave.StartDate).TotalDays);
            decimal leavedays = (workingdays / 30) * empleave.Rate;
            decimal res = empleave.BeginingBalance + leavedays - empleave.LeaveTaken;
            if (res < 0)
                return 0;
            else
                return Math.Round(res); ;

        }
        protected void grvLeaveRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Session["ApprovalLevel"] = true;

            //  Convert.ToInt32(grvLeaveRequestList.SelectedDataKey.Value);
            //pnlApproval.Visible = true;

            _presenter.OnViewLoaded();
            PopApprovalStatus();
            pnlApproval_ModalPopupExtender.Show();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            btnApprove.Enabled = true;
            lblLeaveTyperes.Text = _presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName;
            lblrequesteddaysres.Text = _presenter.CurrentLeaveRequest.RequestedDays.ToString();
            if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName.Contains("Annual"))
            {
                lblViewBalance.Visible = true;
                lblViewBalRes.Visible = true;
                EmployeeLeave empleave = _presenter.GetEmployeeLeave(_presenter.CurrentLeaveRequest.Requester);
                if (empleave != null)
                {
                    lblViewBalRes.Text = CalculateLeave(empleave).ToString();//calculate leave
                }
                else
                {
                    lblViewBalRes.Text = "Emplyee Annual Leave setting is not defined,Please Contact HR Officer.";
                }
            }
            BindLeaveRequestStatus();
            ShowPrint();
            BindLeaveRequestforprint();

        }
        private void CalculateLeavetaken()
        {
            if (_presenter.CurrentLeaveRequest.CurrentLevel == _presenter.CurrentLeaveRequest.LeaveRequestStatuses.Count && _presenter.CurrentLeaveRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName.Contains("Annual"))
                {
                    EmployeeLeave empleave = _presenter.GetEmployeeLeaveforEdit(_presenter.CurrentLeaveRequest.Requester);
                    empleave.LeaveTaken = empleave.LeaveTaken + _presenter.CurrentLeaveRequest.RequestedDays;
                    _presenter.SaveOrUpdateEmployeeLeave(empleave);
                }
            }
        }
        protected void grvLeaveRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


        }
        protected void grvLeaveRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            LeaveRequest LR = e.Row.DataItem as LeaveRequest;
            if (LR != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(LR.Requester).FullName;
                    }
                }
                if (LR.ProgressStatus == ProgressStatus.InProgress.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                }
                else if (LR.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");

                }
            }

        }
        protected void grvLeaveRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLeaveRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {

            BindSearchLeaveRequestGrid();

            // pnlPopUpSearch_ModalPopupExtender.Show();
        }
        private void BindSearchLeaveRequestGrid()
        {
            grvLeaveRequestList.DataSource = _presenter.ListLeaveRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text, ddlProgressStatus.SelectedValue);
            grvLeaveRequestList.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval.Visible = false;
            pnlApproval_ModalPopupExtender.Hide();
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
                rfvRejectedReason.Enabled = true;
            }
            pnlApproval_ModalPopupExtender.Show();
        }
        private void BindLeaveRequestforprint()
        {
            if (_presenter.CurrentLeaveRequest.Id > 0)
            {
                lblRequestNoresult.Text = _presenter.CurrentLeaveRequest.RequestNo;
                lblRequestedDateresult.Text = _presenter.CurrentLeaveRequest.RequestedDate.ToShortDateString();
                lblleavetyperesp.Text = _presenter.CurrentLeaveRequest.LeaveType != null ? _presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName.ToString() : "0";
                lblDatefromres.Text = _presenter.CurrentLeaveRequest.DateFrom.ToShortDateString();
                lblDatetores.Text = _presenter.CurrentLeaveRequest.DateTo.ToShortDateString();
                lblrequesteddaysresp.Text = _presenter.CurrentLeaveRequest.RequestedDays.ToString();
                lblbalanceres.Text = _presenter.CurrentLeaveRequest.Balance.ToString();
                lblapprovalstatusres.Text = _presenter.CurrentLeaveRequest.CurrentStatus;
                lblRequesterres.Text = _presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName;
                lblEmpNoRes.Text = _presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).EmployeeNo;

                grvStatuses.DataSource = _presenter.CurrentLeaveRequest.LeaveRequestStatuses;
                grvStatuses.DataBind();
            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentLeaveRequest.LeaveRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentLeaveRequest.LeaveRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentLeaveRequest.LeaveRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
    }
}
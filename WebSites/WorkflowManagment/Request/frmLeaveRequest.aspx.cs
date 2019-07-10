using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Reflection;
using log4net.Config;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmLeaveRequest : POCBasePage, ILeaveRequestView
    {
        private LeaveRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private LeaveRequest _leaverequest;
        private int _leaverequestId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                CheckApprovalSettings();
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopLeaveType();
                BindSearchLeaveRequestGrid();
                // if (_presenter.CurrentLeaveRequest.Id <= 0)
                //{
                //    AutoNumber();
                // }
            }
            this._presenter.OnViewLoaded();
            BindInitialValues();


        }

        private string AutoNumber()
        {
            return "LR-" + (_presenter.GetLastLeaveRequestId() + 1).ToString();
        }
        [CreateNew]
        public LeaveRequestPresenter Presenter
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
                return "{EEB211B3-70FC-429B-8662-927A0C8A9511}";
            }
        }
        private void IsFindPostBack()
        {
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindInitialValues()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            txtRequester.Text = CurrentUser.FirstName + " " + CurrentUser.LastName;
            //txtEmployeeNo.Text = CurrentUser.EmployeeNo;
            if (_presenter.CurrentLeaveRequest.Id <= 0)
            {

                txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
               

            }
        }
        private void BindLeaveRequest()
        {



            if (_presenter.CurrentLeaveRequest.Id > 0)
            {



                //txtRequestNo.Text = _presenter.CurrentLeaveRequest.RequestNo;
                txtRequestDate.Text = _presenter.CurrentLeaveRequest.RequestedDate.ToString();
                ddlLeaveType.SelectedValue = _presenter.CurrentLeaveRequest.LeaveType != null ? _presenter.CurrentLeaveRequest.LeaveType.Id.ToString() : "0";
                txtDateFrom.Text = _presenter.CurrentLeaveRequest.DateFrom.ToString();
                txtDateTo.Text = _presenter.CurrentLeaveRequest.DateTo.ToString();
                txtAddress.Text = _presenter.CurrentLeaveRequest.Addresswhileonleave;
                txtapplyfor.Text = _presenter.CurrentLeaveRequest.RequestedDays.ToString();
                txtCompReason.Text = _presenter.CurrentLeaveRequest.CompassionateReason;
                txtbalance.Text = _presenter.CurrentLeaveRequest.Balance.ToString();
                txtforward.Text = _presenter.CurrentLeaveRequest.Forward.ToString();
                SelectedLeaveType();
            }
        }
        private void SaveLeaveRequest()
        {
            AppUser CurrentUser = _presenter.CurrentUser();
            try
            {
                _presenter.CurrentLeaveRequest.Requester = CurrentUser.Id;
                _presenter.CurrentLeaveRequest.EmployeeNo = txtEmployeeNo.Text;
                _presenter.CurrentLeaveRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentLeaveRequest.RequestNo = AutoNumber();
                _presenter.CurrentLeaveRequest.RequestedDate = Convert.ToDateTime(txtRequestDate.Text);
                _presenter.CurrentLeaveRequest.LeaveType = _presenter.GetLeaveType(int.Parse(ddlLeaveType.SelectedValue));
                _presenter.CurrentLeaveRequest.DateFrom = Convert.ToDateTime(txtDateFrom.Text);
                _presenter.CurrentLeaveRequest.DateTo = Convert.ToDateTime(txtDateTo.Text);
                _presenter.CurrentLeaveRequest.Addresswhileonleave = txtAddress.Text;
                _presenter.CurrentLeaveRequest.RequestedDays = int.Parse(txtapplyfor.Text);
                _presenter.CurrentLeaveRequest.CompassionateReason = txtCompReason.Text;
                _presenter.CurrentLeaveRequest.Balance = txtbalance.Text != "" ? int.Parse(txtbalance.Text) : 0;
                _presenter.CurrentLeaveRequest.Forward = txtforward.Text != "" ? int.Parse(txtforward.Text) : 0;

                SaveLeaveRequestStatus();
               
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }

            }

        }
        private void SaveLeaveRequestStatus()
        {
            if (_presenter.CurrentLeaveRequest.Id <= 0)
            {
                if (_presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0) != null)
                {
                    int i = 1;
                    foreach (ApprovalLevel AL in _presenter.GetApprovalSetting(RequestType.Leave_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                    {
                        LeaveRequestStatus LRS = new LeaveRequestStatus();
                        LRS.LeaveRequest = _presenter.CurrentLeaveRequest;
                        if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                        {
                            if (_presenter.CurrentUser().Superviser.Value != 0)
                            {
                                LRS.Approver = _presenter.CurrentUser().Superviser.Value;
                            }
                            else
                            {
                                LRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                                LRS.ApprovalDate = DateTime.Today.Date;
                            }
                        }
                        else
                        {
                            LRS.Approver = _presenter.Approver(AL.EmployeePosition.Id).Id;
                        }
                        LRS.WorkflowLevel = i;
                        i++;
                        _presenter.CurrentLeaveRequest.LeaveRequestStatuses.Add(LRS);

                    }
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (!(ddlLeaveType.SelectedItem.Text == "Annual Leave" && int.Parse(txtbalance.Text) <= 0))
            {
                foreach (LeaveRequestStatus LRS in _presenter.CurrentLeaveRequest.LeaveRequestStatuses)
                {
                    if (LRS.ApprovalStatus == null)
                    {
                        SendEmail(LRS);
                        _presenter.CurrentLeaveRequest.CurrentApprover = LRS.Approver;
                        _presenter.CurrentLeaveRequest.CurrentLevel = LRS.WorkflowLevel;
                        _presenter.CurrentLeaveRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;

                    }
                }
            }
        }
        private void SendEmail(LeaveRequestStatus LRS)
        {
            if (_presenter.GetSuperviser(LRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetSuperviser(LRS.Approver).Email, "Leave Request", "'" + (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + "' Requests for Leave Request No. '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(LRS.Approver).AssignedTo).Email, "Leave Request", "'" + (_presenter.GetUser(_presenter.CurrentLeaveRequest.Requester).FullName).ToUpper() + "' Requests for Leave Request No. '" + (_presenter.CurrentLeaveRequest.RequestNo).ToUpper() + "' ");
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
                if (_leaverequestId != 0)
                {
                    return _leaverequestId;
                }
                else
                {
                    return 0;
                }
            }
        }
        protected void btnRequest_Click(object sender, EventArgs e)
        {

            SaveLeaveRequest();

            if (_presenter.CurrentLeaveRequest.LeaveRequestStatuses.Count != 0)
            {
                if (ddlLeaveType.SelectedItem.Text != "Annual Leave")
                {
                    GetCurrentApprover();
                    _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);
                   
                    ClearForm();
                    BindSearchLeaveRequestGrid();
                    Master.ShowMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested for a Leave Type of " + ddlLeaveType.SelectedValue);
                }
                else if (ddlLeaveType.SelectedItem.Text == "Annual Leave" && Convert.ToInt32(txtapplyfor.Text) < (txtforward.Text != "" ? Convert.ToInt32(txtforward.Text) : 0))
                    {
                        GetCurrentApprover();
                        _presenter.SaveOrUpdateLeaveRequest(_presenter.CurrentLeaveRequest);
                        ClearForm();
                        BindSearchLeaveRequestGrid();
                        Master.ShowMessage(new AppMessage("Successfully did a Leave  Request, Reference No - <b>'" + _presenter.CurrentLeaveRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        Log.Info(_presenter.CurrentUser().FullName + " has requested for a Leave Type of " + ddlLeaveType.SelectedValue);
                    }
                    else
                    { Master.ShowMessage(new AppMessage("You don't have sufficient Annual Leave days", Chai.WorkflowManagment.Enums.RMessageType.Error)); }
                
            }
            else
            {
                Master.ShowMessage(new AppMessage("There is an error constracting Approval Process", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
        }
        private void ClearForm()
        {
            //txtRequestNo.Text = "";
            txtRequestDate.Text = "";
            ddlLeaveType.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtAddress.Text = "";
            txtapplyfor.Text = "";
            txtCompReason.Text = "";
            txtbalance.Text = "";
            txtforward.Text = "";

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmLeaveRequest.aspx");
        }
        protected void grvLeaveRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Session["ApprovalLevel"] = true;
            // ClearForm();
            //BindLeaveRequest();
            _leaverequestId = Convert.ToInt32(grvLeaveRequestList.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            BindLeaveRequest();
            if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName.Contains("Annual Leave"))
            {
                lblBalance.Visible = true;
                txtbalance.Visible = true;
                lblforward.Visible = true;
                txtforward.Visible = true;
            }
        }
        protected void grvLeaveRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeleteLeaveRequestg(_presenter.GetLeaveRequestById(Convert.ToInt32(grvLeaveRequestList.DataKeys[e.RowIndex].Value)));

            btnFind_Click(sender, e);
            Master.ShowMessage(new AppMessage("Leave Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));

        }
        protected void grvLeaveRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //LeaveRequest leaverequest = e.Row.DataItem as LeaveRequest;
            //if (leaverequest != null)
            //{
            //    if (leaverequest.GetLeaveRequestStatusworkflowLevel(1).ApprovalStatus != null)
            //    {
            //        e.Row.Cells[5].Enabled = false;
            //        e.Row.Cells[6].Enabled = false;
            //    }

            //}
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvLeaveRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvLeaveRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        private void PopLeaveType()
        {
            ddlLeaveType.DataSource = _presenter.GetLeaveTypes();
            ddlLeaveType.DataBind();

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchLeaveRequestGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            // pnlPopUpSearch_ModalPopupExtender.Show();
        }
        private void BindSearchLeaveRequestGrid()
        {
            grvLeaveRequestList.DataSource = _presenter.ListLeaveRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text);
            grvLeaveRequestList.DataBind();
        }
        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void SelectedLeaveType()
        {
            if (_presenter.CurrentLeaveRequest.LeaveType != null)
            {
                if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Annual")
                {
                    txtAddress.Visible = true;
                    lblAddress.Visible = true;
                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    lblBalance.Visible = true;
                    lblforward.Visible = true;
                    txtforward.Visible = true;
                    txtbalance.Visible = true;

                }
                else if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Compassionate")
                {
                    txtCompReason.Visible = true;
                    lblCompReason.Visible = true;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;



                }
                else if (_presenter.CurrentLeaveRequest.LeaveType.LeaveTypeName == "Sick leave")
                {

                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;


                }
                else
                {

                    txtCompReason.Visible = false;
                    lblCompReason.Visible = false;
                    txtAddress.Visible = false;
                    lblAddress.Visible = false;
                    lblBalance.Visible = false;
                    lblforward.Visible = false;
                    txtforward.Visible = false;
                    txtbalance.Visible = false;
                }
            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        private decimal CalculateLeave(EmployeeLeave empleave)
        {
            decimal workingdays = Convert.ToDecimal((DateTime.Today.Date - empleave.StartDate).TotalDays);
            decimal leavedays = (workingdays / 30) * empleave.Rate;
            decimal res = (empleave.BeginingBalance + leavedays) - empleave.LeaveTaken;
            if (res < 0)
                return 0;
            else
                return Math.Round(res);

        }
        protected void ddlLeaveType_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddlLeaveType.SelectedItem.Text.Contains("Annual"))
            {
                txtAddress.Visible = true;
                lblAddress.Visible = true;

                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                lblBalance.Visible = true;
                lblforward.Visible = true;
                txtforward.Visible = true;
                txtbalance.Visible = true;
                EmployeeLeave empleave = _presenter.GetEmployeeLeave();
                if (empleave != null)
                {
                    txtforward.Text = CalculateLeave(empleave).ToString();
                    lblOpeningBalance.Visible = true;
                    lblOBValue.Visible = true;
                    lblOBValue.Text = Convert.ToInt32(empleave.BeginingBalance).ToString();
                }
                else
                {
                    lblnoempleavesetting.Text = "Your Leave setting is not defined,Please contact HR Officer.";
                }


            }
            else if (ddlLeaveType.SelectedItem.Text.Contains("Compassionate"))
            {
                txtCompReason.Visible = true;
                lblCompReason.Visible = true;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;


            }
            else if (ddlLeaveType.SelectedItem.Text.Contains("Sick Leave"))
            {


                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;


            }
            else
            {


                txtCompReason.Visible = false;
                lblCompReason.Visible = false;
                txtAddress.Visible = false;
                lblAddress.Visible = false;
                lblBalance.Visible = false;
                lblforward.Visible = false;
                txtforward.Visible = false;
                txtbalance.Visible = false;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (_presenter.CurrentLeaveRequest.Id > 0)
            {
                _presenter.DeleteLeaveRequestg(_presenter.CurrentLeaveRequest);
                ClearForm();

                btnDelete.Visible = false;
                Master.ShowMessage(new AppMessage("Leave Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
        }
        private void GetLeaveBalance()
        {
            if (ddlLeaveType.SelectedItem.Text == "Annual Leave")
            {
                if (txtforward.Text != "" && txtapplyfor.Text != "")
                {
                    int bal = (Convert.ToInt32(txtforward.Text) - Convert.ToInt32(txtapplyfor.Text));
                    if (bal < 0)

                        txtbalance.Text = Convert.ToString(0);
                    else
                        txtbalance.Text = bal.ToString();
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please Insert Leave day's brought forward OR I wish to apply for ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
           // CalculateRequestedDays();
        }
        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
            //CalculateRequestedDays();
            //txtforward_TextChanged(sender, e);
            //txtapplyfor_TextChanged(sender, e);
        }
        private void CalculateRequestedDays()
        {
            DateTime Datefrom = Convert.ToDateTime(txtDateFrom.Text);
            DateTime DateTo = Convert.ToDateTime(txtDateTo.Text);
            TimeSpan interval = DateTo - Datefrom;
            txtapplyfor.Text = interval.TotalDays.ToString();
        }
        protected void txtforward_TextChanged(object sender, EventArgs e)
        {
            GetLeaveBalance();
        }
        protected void txtapplyfor_TextChanged(object sender, EventArgs e)
        {
            GetLeaveBalance();
        }
    }
}
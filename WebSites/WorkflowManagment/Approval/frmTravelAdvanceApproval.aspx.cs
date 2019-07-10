using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmTravelAdvanceApproval : POCBasePage, ITravelAdvanceApprovalView
    {
        private TravelAdvanceApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
            }
            this._presenter.OnViewLoaded();
            BindSearchTravelAdvanceRequestGrid();
            if (_presenter.CurrentTravelAdvanceRequest.Id != 0)
            {

                PrintTransaction();
            }

        }
        [CreateNew]
        public TravelAdvanceApprovalPresenter Presenter
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
                return "{DBC4CB73-69E5-42F3-84F8-09F8EA69B06D}";
            }
        }

        #region Field Getters
        public int GetTravelAdvanceRequestId
        {
            get
            {
                if (grvTravelAdvanceRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvTravelAdvanceRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        private void BindAccounts()
        {
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count == _presenter.CurrentTravelAdvanceRequest.CurrentLevel && (_presenter.CurrentUser().EmployeePosition.PositionName == "Finance Officer" || _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.CurrentApprover).IsAssignedJob == true))
            {
                lblAccount.Visible = true;
                lblAccountdd.Visible = true;
            }
            ddlAccount.Items.Clear();
            ddlAccount.Items.Add(new ListItem("Select Account", "0"));
            ddlAccount.DataSource = _presenter.GetAccounts();
            ddlAccount.DataBind();

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
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count == _presenter.CurrentTravelAdvanceRequest.CurrentLevel)
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.TravelAdvance_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentTravelAdvanceRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                /*else if (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                        {
                            will = AL.Will;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.CurrentApproverPosition == AL.EmployeePosition.Id)
                        {
                            will = AL.Will;
                        }
                    }
                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        }
        private void BindSearchTravelAdvanceRequestGrid()
        {
            grvTravelAdvanceRequestList.DataSource = _presenter.ListTravelAdvanceRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvTravelAdvanceRequestList.DataBind();
        }
        private void BindTravelAdvanceRequestStatus()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if (TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.CurrentLevel && _presenter.CurrentTravelAdvanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }
                if (TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count && TARS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Last().PaymentType == "Bank Payment")
                        btnBankPayment.Visible = true;
                }
                else
                    btnPrint.Enabled = false;
            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentTravelAdvanceRequest.CurrentLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count && _presenter.CurrentTravelAdvanceRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Last().PaymentType == "Bank Payment")
                    btnBankPayment.Visible = true;
                SendEmailToRequester();
            }
        }
        private void SendEmail(TravelAdvanceRequestStatus TARS)
        {
            if (TARS.Approver != 0)
            {
                if (_presenter.GetUser(TARS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetSuperviser(TARS.Approver).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                }
                else
                {
                    EmailSender.Send(_presenter.GetSuperviser(_presenter.GetAssignedJobbycurrentuser(TARS.Approver).AssignedTo).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(TARS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        EmailSender.Send(Payer.Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                    }
                    else
                    {
                        EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Travel Advance Approval", (_presenter.CurrentTravelAdvanceRequest.AppUser.FullName).ToUpper() + " Requests for Travel Advance with Travel Advance No. - " + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper());
                    }
                }
            }


        }
        private void SendEmailRejected(TravelAdvanceRequestStatus TARS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).Email, "Travel Advance Request Rejection", "Your Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (TARS.RejectedReason).ToUpper() + "'");

            if (TARS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < TARS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[i].Approver).Email, "Travel Advance Request Rejection", "Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (TARS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            if (_presenter.CurrentTravelAdvanceRequest.CurrentStatus != ApprovalStatus.Rejected.ToString())
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.AppUser.Id).Email, "Tavel Adavnce Completion", "Your Travel Advance Request with Travel Advance No. - '" + (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo).ToUpper() + "' was Completed, Please collect your payment");
        }
        private void GetNextApprover()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if (TARS.ApprovalStatus == null)
                {
                    if (TARS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentTravelAdvanceRequest.CurrentApproverPosition = TARS.ApproverPosition;
                    }
                    SendEmail(TARS);
                    _presenter.CurrentTravelAdvanceRequest.CurrentApprover = TARS.Approver;
                    _presenter.CurrentTravelAdvanceRequest.CurrentLevel = TARS.WorkflowLevel;
                    _presenter.CurrentTravelAdvanceRequest.CurrentStatus = TARS.ApprovalStatus;
                    _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo;
            lblRequestedDateResult.Text = _presenter.CurrentTravelAdvanceRequest.RequestDate.ToString();
            lblRequesterResult.Text = _presenter.CurrentTravelAdvanceRequest.AppUser.FullName;
            lblVisitingTeamResult.Text = _presenter.CurrentTravelAdvanceRequest.VisitingTeam;
            lblPurposeOfTravelResult.Text = _presenter.CurrentTravelAdvanceRequest.PurposeOfTravel;
            lblCommentsResult.Text = _presenter.CurrentTravelAdvanceRequest.Comments.ToString();
            lblTotalTravelAdvanceResult.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentTravelAdvanceRequest.ProgressStatus.ToString();
            lblProjectIdResult.Text = _presenter.CurrentTravelAdvanceRequest.Project.ProjectCode;
            lblGrantIdResult.Text = _presenter.CurrentTravelAdvanceRequest.Grant.GrantCode;

            grvDetails.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses;
            grvStatuses.DataBind();


            foreach (TravelAdvanceRequestDetail detail in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails)
            {
                grvCost.DataSource = detail.TravelAdvanceCosts;
                grvCost.DataBind();
            }
        }
        private void SaveTravelAdvanceRequestStatus()
        {
            foreach (TravelAdvanceRequestStatus TARS in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses)
            {
                if ((TARS.Approver == _presenter.CurrentUser().Id || (TARS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(TARS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(TARS.Approver).AssignedTo : 0)) && TARS.WorkflowLevel == _presenter.CurrentTravelAdvanceRequest.CurrentLevel)
                {
                    TARS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    TARS.Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    TARS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(TARS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(TARS.Approver).AppUser.FullName : "";
                    TARS.RejectedReason = txtRejectedReason.Text;
                    if (TARS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.CurrentLevel == _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses.Count)
                        {
                            _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                            _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationStatus = ProgressStatus.Completed.ToString();

                            TARS.Approver = _presenter.CurrentUser().Id;
                            _presenter.CurrentTravelAdvanceRequest.CurrentStatus = TARS.ApprovalStatus;
                            if (TARS.PaymentType != null)
                            {
                                if (TARS.PaymentType.Contains("Bank Payment"))
                                {
                                    btnBankPayment.Visible = true;
                                }
                            }
                        }
                        GetNextApprover();
                        Log.Info(_presenter.GetUser(TARS.Approver).FullName + " has " + TARS.ApprovalStatus + " Travel Advance Request made by " + _presenter.CurrentTravelAdvanceRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentTravelAdvanceRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        TARS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(TARS);
                        Log.Info(_presenter.GetUser(TARS.Approver).FullName + " has " + TARS.ApprovalStatus + " Travel Advance Request made by " + _presenter.CurrentTravelAdvanceRequest.AppUser.FullName);
                    }
                    break;
                }

            }
        }
        protected void grvTravelAdvanceRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                if (e.CommandName == "ViewItem")
                {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    int reqId = Convert.ToInt32(grvTravelAdvanceRequestList.DataKeys[rowIndex].Value);
                    Session["CurrentTravelAdvanceRequest"] = _presenter.GetTravelAdvanceRequest(reqId);
                    _presenter.CurrentTravelAdvanceRequest = (TravelAdvanceRequest)Session["CurrentTravelAdvanceRequest"];
                    //_presenter.OnViewLoaded();
                    pnlDetail_ModalPopupExtender.Show();
                    dgTravelAdvanceRequestDetail.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
                    dgTravelAdvanceRequestDetail.DataBind();
                }
            }
        }
        protected void grvTravelAdvanceRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            TravelAdvanceRequest CSR = e.Row.DataItem as TravelAdvanceRequest;
            if (CSR != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (CSR.ProgressStatus == ProgressStatus.InProgress.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                    }
                    else if (CSR.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");

                    }
                }
            }
        }
        protected void grvTravelAdvanceRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            BindTravelAdvanceRequestStatus();
            BindAccounts();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void grvTravelAdvanceRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvanceRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchTravelAdvanceRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentTravelAdvanceRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveTravelAdvanceRequestStatus();

                    _presenter.CurrentTravelAdvanceRequest.Account = _presenter.GetAccount(Convert.ToInt32(ddlAccount.SelectedValue));
                    _presenter.SaveOrUpdateTravelAdvanceRequest(_presenter.CurrentTravelAdvanceRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                        Master.ShowMessage(new AppMessage("Travel Advance  Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    else
                        Master.ShowMessage(new AppMessage("Travel Advance  Approval Rejected", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchTravelAdvanceRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                PrintTransaction();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval_ModalPopupExtender.Hide();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "TravelAdvance"));
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
        protected void dgTravelAdvanceRequestDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_presenter.OnViewLoaded();
            int recordId = (int)dgTravelAdvanceRequestDetail.DataKeys[dgTravelAdvanceRequestDetail.SelectedIndex];
            _presenter.CurrentTravelAdvanceRequest = (TravelAdvanceRequest)Session["CurrentTravelAdvanceRequest"];
            grvTravelAdvanceCosts.DataSource = _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(recordId).TravelAdvanceCosts;
            grvTravelAdvanceCosts.DataBind();

            pnlDetail_ModalPopupExtender.Show();

        }
        protected void grvTravelAdvanceCosts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvanceCosts.PageIndex = e.NewPageIndex;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
    }
}
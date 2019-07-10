using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmCashPaymentApproval : POCBasePage, ICashPaymentApprovalView
    {
        private CashPaymentApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                BindSearchCashPaymentRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                if (_presenter.CurrentCashPaymentRequest.Id != 0)
                {
                    PrintTransaction();
                }
            }
        }
        [CreateNew]
        public CashPaymentApprovalPresenter Presenter
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
                return "{00397B85-1427-4EE2-94D7-7A1E8650A568}";
            }
        }
        #region Field Getters
        public int GetCashPaymentRequestId
        {
            get
            {
                if (grvCashPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvCashPaymentRequestList.SelectedDataKey.Value);
                }
                else if (Convert.ToInt32(Session["ReqID"]) != 0)
                {
                    return Convert.ToInt32(Session["ReqID"]);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int GetAccountId
        {
            get { return Convert.ToInt32(ddlAccount.SelectedValue); }
        }
        #endregion
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
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel)
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.CashPayment_Request.ToString().Replace('_', ' ').ToString(), _presenter.CurrentCashPaymentRequest.TotalAmount);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentCashPaymentRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;
                }
                /*else if (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                        {
                            will = AL.Will;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentCashPaymentRequest.CurrentApproverPosition == AL.EmployeePosition.Id)
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
            ddlSrchProgressStatus.Items.Add(new ListItem("Not Retired", "Not Retired"));
            ddlSrchProgressStatus.Items.Add(new ListItem("Retired", "Retired"));
        }
        private void BindSearchCashPaymentRequestGrid()
        {
            grvCashPaymentRequestList.DataSource = _presenter.ListCashPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvCashPaymentRequestList.DataBind();
        }
        private void BindCashPaymentRequestStatus()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if (CPRS.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel && _presenter.CurrentCashPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count && CPRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                    if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Last().PaymentType == "Bank Payment")
                        btnBankPayment.Visible = true;
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnApprove.Enabled = true;
                }

            }
        }
        private void BindAccounts()
        {
            //if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel && (_presenter.CurrentUser().EmployeePosition.PositionName == "Finance Officer" || _presenter.GetUser(_presenter.CurrentCashPaymentRequest.CurrentApprover).IsAssignedJob == true))
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                {
                lblAccount.Visible = true;
                lblAccountdd.Visible = true;
            }


        }
        private void BindProject(DropDownList ddlProject)
        {
            ddlProject.DataSource = _presenter.ListProjects();
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void BindGrant(DropDownList ddlGrant, int ProjectId)
        {
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataValueField = "Id";
            ddlGrant.DataTextField = "GrantCode";
            ddlGrant.DataBind();
        }
        private void BindAccountDescription(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.ListItemAccounts();
            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count && _presenter.CurrentCashPaymentRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Last().PaymentType == "Bank Payment")
                    btnBankPayment.Visible = true;
                if (ddlApprovalStatus.SelectedValue != ApprovalStatus.Rejected.ToString())
                    SendEmailToRequester();
            }
        }
        private void SendEmail(CashPaymentRequestStatus CPRS)
        {
            if (CPRS.Approver != 0)
            {
                if (_presenter.GetUser(CPRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetUser(CPRS.Approver).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                }
                else
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                }
            }
            else
            {
                foreach(AppUser Payer in _presenter.GetAppUsersByEmployeePosition(CPRS.ApproverPosition))
                {
                    if(Payer.IsAssignedJob != true)
                    {
                        EmailSender.Send(Payer.Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. " + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                    }
                    else
                    {
                        EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Payment Approval", (_presenter.CurrentCashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment with Request No. '" + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper());
                    }
                }
            }

        }
        private void SendEmailRejected(CashPaymentRequestStatus CPRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).Email, "Payment Request Rejection", "Your Payment Request with Voucher No. " + (_presenter.CurrentCashPaymentRequest.VoucherNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + "'");

            if (CPRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < CPRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[i].Approver).Email, "Payment Request Rejection", "Payment Request with Voucher No. " + (_presenter.CurrentCashPaymentRequest.VoucherNo).ToUpper()+ " made by " + (_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (CPRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus != "Bank Payment")
                EmailSender.Send(_presenter.GetUser(_presenter.CurrentCashPaymentRequest.AppUser.Id).Email, "Collect your Payment ", "Your Payment Request for Payment - '" + (_presenter.CurrentCashPaymentRequest.RequestNo).ToUpper() + "' was Completed, Please collect your payment");
        }
        private void GetNextApprover()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if (CPRS.ApprovalStatus == null)
                {
                    if (CPRS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentCashPaymentRequest.CurrentApproverPosition = CPRS.ApproverPosition;
                    }
                    SendEmail(CPRS);
                    _presenter.CurrentCashPaymentRequest.CurrentApprover = CPRS.Approver;
                    _presenter.CurrentCashPaymentRequest.CurrentLevel = CPRS.WorkflowLevel;
                    _presenter.CurrentCashPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                    _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SaveCashPaymentRequestStatus()
        {
            foreach (CashPaymentRequestStatus CPRS in _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses)
            {
                if ((CPRS.Approver == _presenter.CurrentUser().Id || (CPRS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(CPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AssignedTo : 0)) && CPRS.WorkflowLevel == _presenter.CurrentCashPaymentRequest.CurrentLevel)
                {
                    CPRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    CPRS.RejectedReason = txtRejectedReason.Text;
                    CPRS.PaymentType = ddlAccount.SelectedValue;
                    CPRS.Date = DateTime.Now;
                    CPRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(CPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CPRS.Approver).AppUser.FullName : "";
                    if (CPRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentCashPaymentRequest.CurrentLevel == _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses.Count)
                        {
                            _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();

                        }
                        GetNextApprover();
                        CPRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentCashPaymentRequest.CurrentStatus = CPRS.ApprovalStatus;
                        if (CPRS.PaymentType.Contains("Bank Payment"))
                        {
                            btnBankPayment.Visible = true;
                            _presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus = "Bank Payment";
                        }
                        Log.Info(_presenter.GetUser(CPRS.Approver).FullName + " has " + CPRS.ApprovalStatus + " Payment Request made by " + _presenter.CurrentCashPaymentRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentCashPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentCashPaymentRequest.CurrentStatus = ApprovalStatus.Rejected.ToString();
                        CPRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(CPRS);
                        Log.Info(_presenter.GetUser(CPRS.Approver).FullName + " has " + CPRS.ApprovalStatus + " Payment Request made by " + _presenter.CurrentCashPaymentRequest.AppUser.FullName);
                    }
                    break;
                }

            }
        }
        protected void grvCashPaymentRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvCashPaymentRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentCashPaymentRequest = _presenter.GetCashPaymentRequest(reqID);
                if (e.CommandName == "ViewItem")
                {
                    dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                    dgCashPaymentRequestDetail.DataBind();
                    grvdetailAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
                    grvdetailAttachments.DataBind();
                    pnlDetail_ModalPopupExtender.Show();
                }
                else if (e.CommandName == "Retire")
                {
                    lblEstimatedAmountresult.Text = _presenter.CurrentCashPaymentRequest.TotalAmount.ToString();
                    txtActualExpenditure.Text = _presenter.CurrentCashPaymentRequest.TotalActualExpendture != 0 ? _presenter.CurrentCashPaymentRequest.TotalActualExpendture.ToString() : "";
                    grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
                    grvAttachments.DataBind();
                    grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                    grvReimbursementdetail.DataBind();
                    GetActualAmount();
                    pnlReimbursement_ModalPopupExtender.Show();
                    if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus == "Retired")
                    {
                        btnPrintReimburse.Enabled = true;
                    }
                }
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);

            if (fileName != String.Empty)
            {



                CPRAttachment attachment = new CPRAttachment();
                attachment.FilePath = "~/CPUploads/" + fileName;
                fuReciept.PostedFile.SaveAs(Server.MapPath("~/CPUploads/") + fileName);
                //Response.Redirect(Request.Url.AbsoluteUri);
                _presenter.CurrentCashPaymentRequest.CPRAttachments.Add(attachment);

                grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
                grvAttachments.DataBind();


            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void DownloadFile2(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentCashPaymentRequest.RemoveCPAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
            grvAttachments.DataBind();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void grvCashPaymentRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            CashPaymentRequest CSR = e.Row.DataItem as CashPaymentRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (CSR != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (CSR.CurrentLevel == CSR.CashPaymentRequestStatuses.Count && CSR.ProgressStatus == "Completed")
                            e.Row.Cells[8].Visible = true;
                        else
                            e.Row.Cells[8].Visible = false;
                    }

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
        protected void grvCashPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();

            PopApprovalStatus();


            Session["PaymentId"] = _presenter.CurrentCashPaymentRequest.Id;
            btnApprove.Enabled = true;
            BindAccounts();
            BindCashPaymentRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void grvCashPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchCashPaymentRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveCashPaymentRequestStatus();
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Payment Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Payment Approval Rejected", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }

                    btnApprove.Enabled = false;
                    BindSearchCashPaymentRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                PrintTransaction();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: While Approving Cash Payment!", RMessageType.Error));
                ExceptionUtility.LogException(ex, ex.Source);
                ExceptionUtility.NotifySystemOps(ex);
            }
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentCashPaymentRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentCashPaymentRequest.RequestDate.Value.ToShortDateString();
            if (_presenter.CurrentCashPaymentRequest.Supplier != null)
            {
                lblPayeeResult.Text = _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() != null ? _presenter.CurrentCashPaymentRequest.Supplier.SupplierName.ToString() : "";
            }
            lblpaytypeRes.Text = _presenter.CurrentCashPaymentRequest.PaymentMethod;
            lblVoucherNoResult.Text = _presenter.CurrentCashPaymentRequest.VoucherNo;
            lblTotalAmountResult.Text = _presenter.CurrentCashPaymentRequest.TotalAmount.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentCashPaymentRequest.ProgressStatus.ToString();
            lblDescResult.Text = _presenter.CurrentCashPaymentRequest.Description;
            lblActualExpendtureRes.Text = _presenter.CurrentCashPaymentRequest.TotalActualExpendture != 0 ? _presenter.CurrentCashPaymentRequest.TotalActualExpendture.ToString() : "";
            lblReimbersestatusRes.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus;
            grvDetails.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }


        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCashPaymentRequest.CashPaymentRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
                rfvRejectedReason.Enabled = true;
            }
            else
            {
                lblRejectedReason.Visible = false;
                txtRejectedReason.Visible = false;
                rfvRejectedReason.Enabled = false;
            }
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void dgCashPaymentRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {
                    if (_presenter.CurrentCashPaymentRequest.CurrentLevel == 1 && (_presenter.CurrentUser().Id == _presenter.CurrentCashPaymentRequest.CurrentApprover))
                    {
                        LinkButton lnkEdit = e.Item.FindControl("lnkEdit") as LinkButton;
                        if (lnkEdit != null)
                            lnkEdit.Visible = false;
                    }

                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id != null)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void dgCashPaymentRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            dgCashPaymentRequestDetail.DataBind();
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void dgCashPaymentRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int CPRDId = (int)dgCashPaymentRequestDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));

                dgCashPaymentRequestDetail.EditItemIndex = -1;
                dgCashPaymentRequestDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                dgCashPaymentRequestDetail.DataBind();
                pnlDetail_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "CashPayment"));
        }
        protected void btnReimburse_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.CPRAttachments.Count != 0)
                {
                    _presenter.CurrentCashPaymentRequest.TotalActualExpendture = Convert.ToDecimal(txtActualExpenditure.Text);
                    _presenter.CurrentCashPaymentRequest.PaymentReimbursementStatus = "Retired";
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                    btnPrintReimburse.Enabled = true;
                    Master.ShowMessage(new AppMessage("Payment Retired Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    BindSearchCashPaymentRequestGrid();
                    //btnReimburse.Enabled = false;
                    pnlReimbursement_ModalPopupExtender.Show();
                    Session["ReqID"] = null;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Error,Please attach Receipt", Chai.WorkflowManagment.Enums.RMessageType.Error));
                    pnlReimbursement_ModalPopupExtender.Show();
                }

                PrintTransaction();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error,'" + ex.Message + "'", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }

        }
        private void GetActualAmount()
        {
            foreach (CashPaymentRequestDetail detail in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                if (detail.ActualExpendture > 0)
                {
                    txtActualExpenditure.Text = (txtActualExpenditure.Text != "" ? Convert.ToDecimal(txtActualExpenditure.Text) : 0 + detail.ActualExpendture).ToString();
                }
            }
        }
        protected void grvReimbursementdetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void grvReimbursementdetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.grvReimbursementdetail.EditItemIndex = e.Item.ItemIndex;
            grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            grvReimbursementdetail.DataBind();
            pnlReimbursement_ModalPopupExtender.Show();
        }
        protected void grvReimbursementdetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal currentActualExpendture = 0;
            int CPRDId = (int)grvReimbursementdetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtEdtActualExpendture = e.Item.FindControl("txtEdtActualExpendture") as TextBox;
                currentActualExpendture = cprd.ActualExpendture;
                cprd.ActualExpendture = Convert.ToDecimal(txtEdtActualExpendture.Text);
                txtActualExpenditure.Text = (((txtActualExpenditure.Text != "" && txtActualExpenditure.Text != "0") ? Convert.ToDecimal(txtActualExpenditure.Text) : 0) - currentActualExpendture).ToString();
                txtActualExpenditure.Text = (((txtActualExpenditure.Text != "" && txtActualExpenditure.Text != "0") ? Convert.ToDecimal(txtActualExpenditure.Text) : 0) + cprd.ActualExpendture).ToString();
                _presenter.CurrentCashPaymentRequest.TotalActualExpendture = Convert.ToDecimal(txtActualExpenditure.Text);
                _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                grvReimbursementdetail.EditItemIndex = -1;
                grvReimbursementdetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
                grvReimbursementdetail.DataBind();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
            pnlReimbursement_ModalPopupExtender.Show();
        }

    }
}
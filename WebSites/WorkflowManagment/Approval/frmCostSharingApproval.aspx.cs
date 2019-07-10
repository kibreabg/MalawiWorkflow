using System;
using System.Collections.Generic;
using System.Linq;
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
using System.IO;
using System.Reflection;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmCostSharingApproval : POCBasePage, ICostSharingApprovalView
    {
        private CostSharingApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                BindSearchCostSharingRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentCostSharingRequest != null)
            {
                if (_presenter.CurrentCostSharingRequest.Id != 0)
                {
                    PrintTransaction();
                }
            }
        }
        [CreateNew]
        public CostSharingApprovalPresenter Presenter
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
                return "{9C411111-6AD3-4C19-BEED-CC6020DB6B7C}";
            }
        }

        #region Field Getters
        public int GetCostSharingRequestId
        {
            get
            {
                if (grvCostSharingRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvCostSharingRequestList.SelectedDataKey.Value);
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
            if (_presenter.CurrentCostSharingRequest.CostSharingRequestStatuses.Count == _presenter.CurrentCostSharingRequest.CurrentLevel)
            {
                ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Bank_Payment.ToString().Replace('_', ' '), ApprovalStatus.Bank_Payment.ToString().Replace('_', ' ')));
            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.CostSharing_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentCostSharingRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;
                }
                /*else if (_presenter.GetUser(_presenter.CurrentCostSharingRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }*/
                else
                {
                    try
                    {
                        if (_presenter.GetUser(_presenter.CurrentCostSharingRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                        {
                            will = AL.Will;
                        }
                    }
                    catch
                    {
                        if (_presenter.CurrentCostSharingRequest.CurrentApproverPosition == AL.EmployeePosition.Id)
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
        private void BindSearchCostSharingRequestGrid()
        {
            grvCostSharingRequestList.DataSource = _presenter.ListCostSharingRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvCostSharingRequestList.DataBind();
        }
        private void BindCostSharingRequestStatus()
        {
            foreach (CostSharingRequestStatus CSRS in _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses)
            {
                if (CSRS.WorkflowLevel == _presenter.CurrentCostSharingRequest.CurrentLevel && _presenter.CurrentCostSharingRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }

                if (_presenter.CurrentCostSharingRequest.CurrentLevel == _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses.Count && CSRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;

                }
                else
                    btnPrint.Enabled = false;
            }
        }
        private void BindAccounts()
        {
            if (_presenter.CurrentCostSharingRequest.CostSharingRequestStatuses.Count == _presenter.CurrentCostSharingRequest.CurrentLevel && (_presenter.CurrentUser().EmployeePosition.PositionName == "Finance Officer" || _presenter.GetUser(_presenter.CurrentCostSharingRequest.CurrentApprover).IsAssignedJob == true))
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
            if (_presenter.CurrentCostSharingRequest.CurrentLevel == _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses.Count && _presenter.CurrentCostSharingRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;

                SendEmailToRequester();
            }
        }
        private void SendEmail(CostSharingRequestStatus CSRS)
        {
            if (CSRS.Approver != 0)
            {
                if (_presenter.GetUser(CSRS.Approver).IsAssignedJob != true)
                {
                    EmailSender.Send(_presenter.GetUser(CSRS.Approver).Email, "Cost Sharing Payment Approval", (_presenter.CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. '" + (_presenter.CurrentCostSharingRequest.RequestNo).ToUpper() + "'");

                }
                else
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(CSRS.Approver).AssignedTo).Email, "Cost Sharing Payment Approval", (_presenter.CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. '" + (_presenter.CurrentCostSharingRequest.RequestNo).ToUpper() + "'");
                }
            }
            else
            {
                foreach (AppUser Payer in _presenter.GetAppUsersByEmployeePosition(CSRS.ApproverPosition))
                {
                    if (Payer.IsAssignedJob != true)
                    {
                        EmailSender.Send(Payer.Email, "Cost Sharing Payment Approval", (_presenter.CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. " + (_presenter.CurrentCostSharingRequest.RequestNo).ToUpper());
                    }
                    else
                    {
                        EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(Payer.Id).AssignedTo).Email, "Cost Sharing Payment Approval", (_presenter.CurrentCostSharingRequest.AppUser.FullName).ToUpper() + " Requests for Cost Sharing with Request No. '" + (_presenter.CurrentCostSharingRequest.RequestNo).ToUpper());
                    }
                }
            }

        }
        private void SendEmailRejected(CostSharingRequestStatus CSRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentCostSharingRequest.AppUser.Id).Email, "Cost Sharing Payment Request Rejection", "Your Cost Sharing Request with Request No. " + (_presenter.CurrentCostSharingRequest.VoucherNo).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason '" + (CSRS.RejectedReason).ToUpper() + "'");
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentCostSharingRequest.AppUser.Id).Email, "Collect your Payment ", "Your Payment Request for Cost Sharing - '" + (_presenter.CurrentCostSharingRequest.RequestNo).ToUpper() + "' was Completed, Please collect your payment");
        }
        private void GetNextApprover()
        {
            foreach (CostSharingRequestStatus CSRS in _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses)
            {
                if (CSRS.ApprovalStatus == null)
                {
                    if (CSRS.Approver == 0)
                    {
                        //This is to handle multiple Finance Officers responding to this request
                        //SendEmailToFinanceOfficers;
                        _presenter.CurrentCostSharingRequest.CurrentApproverPosition = CSRS.ApproverPosition;
                    }
                    SendEmail(CSRS);
                    _presenter.CurrentCostSharingRequest.CurrentApprover = CSRS.Approver;
                    _presenter.CurrentCostSharingRequest.CurrentLevel = CSRS.WorkflowLevel;
                    _presenter.CurrentCostSharingRequest.CurrentStatus = CSRS.ApprovalStatus;
                    _presenter.CurrentCostSharingRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SaveCostSharingRequestStatus()
        {
            foreach (CostSharingRequestStatus CSRS in _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses)
            {
                if ((CSRS.Approver == _presenter.CurrentUser().Id || (CSRS.ApproverPosition == _presenter.CurrentUser().EmployeePosition.Id) || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(CSRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CSRS.Approver).AssignedTo : 0)) && CSRS.WorkflowLevel == _presenter.CurrentCostSharingRequest.CurrentLevel)
                {
                    CSRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    CSRS.RejectedReason = txtRejectedReason.Text;
                    CSRS.Date = DateTime.Now;
                    CSRS.PaymentType = ddlAccount.SelectedValue;
                    CSRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(CSRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(CSRS.Approver).AppUser.FullName : "";
                    if (CSRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentCostSharingRequest.CurrentLevel == _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses.Count)
                        {
                            _presenter.CurrentCostSharingRequest.ProgressStatus = ProgressStatus.Completed.ToString();

                        }
                        GetNextApprover();
                        CSRS.Approver = _presenter.CurrentUser().Id;
                        _presenter.CurrentCostSharingRequest.CurrentStatus = CSRS.ApprovalStatus;
                        if (CSRS.PaymentType.Contains("Bank Payment"))
                        {
                            btnBankPayment.Visible = true;
                            _presenter.CurrentCostSharingRequest.PaymentReimbursementStatus = "Bank Payment";
                        }
                        Log.Info(_presenter.GetUser(CSRS.Approver).FullName + " has " + CSRS.ApprovalStatus + " Cost Sharing Payment Request made by " + _presenter.CurrentCostSharingRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentCostSharingRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentCostSharingRequest.CurrentStatus = ApprovalStatus.Rejected.ToString();
                        CSRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(CSRS);
                        Log.Info(_presenter.GetUser(CSRS.Approver).FullName + " has " + CSRS.ApprovalStatus + " Cost Sharing Request made by " + _presenter.CurrentCostSharingRequest.AppUser.FullName);
                    }
                    break;
                }

            }
        }
        protected void grvCostSharingRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvCostSharingRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentCostSharingRequest = _presenter.GetCostSharingRequest(reqID);
                if (e.CommandName == "ViewItem")
                {

                    dgCostSharingRequestDetail.DataSource = _presenter.CurrentCostSharingRequest.CostSharingRequestDetails;
                    dgCostSharingRequestDetail.DataBind();
                    grvdetailAttachments.DataSource = _presenter.CurrentCostSharingRequest.CSRAttachments;
                    grvdetailAttachments.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
                }
                else if (e.CommandName == "Retire")
                {
                    lblEstimatedAmountresult.Text = _presenter.CurrentCostSharingRequest.EstimatedTotalAmount.ToString();
                    txtActualExpenditure.Text = _presenter.CurrentCostSharingRequest.ActualTotalAmount != 0 ? _presenter.CurrentCostSharingRequest.ActualTotalAmount.ToString() : "";
                    grvAttachments.DataSource = _presenter.CurrentCostSharingRequest.CSRAttachments;
                    grvAttachments.DataBind();
                    pnlReimbursement_ModalPopupExtender.Show();
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



                CSRAttachment attachment = new CSRAttachment();
                attachment.FilePath = "~/CSUploads/" + fileName;
                fuReciept.PostedFile.SaveAs(Server.MapPath("~/CSUploads/") + fileName);
                //Response.Redirect(Request.Url.AbsoluteUri);
                _presenter.CurrentCostSharingRequest.CSRAttachments.Add(attachment);

                grvAttachments.DataSource = _presenter.CurrentCostSharingRequest.CSRAttachments;
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
        }
        protected void DownloadFile2(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentCostSharingRequest.RemoveCSAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentCostSharingRequest.CSRAttachments;
            grvAttachments.DataBind();

        }
        protected void grvCostSharingRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            CostSharingRequest CSR = e.Row.DataItem as CostSharingRequest;
            if (CSR != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if (CSR.CurrentLevel == CSR.CostSharingRequestStatuses.Count && CSR.ProgressStatus == "Completed")
                            e.Row.Cells[9].Visible = true;
                        else
                            e.Row.Cells[9].Visible = false;
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
        protected void grvCostSharingRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            PopApprovalStatus();

            Session["PaymentId"] = _presenter.CurrentCostSharingRequest.Id;
            btnApprove.Enabled = true;
            BindAccounts();

            BindCostSharingRequestStatus();
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void grvCostSharingRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCostSharingRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchCostSharingRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCostSharingRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveCostSharingRequestStatus();
                    if (ddlAccount.SelectedValue != "0")

                        _presenter.SaveOrUpdateCostSharingRequest(_presenter.CurrentCostSharingRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Cost Sharing Payment Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Cost Sharing Payment Approval Rejected", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }

                    btnApprove.Enabled = false;
                    BindSearchCostSharingRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                PrintTransaction();
            }
            catch (Exception ex)
            {

            }
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentCostSharingRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentCostSharingRequest.RequestDate.Value.ToShortDateString();
            lblPayeeResult.Text = _presenter.CurrentCostSharingRequest.Payee.ToString();
            lblVoucherNoResult.Text = _presenter.CurrentCostSharingRequest.VoucherNo;
            lblTotalAmountResult.Text = _presenter.CurrentCostSharingRequest.EstimatedTotalAmount.ToString();
            lblAccountNameResult.Text = _presenter.CurrentCostSharingRequest.ActualTotalAmount.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentCostSharingRequest.ProgressStatus.ToString();
            lblDescResult.Text = _presenter.CurrentCostSharingRequest.Description;
            lblAccountNameResult.Text = _presenter.CurrentCostSharingRequest.ItemAccount.AccountCode;
            lblpaytypeRes.Text = _presenter.CurrentCostSharingRequest.PaymentMethod;
            lblActualExpendtureRes.Text = _presenter.CurrentCostSharingRequest.ActualTotalAmount != null ? _presenter.CurrentCostSharingRequest.ActualTotalAmount.ToString() : "";
            lblReimbersestatusRes.Text = _presenter.CurrentCostSharingRequest.PaymentReimbursementStatus;
            grvDetails.DataSource = _presenter.CurrentCostSharingRequest.CostSharingRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentCostSharingRequest.CostSharingRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {

        }

        protected void grvAttachments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_presenter.CurrentCostSharingRequest != null)
            {
                int attachmentId = Convert.ToInt32(grvAttachments.SelectedDataKey.Value);
                CSRAttachment attachment = _presenter.GetAttachment(attachmentId);

                string Filename = attachment.FilePath;




                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                // context.Response.AppendHeader("content-length", FileData.Length.ToString());
                //  context.Response.ContentType = GetMimeTypeByFileName(Filename);
                context.Response.AppendHeader("content-disposition", "attachment; filename=" + Filename);
                //context.Response.BinaryWrite(FileData);

                context.ApplicationInstance.CompleteRequest();
            }
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentCostSharingRequest.CostSharingRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentCostSharingRequest.CostSharingRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentCostSharingRequest.CostSharingRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
            }
            else
            {
                lblRejectedReason.Visible = false;
                txtRejectedReason.Visible = false;
            }
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void dgCostSharingRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
                if (_presenter.CurrentCostSharingRequest.CostSharingRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);
                        if (_presenter.CurrentCostSharingRequest.CostSharingRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentCostSharingRequest.CostSharingRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }

                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentCostSharingRequest.CostSharingRequestDetails[e.Item.DataSetIndex].Grant.Id != null)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCostSharingRequest.CostSharingRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void dgCostSharingRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCostSharingRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgCostSharingRequestDetail.DataSource = _presenter.CurrentCostSharingRequest.CostSharingRequestDetails;
            dgCostSharingRequestDetail.DataBind();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void dgCostSharingRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int CPRDId = (int)dgCostSharingRequestDetail.DataKeys[e.Item.ItemIndex];
            CostSharingRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCostSharingRequest.GetCostSharingRequestDetails(CPRDId);
            else
                cprd = (CostSharingRequestDetail)_presenter.CurrentCostSharingRequest.CostSharingRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CostSharingRequest = _presenter.CurrentCostSharingRequest;
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;

                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));


                dgCostSharingRequestDetail.EditItemIndex = -1;
                dgCostSharingRequestDetail.DataSource = _presenter.CurrentCostSharingRequest.CostSharingRequestDetails;
                dgCostSharingRequestDetail.DataBind();
                ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
                Master.ShowMessage(new AppMessage("Cost Sharing Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Cost Sharing Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Default.aspx");
        }
        protected void btnBankPayment_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("../Request/frmOperationalControlRequest.aspx?paymentId={0}&Page={1}", Convert.ToInt32(Session["PaymentId"]), "CostSharing"));
        }
        protected void btnReimburse_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCostSharingRequest.CSRAttachments.Count != 0)
                {
                    _presenter.CurrentCostSharingRequest.ActualTotalAmount = Convert.ToDecimal(txtActualExpenditure.Text);
                    _presenter.CurrentCostSharingRequest.PaymentReimbursementStatus = "Retired";
                    _presenter.SaveOrUpdateCostSharingRequest(_presenter.CurrentCostSharingRequest);
                    btnPrintReimburse.Enabled = true;
                    //btnReimburse.Enabled = false;
                    Master.ShowMessage(new AppMessage("Cost Sharing Retired Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    BindSearchCostSharingRequestGrid();
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
        public void SaveCostSharingDetail()
        {

            RemoveCostSharingDetail();
            IList<CostSharingSetting> costsharingsettings = _presenter.GetCostSharingSettings();
            if (costsharingsettings != null)
            {
                foreach (CostSharingSetting CSS in costsharingsettings)
                {
                    CostSharingRequestDetail detail = new CostSharingRequestDetail();
                    detail.CostSharingRequest = _presenter.CurrentCostSharingRequest;
                    detail.Project = CSS.Project;
                    detail.SharedAmount = (CSS.Percentage / 100) * _presenter.CurrentCostSharingRequest.EstimatedTotalAmount;
                    _presenter.CurrentCostSharingRequest.CostSharingRequestDetails.Add(detail);

                }
            }
        }
        public void RemoveCostSharingDetail()
        {
            foreach (CostSharingRequestDetail CSRD in _presenter.CurrentCostSharingRequest.CostSharingRequestDetails)
            {
                _presenter.CurrentCostSharingRequest.RemoveCostSharingRequestDetails(CSRD.Id);

                _presenter.DeleteCostSharingsetting(CSRD);
            }
        }
        protected void grvdetailAttachments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_presenter.CurrentCostSharingRequest != null)
            {
                int attachmentId = Convert.ToInt32(grvdetailAttachments.SelectedDataKey.Value);
                CSRAttachment attachment = _presenter.GetAttachment(attachmentId);

                string Filename = attachment.FilePath;

                //Byte[] FileData = attachment.Data;


                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                // context.Response.AppendHeader("content-length", FileData.Length.ToString());
                // context.Response.ContentType = GetMimeTypeByFileName(Filename);
                context.Response.AppendHeader("content-disposition", "attachment; filename=" + Filename);
                // context.Response.BinaryWrite(FileData);

                context.ApplicationInstance.CompleteRequest();
            }
            pnlReimbursement_ModalPopupExtender.Show();
        }
    }
}
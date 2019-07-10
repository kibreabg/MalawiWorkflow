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
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmOperationalControlApproval : POCBasePage, IOperationalControlApprovalView
    {
        private OperationalControlApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                BindSearchOperationalControlRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentOperationalControlRequest != null)
            {
                if (_presenter.CurrentOperationalControlRequest.Id != 0)
                {
                    PrintTransaction();
                }
            }
        }
        [CreateNew]
        public OperationalControlApprovalPresenter Presenter
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
        public int GetOperationalControlRequestId
        {
            get
            {
                if (grvOperationalControlRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvOperationalControlRequestList.SelectedDataKey.Value);
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
            get { return 0; }
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

            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.OperationalControl_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentOperationalControlRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                else if (_presenter.GetUser(_presenter.CurrentOperationalControlRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
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
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        
            ddlSrchProgressStatus.Items.Add(new ListItem("Retired", "Retired"));
        }
        private void BindSearchOperationalControlRequestGrid()
        {
            grvOperationalControlRequestList.DataSource = _presenter.ListOperationalControlRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvOperationalControlRequestList.DataBind();
        }
        private void BindOperationalControlRequestStatus()
        {
            foreach (OperationalControlRequestStatus OCRS in _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses)
            {
   
                if (_presenter.CurrentOperationalControlRequest.CurrentLevel == _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses.Count && _presenter.CurrentOperationalControlRequest.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    btnPrint.Enabled = true;
                
                    btnApprove.Enabled = false;
                }
                else
                    btnPrint.Enabled = false;
            
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
            if (_presenter.CurrentOperationalControlRequest.CurrentLevel == _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses.Count && _presenter.CurrentOperationalControlRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
            }
        }
        private void SendEmail(OperationalControlRequestStatus OCRS)
        {
            if (_presenter.GetUser(OCRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(OCRS.Approver).Email, "Bank Payment Approval", (_presenter.CurrentOperationalControlRequest.AppUser.FullName).ToUpper() + " Requests for payment");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(OCRS.Approver).AssignedTo).Email, "Bank Payment Approval", (_presenter.CurrentOperationalControlRequest.AppUser.FullName).ToUpper() + " Requests for payment");
            }

        }
        private void SendEmailRejected(OperationalControlRequestStatus OCRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentOperationalControlRequest.AppUser.Id).Email, "Bank Payment Request Rejection", "Your Bank Payment Request with Request No. - '" + (_presenter.CurrentOperationalControlRequest.RequestNo.ToString()).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (OCRS.RejectedReason).ToUpper() + "'");

            if (OCRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < OCRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses[i].Approver).Email, "Bank Payment Request Rejection", "Bank Payment Request with Request No. - '" + (_presenter.CurrentOperationalControlRequest.RequestNo.ToString()).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentOperationalControlRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (OCRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        
        private void GetNextApprover()
        {
            foreach (OperationalControlRequestStatus OCRS in _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses)
            {
                if (OCRS.ApprovalStatus == null)
                {
                    SendEmail(OCRS);
                    _presenter.CurrentOperationalControlRequest.CurrentApprover = OCRS.Approver;
                    _presenter.CurrentOperationalControlRequest.CurrentLevel = OCRS.WorkflowLevel;
                    _presenter.CurrentOperationalControlRequest.CurrentStatus = OCRS.ApprovalStatus;
                    _presenter.CurrentOperationalControlRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SaveOperationalControlRequestStatus()
        {
            foreach (OperationalControlRequestStatus OCRS in _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses)
            {
                if ((OCRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(OCRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(OCRS.Approver).AssignedTo : 0)) && OCRS.WorkflowLevel == _presenter.CurrentOperationalControlRequest.CurrentLevel)
                {
                    OCRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    OCRS.RejectedReason = txtRejectedReason.Text;
                    OCRS.Account = _presenter.GetAccount(_presenter.CurrentOperationalControlRequest.Account.Id);
                    OCRS.Date = DateTime.Now;
                    OCRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(OCRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(OCRS.Approver).AppUser.FullName : "";
                    if (OCRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentOperationalControlRequest.CurrentLevel == _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses.Count)
                        {
                            _presenter.CurrentOperationalControlRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        OCRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(OCRS.Approver).FullName + " has " + OCRS.ApprovalStatus + " Bank Payment Request made by " + _presenter.CurrentOperationalControlRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentOperationalControlRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        OCRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(OCRS);
                        Log.Info(_presenter.GetUser(OCRS.Approver).FullName + " has " + OCRS.ApprovalStatus + " Bank Payment Request made by " + _presenter.CurrentOperationalControlRequest.AppUser.FullName);
                    }
                    break;
                }
                
            }
        }
        protected void grvOperationalControlRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvOperationalControlRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentOperationalControlRequest = _presenter.GetOperationalControlRequest(reqID);
                if (e.CommandName == "ViewItem")
                {
                    dgOperationalControlRequestDetail.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
                    dgOperationalControlRequestDetail.DataBind();
                    grvdetailAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
                    grvdetailAttachments.DataBind();
                    pnlDetail_ModalPopupExtender.Show();
                }
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
        protected void grvOperationalControlRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            OperationalControlRequest CSR = e.Row.DataItem as OperationalControlRequest;
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
        protected void grvOperationalControlRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentOperationalControlRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                PrintTransaction();
            }
            
            PopApprovalStatus();           
            
            btnApprove.Enabled = true;
            ShowPrint();
            BindOperationalControlRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();
        }
        
        protected void grvOperationalControlRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvOperationalControlRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchOperationalControlRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentOperationalControlRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveOperationalControlRequestStatus();
                    
                    _presenter.SaveOrUpdateOperationalControlRequest(_presenter.CurrentOperationalControlRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Bank Payment Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Bank Payment Approval Rejected", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    btnApprove.Enabled = false;
                    BindSearchOperationalControlRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                    PrintTransaction();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentOperationalControlRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentOperationalControlRequest.RequestDate.Value.ToShortDateString();
            lblBeneficiaryNameResult.Text = _presenter.CurrentOperationalControlRequest.Beneficiary.BeneficiaryName;
            lblDescriptionResult.Text = _presenter.CurrentOperationalControlRequest.Description;
            lblBranchCodeResult.Text = _presenter.CurrentOperationalControlRequest.BranchCode.ToString();
            lblVoucherNoResult.Text = _presenter.CurrentOperationalControlRequest.VoucherNo.ToString();
            lblTotalAmountResult.Text = _presenter.CurrentOperationalControlRequest.TotalAmount.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentOperationalControlRequest.ProgressStatus.ToString();
            lblPayMethRes.Text = _presenter.CurrentOperationalControlRequest.PaymentMethod;
            lblActualExpendtureRes.Text = _presenter.CurrentOperationalControlRequest.TotalActualExpendture != null ? _presenter.CurrentOperationalControlRequest.TotalActualExpendture.ToString() : "";
            lblReimbersestatusRes.Text = _presenter.CurrentOperationalControlRequest.PaymentReimbursementStatus;
            //lblGrantIdResult.Text = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[0].Grant.GrantCode;
            //lblProjectIdResult.Text = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[0].Project.ProjectCode;
            grvDetails.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentOperationalControlRequest.OperationalControlRequestStatuses[e.Row.RowIndex].Approver).FullName;
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
        protected void dgOperationalControlRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
                if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                    if (ddlProject != null)
                    {
                        BindProject(ddlProject);
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Project.Id != 0)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlAccountDescription != null)
                    {
                        BindAccountDescription(ddlAccountDescription);
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                    DropDownList ddlEdtGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                    if (ddlEdtGrant != null)
                    {
                        BindGrant(ddlEdtGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Grant.Id != null)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void dgOperationalControlRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgOperationalControlRequestDetail.EditItemIndex = e.Item.ItemIndex;
            dgOperationalControlRequestDetail.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
            dgOperationalControlRequestDetail.DataBind();
            pnlDetail_ModalPopupExtender.Show();
        }
        protected void dgOperationalControlRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int CPRDId = (int)dgOperationalControlRequestDetail.DataKeys[e.Item.ItemIndex];
            OperationalControlRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentOperationalControlRequest.GetOperationalControlRequestDetail(CPRDId);
            else
                cprd = (OperationalControlRequestDetail)_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));

                dgOperationalControlRequestDetail.EditItemIndex = -1;
                dgOperationalControlRequestDetail.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
                dgOperationalControlRequestDetail.DataBind();
                pnlDetail_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bank Payment Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
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
       
       
       

       
}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmPaymentReimbursementApproval : POCBasePage, IPaymentReimbursementApprovalView
    {
        private PaymentReimbursementApprovalPresenter _presenter;
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                PopProgressStatus();
            }
            this._presenter.OnViewLoaded();
            BindSearchPaymentReimbursementRequestGrid();
         
            if (_presenter.CurrentPaymentReimbursementRequest.Id != 0)
            {
                PrintTransaction();
            }
        }
        [CreateNew]
        public PaymentReimbursementApprovalPresenter Presenter
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
                return "{A1CF3CA4-8F4D-477F-82D2-15A7B689B697}";
            }
        }
        #region Field Getters
        public int GetPaymentReimbursementRequestId
        {
            get
            {
                if (grvPaymentReimbursementRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvPaymentReimbursementRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        }
        private void PopApprovalStatus()
        {
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 2) == s[i].Substring(0, 2))
                {
                    ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                }

            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.PaymentReimbursement_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentPaymentReimbursementRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;
                }
                else if (_presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
        }
        private void BindSearchPaymentReimbursementRequestGrid()
        {
            grvPaymentReimbursementRequestList.DataSource = _presenter.ListPaymentReimbursementRequests(txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvPaymentReimbursementRequestList.DataBind();
        }
        private void BindPaymentReimbursementRequestStatus()
        {
            // PaymentReimbursementApprovalPresenter _presenterm = new   PaymentReimbursementApprovalPresenter;
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if (PRRS.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel && _presenter.CurrentPaymentReimbursementRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    btnApprove.Enabled = true;
                }
                else
                    btnApprove.Enabled = false;
                if (PRRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                }
            }
        }
        private void ShowPrint()
        {
            btnPrint.Enabled = true;
        }
        private void SendEmail(PaymentReimbursementRequestStatus PRRS)
        {
            if (_presenter.GetUser(PRRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(PRRS.Approver).Email, "Payment Reimbursement Request", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo).Email, "Payment Reimbursement Request", (_presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.FullName).ToUpper() + " Requests for Payment Reimbursement");
            }
            EmailSender.Send(_presenter.GetUser(PRRS.Approver).Email, "Payment Reimbursement Request", "Request for Payment Reimbursement");
        }
        private void SavePaymentReimbursementRequestStatus()
        {
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if ((PRRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(PRRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRRS.Approver).AssignedTo : 0)) && PRRS.WorkflowLevel == _presenter.CurrentPaymentReimbursementRequest.CurrentLevel)
                {
                    PRRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    PRRS.RejectedReason = txtRejectedReason.Text;
                    PRRS.Date = Convert.ToDateTime(DateTime.Today.ToShortDateString());
                    if (PRRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        _presenter.CurrentPaymentReimbursementRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.PaymentReimbursementStatus = "Finished";
                        GetNextApprover();

                    }
                    else
                    {
                        _presenter.CurrentPaymentReimbursementRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                    }
                }
                break;
            }
        }
        private void GetNextApprover()
        {
            foreach (PaymentReimbursementRequestStatus PRRS in _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses)
            {
                if (PRRS.ApprovalStatus == null)
                {
                    SendEmail(PRRS);
                    _presenter.CurrentPaymentReimbursementRequest.CurrentApprover = PRRS.Approver;
                    _presenter.CurrentPaymentReimbursementRequest.CurrentLevel = PRRS.WorkflowLevel;
                    _presenter.CurrentPaymentReimbursementRequest.CurrentStatus = PRRS.ApprovalStatus;
                    _presenter.CurrentPaymentReimbursementRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        protected void grvPaymentReimbursementRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grvPaymentReimbursementRequestList.SelectedDataKey.Value
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            //grvAttachments.DataSource = _presenter.CurrentPaymentReimbursementRequest.CPRAttachments;
            //grvAttachments.DataBind();
            BindPaymentReimbursementRequestStatus();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();

        }
        protected void grvPaymentReimbursementRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvPaymentReimbursementRequestList_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                reqID = (int)grvPaymentReimbursementRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                _presenter.CurrentPaymentReimbursementRequest = _presenter.GetPaymentReimbursementRequest(reqID);
                _presenter.OnViewLoaded();
                dgReimbursementDetail.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails;
                dgReimbursementDetail.DataBind();
                pnlDetail.Visible = true;
            }
        }
        protected void grvPaymentReimbursementRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPaymentReimbursementRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
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
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchPaymentReimbursementRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentPaymentReimbursementRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SavePaymentReimbursementRequestStatus();
                    _presenter.SaveOrUpdatePaymentReimbursementRequest(_presenter.CurrentPaymentReimbursementRequest);
                    ShowPrint();
                    Master.ShowMessage(new AppMessage("Payment Reimbursement Approval Processed", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchPaymentReimbursementRequestGrid();
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval_ModalPopupExtender.Hide();
        }
        private void PrintTransaction()
        {
            pnlApproval_ModalPopupExtender.Hide();

            lblRequestNoResult.Text = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.RequestNo.ToString();
            lblRequestedDateResult.Text = _presenter.CurrentPaymentReimbursementRequest.RequestDate.Value.ToShortDateString();
            lblRequesterResult.Text = _presenter.CurrentPaymentReimbursementRequest.CashPaymentRequest.AppUser.UserName;
            lblCommentResult.Text = _presenter.CurrentPaymentReimbursementRequest.Comment.ToString();
            lblPayMethRes.Text = _presenter.CurrentPaymentReimbursementRequest.PaymentMethod;
            lblApprovalStatusResult.Text = _presenter.CurrentPaymentReimbursementRequest.ProgressStatus.ToString();

            grvDetails.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses;
            grvStatuses.DataBind();
        }
        public string GetMimeTypeByFileName(string sFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(sFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
            }

            return sMime;
        }
        protected void grvAttachments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_presenter.CurrentPaymentReimbursementRequest != null)
            {
                int attachmentId = Convert.ToInt32(grvAttachments.SelectedDataKey.Value);
                ELRAttachment attachment = _presenter.GetAttachment(attachmentId);

                string Filename = attachment.FilePath;

              

                System.Web.HttpContext context = System.Web.HttpContext.Current;
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                //context.Response.AppendHeader("content-length", FileData.Length.ToString());
                context.Response.ContentType = GetMimeTypeByFileName(Filename);
                context.Response.AppendHeader("content-disposition", "attachment; filename=" + Filename);
               // context.Response.BinaryWrite(FileData);

                context.ApplicationInstance.CompleteRequest();
            }
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
}
}
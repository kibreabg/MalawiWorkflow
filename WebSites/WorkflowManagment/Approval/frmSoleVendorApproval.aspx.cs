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
using Chai.WorkflowManagment.CoreDomain.Requests;
using System.Data.Entity.Validation;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmSoleVendorApproval : POCBasePage, ISoleVendorApprovalView
    {
        private SoleVendorApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        private SoleVendorRequest _solevendorrequest;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                BindSearchSoleVendorRequestGrid();
            }
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentSoleVendorRequest != null)
            {
                if (_presenter.CurrentSoleVendorRequest.Id != 0)
                {
                    BindSoleVendorRequestforprint();
                }
            }

        }
        [CreateNew]
        public SoleVendorApprovalPresenter Presenter
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
        public CoreDomain.Requests.SoleVendorRequest SoleVendorRequest
        {
            get
            {
                return _solevendorrequest;
            }
            set
            {
                _solevendorrequest = value;
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
        public int SoleVendorRequestId
        {
            get
            {
                if (grvSoleVendorRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvSoleVendorRequestList.SelectedDataKey.Value);
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
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.SoleVendor_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if ((AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager") && _presenter.CurrentSoleVendorRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;

                }
                else if (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
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
        private void BindSoleVendorRequestStatus()
        {
            foreach (SoleVendorRequestStatus SVRS in _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses)
            {
                if (SVRS.WorkflowLevel == _presenter.CurrentSoleVendorRequest.CurrentLevel && _presenter.CurrentSoleVendorRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    ddlApprovalStatus.SelectedValue = SVRS.ApprovalStatus;
                    txtRejectedReason.Text = SVRS.RejectedReason;
                    btnApprove.Enabled = true;

                }
                if (_presenter.CurrentSoleVendorRequest.CurrentLevel == _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count && SVRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    //btnPurchaseOrder.Enabled = true;
                    btnApprove.Enabled = false;
                }
                else
                {
                    btnPrint.Enabled = false;
                    //btnPurchaseOrder.Enabled = false;
                    btnApprove.Enabled = true;
                }

            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentSoleVendorRequest.CurrentLevel == _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count && _presenter.CurrentSoleVendorRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
                //btnPurchaseOrder.Enabled = true;
                SendEmailToRequester();

            }

        }
        private void SendEmail(SoleVendorRequestStatus SVRS)
        {
            if (_presenter.GetUser(SVRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(SVRS.Approver).Email, "Sole Vendor Request", (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName).ToUpper() + " Requests for sole Vendor with Sole Vendor Request No. - '" + (_presenter.CurrentSoleVendorRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(SVRS.Approver).AssignedTo).Email, "Sole Vendor Request", (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName).ToUpper() + " Requests for Leave with Leave Request No. - '" + (_presenter.CurrentSoleVendorRequest.RequestNo).ToUpper() + "'");
            }
        }
        private void SendEmailRejected(SoleVendorRequestStatus SVRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).Email, "Sole Vendor Request Rejection", "Your Sole Vendor Request with Sole Vendor Request No. " + (_presenter.CurrentSoleVendorRequest.RequestNo).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (SVRS.RejectedReason).ToUpper() + "'");

            if (SVRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < SVRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[i].Approver).Email, "Sole Vendor Request Rejection", "Leave Request with Leave Request No. - " + (_presenter.CurrentSoleVendorRequest.RequestNo).ToUpper() + " made by " + (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName).ToUpper() + " was Rejected by " + _presenter.CurrentUser().FullName + " for this reason - '" + (SVRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).Email, "Sole Vendor Request ", "Your Sole Vendor Request with Sole Vendor Request No. - '" + (_presenter.CurrentSoleVendorRequest.RequestNo).ToUpper() + "' was Completed.");
        }
        private void GetNextApprover()
        {
            foreach (SoleVendorRequestStatus SVRS in _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses)
            {
                if (SVRS.ApprovalStatus == null)
                {
                    SendEmail(SVRS);
                    _presenter.CurrentSoleVendorRequest.CurrentApprover = SVRS.Approver;
                    _presenter.CurrentSoleVendorRequest.CurrentLevel = SVRS.WorkflowLevel;
                    _presenter.CurrentSoleVendorRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    _presenter.CurrentSoleVendorRequest.CurrentStatus = SVRS.ApprovalStatus;
                    break;

                }
            }
        }
        private void SaveSoleVendorRequestStatus()
        {
            foreach (SoleVendorRequestStatus SVRS in _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses)
            {
                if ((SVRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(SVRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(SVRS.Approver).AssignedTo : 0)) && SVRS.WorkflowLevel == _presenter.CurrentSoleVendorRequest.CurrentLevel)
                {
                    SVRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    SVRS.RejectedReason = txtRejectedReason.Text;
                    SVRS.ApprovalDate = DateTime.Today.Date;
                    SVRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(SVRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(SVRS.Approver).AppUser.FullName : "";
                    if (SVRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {

                        if (_presenter.CurrentSoleVendorRequest.CurrentLevel == _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count)
                        {
                            _presenter.CurrentSoleVendorRequest.CurrentApprover = SVRS.Approver;
                            _presenter.CurrentSoleVendorRequest.CurrentLevel = SVRS.WorkflowLevel;
                            _presenter.CurrentSoleVendorRequest.CurrentStatus = SVRS.ApprovalStatus;
                            _presenter.CurrentSoleVendorRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        SVRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(SVRS.Approver).FullName + " has " + SVRS.ApprovalStatus + " Sole Vendor Request made by " + _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName);
                    }
                    else
                    {
                        _presenter.CurrentSoleVendorRequest.CurrentApprover = SVRS.Approver;
                        _presenter.CurrentSoleVendorRequest.CurrentLevel = SVRS.WorkflowLevel;
                        _presenter.CurrentSoleVendorRequest.CurrentStatus = SVRS.ApprovalStatus;
                        _presenter.CurrentSoleVendorRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        SVRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(SVRS);
                        Log.Info(_presenter.GetUser(SVRS.Approver).FullName + " has " + SVRS.ApprovalStatus + " Sole Vendor Request made by " + _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName);
                    }
                    break;
                }

            }

        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {

                if (_presenter.CurrentSoleVendorRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveSoleVendorRequestStatus();
                    Session["PurchaseId"] = _presenter.CurrentSoleVendorRequest.PurchaseRequest.Id;
                    _presenter.SaveOrUpdateSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
                    ShowPrint();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Sole Vendor Approval Processed ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Sole Vendor Request Rejected ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    btnApprove.Enabled = false;

                    BindSearchSoleVendorRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
                BindSoleVendorRequestforprint();
            }
            catch (DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("There is an error approving the Sole Vendor Request", Chai.WorkflowManagment.Enums.RMessageType.Error));
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
        private void BindSearchSoleVendorRequestGrid()
        {
            grvSoleVendorRequestList.DataSource = _presenter.ListSoleVendorRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text, ddlProgressStatus.SelectedValue);
            grvSoleVendorRequestList.DataBind();
        }
        protected void grvSoleVendorRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {

            _presenter.OnViewLoaded();
            PopApprovalStatus();
            BindSoleVendorRequestStatus();
            grvAttachments.DataSource = _presenter.CurrentSoleVendorRequest.SVRAttachments;
            grvAttachments.DataBind();
            txtRejectedReason.Visible = false;
            rfvRejectedReason.Enabled = false;
            pnlApproval_ModalPopupExtender.Show();

        }
        protected void grvSoleVendorRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


        }
        protected void grvSoleVendorRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /* Button btnStatus = e.Row.FindControl("btnStatus") as Button;
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
             }*/

        }
        protected void grvSoleVendorRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSoleVendorRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchSoleVendorRequestGrid();
            // pnlPopUpSearch_ModalPopupExtender.Show();
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
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
            pnlDetail_ModalPopupExtender.Show();
        }
        private void BindSoleVendorRequestforprint()
        {
            lblRequestNoresult.Text = _presenter.CurrentSoleVendorRequest.RequestNo;
            lblRequestedDateresult.Text = _presenter.CurrentSoleVendorRequest.RequestDate.ToString();
            lblContactPersonNumberRes.Text = _presenter.CurrentSoleVendorRequest.ContactPersonNumber;
            lblProposedPurchasedpriceres.Text = _presenter.CurrentSoleVendorRequest.ProposedPurchasedPrice.ToString();
            lblProposedSupplierresp.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierName;
            lblPaymentMethRes.Text = _presenter.CurrentSoleVendorRequest.PaymentMethod;
            lblSoleSourceJustificationPreparedByresp.Text = _presenter.CurrentSoleVendorRequest.SoleSourceJustificationPreparedBy;
            lblSoleVendorJustificationTyperes.Text = _presenter.CurrentSoleVendorRequest.SoleVendorJustificationType;
            lblapprovalstatusres.Text = _presenter.CurrentSoleVendorRequest.CurrentStatus;
            lblRequesterres.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;


            grvStatuses.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        //protected void btnPurchaseOrder_Click(object sender, EventArgs e)
        //{
        //    int purchaseID = _presenter.CurrentSoleVendorRequest.PurchaseRequest.Id;
        //    Response.Redirect(String.Format("frmPurchaseOrderSoleVendor.aspx?SoleVendorRequestId={0}", purchaseID));
        //}

        protected void grvSoleVendorRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqID = (int)grvSoleVendorRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqID;
                _presenter.CurrentSoleVendorRequest = _presenter.GetSoleVendorRequestById(reqID);
                if (e.CommandName == "ViewItem")
                {
                    dgSoleVendorRequestDetail.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
                    dgSoleVendorRequestDetail.DataBind();
                    pnlDetail_ModalPopupExtender.Show();
                }
            }
        }
    }
}
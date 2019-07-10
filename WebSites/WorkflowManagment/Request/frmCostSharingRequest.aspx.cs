using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using log4net.Config;
using log4net;
using System.Reflection;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmCostSharingRequest : POCBasePage, ICostSharingRequestView
    {
        private CostSharingRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindCostSharingRequests();
                BindAccountDescription();
                PopPayee();
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
            
        }

        [CreateNew]
        public CostSharingRequestPresenter Presenter
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
                return "{BECD9CB0-C328-4E89-ABD7-A54D1B2B3570}";
            }
        }
        #region Field Getters
        public int GetCostSharingRequestId
        {
            get
            {
                if (grvCashPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvCashPaymentRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetRequestNo
        {
            get { return AutoNumber(); }
        }
        public string GetPayee
        {
            get { return ddlPayee.SelectedValue; }
        }
        public string GetDescription
        {
            get { return txtDescription.Text; }
        }
        public string GetVoucherNo
        {
            get { return AutoNumber(); }
        }
        public decimal EstimatedTotalAmount
        {
            get { return Convert.ToDecimal(txtEstimatedCost.Text); }
        }
        public int ItemAccountId
        {
            get { return Convert.ToInt32(ddlAccountDescription.SelectedValue); }
        }
        public string GetAmountType
        {
            get { return ddlAmountType.SelectedValue; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        #endregion
        private string AutoNumber()
        {
           return  "CSV-" + (_presenter.GetLastCostSharingRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.CostSharing_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            // txtVoucherNo.Text = String.Empty;
            //txtVoucherNo.Text = String.Empty;
            txtEstimatedCost.Text = String.Empty;
        }
        private void PopPayee()
        {
            ddlPayee.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Payee ";
            lst.Value = "";
            ddlPayee.Items.Add(lst);
            ddlPayee.DataSource = _presenter.GetSuppliers();
            ddlPayee.DataBind();
        }
        private void BindCostSharingRequests()
        {
            grvCashPaymentRequestList.DataSource = _presenter.ListCostSharingRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvCashPaymentRequestList.DataBind();
        }
        private void BindCostSharingRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCostSharingRequest != null)
            {
                // txtRequestNo.Text = _presenter.CurrentCashPaymentRequest.RequestNo.ToString();
                ddlPayee.SelectedValue = _presenter.CurrentCostSharingRequest.Payee;
                txtDescription.Text = _presenter.CurrentCostSharingRequest.Description;
                ddlPayMethods.Text = _presenter.CurrentCostSharingRequest.PaymentMethod;
                txtEstimatedCost.Text = _presenter.CurrentCostSharingRequest.EstimatedTotalAmount.ToString();
                ddlAccountDescription.SelectedValue = _presenter.CurrentCostSharingRequest.ItemAccount.Id.ToString();
                BindCostSharingRequests();
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
        private void BindAccountDescription()
        {
            ddlAccountDescription.DataSource = _presenter.ListItemAccounts();
            ddlAccountDescription.DataValueField = "Id";
            ddlAccountDescription.DataTextField = "AccountName";
            ddlAccountDescription.DataBind();
        }
       
        protected void grvCashPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CashPaymentRequest"] = true;
            //ClearForm();
            BindCostSharingRequestFields();
            grvAttachments.DataSource = _presenter.CurrentCostSharingRequest.CSRAttachments;
            grvAttachments.DataBind();
            btnDelete.Visible = true;
            if (_presenter.CurrentCostSharingRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvCashPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateCostSharingRequest();
                if (_presenter.CurrentCostSharingRequest.CostSharingRequestDetails.Count != 0)
                {
                    if ((ddlAmountType.SelectedValue == "Estimated Amount" || ddlAmountType.SelectedValue == "Actual Amount") && _presenter.CurrentCostSharingRequest.CSRAttachments.Count != 0)
                    {
                        _presenter.SaveOrUpdateCostSharingRequest(_presenter.CurrentCostSharingRequest);
                        BindCostSharingRequests();
                        Master.ShowMessage(new AppMessage("Successfully did a Cost Sharing Payment  Request, Reference No - <b>'" + _presenter.CurrentCostSharingRequest.VoucherNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        Log.Info(_presenter.CurrentUser().FullName + " has requested a Cost Sharing Payment of Total Amount " + _presenter.CurrentCostSharingRequest.EstimatedTotalAmount.ToString());
                        btnSave.Visible = false;
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Please Attach Receipt", Chai.WorkflowManagment.Enums.RMessageType.Error));
                    }
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Cost Sharing Setting is not defined Please Contact Administrator", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteCostSharingRequest(_presenter.CurrentCostSharingRequest);
            ClearFormFields();
            BindCostSharingRequests();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Cost Sharing Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindCostSharingRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);   
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCostSharingRequest.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }

        #region Attachments
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile();
        }
        protected void DownloadFile(object sender, EventArgs e)
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
            //Response.Redirect(Request.Url.AbsoluteUri);


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
        #endregion 
        
    }
}
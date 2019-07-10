using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmPaymentReimbursementRequest : POCBasePage, IPaymentReimbursementRequestView
    {
        private PaymentReimbursementRequestPresenter _presenter;
        private IList<PaymentReimbursementRequest> _PaymentReimbursementRequests;
        int tarId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                CheckApprovalSettings();
                BindCashPayments();
                BindPaymentReimbursementRequests();
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
        }
        [CreateNew]
        public PaymentReimbursementRequestPresenter Presenter
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
                return "{0B827E67-C83E-45CB-B048-A709BD4625C8}";
            }
        }

        #region Field Getters
        public int GetTARequestId
        {
            get
            {
                if (tarId != 0)
                {
                    return Convert.ToInt32(tarId);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetComment
        {
            get { return txtComment.Text; }
        }
        public string GetExpenseType
        {
            get { return ddlExpenseType.SelectedValue; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        public IList<PaymentReimbursementRequest> PaymentReimbursementRequests
        {
            get
            {
                return _PaymentReimbursementRequests;
            }
            set
            {
                _PaymentReimbursementRequests = value;
            }
        }
        #endregion
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.PaymentReimbursement_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindPaymentReimbursementRequests()
        {
            grvPaymentReimbursementRequestList.DataSource = _presenter.ListPaymentReimbursementRequests(txtSrchRequestDate.Text);
            grvPaymentReimbursementRequestList.DataBind();
        }
        private void BindCashPayments()
        {
            grvCashPayments.DataSource = _presenter.ListCashPaymentsNotExpensed();
            grvCashPayments.DataBind();
        }
        private void BindPaymentReimbursementRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                txtComment.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.Comment;
                ddlPayMethods.Text = _presenter.CurrentCashPaymentRequest.PaymentMethod;
                ddlExpenseType.Text = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.ExpenseType;
                BindPaymentReimbursementRequests();
                //grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CPRAttachments;
                //grvAttachments.DataBind();
            }
        }
        private void PopulateReimbursement()
        {
            foreach (CashPaymentRequestDetail CPRD in _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails)
            {
                PaymentReimbursementRequestDetail PRRD = new PaymentReimbursementRequestDetail();
                PRRD.AmountAdvanced = CPRD.Amount;
                PRRD.ItemAccount = CPRD.ItemAccount;
                PRRD.Project = CPRD.Project;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Add(PRRD);
            }
        }
        private void BindPaymentReimbursementDetails()
        {
            if (_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Count == 0)
            {
                PopulateReimbursement();
            }
            dgPaymentReimbursementDetail.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails;
            dgPaymentReimbursementDetail.DataBind();
        }
        private void SetReimbursementDetails()
        {
            int index = 0;
            foreach (DataGridItem dgi in dgPaymentReimbursementDetail.Items)
            {
                int id = (int)dgPaymentReimbursementDetail.DataKeys[dgi.ItemIndex];

                PaymentReimbursementRequestDetail detail;
                if (id > 0)
                    detail = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.GetPaymentReimbursementRequestDetail(id);
                else
                    detail = (PaymentReimbursementRequestDetail)_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails[index];

                TextBox txtActualExpenditure = dgi.FindControl("txtActualExpenditure") as TextBox;
                detail.ActualExpenditure = Convert.ToDecimal(txtActualExpenditure.Text);
                TextBox txtVariance = dgi.FindControl("txtVariance") as TextBox;
                detail.Variance = Convert.ToDecimal(txtVariance.Text);
                index++;
                _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.PaymentReimbursementRequestDetails.Add(detail);
            }
        }
        protected void grvPaymentReimbursementRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PaymentReimbursementRequest"] = true;
            //ClearForm();
            BindPaymentReimbursementRequestFields();
            btnSave.Visible = true;
        }
        protected void grvPaymentReimbursementRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvPaymentReimbursementRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPaymentReimbursementRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPayments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvCashPayments.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvCashPayments_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grvCashPayments_SelectedIndexChanged(object sender, EventArgs e)
        {
            tarId = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            Session["tarId"] = Convert.ToInt32(grvCashPayments.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            btnSave.Visible = true;
            btnSave.Enabled = true;
            grvCashPayments.Visible = false;
            pnlInfo.Visible = false;
            BindPaymentReimbursementDetails();
        }
        protected void dgPaymentReimbursementDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void txtActualExpenditure_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            HiddenField hfAmountAdvanced = txt.FindControl("hfAmountAdvanced") as HiddenField;
            TextBox txtActualExpenditure = txt.FindControl("txtActualExpenditure") as TextBox;
            TextBox txtVariance = txt.FindControl("txtVariance") as TextBox;
            txtVariance.Text = ((Convert.ToDecimal(hfAmountAdvanced.Value) - Convert.ToDecimal(txtActualExpenditure.Text))).ToString();
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            UploadFile();
        }
        private void UploadFile()
        {
            string filePath = fuReciept.PostedFile.FileName;
            string filename = Path.GetFileName(filePath);
            string ext = Path.GetExtension(filename);
            string contenttype = String.Empty;

            //Set the contenttype based on File Extension
            switch (ext)
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".xls":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    contenttype = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                    contenttype = "image/jpg";
                    break;
                case ".png":
                    contenttype = "image/png";
                    break;
                case ".gif":
                    contenttype = "image/gif";
                    break;
                case ".pdf":
                    contenttype = "application/pdf";
                    break;
            }
            if (contenttype != String.Empty)
            {

                Stream fs = fuReciept.PostedFile.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                CPRAttachment attachment = new CPRAttachment();
                attachment.FilePath = filename;
             

                //_presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CPRAttachments.Add(attachment);
                ////insert the file into database
                //grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.PaymentReimbursementRequest.CPRAttachments;
                //grvAttachments.DataBind();
            }
            else
            {
                Master.ShowMessage(new AppMessage("File format not recognised. Upload Image/Word/PDF/Excel formats ", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetReimbursementDetails();
            _presenter.SaveOrUpdatePaymentReimbursementRequest(Convert.ToInt32(Session["tarId"]));
            BindPaymentReimbursementRequests();
            Master.ShowMessage(new AppMessage("Cash Payment Successfully Reimbursed!", Chai.WorkflowManagment.Enums.RMessageType.Info));
            btnSave.Visible = false;
            Session["tarId"] = null;
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindPaymentReimbursementRequests();
            pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmPaymentReimbursementRequest.aspx");
        }
        
       
    }
}
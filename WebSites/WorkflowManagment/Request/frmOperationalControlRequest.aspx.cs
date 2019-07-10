using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmOperationalControlRequest : POCBasePage, IOperationalControlRequestView
    {
        private OperationalControlRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindOperationalControlRequests();
                PopBeneficiaries();
                PopBankAccounts();

            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

            if (!this.IsPostBack)
            {
                PopulateBankPaymentDetail();
                BindOperationalControlDetails();
                Bindattachments();
            }
            //if (_presenter.CurrentOperationalControlRequest.Id <= 0)
            //{
            //    AutoNumber();
            //}
        }

        [CreateNew]
        public OperationalControlRequestPresenter Presenter
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
                return "{8C9E89D2-3048-4242-A0A9-88D41FF8C700}";
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
        public int GetBankAccountId
        {
            get { return Convert.ToInt32(ddlBankAccount.SelectedValue); }
        }
        //public string GetPayee
        //{
        //    get { return txtPayee.Text; }
        //}
        public string GetDescription
        {
            get { return txtDescription.Text; }
        }
        public int GetBeneficiaryId
        {
            get { return int.Parse(ddlBeneficiary.SelectedValue); }
        }
        public string GetBranchCode
        {
            get { return txtBranchCode.Text; }
        }
        public string GetBankName
        {
            get { return txtBankName.Text; }
        }
        public string GetVoucherNo
        {
            get { return AutoNumber(); }
        }

        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        public string GetPageType
        {
            get { if (Request.QueryString["Page"] != null) { return Request.QueryString["Page"]; } else return "BankPayment"; }
        }
        #endregion
        private void PopulateBankPaymentDetail()
        {
            if (Request.QueryString["Page"] != null)
            {
                if (Request.QueryString["Page"].Contains("CashPayment"))
                {
                    CashPaymentRequest CPR = _presenter.GetCashPaymentRequest(Convert.ToInt32(Request.QueryString["PaymentId"]));
                    if (CPR != null)
                    {


                        foreach (CashPaymentRequestDetail CPRD in CPR.CashPaymentRequestDetails)
                        {
                            OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                            OCRD.ItemAccount = CPRD.ItemAccount;
                            OCRD.Project = CPRD.Project;
                            OCRD.Grant = CPRD.Grant;
                            OCRD.Amount = CPRD.Amount;
                            OCRD.ActualExpendture = CPRD.Amount;
                            OCRD.AccountCode = CPRD.AccountCode;
                            _presenter.CurrentOperationalControlRequest.TotalAmount += OCRD.Amount;
                            _presenter.CurrentOperationalControlRequest.TotalActualExpendture += OCRD.Amount;
                            OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                            _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);
                        }
                        if (CPR.CPRAttachments.Count > 0)
                        {
                            foreach (CPRAttachment CP in CPR.CPRAttachments)
                            {
                                OCRAttachment OPA = new OCRAttachment();

                                OPA.FilePath = CP.FilePath;
                                OPA.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(OPA);
                            }
                        }

                    }
                }

                else if (Request.QueryString["Page"].Contains("CostSharing"))
                {
                    CostSharingRequest CPR = _presenter.GetCostSharingPaymentRequest(Convert.ToInt32(Request.QueryString["PaymentId"]));
                    if (CPR != null)
                    {


                        foreach (CostSharingRequestDetail CPRD in CPR.CostSharingRequestDetails)
                        {
                            OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                            OCRD.ItemAccount = CPRD.CostSharingRequest.ItemAccount;
                            OCRD.Project = CPRD.Project;
                            OCRD.Grant = CPRD.Grant;
                            OCRD.Amount = CPRD.SharedAmount;
                            OCRD.ActualExpendture = CPRD.SharedAmount;
                            OCRD.AccountCode = CPRD.CostSharingRequest.ItemAccount.AccountCode;
                            _presenter.CurrentOperationalControlRequest.TotalAmount += OCRD.Amount;
                            _presenter.CurrentOperationalControlRequest.TotalActualExpendture += OCRD.Amount;
                            OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                            _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);
                        }
                        if (CPR.CSRAttachments.Count > 0)
                        {
                            foreach (CSRAttachment CP in CPR.CSRAttachments)
                            {
                                OCRAttachment OPA = new OCRAttachment();
                                OPA.FilePath = CP.FilePath;
                                OPA.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(OPA);
                            }
                        }
                    }
                }
                /*else if (Request.QueryString["Page"].Contains("TravelAdvance"))
                {
                    CostSharingRequest CPR = _presenter.GetCostSharingPaymentRequest(Convert.ToInt32(Request.QueryString["PaymentId"]));
                    if (CPR != null)
                    {


                        foreach (CostSharingRequestDetail CPRD in CPR.CostSharingRequestDetails)
                        {
                            OperationalControlRequestDetail OCRD = new OperationalControlRequestDetail();
                            OCRD.ItemAccount = CPRD.CostSharingRequest.ItemAccount;
                            OCRD.Project = CPRD.Project;
                            OCRD.Grant = CPRD.Grant;
                            OCRD.Amount = CPRD.SharedAmount;
                            OCRD.ActualExpendture = CPRD.SharedAmount;
                            OCRD.AccountCode = CPRD.CostSharingRequest.ItemAccount.AccountCode;
                            _presenter.CurrentOperationalControlRequest.TotalAmount += OCRD.Amount;
                            _presenter.CurrentOperationalControlRequest.TotalActualExpendture += OCRD.Amount;
                            OCRD.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                            _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(OCRD);
                        }
                        if (CPR.CSRAttachments.Count > 0)
                        {
                            foreach (CSRAttachment CP in CPR.CSRAttachments)
                            {
                                OCRAttachment OPA = new OCRAttachment();
                                OPA.FilePath = CP.FilePath;
                                OPA.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                                _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(OPA);
                            }
                        }
                    }
                }*/
            }
        }
        private string AutoNumber()
        {
            return "BP-" + (_presenter.GetLastOperationalControlRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.OperationalControl_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            // txtVoucherNo.Text = String.Empty;
            //txtPayee.Text = String.Empty;
            // txtVoucherNo.Text = String.Empty;
        }
        private void PopBankAccounts()
        {
            ddlBankAccount.DataSource = _presenter.GetBankAccounts();
            ddlBankAccount.DataBind();
        }
        private void PopBeneficiaries()
        {
            ddlBeneficiary.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Beneficiary ";
            lst.Value = "0";
            ddlBeneficiary.Items.Add(lst);
            ddlBeneficiary.DataSource = _presenter.GetBeneficiaries();
            ddlBeneficiary.DataBind();
        }
        private void BindOperationalControlDetails()
        {
            dgOperationalControlDetail.DataSource = _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails;
            dgOperationalControlDetail.DataBind();
        }
        private void Bindattachments()
        {
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
        }
        private void BindOperationalControlRequests()
        {
            grvOperationalControlRequestList.DataSource = _presenter.ListOperationalControlRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvOperationalControlRequestList.DataBind();
        }
        private void BindOperationalControlRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentOperationalControlRequest != null)
            {
                // txtRequestNo.Text = _presenter.CurrentOperationalControlRequest.RequestNo.ToString();
                //txtPayee.Text = _presenter.CurrentOperationalControlRequest.Payee;
                ddlBeneficiary.SelectedValue = _presenter.CurrentOperationalControlRequest.Beneficiary.Id.ToString();
                txtBranchCode.Text = _presenter.CurrentOperationalControlRequest.BranchCode;
                txtBankName.Text = _presenter.CurrentOperationalControlRequest.BankName;
                txtDescription.Text = _presenter.CurrentOperationalControlRequest.Description;
                ddlPayMethods.Text = _presenter.CurrentOperationalControlRequest.PaymentMethod;
                // txtVoucherNo.Text = _presenter.CurrentOperationalControlRequest.VoucherNo.ToString();
                ddlBankAccount.SelectedValue = _presenter.CurrentOperationalControlRequest.Account.Id.ToString();
                txtBankAccountNo.Text = _presenter.GetBankAccount(_presenter.CurrentOperationalControlRequest.Account.Id).AccountNo;
                BindOperationalControlDetails();
                BindOperationalControlRequests();

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
        protected void dgOperationalControlDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgOperationalControlDetail.EditItemIndex = -1;
        }
        protected void dgOperationalControlDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            OperationalControlRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentOperationalControlRequest.GetOperationalControlRequestDetail(CPRDId);
            else
                cprd = (OperationalControlRequestDetail)_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.ItemIndex];
            try
            {
                if (CPRDId > 0)
                {
                    _presenter.CurrentOperationalControlRequest.RemoveOperationalControlRequestDetail(id);
                    if (_presenter.GetOperationalControlRequestDetail(id) != null)
                        _presenter.DeleteOperationalControlRequestDetail(_presenter.GetOperationalControlRequestDetail(id));
                    _presenter.SaveOrUpdateOperationalControlRequest(_presenter.CurrentOperationalControlRequest);
                }
                else { _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Remove(cprd); }
                BindOperationalControlDetails();

                Master.ShowMessage(new AppMessage("Bank Payment Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bank Payment Request Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgOperationalControlDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            //CarRental CarRental = new CarRental();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    OperationalControlRequestDetail cprd = new OperationalControlRequestDetail();
                    cprd.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                    TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                    cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtAccountCode = e.Item.FindControl("txtAccountCode") as TextBox;
                    cprd.AccountCode = txtAccountCode.Text;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                    cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                    _presenter.CurrentOperationalControlRequest.TotalAmount += cprd.Amount;

                    _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(cprd);

                    dgOperationalControlDetail.EditItemIndex = -1;
                    BindOperationalControlDetails();
                    Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Added!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Bank Payment " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgOperationalControlDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgOperationalControlDetail.EditItemIndex = e.Item.ItemIndex;
            BindOperationalControlDetails();
            //BindCarRentals();
        }
        protected void dgOperationalControlDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int CPRDId = (int)dgOperationalControlDetail.DataKeys[e.Item.ItemIndex];
            OperationalControlRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentOperationalControlRequest.GetOperationalControlRequestDetail(CPRDId);
            else
                cprd = (OperationalControlRequestDetail)_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.OperationalControlRequest = _presenter.CurrentOperationalControlRequest;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                //  _presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Add(cprd);

                dgOperationalControlDetail.EditItemIndex = -1;
                BindOperationalControlDetails();
                Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bank Payment. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgOperationalControlDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                BindProject(ddlProject);
                DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
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
        protected void grvOperationalControlRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["OperationalControlRequest"] = true;
            //ClearForm();
            BindOperationalControlRequestFields();
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
            btnDelete.Visible = true;
            if (_presenter.CurrentOperationalControlRequest.CurrentStatus != null)
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
        protected void grvOperationalControlRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvOperationalControlRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentOperationalControlRequest.OperationalControlRequestDetails.Count != 0)
                {
                    if (_presenter.CurrentOperationalControlRequest.OCRAttachments.Count != 0)
                    {
                        _presenter.SaveOrUpdateOperationalControlRequest();
                        BindOperationalControlRequests();
                        Master.ShowMessage(new AppMessage("Successfully did a Bank Payment  Request, Reference No - <b>'" + _presenter.CurrentOperationalControlRequest.VoucherNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        Log.Info(_presenter.CurrentUser().FullName + " has requested a Bank Payment for a total amount of " + _presenter.CurrentOperationalControlRequest.TotalAmount.ToString());
                        btnSave.Visible = false;
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Please Attach Receipt", Chai.WorkflowManagment.Enums.RMessageType.Error));
                    }
                }


                else
                {
                    Master.ShowMessage(new AppMessage("Please insert at least one Item Detail", Chai.WorkflowManagment.Enums.RMessageType.Error));
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
            _presenter.DeleteOperationalControlRequest(_presenter.CurrentOperationalControlRequest);
            ClearFormFields();
            BindOperationalControlRequests();
            BindOperationalControlDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Bank Payment Request Successfully Deleted!", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindOperationalControlRequests();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
            //pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmOperationalControlRequest.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void ddlAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlEdtProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlEdtGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlFGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAccount = (DropDownList)sender;
            if (Convert.ToInt32(ddlAccount.SelectedValue) != 0)
                txtBankAccountNo.Text = _presenter.GetBankAccount(Convert.ToInt32(ddlAccount.SelectedValue)).AccountNo;
        }

        protected void ddlBeneficiary_SelectedIndexChanged(object sender, EventArgs e)
        {
            Beneficiary Benef = _presenter.GetBeneficiary(Convert.ToInt32(ddlBeneficiary.SelectedValue));
            if (Benef != null)
            {

                txtBranchCode.Text = Benef.BranchName;
                txtBankName.Text = Benef.BankName;
            }


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
            _presenter.CurrentOperationalControlRequest.RemoveOCAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);

            if (fileName != String.Empty)
            {



                OCRAttachment attachment = new OCRAttachment();
                attachment.FilePath = "~/OCUploads/" + fileName;
                fuReciept.PostedFile.SaveAs(Server.MapPath("~/OCUploads/") + fileName);
                //Response.Redirect(Request.Url.AbsoluteUri);
                _presenter.CurrentOperationalControlRequest.OCRAttachments.Add(attachment);

                grvAttachments.DataSource = _presenter.CurrentOperationalControlRequest.OCRAttachments;
                grvAttachments.DataBind();


            }
            else
            {
                Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }


        }


        #endregion
        #region Beneficaries
        void BindBeneficiaries()
        {
            dgBeneficiary.DataSource = _presenter.ListBeneficiaries("");
            dgBeneficiary.DataBind();
            PopBeneficiaries();
        }
        protected void dgBeneficiary_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = -1;
            pnlBeneficary_ModalPopupExtender.Show();
        }
        protected void dgBeneficiary_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);
            try
            {
                _presenter.DeleteBeneficiary(beneficiary);
                BindBeneficiaries();

                Master.ShowMessage(new AppMessage("beneficiary was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete beneficiary. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
            pnlBeneficary_ModalPopupExtender.Show();
        }
        protected void dgBeneficiary_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Beneficiary beneficiary = new Beneficiary();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtName = e.Item.FindControl("txtBeneficiaryName") as TextBox;
                    beneficiary.BeneficiaryName = txtName.Text;
                    TextBox txtBranchName = e.Item.FindControl("txtBranchName") as TextBox;
                    beneficiary.BranchName = txtBranchName.Text;
                    TextBox txtBankName = e.Item.FindControl("txtBankName") as TextBox;
                    beneficiary.BankName = txtBankName.Text;
                    TextBox txtSortCode = e.Item.FindControl("txtSortCode") as TextBox;
                    beneficiary.SortCode = txtSortCode.Text;
                    TextBox txtAccountNumber = e.Item.FindControl("txtAccountNumber") as TextBox;
                    beneficiary.AccountNumber = txtAccountNumber.Text;
                    SaveBeneficiary(beneficiary);
                    dgBeneficiary.EditItemIndex = -1;
                    BindBeneficiaries();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Beneficiary " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            pnlBeneficary_ModalPopupExtender.Show();
        }
        private void SaveBeneficiary(Beneficiary beneficiary)
        {
            try
            {
                if (beneficiary.Id <= 0)
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgBeneficiary_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = e.Item.ItemIndex;

            BindBeneficiaries();
            pnlBeneficary_ModalPopupExtender.Show();
        }
        protected void dgBeneficiary_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgBeneficiary_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtEdtBeneficiaryName") as TextBox;
                beneficiary.BeneficiaryName = txtName.Text;
                TextBox txtBranchName = e.Item.FindControl("txtEdtBranchName") as TextBox;
                beneficiary.BranchName = txtBranchName.Text;
                TextBox txtBankName = e.Item.FindControl("txtEdtBankName") as TextBox;
                beneficiary.BankName = txtBankName.Text;
                TextBox txtSortCode = e.Item.FindControl("txtEdtSortCode") as TextBox;
                beneficiary.SortCode = txtSortCode.Text;
                TextBox txtAccountNumber = e.Item.FindControl("txtEdtAccountNumber") as TextBox;
                beneficiary.AccountNumber = txtAccountNumber.Text;
                SaveBeneficiary(beneficiary);
                dgBeneficiary.EditItemIndex = -1;
                BindBeneficiaries();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Beneficiary. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
            pnlBeneficary_ModalPopupExtender.Show();
        }
        protected void btnpop_Click(object sender, EventArgs e)
        {
            BindBeneficiaries();
            pnlBeneficary_ModalPopupExtender.Show();
        }
        #endregion
    }

}

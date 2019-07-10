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
    public partial class frmBankPaymentRequest : POCBasePage, IBankPaymentRequestView
    {
        private BankPaymentRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindBankPaymentRequests();
                BindBankPaymentDetails();
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentBankPaymentRequest.Id <= 0)
            {
                AutoNumber();
            }
        }

        [CreateNew]
        public BankPaymentRequestPresenter Presenter
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
                return "{034BD8C5-81DB-4D73-AC88-376E26F8E1E3}";
            }
        }
        #region Field Getters
        public int GetBankPaymentRequestId
        {
            get
            {
                if (grvBankPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvBankPaymentRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetRequestNo
        {
            get { return txtVoucherNo.Text; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        #endregion
        private void AutoNumber()
        {
            txtVoucherNo.Text = "BP-" + (_presenter.GetLastBankPaymentRequestId() + 1).ToString() + DateTime.Now.Second.ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.BankPayment_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindAccounts(DropDownList ddlAccount)
        {
            ddlAccount.DataSource = _presenter.ListAccounts();
            ddlAccount.DataValueField = "Id";
            ddlAccount.DataTextField = "AccountNo";
            ddlAccount.DataBind();
        }
        private void BindBankPaymentDetails()
        {
            dgBankPaymentDetail.DataSource = _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails;
            dgBankPaymentDetail.DataBind();
        }
        private void BindBankPaymentRequests()
        {
            grvBankPaymentRequestList.DataSource = _presenter.ListBankPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvBankPaymentRequestList.DataBind();
        }
        private void BindBankPaymentRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentBankPaymentRequest != null)
            {
                // txtRequestNo.Text = _presenter.CurrentBankPaymentRequest.RequestNo.ToString();
                BindBankPaymentDetails();
                BindBankPaymentRequests();
            }
        }
        protected void dgBankPaymentDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBankPaymentDetail.EditItemIndex = -1;
        }
        protected void dgBankPaymentDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBankPaymentDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgBankPaymentDetail.DataKeys[e.Item.ItemIndex];
            BankPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentBankPaymentRequest.GetBankPaymentRequestDetail(CPRDId);
            else
                cprd = (BankPaymentRequestDetail)_presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails[e.Item.ItemIndex];
            try
            {
                if (CPRDId > 0)
                {
                    _presenter.CurrentBankPaymentRequest.RemoveBankPaymentRequestDetail(id);
                    if (_presenter.GetBankPaymentRequestDetail(id) != null)
                        _presenter.DeleteBankPaymentRequestDetail(_presenter.GetBankPaymentRequestDetail(id));
                    _presenter.SaveOrUpdateBankPaymentRequest(_presenter.CurrentBankPaymentRequest);
                }
                else { _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails.Remove(cprd); }
                BindBankPaymentDetails();

                Master.ShowMessage(new AppMessage("Bank Payment Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bank Payment Request Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBankPaymentDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    BankPaymentRequestDetail cprd = new BankPaymentRequestDetail();
                    cprd.BankPaymentRequest = _presenter.CurrentBankPaymentRequest;
                    TextBox txtBankCode = e.Item.FindControl("txtBankCode") as TextBox;
                    cprd.BankCode = Convert.ToInt32(txtBankCode.Text);
                    TextBox txtName = e.Item.FindControl("txtName") as TextBox;
                    cprd.Name = txtName.Text;
                    TextBox txtAmount = e.Item.FindControl("txtAmount") as TextBox;
                    cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                    TextBox txtCurrency = e.Item.FindControl("txtCurrency") as TextBox;
                    cprd.Currency = txtCurrency.Text;
                    DropDownList ddlAccount = e.Item.FindControl("ddlAccount") as DropDownList;
                    cprd.Account = _presenter.GetAccount(Convert.ToInt32(ddlAccount.SelectedValue));
                    //Added Fields
                    cprd.Reference = txtVoucherNo.Text;
                    cprd.ProcessDate = Convert.ToDateTime(txtRequestDate.Text);

                    _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails.Add(cprd);

                    dgBankPaymentDetail.EditItemIndex = -1;
                    BindBankPaymentDetails();
                    Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Added!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Bank Payment Detail " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgBankPaymentDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBankPaymentDetail.EditItemIndex = e.Item.ItemIndex;
            BindBankPaymentDetails();
            //BindCarRentals();
        }
        protected void dgBankPaymentDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int CPRDId = (int)dgBankPaymentDetail.DataKeys[e.Item.ItemIndex];
            BankPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentBankPaymentRequest.GetBankPaymentRequestDetail(CPRDId);
            else
                cprd = (BankPaymentRequestDetail)_presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.BankPaymentRequest = _presenter.CurrentBankPaymentRequest;
                TextBox txtBankCode = e.Item.FindControl("txtEdtBankCode") as TextBox;
                cprd.BankCode = Convert.ToInt32(txtBankCode.Text);
                TextBox txtName = e.Item.FindControl("txtEdtName") as TextBox;
                cprd.Name = txtName.Text;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                TextBox txtCurrency = e.Item.FindControl("txtEdtCurrency") as TextBox;
                cprd.Currency = txtCurrency.Text;
                DropDownList ddlAccount = e.Item.FindControl("ddlEdtAccount") as DropDownList;
                cprd.Account = _presenter.GetAccount(Convert.ToInt32(ddlAccount.SelectedValue));

                dgBankPaymentDetail.EditItemIndex = -1;
                BindBankPaymentDetails();
                Master.ShowMessage(new AppMessage("Bank Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bank Payment Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBankPaymentDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlAccount = e.Item.FindControl("ddlAccount") as DropDownList;
                BindAccounts(ddlAccount);
            }
            else
            {
                DropDownList ddlAccount = e.Item.FindControl("ddlEdtAccount") as DropDownList;
                if (ddlAccount != null)
                {
                    BindAccounts(ddlAccount);
                    if (_presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails[e.Item.DataSetIndex].Account.Id != 0)
                    {
                        ListItem liI = ddlAccount.Items.FindByValue(_presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails[e.Item.DataSetIndex].Account.Id.ToString());
                        if (liI != null)
                            liI.Selected = true;
                    }
                }
            }
        }
        protected void grvBankPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["BankPaymentRequest"] = true;
            //ClearForm();
            BindBankPaymentRequestFields();
            btnDelete.Visible = true;
            if (_presenter.CurrentBankPaymentRequest.CurrentStatus != null)
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
        protected void grvBankPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBankPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (_presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails.Count != 0)
            {
                _presenter.SaveOrUpdateBankPaymentRequest();
                BindBankPaymentRequests();
                Master.ShowMessage(new AppMessage("Bank Payment Successfully Requested", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has requested a Bank Payment");
                btnSave.Visible = false;
            }
            else
            {
                Master.ShowMessage(new AppMessage("Please insert at least one Item Detail", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteBankPaymentRequest(_presenter.CurrentBankPaymentRequest);
            BindBankPaymentRequests();
            BindBankPaymentDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Bank Payment Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindBankPaymentRequests();
            pnlSearch_ModalPopupExtender.Show();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmBankPaymentRequest.aspx");
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }        
    }
}
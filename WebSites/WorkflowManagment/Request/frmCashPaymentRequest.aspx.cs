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
    public partial class frmCashPaymentRequest : POCBasePage, ICashPaymentRequestView
    {
        private CashPaymentRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindCashPaymentRequests();
                BindCashPaymentDetails();
                PopPayee();

            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

        }

        [CreateNew]
        public CashPaymentRequestPresenter Presenter
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
        public int GetCashPaymentRequestId
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
        public int GetPayee
        {
            get { return Convert.ToInt32(ddlPayee.SelectedValue); }
        }
        public string GetDescription
        {
            get { return txtDescription.Text; }
        }
        public string GetVoucherNo
        {
            get { return AutoNumber(); }
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
            return "CPV-" + (_presenter.GetLastCashPaymentRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.CashPayment_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void ClearFormFields()
        {
            // txtVoucherNo.Text = String.Empty;
            // txtVoucherNo.Text = String.Empty;
            ddlAmountType.SelectedValue = "";
        }
        private void BindCashPaymentDetails()
        {
            dgCashPaymentDetail.DataSource = _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails;
            dgCashPaymentDetail.DataBind();
        }
        private void BindCashPaymentRequests()
        {
            grvCashPaymentRequestList.DataSource = _presenter.ListCashPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvCashPaymentRequestList.DataBind();
        }
        private void PopPayee()
        {
            ddlPayee.Items.Clear();
            ListItem lst = new ListItem();
            lst.Text = " Select Payee ";
            lst.Value = "0";
            ddlPayee.Items.Add(lst);
            ddlPayee.DataSource = _presenter.GetSuppliers();
            ddlPayee.DataBind();
        }
        private void BindCashPaymentRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentCashPaymentRequest != null)
            {
                // txtRequestNo.Text = _presenter.CurrentCashPaymentRequest.RequestNo.ToString();
                if(_presenter.CurrentCashPaymentRequest.Supplier != null)
                {
                    ddlPayee.SelectedValue = _presenter.CurrentCashPaymentRequest.Supplier.Id.ToString();
                }
                else
                {
                    ddlPayee.SelectedValue = "0";
                }                
                txtDescription.Text = _presenter.CurrentCashPaymentRequest.Description;
                ddlPayMethods.Text = _presenter.CurrentCashPaymentRequest.PaymentMethod;
                ddlAmountType.SelectedValue = _presenter.CurrentCashPaymentRequest.AmountType;
                BindCashPaymentDetails();
                BindCashPaymentRequests();
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
        protected void dgCashPaymentDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = -1;
        }
        protected void dgCashPaymentDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            int CPRDId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];
            try
            {
                if (CPRDId > 0)
                {
                    _presenter.CurrentCashPaymentRequest.RemoveCashPaymentRequestDetail(id);
                    if (_presenter.GetCashPaymentRequestDetail(id) != null)
                        _presenter.DeleteCashPaymentRequestDetail(_presenter.GetCashPaymentRequestDetail(id));
                    _presenter.SaveOrUpdateCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
                }
                else { _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Remove(cprd); }
                BindCashPaymentDetails();

                Master.ShowMessage(new AppMessage("Payment Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Payment Request Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgCashPaymentDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    CashPaymentRequestDetail cprd = new CashPaymentRequestDetail();
                    cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
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
                    _presenter.CurrentCashPaymentRequest.TotalAmount += cprd.Amount;
                    if (ddlAmountType.SelectedValue == "Actual Amount")
                    {
                        cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                    }
                    _presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Add(cprd);

                    dgCashPaymentDetail.EditItemIndex = -1;
                    BindCashPaymentDetails();
                    Master.ShowMessage(new AppMessage("Payment Detail Successfully Added", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Payment " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgCashPaymentDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCashPaymentDetail.EditItemIndex = e.Item.ItemIndex;
            BindCashPaymentDetails();
            //BindCarRentals();
        }
        protected void dgCashPaymentDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            decimal previousAmount = 0;
            int CPRDId = (int)dgCashPaymentDetail.DataKeys[e.Item.ItemIndex];
            CashPaymentRequestDetail cprd;

            if (CPRDId > 0)
                cprd = _presenter.CurrentCashPaymentRequest.GetCashPaymentRequestDetail(CPRDId);
            else
                cprd = (CashPaymentRequestDetail)_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.ItemIndex];

            try
            {
                cprd.CashPaymentRequest = _presenter.CurrentCashPaymentRequest;
                TextBox txtAmount = e.Item.FindControl("txtEdtAmount") as TextBox;
                previousAmount = cprd.Amount; //This is the Total Amount of this request before any edit
                cprd.Amount = Convert.ToDecimal(txtAmount.Text);
                TextBox txtEdtAccountCode = e.Item.FindControl("txtEdtAccountCode") as TextBox;
                cprd.AccountCode = txtEdtAccountCode.Text;
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                cprd.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                DropDownList ddlProject = e.Item.FindControl("ddlEdtProject") as DropDownList;
                cprd.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlGrant = e.Item.FindControl("ddlEdtGrant") as DropDownList;
                cprd.Grant = _presenter.GetGrant(int.Parse(ddlGrant.SelectedValue));
                _presenter.CurrentCashPaymentRequest.TotalAmount -= previousAmount; //Subtract the previous Total amount
                _presenter.CurrentCashPaymentRequest.TotalAmount += cprd.Amount; //Then add the new individual amounts to the Total amount

                if (ddlAmountType.SelectedValue == "Actual Amount")
                {
                    cprd.ActualExpendture = Convert.ToDecimal(txtAmount.Text);
                }

                dgCashPaymentDetail.EditItemIndex = -1;
                BindCashPaymentDetails();
                Master.ShowMessage(new AppMessage("Payment Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Payment. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgCashPaymentDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
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
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails != null)
                {
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
                        if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id != 0)
                        {
                            ListItem liI = ddlEdtGrant.Items.FindByValue(_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void grvCashPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["CashPaymentRequest"] = true;
            //ClearForm();
            BindCashPaymentRequestFields();
            grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
            grvAttachments.DataBind();
            if (_presenter.CurrentCashPaymentRequest.CurrentStatus != null)
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
        protected void grvCashPaymentRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null)
            {
                CashPaymentRequest cashPayment = e.Row.DataItem as CashPaymentRequest;
                if (cashPayment.CurrentStatus == "Rejected")
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentCashPaymentRequest.CashPaymentRequestDetails.Count != 0)
                {
                    if ((ddlAmountType.SelectedValue == "Estimated Amount" || ddlAmountType.SelectedValue == "Actual Amount") && _presenter.CurrentCashPaymentRequest.CPRAttachments.Count != 0)
                    {
                        _presenter.SaveOrUpdateCashPaymentRequest();
                        BindCashPaymentRequests();
                        Master.ShowMessage(new AppMessage("Successfully did a Payment  Request, Reference No - <b>'" + _presenter.CurrentCashPaymentRequest.VoucherNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                        Log.Info(_presenter.CurrentUser().FullName + " has requested a Payment of Total Amount " + _presenter.CurrentCashPaymentRequest.TotalAmount.ToString());
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
                Master.ShowMessage(new AppMessage(ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
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
            _presenter.DeleteCashPaymentRequest(_presenter.CurrentCashPaymentRequest);
            ClearFormFields();
            BindCashPaymentRequests();
            BindCashPaymentDetails();
            btnDelete.Visible = false;
            Master.ShowMessage(new AppMessage("Payment Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindCashPaymentRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmCashPaymentRequest.aspx");
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
        protected void ddlEdtAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtEdtAccountCode = ddl.FindControl("txtEdtAccountCode") as TextBox;
            txtEdtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
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
            _presenter.CurrentCashPaymentRequest.RemoveCPAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentCashPaymentRequest.CPRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


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
        #endregion


    }
}
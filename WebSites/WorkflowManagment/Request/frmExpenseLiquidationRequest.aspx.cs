using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmExpenseLiquidationRequest : POCBasePage, IExpenseLiquidationRequestView
    {
        private ExpenseLiquidationRequestPresenter _presenter;
        private IList<ExpenseLiquidationRequest> _ExpenseLiquidationRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        int tarId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                BindTravelAdvances();
                BindExpenseLiquidationRequests();
            }

            this._presenter.OnViewLoaded();
        }
        [CreateNew]
        public ExpenseLiquidationRequestPresenter Presenter
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
                return "{E8C99D50-3363-4518-AD1D-9A370D6EC30F}";
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
        public string GetExpenseType
        {
            get { return ddlPExpenseType.SelectedValue; }
        }
        public string GetComment
        {
            get { return txtComment.Text; }
        }
        public string GetTravelAdvReqDate
        {
            get { return txtTravelAdvReqDate.Text; }
        }
        public string GetAdditionalComment
        {
            get { return txtAdditionalComment.Text; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        public IList<ExpenseLiquidationRequest> ExpenseLiquidationRequests
        {
            get
            {
                return _ExpenseLiquidationRequests;
            }
            set
            {
                _ExpenseLiquidationRequests = value;
            }
        }
        #endregion
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.ExpenseLiquidation_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindExpenseLiquidationRequests()
        {
            grvExpenseLiquidationRequestList.DataSource = _presenter.ListExpenseLiquidationRequests(ddlSrchExpenseType.SelectedValue, txtSrchRequestDate.Text);
            grvExpenseLiquidationRequestList.DataBind();
        }
        private void BindTravelAdvances()
        {
            grvTravelAdvances.DataSource = _presenter.ListTravelAdvancesNotExpensed();
            grvTravelAdvances.DataBind();
        }
        private void BindExpenseLiquidationRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentTravelAdvanceRequest != null)
            {
                ddlPExpenseType.SelectedValue = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseType;
                txtComment.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.Comment;
                txtTotActual.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure.ToString();
                txtTotalAdvance.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalTravelAdvance.ToString();
                txtAdditionalComment.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.AdditionalComment;
                ddlPayMethods.Text = _presenter.CurrentTravelAdvanceRequest.PaymentMethod;
                BindExpenseLiquidationDetails();
                BindExpenseLiquidationRequests();
            }
        }
        private void BindProject(DropDownList ddlProject)
        {
            ddlProject.DataSource = _presenter.ListProjects();
            ddlProject.DataValueField = "Id";
            ddlProject.DataTextField = "ProjectCode";
            ddlProject.DataBind();
        }
        private void PopExpenseTypes(DropDownList ddlExpenseType)
        {
            ddlExpenseType.DataSource = _presenter.GetExpenseTypes();
            ddlExpenseType.DataBind();


            //ddlAccountDescription.Items.Insert(0, new ListItem("---Select Account Description---", "0"));
            //ddlAccountDescription.SelectedIndex = 0;
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
        private void PopulateLiquidation()
        {
            foreach (TravelAdvanceRequestDetail TAD in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails)
            {
                foreach (TravelAdvanceCost TAC in TAD.TravelAdvanceCosts)
                {
                    ExpenseLiquidationRequestDetail ELD = new ExpenseLiquidationRequestDetail();
                    ELD.AmountAdvanced = TAC.Total;
                    ELD.ExpenseType = TAC.ExpenseType;
                    //ELD.ItemAccount = TAC.ItemAccount;
                    ELD.Project = TAC.TravelAdvanceRequestDetail.TravelAdvanceRequest.Project;
                    ELD.Grant = TAC.TravelAdvanceRequestDetail.TravelAdvanceRequest.Grant;
                    _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails.Add(ELD);
                }
            }
        }
        private void BindExpenseLiquidationDetails()
        {
            if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails.Count == 0)
            {
                PopulateLiquidation();
            }
            txtTravelAdvReqDate.Text = _presenter.CurrentTravelAdvanceRequest.RequestDate.Value.ToShortDateString();
            dgExpenseLiquidationDetail.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
            dgExpenseLiquidationDetail.DataBind();
            grvAttachments.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments;
            grvAttachments.DataBind();
        }
        private void SetLiquidationDetails()
        {
            int index = 0;
            foreach (DataGridItem dgi in dgExpenseLiquidationDetail.Items)
            {
                int id = (int)dgExpenseLiquidationDetail.DataKeys[dgi.ItemIndex];

                ExpenseLiquidationRequestDetail detail;
                if (id > 0)
                    detail = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.GetExpenseLiquidationRequestDetail(id);
                else
                    detail = (ExpenseLiquidationRequestDetail)_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[index];

                if (dgi.ItemType == ListItemType.Item || dgi.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtFRefNo = dgi.FindControl("txtRefNo") as TextBox;
                    detail.RefNo = txtFRefNo.Text;
                    DropDownList ddlAccountDescription = dgi.FindControl("ddlAccountDescription") as DropDownList;
                    detail.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    DropDownList ddlProject = dgi.FindControl("ddlProject") as DropDownList;
                    detail.Project = _presenter.GetProject(Convert.ToInt32(ddlProject.SelectedValue));
                    DropDownList ddlGrant = dgi.FindControl("ddlGrant") as DropDownList;
                    detail.Grant = _presenter.GetGrant(Convert.ToInt32(ddlGrant.SelectedValue));
                    TextBox txtActualExpenditure = dgi.FindControl("txtActualExpenditure") as TextBox;
                    detail.ActualExpenditure = Convert.ToDecimal(txtActualExpenditure.Text);
                    TextBox txtVariance = dgi.FindControl("txtVariance") as TextBox;
                    detail.Variance = Convert.ToDecimal(txtVariance.Text);
                    //_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure + detail.ActualExpenditure;
                    //txtTotActual.Text = (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure).ToString();
                }

                index++;
                //_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails.Add(detail);
            }
        }
        protected void grvExpenseLiquidationRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ExpenseLiquidationRequest"] = true;
            //ClearForm();
            tarId = (int)grvExpenseLiquidationRequestList.DataKeys[grvExpenseLiquidationRequestList.SelectedIndex].Value;
            Session["tarId"] = (int)grvExpenseLiquidationRequestList.DataKeys[grvExpenseLiquidationRequestList.SelectedIndex].Value;
            BindExpenseLiquidationRequestFields();
            grvAttachments.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments;
            grvAttachments.DataBind();
            PrintTransaction();
            btnPrint.Enabled = true;
            //This is done so that the user can not ammend a liquidation while it's in an approval process. But one can ammend a rejected liquidation.
            if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.CurrentStatus != null)
            {
                if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.CurrentStatus == ApprovalStatus.Rejected.ToString())
                {
                    btnSave.Visible = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                }

            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvExpenseLiquidationRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ExpenseLiquidationRequest liquidation = e.Row.DataItem as ExpenseLiquidationRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {               
                e.Row.Cells[0].Text = liquidation.TravelAdvanceRequest.TravelAdvanceNo;
                //The Rejected Expense Liquidation appears on the search grid of the requester as a Red colored row
                if(liquidation.CurrentStatus == ApprovalStatus.Rejected.ToString())
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }

            }
        }
        protected void grvExpenseLiquidationRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvExpenseLiquidationRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvTravelAdvances_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvances.PageIndex = e.NewPageIndex;
            BindTravelAdvances();
        }
        protected void grvTravelAdvances_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grvTravelAdvances_SelectedIndexChanged(object sender, EventArgs e)
        {
            tarId = Convert.ToInt32(grvTravelAdvances.SelectedDataKey[0]);
            Session["tarId"] = Convert.ToInt32(grvTravelAdvances.SelectedDataKey[0]);
            _presenter.OnViewLoaded();
            btnSave.Visible = true;
            grvTravelAdvances.Visible = false;
            pnlInfo.Visible = false;
            BindExpenseLiquidationDetails();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void dgExpenseLiquidationDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProject = e.Item.FindControl("ddlFProject") as DropDownList;
                BindProject(ddlProject);
                DropDownList ddlGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlFAccountDescription") as DropDownList;
                BindAccountDescription(ddlAccountDescription);
                DropDownList ddlExpenseType = e.Item.FindControl("ddlExpenseType") as DropDownList;
                PopExpenseTypes(ddlExpenseType);
            }
            else if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails != null)
                {
                    DropDownList ddlProject = e.Item.FindControl("ddlProject") as DropDownList;
                    BindProject(ddlProject);
                    if (ddlProject != null)
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Project != null)
                        {
                            ListItem liI = ddlProject.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Project.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    if (ddlGrant != null)
                    {
                        BindGrant(ddlGrant, Convert.ToInt32(ddlProject.SelectedValue));
                        if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Grant.Id != null)
                        {
                            ListItem liI = ddlGrant.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    BindAccountDescription(ddlAccountDescription);
                    if (ddlAccountDescription != null)
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ItemAccount != null)
                        {
                            ListItem liI = ddlAccountDescription.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                    DropDownList ddlEdtExpenseType = e.Item.FindControl("ddlEdtExpenseType") as DropDownList;
                    if (ddlEdtExpenseType != null)
                    {
                        PopExpenseTypes(ddlEdtExpenseType);
                        if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ExpenseType != null)
                        {
                            ListItem liI = ddlEdtExpenseType.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails[e.Item.DataSetIndex].ExpenseType.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                }
            }
        }
        protected void dgExpenseLiquidationDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    ExpenseLiquidationRequestDetail elrd1 = new ExpenseLiquidationRequestDetail();
                    TextBox txtFRefNo = e.Item.FindControl("txtFRefNo") as TextBox;
                    elrd1.RefNo = txtFRefNo.Text;
                    elrd1.ExpenseLiquidationRequest = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest;
                    DropDownList ddlFAccountDescription1 = e.Item.FindControl("ddlFAccountDescription") as DropDownList;
                    elrd1.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlFAccountDescription1.SelectedValue));
                    DropDownList ddlExpenseType = e.Item.FindControl("ddlExpenseType") as DropDownList;
                    elrd1.ExpenseType = _presenter.GetExpenseType(Convert.ToInt32(ddlExpenseType.SelectedValue));
                    DropDownList ddlFProject1 = e.Item.FindControl("ddlFProject") as DropDownList;
                    elrd1.Project = _presenter.GetProject(Convert.ToInt32(ddlFProject1.SelectedValue));
                    DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                    elrd1.Grant = _presenter.GetGrant(Convert.ToInt32(ddlFGrant.SelectedValue));
                    TextBox txtFAmount1 = e.Item.FindControl("txtFAmount") as TextBox;
                    elrd1.AmountAdvanced = Convert.ToDecimal(txtFAmount1.Text);
                    TextBox txtFActualExpenditure1 = e.Item.FindControl("txtFActualExpenditure") as TextBox;
                    elrd1.ActualExpenditure = Convert.ToDecimal(txtFActualExpenditure1.Text);
                    TextBox txtFVariance1 = e.Item.FindControl("txtFVariance") as TextBox;
                    elrd1.Variance = Convert.ToDecimal(txtFVariance1.Text);
                    _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails.Add(elrd1);
                    SetLiquidationDetails();
                    dgExpenseLiquidationDetail.EditItemIndex = -1;
                    BindExpenseLiquidationDetails();
                    Master.ShowMessage(new AppMessage("Expense Liquidation Detail Successfully Added!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Expense Liquidation Detail " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void txtActualExpenditure_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            HiddenField hfAmountAdvanced = txt.FindControl("hfAmountAdvanced") as HiddenField;
            TextBox txtActualExpenditure = txt.FindControl("txtActualExpenditure") as TextBox;
            TextBox txtVariance = txt.FindControl("txtVariance") as TextBox;
            if (txtActualExpenditure.Text == "")
                txtActualExpenditure.Text = "0";
            txtVariance.Text = ((Convert.ToDecimal(hfAmountAdvanced.Value) - Convert.ToDecimal(txtActualExpenditure.Text))).ToString();


        }
        protected void txtFActualExpenditure_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            TextBox txtFAmount = txt.FindControl("txtFAmount") as TextBox;
            TextBox txtFActualExpenditure = txt.FindControl("txtFActualExpenditure") as TextBox;
            TextBox txtFVariance = txt.FindControl("txtFVariance") as TextBox;
            txtFVariance.Text = ((Convert.ToDecimal(txtFAmount.Text) - Convert.ToDecimal(txtFActualExpenditure.Text))).ToString();

        }
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
            _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.RemoveELAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {



                    ELRAttachment attachment = new ELRAttachment();
                    attachment.FilePath = "~/ELUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/ELUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments;
                    grvAttachments.DataBind();


                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            catch (HttpException ex)
            {
                Master.ShowMessage(new AppMessage("Unable to upload the file,The file is to big or The internet is too slow " + ex.InnerException.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetLiquidationDetails();

            if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ELRAttachments.Count != 0)
            {
                //For update cases make the totals equal to zero first then add up the individuals
                _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure = 0;
                _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalTravelAdvance = 0;
                foreach (ExpenseLiquidationRequestDetail detail in _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails)
                {
                    _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure + detail.ActualExpenditure;
                    _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalTravelAdvance = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalTravelAdvance + detail.AmountAdvanced;
                    txtTotActual.Text = (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalActualExpenditure).ToString();
                    txtTotalAdvance.Text = (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TotalTravelAdvance).ToString();
                }

                int tarID = Convert.ToInt32(Session["tarId"]);
                _presenter.SaveOrUpdateExpenseLiquidationRequest(tarID);
                BindExpenseLiquidationRequests();
                PrintTransaction();
                Master.ShowMessage(new AppMessage("Expense Successfully Liquidated", Chai.WorkflowManagment.Enums.RMessageType.Info));
                Log.Info(_presenter.CurrentUser().FullName + " has requested an Expense Liquidation for a total amount of " + _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString());
                btnSave.Visible = false;
                btnPrint.Enabled = true;
                Session["tarId"] = null;
            }
            else { Master.ShowMessage(new AppMessage("Please attach Receipt", Chai.WorkflowManagment.Enums.RMessageType.Error)); }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindExpenseLiquidationRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmExpenseLiquidationRequest.aspx");
        }
        protected void ddlAccountDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            TextBox txtAccountCode = ddl.FindControl("txtAccountCode") as TextBox;
            txtAccountCode.Text = _presenter.GetItemAccount(Convert.ToInt32(ddl.SelectedValue)).AccountCode;
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlEdtGrant = ddl.FindControl("ddlGrant") as DropDownList;
            BindGrant(ddlEdtGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlFProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFGrant = ddl.FindControl("ddlFGrant") as DropDownList;
            BindGrant(ddlFGrant, Convert.ToInt32(ddl.SelectedValue));
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest != null)
            {
                lblRequestNoResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TravelAdvanceRequest.TravelAdvanceNo.ToString();
                lblRequestedDateResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.RequestDate.ToString();
                lblRequesterResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.TravelAdvanceRequest.AppUser.UserName;
                // lblExpenseTypeResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseType.ToString();
                lblCommentResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.Comment.ToString();
                lblApprovalStatusResult.Text = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ProgressStatus.ToString();

                grvDetails.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestDetails;
                grvDetails.DataBind();

                grvStatuses.DataSource = _presenter.CurrentTravelAdvanceRequest.ExpenseLiquidationRequest.ExpenseLiquidationRequestStatuses;
                grvStatuses.DataBind();
            }

        }
    }
}
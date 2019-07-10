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
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmTravelAdvanceRequest : POCBasePage, ITravelAdvanceRequestView
    {
        private TravelAdvanceRequestPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        TravelAdvanceRequestDetail tac;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                PopRequestPersonnels();
                PopProjects();
                BindTravelAdvanceDetails();
                BindTravelAdvanceRequests();
                //if (_presenter.CurrentTravelAdvanceRequest.Id <= 0)
                //{
                //    AutoNumber();
                //}
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentTravelAdvanceRequest != null)
            {
                if (_presenter.CurrentTravelAdvanceRequest.Id != 0)
                {
                    PrintTransaction();
                    btnPrint.Enabled = true;
                }
            }

        }
        [CreateNew]
        public TravelAdvanceRequestPresenter Presenter
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
                return "{3FB48884-3B42-47CB-B5DA-10D330CAED92}";
            }
        }

        #region Field Getters
        public int GetTARequestId
        {
            get
            {
                if (grvTravelAdvanceRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvTravelAdvanceRequestList.SelectedDataKey.Value);
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
        public DateTime GetRequestDate
        {
            get { return Convert.ToDateTime(txtRequestDate.Text); }
        }
        public string GetPurposeOfTravel
        {
            get { return txtPurposeOfTravel.Text; }
        }
        public string GetComments
        {
            get { return txtComments.Text; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        public string GetVisitingTeam
        {
            get { return txtVisitingTeam.Text; }
        }
        public int GetProjectId
        {
            get { return Convert.ToInt32(ddlProject.SelectedValue); }
        }
        public int GetGrantId
        {
            get { return Convert.ToInt32(ddlGrant.SelectedValue); }
        }
        #endregion
        private string AutoNumber()
        {
            return "TR-" + (_presenter.GetLastTravelAdvanceRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.TravelAdvance_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void PopRequestPersonnels()
        {
            txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;
        }
        private void PopItemAccounts(DropDownList ddlAccountDescription)
        {
            ddlAccountDescription.DataSource = _presenter.GetItemAccounts();
            ddlAccountDescription.DataBind();
            ddlAccountDescription.SelectedValue = _presenter.GetDefaultItemAccount().Id.ToString();

            //ddlAccountDescription.Items.Insert(0, new ListItem("---Select Account Description---", "0"));
            //ddlAccountDescription.SelectedIndex = 0;
        }
        private void PopExpenseTypes(DropDownList ddlExpenseType)
        {
            ddlExpenseType.DataSource = _presenter.GetExpenseTypes();
            ddlExpenseType.DataBind();


            //ddlAccountDescription.Items.Insert(0, new ListItem("---Select Account Description---", "0"));
            //ddlAccountDescription.SelectedIndex = 0;
        }
        private void PopProjects()
        {
            ddlProject.DataSource = _presenter.GetProjects();
            ddlProject.DataBind();

            ddlProject.Items.Insert(0, new ListItem("---Select Project---", "0"));
            ddlProject.SelectedIndex = 0;
        }
        private void PopGrants(int ProjectId)
        {
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataBind();

            ddlGrant.Items.Insert(0, new ListItem("---Select Grant---", "0"));
            ddlGrant.SelectedIndex = 0;
        }
        private void BindTravelAdvanceDetails()
        {
            dgTravelAdvanceRequestDetail.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
            dgTravelAdvanceRequestDetail.DataBind();
        }
        private void BindTravelAdvanceRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentTravelAdvanceRequest != null)
            {
                //txtTravelAdvanceNo.Text = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo.ToString();
                txtVisitingTeam.Text = _presenter.CurrentTravelAdvanceRequest.VisitingTeam;
                txtPurposeOfTravel.Text = _presenter.CurrentTravelAdvanceRequest.PurposeOfTravel.ToString();
                txtComments.Text = _presenter.CurrentTravelAdvanceRequest.Comments.ToString();
                ddlPayMethods.Text = _presenter.CurrentTravelAdvanceRequest.PaymentMethod;
                txtTotal.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
                ddlProject.SelectedValue = _presenter.CurrentTravelAdvanceRequest.Project.Id.ToString();
                PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
                ddlGrant.SelectedValue = _presenter.CurrentTravelAdvanceRequest.Grant.Id.ToString();
                BindTravelAdvanceDetails();
                BindTravelAdvanceRequests();

            }
        }
        private void BindCostsGrid(TravelAdvanceRequestDetail Tad)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            dgTravelAdvanceRequestCost.DataSource = tac.TravelAdvanceCosts;
            dgTravelAdvanceRequestCost.DataBind();
        }
        private void BindTravelAdvanceRequests()
        {
            grvTravelAdvanceRequestList.DataSource = _presenter.ListTravelAdvanceRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvTravelAdvanceRequestList.DataBind();
        }
        private void ClearFormFields()
        {
            txtVisitingTeam.Text = String.Empty;
            txtPurposeOfTravel.Text = String.Empty;
            txtComments.Text = String.Empty;
            txtTotal.Text = String.Empty;
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo.ToString();
            lblRequestedDateResult.Text = _presenter.CurrentTravelAdvanceRequest.RequestDate.ToString();
            lblRequesterResult.Text = _presenter.CurrentTravelAdvanceRequest.AppUser.UserName.ToString();
            lblVisitingTeamResult.Text = _presenter.CurrentTravelAdvanceRequest.VisitingTeam.ToString();
            lblPurposeOfTravelResult.Text = _presenter.CurrentTravelAdvanceRequest.PurposeOfTravel.ToString();
            lblCommentsResult.Text = _presenter.CurrentTravelAdvanceRequest.Comments.ToString();
            lblTotalTravelAdvanceResult.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentTravelAdvanceRequest.ProgressStatus.ToString();
            lblProjectIdResult.Text = _presenter.CurrentTravelAdvanceRequest.Project.ProjectCode;

            grvDetails.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses;
            grvStatuses.DataBind();


            foreach (TravelAdvanceRequestDetail detail in _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails)
            {
                grvCost.DataSource = detail.TravelAdvanceCosts;
                grvCost.DataBind();
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails.Count != 0)
                {
                    _presenter.SaveOrUpdateTARequest();
                    BindTravelAdvanceRequests();
                    Master.ShowMessage(new AppMessage("Successfully did a Travel Advance Request, Reference No - <b>'" + _presenter.CurrentTravelAdvanceRequest.TravelAdvanceNo + "'</b> ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested a Travel Advance of Total Amount " + _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString());
                    btnSave.Visible = false;
                    PrintTransaction();
                    btnPrint.Enabled = true;
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
                        AutoNumber();
                    }
                }
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteTravelAdvanceRequest(_presenter.CurrentTravelAdvanceRequest);
            ClearFormFields();
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            BindTravelAdvanceRequests();
            Master.ShowMessage(new AppMessage("Travel Advance Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindTravelAdvanceRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmTravelAdvanceRequest.aspx");
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
        }
        protected void grvTravelAdvanceRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvTravelAdvanceRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvTravelAdvanceRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["TravelAdvanceRequest"] = true;
            //ClearForm();
            BindTravelAdvanceRequestFields();
            btnDelete.Visible = true;
            btnPrint.Enabled = true;
            PrintTransaction();
            if (_presenter.CurrentTravelAdvanceRequest.CurrentStatus != null)
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
        protected void grvTARDetail_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }
        protected void grvTARDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            //hfDetailId.Value = grvTARDetail.SelectedRow.RowIndex.ToString();
            //BindCostsGrid(Convert.ToInt32(hfDetailId.Value));
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void dgTravelAdvanceRequestDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {

        }
        protected void dgTravelAdvanceRequestDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgTravelAdvanceRequestDetail.DataKeys[e.Item.ItemIndex];
            int TARDId = (int)dgTravelAdvanceRequestDetail.DataKeys[e.Item.ItemIndex];
            TravelAdvanceRequestDetail tard;

            if (TARDId > 0)
                tard = _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(TARDId);
            else
                tard = (TravelAdvanceRequestDetail)_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[e.Item.ItemIndex];
            try
            {
                if (TARDId > 0)
                {
                    _presenter.CurrentTravelAdvanceRequest.RemoveTravelAdvanceRequestDetail(id);
                    if (_presenter.GetTravelAdvanceRequestDetail(id) != null)
                        _presenter.DeleteTravelAdvanceRequestDetail(_presenter.GetTravelAdvanceRequestDetail(id));
                    _presenter.SaveOrUpdateTARequest(_presenter.CurrentTravelAdvanceRequest);
                }
                else { _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails.Remove(tard); }
                BindTravelAdvanceDetails();

                Master.ShowMessage(new AppMessage("Travel Advance Request Detail was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Travel Advance Request Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgTravelAdvanceRequestDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgTravelAdvanceRequestDetail.EditItemIndex = e.Item.ItemIndex;
            BindTravelAdvanceDetails();
        }
        protected void dgTravelAdvanceRequestDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TravelAdvanceRequestDetail tarDetail = new TravelAdvanceRequestDetail();
                    tarDetail.TravelAdvanceRequest = _presenter.CurrentTravelAdvanceRequest;
                    TextBox txtCityFrom = e.Item.FindControl("txtCityFrom") as TextBox;
                    tarDetail.CityFrom = txtCityFrom.Text;
                    TextBox txtCityTo = e.Item.FindControl("txtCityTo") as TextBox;
                    tarDetail.CityTo = txtCityTo.Text;
                    RadioButtonList rblHotelBooked = e.Item.FindControl("rblHotelBooked") as RadioButtonList;
                    tarDetail.HotelBooked = rblHotelBooked.SelectedValue;
                    TextBox txtFromDate = e.Item.FindControl("txtFromDate") as TextBox;
                    tarDetail.FromDate = Convert.ToDateTime(txtFromDate.Text);
                    TextBox txtToDate = e.Item.FindControl("txtToDate") as TextBox;
                    tarDetail.ToDate = Convert.ToDateTime(txtToDate.Text);
                    DropDownList ddlModeOfTravel = e.Item.FindControl("ddlModeOfTravel") as DropDownList;
                    tarDetail.ModeOfTravel = ddlModeOfTravel.SelectedValue;
                    TextBox txtAirFare = e.Item.FindControl("txtAirFare") as TextBox;
                    if (!String.IsNullOrEmpty(txtAirFare.Text))
                        tarDetail.AirFare = Convert.ToDecimal(txtAirFare.Text);

                    _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails.Add(tarDetail);
                    dgTravelAdvanceRequestDetail.EditItemIndex = -1;
                    BindTravelAdvanceDetails();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Travel Advance Detail " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }

        }
        protected void dgTravelAdvanceRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
            }
            else
            {
                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails != null)
                {
                    DropDownList ddlEdtModeofTravel = e.Item.FindControl("ddlEdtModeofTravel") as DropDownList;
                    if (ddlEdtModeofTravel != null)
                    {
                        if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[e.Item.DataSetIndex].ModeOfTravel != null)
                        {
                            ListItem liI = ddlEdtModeofTravel.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[e.Item.DataSetIndex].ModeOfTravel.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                }
            }
        }
        protected void dgTravelAdvanceRequestDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgTravelAdvanceRequestDetail.DataKeys[e.Item.ItemIndex];
            TravelAdvanceRequestDetail tarDetail;

            if (id > 0)
                tarDetail = _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(id);
            else
                tarDetail = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[e.Item.ItemIndex];

            try
            {
                tarDetail.TravelAdvanceRequest = _presenter.CurrentTravelAdvanceRequest;
                TextBox txtCityFrom = e.Item.FindControl("txtEdtCityFrom") as TextBox;
                tarDetail.CityFrom = txtCityFrom.Text;
                TextBox txtCityTo = e.Item.FindControl("txtEdtCityTo") as TextBox;
                tarDetail.CityTo = txtCityTo.Text;
                RadioButtonList rblHotelBooked = e.Item.FindControl("rblEdtHotelBooked") as RadioButtonList;
                tarDetail.HotelBooked = rblHotelBooked.SelectedValue;
                TextBox txtFromDate = e.Item.FindControl("txtEdtFromDate") as TextBox;
                tarDetail.FromDate = Convert.ToDateTime(txtFromDate.Text);
                TextBox txtToDate = e.Item.FindControl("txtEdtToDate") as TextBox;
                tarDetail.ToDate = Convert.ToDateTime(txtToDate.Text);
                DropDownList ddlEdtModeOfTravel = e.Item.FindControl("ddlEdtModeOfTravel") as DropDownList;
                tarDetail.ModeOfTravel = ddlEdtModeOfTravel.SelectedValue;
                TextBox txtAirFare = e.Item.FindControl("txtEdtAirFare") as TextBox;
                if (!String.IsNullOrEmpty(txtAirFare.Text))
                    tarDetail.AirFare = Convert.ToDecimal(txtAirFare.Text);

                dgTravelAdvanceRequestDetail.EditItemIndex = -1;
                BindTravelAdvanceDetails();
                Master.ShowMessage(new AppMessage("Travel Advance Detail Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Travel Advance Detail " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgTravelAdvanceRequestDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            int TACId = (int)dgTravelAdvanceRequestDetail.DataKeys[dgTravelAdvanceRequestDetail.SelectedItem.ItemIndex];
            int Id = dgTravelAdvanceRequestDetail.SelectedItem.ItemIndex;


            if (TACId > 0)
                Session["tac"] = _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(TACId);
            else
                Session["tac"] = _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[dgTravelAdvanceRequestDetail.SelectedItem.ItemIndex];


            int recordId = (int)dgTravelAdvanceRequestDetail.SelectedIndex;
            if (_presenter.CurrentTravelAdvanceRequest.Id > 0)
            {
                hfDetailId.Value = TACId.ToString();
            }
            else
            {
                hfDetailId.Value = dgTravelAdvanceRequestDetail.SelectedItem.ItemIndex.ToString();
            }
            BindCostsGrid(tac);
            pnlTACost_ModalPopupExtender.Show();
        }
        protected void dgTravelAdvanceRequestCost_CancelCommand(object source, DataGridCommandEventArgs e)
        {

        }
        protected void dgTravelAdvanceRequestCost_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            int id = (int)dgTravelAdvanceRequestCost.DataKeys[e.Item.ItemIndex];
            int TACId = (int)dgTravelAdvanceRequestCost.DataKeys[e.Item.ItemIndex];
            TravelAdvanceCost taco;

            if (TACId > 0)
                taco = _presenter.GetTravelAdvanceCost(TACId);
            else
                taco = (TravelAdvanceCost)tac.TravelAdvanceCosts[e.Item.ItemIndex];

            try
            {
                if (TACId > 0)
                {
                    _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).RemoveTravelAdvanceCost(id);
                    if (_presenter.GetTravelAdvanceCost(id) != null)
                        _presenter.DeleteTravelAdvanceCost(_presenter.GetTravelAdvanceCost(id));
                    _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance - taco.Total;
                    txtTotal.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
                    _presenter.SaveOrUpdateTARequest(_presenter.CurrentTravelAdvanceRequest);
                }
                else {

                    _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts.Remove(taco);
                    _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance - taco.Total;
                    txtTotal.Text = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance.ToString();
                }
                BindCostsGrid(taco.TravelAdvanceRequestDetail);
                pnlTACost_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Travel Advance Cost was removed successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Travel Advance Cost. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgTravelAdvanceRequestCost_EditCommand(object source, DataGridCommandEventArgs e)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            this.dgTravelAdvanceRequestCost.EditItemIndex = e.Item.ItemIndex;
            int TACId = (int)dgTravelAdvanceRequestCost.DataKeys[e.Item.ItemIndex];
            TravelAdvanceCost taco;

            if (TACId > 0)
                taco = _presenter.GetTravelAdvanceCost(TACId);
            else
                taco = (TravelAdvanceCost)tac.TravelAdvanceCosts[e.Item.ItemIndex];
            BindCostsGrid(taco.TravelAdvanceRequestDetail);
            pnlTACost_ModalPopupExtender.Show();
        }
        protected void dgTravelAdvanceRequestCost_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TravelAdvanceCost taCost = new TravelAdvanceCost();
                    taCost.TravelAdvanceRequestDetail = tac;
                    DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                    ItemAccount itemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlAccountDescription.SelectedValue));
                    taCost.ItemAccount = itemAccount;
                    taCost.AccountCode = itemAccount.AccountCode;
                    DropDownList ddlExpenseType = e.Item.FindControl("ddlExpenseType") as DropDownList;
                    ExpenseType expensetype = _presenter.GetExpenseType(int.Parse(ddlExpenseType.SelectedValue));
                    taCost.ExpenseType = expensetype;
                    TextBox txtDays = e.Item.FindControl("txtDays") as TextBox;
                    taCost.Days = Convert.ToInt32(txtDays.Text);
                    TextBox txtUnitCost = e.Item.FindControl("txtUnitCost") as TextBox;
                    taCost.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                    TextBox txtNoOfUnits = e.Item.FindControl("txtNoOfUnits") as TextBox;
                    taCost.NoOfUnits = Convert.ToInt32(txtNoOfUnits.Text);
                    //TextBox txtTotal = e.Item.FindControl("txtTotal") as TextBox;
                    taCost.Total = Convert.ToInt32(txtDays.Text) * Convert.ToDecimal(txtUnitCost.Text) * Convert.ToInt32(txtNoOfUnits.Text);
                    _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance = _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance + taCost.Total;
                    txtTotal.Text = (_presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance).ToString();
                    if (_presenter.CurrentTravelAdvanceRequest.Id > 0)
                        _presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts.Add(taCost);
                    else
                        _presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[Convert.ToInt32(hfDetailId.Value)].TravelAdvanceCosts.Add(taCost);

                    dgTravelAdvanceRequestCost.EditItemIndex = -1;
                    BindCostsGrid(taCost.TravelAdvanceRequestDetail);
                    pnlTACost_ModalPopupExtender.Show();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Save Travel Advance Cost " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgTravelAdvanceRequestCost_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlAccountDescription = e.Item.FindControl("ddlAccountDescription") as DropDownList;
                PopItemAccounts(ddlAccountDescription);
                DropDownList ddlExpenseType = e.Item.FindControl("ddlExpenseType") as DropDownList;
                PopExpenseTypes(ddlExpenseType);
            }
            else
            {
                if (tac.TravelAdvanceCosts != null)
                {
                    DropDownList ddlEdtAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                    if (ddlEdtAccountDescription != null)
                    {
                        PopItemAccounts(ddlEdtAccountDescription);
                        //if (_presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts[e.Item.DataSetIndex].ItemAccount.Id != 0)
                        //{
                        //    ListItem liI = ddlEdtAccountDescription.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                        //    if (liI != null)
                        //        liI.Selected = true;
                        //}
                    }
                    DropDownList ddlEdtExpenseType = e.Item.FindControl("ddlEdtExpenseType") as DropDownList;
                    if (ddlEdtExpenseType != null)
                    {
                        PopExpenseTypes(ddlEdtExpenseType);


                        if (_presenter.CurrentTravelAdvanceRequest.Id > 0)
                        {
                            if (_presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts[e.Item.DataSetIndex] != null)
                            {
                                if (_presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts[e.Item.DataSetIndex].ExpenseType.Id != 0)
                                {
                                    ListItem liI = ddlEdtExpenseType.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.GetTravelAdvanceRequestDetail(Convert.ToInt32(hfDetailId.Value)).TravelAdvanceCosts[e.Item.DataSetIndex].ExpenseType.Id.ToString());
                                    if (liI != null)
                                        liI.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[Convert.ToInt32(hfDetailId.Value)].TravelAdvanceCosts[e.Item.DataSetIndex] != null)
                            {
                                if (_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[Convert.ToInt32(hfDetailId.Value)].TravelAdvanceCosts[e.Item.DataSetIndex].ExpenseType.Id != 0)
                                {
                                    ListItem liI = ddlEdtExpenseType.Items.FindByValue(_presenter.CurrentTravelAdvanceRequest.TravelAdvanceRequestDetails[Convert.ToInt32(hfDetailId.Value)].TravelAdvanceCosts[e.Item.DataSetIndex].ExpenseType.Id.ToString());
                                    if (liI != null)
                                        liI.Selected = true;
                                }
                            }
                        }




                       
                    }

                }
            }
        }
        protected void dgTravelAdvanceRequestCost_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            tac = Session["tac"] as TravelAdvanceRequestDetail;
            int id = (int)dgTravelAdvanceRequestCost.DataKeys[e.Item.ItemIndex];
            TravelAdvanceCost taCost;
            decimal pretacost = 0;
            if (id > 0)
                taCost = tac.GetTravelAdvanceCost(id);
            else

                taCost = tac.TravelAdvanceCosts[e.Item.ItemIndex];

            try
            {

                taCost.TravelAdvanceRequestDetail = taCost.TravelAdvanceRequestDetail;
                DropDownList ddlEdtAccountDescription = e.Item.FindControl("ddlEdtAccountDescription") as DropDownList;
                ItemAccount itemEdtAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlEdtAccountDescription.SelectedValue));
                taCost.ItemAccount = itemEdtAccount;
                taCost.AccountCode = itemEdtAccount.AccountCode;
                DropDownList ddlExpenseType = e.Item.FindControl("ddlEdtExpenseType") as DropDownList;
                ExpenseType expensetype = _presenter.GetExpenseType(int.Parse(ddlExpenseType.SelectedValue));
                taCost.ExpenseType = expensetype;
                TextBox txtEdtDays = e.Item.FindControl("txtEdtDays") as TextBox;
                taCost.Days = Convert.ToInt32(txtEdtDays.Text);
                TextBox txtEdtUnitCost = e.Item.FindControl("txtEdtUnitCost") as TextBox;
                taCost.UnitCost = Convert.ToDecimal(txtEdtUnitCost.Text);
                TextBox txtEdtNoOfUnits = e.Item.FindControl("txtEdtNoOfUnits") as TextBox;
                taCost.NoOfUnits = Convert.ToInt32(txtEdtNoOfUnits.Text);
                //TextBox txtEdtTotal = e.Item.FindControl("txtEdtTotal") as TextBox;
                pretacost = taCost.Total;
                taCost.Total = Convert.ToInt32(txtEdtDays.Text) * Convert.ToDecimal(txtEdtUnitCost.Text) * Convert.ToInt32(txtEdtNoOfUnits.Text);
                _presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance = (_presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance + taCost.Total) - pretacost;
                txtTotal.Text = (_presenter.CurrentTravelAdvanceRequest.TotalTravelAdvance).ToString();
                dgTravelAdvanceRequestCost.EditItemIndex = -1;
                BindCostsGrid(taCost.TravelAdvanceRequestDetail);
                pnlTACost_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Travel Advance Cost Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Travel Advance Cost " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
    }
}
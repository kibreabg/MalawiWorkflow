using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Chai.WorkflowManagment.Modules.Shell.Views;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Modules.Shell.MasterPages;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;

public partial class ShellDefault : Microsoft.Practices.CompositeWeb.Web.UI.Page, IBaseMasterView
{
    private BaseMasterPresenter _presenter;
    private AppUser currentUser;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._presenter.OnViewInitialized();
            BindCashPaymentRequests();
            BindLeaveRequests();
            BindVehicleRequests();
            BindCostSharingRequests();
            BindTravelAdvanceRequests();
            BindPurchaseRequests();
            BindBankPaymentRequests();
            BindBidAnalysisRequests();
            BindSoleVendorRequests();
        }
        this._presenter.OnViewLoaded();
        MyTasks();
        MyRequests();
        if (_presenter.CurrentUser.EmployeePosition.PositionName == "Admin/HR Officer" || _presenter.CurrentUser.EmployeePosition.PositionName == "Operational Manager" || _presenter.CurrentUser.EmployeePosition.PositionName == "Country Director" || _presenter.CurrentUser.EmployeePosition.PositionName == "Finance Officer" || _presenter.CurrentUser.EmployeePosition.PositionName == "Finance Manager")
        {
            reimbersmentstatuses.Visible = true;
            ReimbersmentStatus();
        }
    }

    [CreateNew]
    public BaseMasterPresenter Presenter
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

    public string TabId
    {
        get { return Request.QueryString[AppConstants.TABID]; }
    }
    public string NodeId
    {
        get { return Request.QueryString[AppConstants.NODEID]; }
    }
    public Chai.WorkflowManagment.CoreDomain.Users.AppUser CurrentUser
    {
        get
        {
            return currentUser;
        }
        set
        {
            currentUser = value;
        }
    }
    private void MyTasks()
    {
        if (_presenter.GetLeaveTasks() != 0)
        {
            lblLeaverequests.Text = _presenter.GetLeaveTasks().ToString();
            lnkLeaveRequest.Enabled = true;
            lnkLeaveRequest.PostBackUrl = ResolveUrl("Approval/frmLeaveApproval.aspx");
        }
        else
        {
            lblLeaverequests.Text = Convert.ToString(0);
        }
        if (_presenter.GetVehicleTasks() != 0)
        {
            lblVehicleRequest.Text = _presenter.GetVehicleTasks().ToString();
            lnkVehicleRequest.Enabled = true;
            lnkVehicleRequest.PostBackUrl = ResolveUrl("Approval/frmVehicleApproval.aspx");
        }
        else
        {
            lblVehicleRequest.Text = Convert.ToString(0);
        }
        if (_presenter.GetCashPaymentRequestTasks() != 0)
        {
            lblPaymentRequest.Text = _presenter.GetCashPaymentRequestTasks().ToString();
            lnkPaymentRequest.Enabled = true;
            lnkPaymentRequest.PostBackUrl = lnkPaymentRequest.ResolveUrl("Approval/frmCashPaymentApproval.aspx");
        }
        else { lblPaymentRequest.Text = Convert.ToString(0); }
        if (_presenter.GetCostSharingRequestTasks() != 0)
        {
            lblCostSharingRequest.Text = _presenter.GetCostSharingRequestTasks().ToString();
            lnkCostSharingRequest.Enabled = true;
            lnkCostSharingRequest.PostBackUrl = lnkCostSharingRequest.ResolveUrl("Approval/frmCostSharingApproval.aspx");
        }
        else { lblCostSharingRequest.Text = Convert.ToString(0); }
        if (_presenter.GetPurchaseRequestsTasks() != 0)
        {
            lblpurchaserequest.Text = _presenter.GetPurchaseRequestsTasks().ToString();
            lnkPurchaseRequest.Enabled = true;
            lnkPurchaseRequest.PostBackUrl = ResolveUrl("Approval/frmPurchaseApprovalDetail.aspx");
        }
        else { lblpurchaserequest.Text = Convert.ToString(0); }

        if (_presenter.GetTravelAdvanceRequestTasks() != 0)
        {
            lblTravelAdvanceRequest.Text = _presenter.GetTravelAdvanceRequestTasks().ToString();
            lnkTravelAdvanceRequest.Enabled = true;
            lnkTravelAdvanceRequest.PostBackUrl = ResolveUrl("Approval/frmTravelAdvanceApproval.aspx");
        }
        else { lblTravelAdvanceRequest.Text = Convert.ToString(0); }

        if (_presenter.GetReviewExpenseLiquidationRequestsTasks() != 0)
        {
            lblreviewliquidation.Text = _presenter.GetReviewExpenseLiquidationRequestsTasks().ToString();
            lnkreviewliquidation.Enabled = true;
            lnkreviewliquidation.PostBackUrl = ResolveUrl("Approval/frmExpenseLiquidationApproval.aspx");
        }
        else
        { lblreviewliquidation.Text = Convert.ToString(0); }
        if (_presenter.GetExpenseLiquidationRequestsTasks() != 0)
        {
            lblExpenseLiquidation.Text = _presenter.GetExpenseLiquidationRequestsTasks().ToString();
            lnkExpenseLiquidation.Enabled = true;
            lnkExpenseLiquidation.PostBackUrl = ResolveUrl("Request/frmExpenseLiquidationRequest.aspx");

        }
        else
        {
            lblExpenseLiquidation.Text = Convert.ToString(0);
        }
        if (_presenter.GetBankPaymentRequestsTasks() != 0)
        {
            lblbankpayment.Text = _presenter.GetBankPaymentRequestsTasks().ToString();
            lnkbankpayment.Enabled = true;
            lnkbankpayment.PostBackUrl = ResolveUrl("Approval/frmOperationalControlApproval.aspx");

        }
        else
        {
            lblbankpayment.Text = Convert.ToString(0);
        }

        if (_presenter.GetBidAnalysisRequestsTasks() != 0)
        {
            lblBidAnalysis.Text = _presenter.GetBidAnalysisRequestsTasks().ToString();
            lnkBidAnalysis.Enabled = true;
            lnkBidAnalysis.PostBackUrl = ResolveUrl("Approval/frmBidAnalysisApproval.aspx");

        }
        else
        {
            lblBidAnalysis.Text = Convert.ToString(0);
        }
        if (_presenter.GetSoleVendorRequestsTasks() != 0)
        {
            lblSolVendor.Text = _presenter.GetSoleVendorRequestsTasks().ToString();
            lnkSoleVendor.Enabled = true;
            lnkSoleVendor.PostBackUrl = ResolveUrl("Approval/frmSoleVendorApproval.aspx");

        }
        else
        {
            lblSolVendor.Text = Convert.ToString(0);
        }
    }
    private void MyRequests()
    {
        if (_presenter.GetLeaveMyRequest() != 0)
        {
            lblLeaveStatus.Text = ProgressStatus.InProgress.ToString();
            lblLeaveStatus.ForeColor = System.Drawing.Color.Green;
        }

        if (_presenter.GetVehicleMyRequest() != 0)
        {
            lblVehicleStatus.Text = ProgressStatus.InProgress.ToString();
            lblVehicleStatus.ForeColor = System.Drawing.Color.Green;

        }

        if (_presenter.GetCashPaymentRequestMyRequests() != 0)
        {
            lblPaymentStatus.Text = ProgressStatus.InProgress.ToString();
            lblPaymentStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetCostSharingRequestMyRequests() != 0)
        {
            lblCostSharingStatus.Text = ProgressStatus.InProgress.ToString();
            lblCostSharingStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetTravelAdvanceRequestMyRequest() != 0)
        {
            lblTravelStatus.Text = ProgressStatus.InProgress.ToString();
            lblTravelStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetPurchaseRequestsMyRequest() != 0)
        {
            lblPurchaseStatus.Text = ProgressStatus.InProgress.ToString();
            lblPurchaseStatus.ForeColor = System.Drawing.Color.Green;

        }

        if (_presenter.GetBankRequestsMyRequest() != 0)
        {
            lblBankRequestStatus.Text = ProgressStatus.InProgress.ToString();
            lblBankRequestStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetBidAnalysisRequestsMyRequest() != 0)
        {
            lblBidAnalysisStatus.Text = ProgressStatus.InProgress.ToString();
            lblBidAnalysisStatus.ForeColor = System.Drawing.Color.Green;

        }
        if (_presenter.GetSoleVendorRequestsMyRequest() != 0)
        {
            lblSoleVendorStatus.Text = ProgressStatus.InProgress.ToString();
            lblSoleVendorStatus.ForeColor = System.Drawing.Color.Green;

        }

    }
    private void ReimbersmentStatus()
    {
        if (_presenter.GetCashPaymentReimbersment() != 0)
        {
            lblCashPaymentreimbersment.Text = _presenter.GetCashPaymentReimbersment().ToString();

        }
        else
        {
            lblCashPaymentreimbersment.Text = Convert.ToString(0);
        }

        if (_presenter.GetCostSharingPaymentReimbersment() != 0)
        {
            lblCostPaymentreimbersment.Text = _presenter.GetCostSharingPaymentReimbersment().ToString();

        }
        else
        {
            lblCostPaymentreimbersment.Text = Convert.ToString(0);
        }
    }
    private void BindLeaveRequests()
    {
        grvLeaveProgress.DataSource = _presenter.ListLeaveApprovalProgress();
        grvLeaveProgress.DataBind();
    }
    private void BindVehicleRequests()
    {
        grvVehicleProgress.DataSource = _presenter.ListVehicleApprovalProgress();
        grvVehicleProgress.DataBind();
    }
    private void BindCashPaymentRequests()
    {
        grvPaymentProgress.DataSource = _presenter.ListPaymentApprovalProgress();
        grvPaymentProgress.DataBind();
    }
    private void BindCostSharingRequests()
    {
        grvCostProgress.DataSource = _presenter.ListCostApprovalProgress();
        grvCostProgress.DataBind();
    }
    private void BindTravelAdvanceRequests()
    {
        grvTravelProgress.DataSource = _presenter.ListTravelApprovalProgress();
        grvTravelProgress.DataBind();
    }
    private void BindPurchaseRequests()
    {
        grvPurchaseProgress.DataSource = _presenter.ListPurchaseApprovalProgress();
        grvPurchaseProgress.DataBind();
    }
    private void BindBankPaymentRequests()
    {
        grvBankProgress.DataSource = _presenter.ListBankPaymentApprovalProgress();
        grvBankProgress.DataBind();
    }

    private void BindBidAnalysisRequests()
    {
        grvBidAnalysisProgress.DataSource = _presenter.ListBidAnalysisApprovalProgress();
        grvBidAnalysisProgress.DataBind();
    }

    private void BindSoleVendorRequests()
    {
        grvSoleVendorProgress.DataSource = _presenter.ListSoleVendorApprovalProgress();
        grvSoleVendorProgress.DataBind();
    }

    protected void grvLeaveProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListLeaveApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListLeaveApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListLeaveApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvVehicleProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListVehicleApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListVehicleApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListVehicleApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvPaymentProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListPaymentApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListPaymentApprovalProgress().Count != 0)
                {
                    if (_presenter.ListPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                        e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
                    else
                        e.Row.Cells[2].Text = "Finance Officer";
                }
            }
        }        
    }
    protected void grvCostProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListCostApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListCostApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListCostApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvTravelProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListTravelApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListTravelApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListTravelApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvPurchaseProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListPurchaseApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListPurchaseApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListPurchaseApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvBankProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListBankPaymentApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListBankPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListBankPaymentApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvBidAnalysisProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListBidAnalysisApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListBidAnalysisApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListBidAnalysisApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
    protected void grvSoleVendorProgress_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (_presenter.ListSoleVendorApprovalProgress() != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.ListSoleVendorApprovalProgress()[e.Row.RowIndex].CurrentApprover != 0)
                    e.Row.Cells[2].Text = _presenter.GetUser(_presenter.ListSoleVendorApprovalProgress()[e.Row.RowIndex].CurrentApprover).FullName;
            }
        }
    }
}

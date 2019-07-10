using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Requests;
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

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmPurchaseOrderSoleVendor : POCBasePage, IPurchaseOrderSoleVendorView
    {
        private PurchaseOrderSoleVendorPresenter _presenter;

        private SoleVendorRequest _solevendorrequest;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors == null)
                {
                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors = new PurchaseOrderSoleVendor();
                }
                BindPurchaseOrder();
                btnPrintPurchaseForm.Enabled = true;
                btnPrintPurchaseOrder.Enabled = true;
                //BindRepeater();  
            }
            this._presenter.OnViewLoaded();

            if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors != null)
            {
                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Id != 0)
                {
                    PrintTransaction();
                    BindRepeater();
                }
            }
        }

        [CreateNew]
        public PurchaseOrderSoleVendorPresenter Presenter
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
                return "{64D3AC5F-DD78-414C-98F8-63EC02CB9673}";
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
            get { return string.Empty; }
        }
        public string RequestDate
        {
            get { return string.Empty; }
        }

        public int SoleVendorRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["SoleVendorRequestId"]);
                }
                return 0;
            }
        }
        public string RequestType
        {
            get
            {
                if (Request.QueryString["RequestType"].ToString() != string.Empty)
                {
                    return Request.QueryString["RequestType"].ToString();
                }
                return string.Empty;
            }
        }
        private void BindRepeater()
        {
            Repeater1.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails;
            Repeater1.DataBind();

            Label lblPONumberP = Repeater1.Controls[0].Controls[0].FindControl("lblPONumberP") as Label;
            lblPONumberP.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber;
            Label lblRequesterP = Repeater1.Controls[0].Controls[0].FindControl("lblRequesterP") as Label;
            lblRequesterP.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;
            Label lblDateP = Repeater1.Controls[0].Controls[0].FindControl("lblDateP") as Label;
            lblDateP.Text = _presenter.CurrentSoleVendorRequest.RequestDate.ToString();
            if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Supplier != null)
            {
                Label lblSupplierName = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierName") as Label;
                lblSupplierName.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Supplier.SupplierName;
                Label lblSupplierAddress = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierAddress") as Label;
                lblSupplierAddress.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Supplier.SupplierAddress;
                Label lblSupplierContactP = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierContactP") as Label;
                lblSupplierContactP.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Supplier.SupplierContact;
            }
            Label lblBillToP = Repeater1.Controls[0].Controls[0].FindControl("lblBillToP") as Label;
            lblBillToP.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto;
            Label lblShipTo = Repeater1.Controls[0].Controls[0].FindControl("lblShipTo") as Label;
            lblShipTo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo;
            Label lblPaymentTermsP = Repeater1.Controls[0].Controls[0].FindControl("lblPaymentTermsP") as Label;
            lblPaymentTermsP.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms;
            Label lblDeliveryFeesP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblDeliveryFeesP") as Label;
            lblDeliveryFeesP.Text = Convert.ToString(_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees);
            Label lblItemTotalP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblItemTotalP") as Label;
            Label lblVatP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblVatP") as Label;
            Label lblTotalP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblTotalP") as Label;
            foreach (PurchaseOrderSoleVendorDetail POD in _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails)
            {
                lblItemTotalP.Text = ((!String.IsNullOrEmpty(lblItemTotalP.Text) ? Convert.ToDecimal(lblItemTotalP.Text) : 0) + POD.TotalCost).ToString();
            }
            lblTotalP.Text = Convert.ToString((!String.IsNullOrEmpty(lblItemTotalP.Text) ? Convert.ToDecimal(lblItemTotalP.Text) : 0) + (!String.IsNullOrEmpty(lblVatP.Text) ? Convert.ToDecimal(lblVatP.Text) : 0) + (!String.IsNullOrEmpty(lblDeliveryFeesP.Text) ? Convert.ToDecimal(lblDeliveryFeesP.Text) : 0));

        }

        private void AutoNumber()
        {
            txtPONo.Text = "POSV-" + (_presenter.GetLastPurchaseOrderSoleVendorId() + 1);
        }
        private void BindPurchaseOrder()
        {
            this._presenter.OnViewLoaded();
            txtDate.Text = DateTime.Today.ToString();
            txtRequester.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;
            txtSupplierName.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierName;
            txtSupplierAddress.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierAddress;
            txtSupplierContact.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierContact;
            if(_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails.Count == 0)
            {
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails = new List<PurchaseOrderSoleVendorDetail>();
                AddPurchasingItembySoleVendor();
            }
            else
            {
                BindPODetailForSole();
            }
            

            if (_presenter.CurrentSoleVendorRequest != null)
            {
                if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors != null)
                {
                    if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Id > 0)
                    {
                        txtPONo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber;
                        txtDate.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PODate.ToString();
                        txtShipTo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo;
                        txtBillto.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto;
                        txtDeliveeryFees.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees.ToString();
                        txtPaymentTerms.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms;
                        btnPrintPurchaseOrder.Enabled = true;
                        btnPrintPurchaseForm.Enabled = true;
                    }
                    else
                    {
                        
                        txtDate.Text = DateTime.Today.ToString();
                        AutoNumber();
                    }
                }         
            }
        }
        private void SavePurchaseOrder()
        {
            try
            {
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber = txtPONo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PODate = Convert.ToDateTime(txtDate.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto = txtBillto.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo = txtShipTo.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees = Convert.ToDecimal(txtDeliveeryFees.Text);
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms = txtPaymentTerms.Text;
                _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.SoleVendorRequest = _presenter.CurrentSoleVendorRequest;
                if (_presenter.CurrentSoleVendorRequest != null)
                {
                    _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Supplier = _presenter.CurrentSoleVendorRequest.Supplier;
                }
                //_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Status = "Completed";       
                Master.ShowMessage(new AppMessage("Purchase Order Successfully Approved", Chai.WorkflowManagment.Enums.RMessageType.Info));

            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("There was an error Saving Purchase Order", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
        }

        private void BindPODetailForSole()
        {
            dgPODetail.DataSource = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails;
            dgPODetail.DataBind();
        }

        #region PurchaseOrderDetail
        private void AddPurchasingItembySoleVendor()
        {
            if (_presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Id <= 0)
            {
                if (_presenter.CurrentSoleVendorRequest != null)
                {
                    foreach (SoleVendorRequestDetail detail in _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails)
                    {
                        PurchaseOrderSoleVendorDetail POD = new PurchaseOrderSoleVendorDetail();
                        POD.ItemAccount = _presenter.GetItemAccount(detail.ItemAccount.Id);
                        POD.Qty = detail.Qty;
                        POD.UnitCost = detail.UnitCost;
                        POD.TotalCost = detail.TotalCost;
                        _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PurchaseOrderSoleVendorDetails.Add(POD);
                    }
                }
            }
            BindPODetailForSole();
        }
        #endregion
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                SavePurchaseOrder();
                _presenter.SaveOrUpdateSoleVendorRequest(_presenter.CurrentSoleVendorRequest);
                BindRepeater();
                PrintTransaction();
                btnPrintPurchaseOrder.Enabled = true;
                btnPrintPurchaseForm.Enabled = true;
                btnRequest.Enabled = false;
                // Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentBidAnalysisRequest.Id, "Enabled"));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Unable to save Purchase order", Chai.WorkflowManagment.Enums.RMessageType.Error));
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentSoleVendorRequest.Id, "Enabled"));
        }
        private void PrintTransaction()
        {
            lblRequestNoResult.Text = _presenter.CurrentSoleVendorRequest.RequestNo;
            lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;
            lblRequestedDateResult.Text = _presenter.CurrentSoleVendorRequest.RequestDate.ToString();
            //    lblDeliverToResult.Text = _presenter.CurrentBidAnalysisRequest..DeliverTo;
            lblPurchaseOrderNo.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PoNumber;
            //  lblDeliveryDateresult.Text = _presenter.CurrentBidAnalysisRequest..Requireddateofdelivery.ToString();
            lblPurposeResult.Text = _presenter.CurrentSoleVendorRequest.ContactPersonNumber;
            lblBillToResult.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.Billto;
            lblShipToResult.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.ShipTo;
            lblPaymentTerms.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.PaymentTerms;
            lblDeliveryFeesResult.Text = _presenter.CurrentSoleVendorRequest.PurchaseOrderSoleVendors.DeliveryFees.ToString();

            lblSuggestedSupplierResult.Text = _presenter.CurrentSoleVendorRequest.Supplier.SupplierName;
            if (_presenter.CurrentSoleVendorRequest != null)
            {
                lblReasonforSelectionResult.Text = _presenter.CurrentSoleVendorRequest.SoleVendorJustificationType;
                lblSelectedbyResult.Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.AppUser.Id).FullName;

                grvDetails.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestDetails;
                grvDetails.DataBind();
            }
            grvStatuses.DataSource = _presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentSoleVendorRequest.SoleVendorRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
    }
}
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
    public partial class frmPurchaseOrder : POCBasePage, IPurchaseOrderView
    {
        private PurchaseOrderPresenter _presenter;
        private BidAnalysisRequest _bidanalysisrequest;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
               
                    if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders == null)
                    {
                        _presenter.CurrentBidAnalysisRequest.PurchaseOrders = new PurchaseOrder();

                    }
                
                BindPurchaseOrder();
                btnPrintPurchaseForm.Enabled = true;
                btnPrintPurchaseOrder.Enabled = true;
                //BindRepeater();  
            }
            this._presenter.OnViewLoaded();
           
                if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders != null)
                {
                    if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Id != 0)
                    {
                        PrintTransaction();
                        BindRepeater();
                    }
               
           

            }
            //btnPrintworksheet.Attributes.Add("onclick", "javascript:Clickheretoprint('divprint'); return false;");
            //BindJS();
        }

        [CreateNew]
        public PurchaseOrderPresenter Presenter
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
        public CoreDomain.Requests.BidAnalysisRequest BidAnalysisRequest
        {
            get
            {
                return _bidanalysisrequest;
            }
            set
            {
                _bidanalysisrequest = value;
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
        public int BidAnalysisRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]);
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
           

                Repeater1.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails;
                Repeater1.DataBind();

                Label lblPONumberP = Repeater1.Controls[0].Controls[0].FindControl("lblPONumberP") as Label;
                lblPONumberP.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber;
                Label lblRequesterP = Repeater1.Controls[0].Controls[0].FindControl("lblRequesterP") as Label;
                lblRequesterP.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                Label lblDateP = Repeater1.Controls[0].Controls[0].FindControl("lblDateP") as Label;
                lblDateP.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
                if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier != null)
                {
                    Label lblSupplierName = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierName") as Label;
                    lblSupplierName.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier.SupplierName;
                    Label lblSupplierAddress = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierAddress") as Label;
                    lblSupplierAddress.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier.SupplierAddress;
                    Label lblSupplierContactP = Repeater1.Controls[0].Controls[0].FindControl("lblSupplierContactP") as Label;
                    lblSupplierContactP.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier.SupplierContact;
                }
                Label lblBillToP = Repeater1.Controls[0].Controls[0].FindControl("lblBillToP") as Label;
                lblBillToP.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto;
                Label lblShipTo = Repeater1.Controls[0].Controls[0].FindControl("lblShipTo") as Label;
                lblShipTo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo;
                Label lblPaymentTermsP = Repeater1.Controls[0].Controls[0].FindControl("lblPaymentTermsP") as Label;
                lblPaymentTermsP.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms;
                Label lblDeliveryFeesP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblDeliveryFeesP") as Label;
                lblDeliveryFeesP.Text = Convert.ToString(_presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees);
                Label lblItemTotalP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblTotalP") as Label;
                Label lblVatP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblVatP") as Label;
               
                Label lblTotalP = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblTotalP") as Label;
                foreach (BidderItemDetail detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank().BidderItemDetails)
                {
                    PurchaseOrderDetail POD = new PurchaseOrderDetail();
                    POD.ItemAccount = _presenter.GetItemAccount(detail.ItemAccount.Id);
                    POD.Qty = detail.Qty;
                    POD.UnitCost = detail.UnitCost;
                    POD.TotalCost = detail.TotalCost;
                    _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Add(POD);
               
                   lblItemTotalP.Text = ((lblItemTotalP.Text != "" ? Convert.ToDecimal(lblItemTotalP.Text) : 0) + POD.TotalCost).ToString();
                }

                lblTotalP.Text = Convert.ToString((!String.IsNullOrEmpty(lblItemTotalP.Text) ? Convert.ToDecimal(lblItemTotalP.Text) : 0) + (!String.IsNullOrEmpty(lblVatP.Text) ? Convert.ToDecimal(lblVatP.Text) : 0) + (!String.IsNullOrEmpty(lblDeliveryFeesP.Text) ? Convert.ToDecimal(lblDeliveryFeesP.Text) : 0));
                Label lblAuthorizedBy = Repeater1.Controls[Repeater1.Controls.Count - 1].FindControl("lblAuthorizedByP") as Label;
                foreach (BidAnalysisRequestStatus detail in _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses)
                {
                    if (detail.ApprovalStatus == ApprovalStatus.Authorized.ToString())
                    {
                        lblAuthorizedBy.Text = _presenter.GetUser(detail.Approver).FullName;
                    }
                }
            //lblAuthorizedBy.
        }
        private void AutoNumber()
        {
            txtPONo.Text = "PO-" + (_presenter.GetLastPurchaseOrderId() + 1);
        }
        private void BindPurchaseOrder()
        {
            this._presenter.OnViewLoaded();
            txtDate.Text = DateTime.Today.ToString();



            txtRequester.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                if (_presenter.CurrentBidAnalysisRequest != null)
                {
                    Bidder bider = _presenter.CurrentBidAnalysisRequest.GetBidderbyRank();

                    if (bider != null)
                    {
                        txtSupplierName.Text = bider.Supplier.SupplierName;
                        txtSupplierAddress.Text = bider.Supplier.SupplierAddress;
                        txtSupplierContact.Text = bider.Supplier.SupplierContact;
                    }
                }

                if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders != null)
                {
                    if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Id > 0)
                    {
                        txtPONo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber;
                        txtDate.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PODate.ToString();
                        txtShipTo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo;
                        txtBillto.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto;
                        txtDeliveeryFees.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees.ToString();
                        txtPaymentTerms.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms;
                        btnPrintPurchaseOrder.Enabled = true;
                        btnPrintPurchaseForm.Enabled = true;
                    }
                    else
                    {
                        txtDate.Text = DateTime.Today.ToString();
                        AutoNumber();
                    }
                }

                BindPODetail();
            

        }
        private void SavePurchaseOrder()
        {
            
            try
            {

                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber = txtPONo.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PODate = Convert.ToDateTime(txtDate.Text);
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto = txtBillto.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo = txtShipTo.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees = Convert.ToDecimal(txtDeliveeryFees.Text);
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms = txtPaymentTerms.Text;
                _presenter.CurrentBidAnalysisRequest.PurchaseOrders.BidAnalysisRequest = _presenter.CurrentBidAnalysisRequest;
                if (_presenter.CurrentBidAnalysisRequest != null)
                {
                    _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Supplier = _presenter.CurrentBidAnalysisRequest.GetBidderbyRank().Supplier;
                }

                AddPurchasingItem();

                
                //_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Status = "Completed";       
                Master.ShowMessage(new AppMessage("Purchase Order Successfully Approved", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("There was an error Saving Purchase Order", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
           

               

            
        }
        private void BindPODetail()
        {

            dgPODetail.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders[0].BidderItemDetails;
            dgPODetail.DataBind();

        }
       

        #region PurchaseOrderDetail
        private void AddPurchasingItem()
        {
            if (_presenter.CurrentBidAnalysisRequest.PurchaseOrders.Id <= 0)
            {

                if (_presenter.CurrentBidAnalysisRequest != null)
                {

                    foreach (BidderItemDetail detail in _presenter.CurrentBidAnalysisRequest.GetBidderbyRank().BidderItemDetails)
                    {
                        PurchaseOrderDetail POD = new PurchaseOrderDetail();
                        POD.ItemAccount = _presenter.GetItemAccount(detail.ItemAccount.Id);
                        POD.Qty = detail.Qty;
                        POD.UnitCost = detail.UnitCost;
                        POD.TotalCost = detail.TotalCost;
                        _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Add(POD);
                    }
                    
                }
                else
                {
                    foreach (BidAnalysisRequestDetail PR in _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestDetails)
                    {
                        PurchaseOrderDetail POD = new PurchaseOrderDetail();
                        POD.ItemAccount = _presenter.GetItemAccount(PR.ItemAccount.Id);
                        POD.Qty = PR.Qty;
                        POD.UnitCost = PR.Priceperunit;
                        POD.TotalCost = PR.EstimatedCost;
                        _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PurchaseOrderDetails.Add(POD);

                    }
                }
            }

            //BindPODetail();
        }


       
        #endregion
        protected void btnRequest_Click(object sender, EventArgs e)
        {
           
                try
                {
                    SavePurchaseOrder();
                    _presenter.SaveOrUpdateBidAnalysisRequest(_presenter.CurrentBidAnalysisRequest);
                    BindRepeater();
                    PrintTransaction();
                    btnPrintPurchaseOrder.Enabled = true;
                    btnPrintPurchaseForm.Enabled = true;
                    // Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentBidAnalysisRequest.Id, "Enabled"));
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("frmPurchaseApproval.aspx?PurchaseRequestId={0}&PnlStatus={1}", _presenter.CurrentBidAnalysisRequest.Id, "Enabled"));
        }
        private void PrintTransaction()
        {
            
                lblRequestNoResult.Text = _presenter.CurrentBidAnalysisRequest.RequestNo;
                lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                lblRequestedDateResult.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
                //    lblDeliverToResult.Text = _presenter.CurrentBidAnalysisRequest..DeliverTo;
                lblPurchaseOrderNo.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PoNumber;
                //  lblDeliveryDateresult.Text = _presenter.CurrentBidAnalysisRequest..Requireddateofdelivery.ToString();
             //   lblPurposeResult.Text = _presenter.CurrentBidAnalysisRequest.Neededfor;
                lblBillToResult.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.Billto;
                lblShipToResult.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.ShipTo;
                lblPaymentTerms.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.PaymentTerms;
                lblDeliveryFeesResult.Text = _presenter.CurrentBidAnalysisRequest.PurchaseOrders.DeliveryFees.ToString();
            
                lblSuggestedSupplierResult.Text = _presenter.CurrentBidAnalysisRequest.GetBidderbyRank().Supplier.SupplierName;
                if (_presenter.CurrentBidAnalysisRequest != null)
                {
                    lblReasonforSelectionResult.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
                    lblSelectedbyResult.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;

                    grvDetails.DataSource = _presenter.CurrentBidAnalysisRequest.GetAllBidderItemDetails();
                    grvDetails.DataBind();

                    
                }
                grvStatuses.DataSource = _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses;
                grvStatuses.DataBind();
           
        }
        
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
           
            if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
           
        }
    }
}
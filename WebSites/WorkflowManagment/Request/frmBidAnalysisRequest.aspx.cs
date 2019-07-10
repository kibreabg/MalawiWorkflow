using System;
using System.Collections.Generic;
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
using System.IO;
using Chai.WorkflowManagment.CoreDomain.Request;
using System.Data;
using Chai.WorkflowManagment.CoreDomain.Setting;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmBidAnalysisRequest : POCBasePage, IBidAnalysisRequestView
    {

        private BidAnalysisRequestPresenter _presenter;
        private IList<BidAnalysisRequest> _BidAnalysisRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        Bidder bidd;
        BidAnalysisRequest bid;
        private decimal totalamaount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                btncancelCost.Attributes.Add("onClick", "return false;");
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                PopProjects();
                BindBidAnalysisRequests();
                BindBidAnalysisRequestsList();
                //Fill the Bid Analysis Request with the Purchase Request information
                PurchaseRequest purchaseRequest = _presenter.GetPurchaseRequest(GetPurchaseRequestId);
                _presenter.CurrentBidAnalysisRequest.PurchaseRequest = purchaseRequest;


                PopBidAnalysisRequesters();



                if (_presenter.CurrentBidAnalysisRequest.Id <= 0)
                {
                    AutoNumber();
                    //  btnDelete.Visible = false;
                }
            }
            // txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                if (_presenter.CurrentBidAnalysisRequest.Id != 0)
                {
                    PrintTransaction();
                    btnPrintworksheet.Enabled = true;
                }
            }

        }
        [CreateNew]
        public BidAnalysisRequestPresenter Presenter
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
                return "{D1B7939C-7154-4403-B535-B4D33684CE21}";
            }
        }
        #region Field Getters
        public int GetBidAnalysisRequestId
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
        public int GetPurchaseRequestId
        {
            get
            {
                if (Convert.ToInt32(Request.QueryString["PurchaseRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["PurchaseRequestId"]);
                }
                return 0;
            }
        }
        public int GetBARequestId
        {
            get
            {
                if (grvBidAnalysisRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvBidAnalysisRequestList.SelectedDataKey.Value);
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
        public DateTime GetAnalysedDate
        {
            get
            {
                return Convert.ToDateTime(txtAnalyzedDate.Text);
            }
        }
        public string GetNeededFor
        {
            get { return txtselectionfor.Text; }
        }
        public string GetPaymentMethod
        {
            get { return ddlPayMethods.Text; }
        }
        public string GetSpecialNeed
        {
            get { return txtSpecialNeed.Text; }
        }
        public int GetProjectId
        {
            get { return Convert.ToInt32(ddlProject.SelectedValue); }
        }
        public int GetGrantId
        {
            get { return Convert.ToInt32(ddlGrant.SelectedValue); }
        }

        public IList<BidAnalysisRequest> BidAnalysisRequests
        {
            get
            {
                return _BidAnalysisRequests;
            }
            set
            {
                _BidAnalysisRequests = value;
            }
        }

        public string GetReasonForSelection
        {
            get
            {
                return txtselectionfor.Text;
            }
        }
        #endregion

        private void BindBidAnalysisRequests()
        {
            dgBidders.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders;
            dgBidders.DataBind();
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();
        }
        #region Bidders

        private void BindSupplier(DropDownList ddlSupplier, int SupplierTypeId)
        {
            if (ddlSupplier.Items.Count > 0)
            {
                ddlSupplier.Items.Clear();
            }
            ddlSupplier.DataSource = _presenter.GetSuppliers(SupplierTypeId);
            ddlSupplier.DataBind();
        }
        private void BindSupplierType(DropDownList ddlSupplierType)
        {
            ddlSupplierType.DataSource = _presenter.GetSupplierTypes();
            ddlSupplierType.DataBind();
        }


        private void BindItemdetailGrid(Bidder Tad)
        {
            bidd = Session["bidd"] as Bidder;
            dgItemDetail.DataSource = bidd.BidderItemDetails;
            dgItemDetail.DataBind();
        }

        private void BindBidder()
        {
            dgBidders.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders;
            dgBidders.DataBind();
        }

        #endregion
        #region BidderItemDetail
        //  protected void btnAddItemdetail_Click(object sender, EventArgs e)
        //{
        //   SetBidderItemDetail();
        // }
        private void BindItemDetails()
        {
            Session["bidder"] = bidd;
            /*  if (bidder.BidderItemDetails.Count == 0)
              {
                  BindBidAnalysisRequests();
                 // AddRequestedItem();
              }*/
            dgItemDetail.DataSource = bidd.BidderItemDetails;
            dgItemDetail.DataBind();
        }

        private void SetBidderItemDetail()
        {


            bidd = Session["bidder"] as Bidder;
            int index = 0;
            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];

                BidderItemDetail detail;
                if (id > 0)
                {
                    detail = bidd.GetBidderItemDetail(id);
                }
                else
                {
                    detail = (BidderItemDetail)bidd.BidderItemDetails[index];
                }


                TextBox txtQty = dgi.FindControl("txtQty") as TextBox;
                detail.Qty = Convert.ToInt32(txtQty.Text);
                TextBox txtUnitCost = dgi.FindControl("txtUnitCost") as TextBox;
                detail.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                TextBox txtTotalCost = dgi.FindControl("txtTotalCost") as TextBox;
                detail.TotalCost = Convert.ToDecimal(txtTotalCost.Text);
                bidd.BidderItemDetails.Add(detail);
                index++;


            }
            Master.ShowMessage(new AppMessage("Bidder Items successfully saved!", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }


        #endregion
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
        protected void ddlFSupplierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlFSupplier = ddl.FindControl("ddlFSupplier") as DropDownList;
            BindSupplier(ddlFSupplier, Convert.ToInt32(ddl.SelectedValue));
        }
        protected void ddlSupplierType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            DropDownList ddlSupplier = ddl.FindControl("ddlSupplier") as DropDownList;
            BindSupplier(ddlSupplier, Convert.ToInt32(ddl.SelectedValue));
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteBidAnalysisRequest(_presenter.CurrentBidAnalysisRequest);

            BindBidAnalysisRequests();
            // btnDelete.Enabled = false;
            Master.ShowMessage(new AppMessage("Bid Analysis Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindBidAnalysisRequestsList();
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
            Response.Redirect("frmBidAnalysisRequest.aspx");
        }
        /*   protected void DeleteFile(object sender, EventArgs e)
           {
               string filePath = (sender as LinkButton).CommandArgument;
               _presenter.CurrentBidAnalysisRequest.ELRAttachments.Removet(filePath);
               File.Delete(Server.MapPath(filePath));
               grvAttachments.DataSource = _presenter.CurrentPurchaseRequest.BidAnalysises.BAAttachments;
               grvAttachments.DataBind();
               //Response.Redirect(Request.Url.AbsoluteUri);


           }*/
        /*   private void UploadFile()
           {
               string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);

               if (fileName != String.Empty)
               {



                   BAAttachment attachment = new BAAttachment();
                   attachment.FilePath = "~/BAUploads/" + fileName;
                   fuReciept.PostedFile.SaveAs(Server.MapPath("~/BAUploads/") + fileName);
                   //Response.Redirect(Request.Url.AbsoluteUri);
                   _presenter.CurrentBidAnalysisRequest.ELRAttachments.Add(attachment);

                   grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.ELRAttachments;
                   grvAttachments.DataBind();


               }
               else
               {
                   Master.ShowMessage(new AppMessage("Please select file ", Chai.WorkflowManagment.Enums.RMessageType.Error));
               }
           }*/
        /*     private void BindAttachments()
             {
                 if (_presenter.CurrentPurchaseRequest.BidAnalysises.Id > 0)
                 {
                     grvAttachments.DataSource = _presenter.CurrentPurchaseRequest.BidAnalysises.BAAttachments;
                     grvAttachments.DataBind();
                 }
             }*/
        #endregion
        private void BindBidAnalysisRequestsList()
        {
            grvBidAnalysisRequestList.DataSource = _presenter.ListBidAnalysisRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvBidAnalysisRequestList.DataBind();
        }
        private string AutoNumber()
        {
            return "BAR-" + (_presenter.GetLastBidAnalysisRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {

            if (_presenter.GetApprovalSetting(RequestType.Bid_Analysis_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void BindBidAnalysisRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                txtRequester.Text = _presenter.CurrentBidAnalysisRequest.AppUser.UserName.ToString();
                // txtRequestDate.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.Value.ToShortDateString();
                txtAnalyzedDate.Text = _presenter.CurrentBidAnalysisRequest.AnalyzedDate.ToShortDateString();
                ddlPayMethods.Text = _presenter.CurrentBidAnalysisRequest.PaymentMethod;
                txtSpecialNeed.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;
                txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
                txtTotal.Text = Convert.ToDecimal(_presenter.CurrentBidAnalysisRequest.TotalPrice).ToString();

                ddlProject.SelectedValue = _presenter.CurrentBidAnalysisRequest.Project.Id.ToString();

                PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
                ddlGrant.SelectedValue = _presenter.CurrentBidAnalysisRequest.Grant.Id.ToString();
                BindBidAnalysisRequests();
                btnPrintworksheet.Enabled = true;
                PrintTransaction();
            }
        }
        protected void dgBidders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            _presenter.CurrentBidAnalysisRequest.RemoveBAAttachment(filePath);
            File.Delete(Server.MapPath(filePath));
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();
            //Response.Redirect(Request.Url.AbsoluteUri);


        }
        #region Bidders
        private void SaveBidAnalysis()
        {
            try
            {
                _presenter.CurrentBidAnalysisRequest.PurchaseRequest.Id = Convert.ToInt32(GetPurchaseRequestId);
                _presenter.CurrentBidAnalysisRequest.AnalyzedDate = Convert.ToDateTime(txtAnalyzedDate.Text);
                //   _presenter.CurrentBidAnalysisRequest.Neededfor = txtselectionfor.Text;
                _presenter.CurrentBidAnalysisRequest.SpecialNeed = txtSpecialNeed.Text;
                _presenter.CurrentBidAnalysisRequest.ReasonforSelection = txtselectionfor.Text;

                //foreach (Bidder bider in _presenter.CurrentBidAnalysisRequest.Bidders)
                //{
                //    foreach (BidderItemDetail detail in bider.BidderItemDetails)
                //    {
                //        totalamaount = totalamaount + detail.TotalCost;
                //    }
                //    totalamaount = Convert.ToDecimal(txtTotal.Text);
                //}
                _presenter.CurrentBidAnalysisRequest.TotalPrice = totalamaount;
                _presenter.CurrentBidAnalysisRequest.SelectedBy = _presenter.CurrentUser().Id;
                if (_presenter.CurrentBidAnalysisRequest.GetBidderbyRank().Supplier != null)
                    _presenter.CurrentBidAnalysisRequest.Supplier = _presenter.CurrentBidAnalysisRequest.GetBidderbyRank().Supplier;
                _presenter.CurrentBidAnalysisRequest.Status = "Completed";
            }
            catch (Exception ex)
            {

            }

        }
        protected void dgBidders_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }
        //protected void dgBidders_ItemCommand(object source, DataGridCommandEventArgs e)
        //{
        //    Chai.WorkflowManagment.CoreDomain.Requests.Bidder bidder = new Chai.WorkflowManagment.CoreDomain.Requests.Bidder();
        //    if (e.CommandName == "AddNew")
        //    {
        //        try
        //        {
        //            DropDownList ddlSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
        //            bidder.SupplierType = _presenter.GetSupplierType(Convert.ToInt32(ddlSupplierType.SelectedValue));
        //            DropDownList ddlSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
        //            bidder.Supplier = _presenter.GetSupplier(Convert.ToInt32(ddlSupplier.SelectedValue));
        //            TextBox txtFLeadTimefromSupplier = e.Item.FindControl("txtFLeadTimefromSupplier") as TextBox;
        //            bidder.LeadTimefromSupplier = txtFLeadTimefromSupplier.Text;
        //            TextBox txtFContactDetails = e.Item.FindControl("txtFContactDetails") as TextBox;
        //            bidder.ContactDetails = txtFContactDetails.Text;
        //            TextBox txtFSpecialTermsDelivery = e.Item.FindControl("txtFSpecialTermsDeliveryy") as TextBox;
        //            bidder.SpecialTermsDelivery = txtFSpecialTermsDelivery.Text;
        //            TextBox txtFRank = e.Item.FindControl("txtFRank") as TextBox;
        //            bidder.Rank = Convert.ToInt32(txtFRank.Text);


        //            _presenter.CurrentBidAnalysisRequest.Bidders.Add(bidder);

        //            dgBidders.EditItemIndex = -1;
        //            BindBidder();

        //        }
        //        catch (Exception ex)
        //        {
        //            Master.ShowMessage(new AppMessage("Error: Unable to Add Bidder " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
        //        }
        //    }
        //}
        protected void dgBidders_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBidders.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Requests.Bidder bidder = _presenter.CurrentBidAnalysisRequest.GetBidder(id);

            try
            {
                DropDownList ddlSupplierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                bidder.SupplierType = _presenter.GetSupplierType(Convert.ToInt32(ddlSupplierType.SelectedValue));
                DropDownList ddlSupplier = e.Item.FindControl("ddlSupplier") as DropDownList;
                bidder.Supplier = _presenter.GetSupplier(Convert.ToInt32(ddlSupplier.SelectedValue));
                TextBox txtFLeadTimefromSupplier = e.Item.FindControl("txtLeadTimefromSupplier") as TextBox;
                bidder.LeadTimefromSupplier = txtFLeadTimefromSupplier.Text;
                TextBox txtContactDetails = e.Item.FindControl("txtContactDetails") as TextBox;
                bidder.ContactDetails = txtContactDetails.Text;
                TextBox txtSpecialTermsDelivery = e.Item.FindControl("txtSpecialTermsDelivery") as TextBox;
                bidder.SpecialTermsDelivery = txtSpecialTermsDelivery.Text;
                TextBox txtFRank = e.Item.FindControl("txtRank") as TextBox;
                bidder.Rank = Convert.ToInt32(txtFRank.Text);

                dgBidders.EditItemIndex = -1;
                BindBidder();
                Master.ShowMessage(new AppMessage("Bidder Detail Successfully Updated", RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bidder " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }




        }
        private void UploadFile()
        {
            string fileName = Path.GetFileName(fuReciept.PostedFile.FileName);
            try
            {
                if (fileName != String.Empty)
                {



                    BAAttachment attachment = new BAAttachment();
                    attachment.FilePath = "~/BAUploads/" + fileName;
                    fuReciept.PostedFile.SaveAs(Server.MapPath("~/BAUploads/") + fileName);
                    //Response.Redirect(Request.Url.AbsoluteUri);
                    _presenter.CurrentBidAnalysisRequest.BAAttachments.Add(attachment);

                    grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
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
        protected void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                //try
                //{
                //  _presenter.SaveOrUpdateBidAnalysisRequest();
                if (_presenter.CurrentBidAnalysisRequest.BAAttachments.Count != 0)
                {

                    _presenter.SaveOrUpdateBidAnalysisRequest();
                    BindBidAnalysisRequests();

                    Master.ShowMessage(new AppMessage("Successfully did a Bid Analysis  Request, Reference No - <b>'" + _presenter.CurrentBidAnalysisRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested a For a Bid Analyis");
                    //btnSave.Visible = false;
                    PrintTransaction();
                    btnPrintworksheet.Enabled = true;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("Please Attach Bid Analysis Quotation", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
                decimal price = 0;
                foreach (Bidder bider in _presenter.CurrentBidAnalysisRequest.Bidders)
                {


                    if (_presenter.CurrentBidAnalysisRequest.GetBidderbyRank().Rank == 1)
                    {

                        foreach (BidderItemDetail biditemdet in bider.BidderItemDetails)
                        {

                            price = price + biditemdet.TotalCost;
                        }
                    }
                    txtTotal.Text = price.ToString();
                    break;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        private void BindBidRequestforprint()
        {

            /*
           lblrequestNo.Text = _presenter.CurrentBidAnalysisRequest.RequestNo;

             lblRequester.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
             lblRequestDate0.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
             // lblApprovedBy.Text = _presenter.CurrentBidAnalysisRequest..ContactPersonNumber;
             lblSpecialNeed.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;

             lblEstimatedTotalCost.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice;
             lblselectedSupplier.Text = _presenter.CurrentBidAnalysisRequest.Supplier;
             lblReasonforsel.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
             lblSelectedBy.Text = _presenter.CurrentBidAnalysisRequest.SelectedBy;

             */



        }
        #endregion
        protected void dgItemDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            bidd = Session["bidd"] as Bidder;
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFItemAcc = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                BindItems(ddlFItemAcc);


                /* if ((_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].BidderItemDetails != null))
                 {
                     DropDownList ddlItemAcc = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                     ListItem liI = ddlItemAcc.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].BidderItemDetails[0].ItemAccount.Id.ToString());
                     if (liI != null)
                         liI.Selected = true;
                 }*/

            }


            else
            {
                if (bidd.BidderItemDetails != null)
                {
                    DropDownList ddlItemAcc = e.Item.FindControl("ddlItemAcc") as DropDownList;
                    if (ddlItemAcc != null)
                    {
                        BindItems(ddlItemAcc);

                        if (_presenter.CurrentBidAnalysisRequest.Id > 0)
                        {
                            if (_presenter.CurrentBidAnalysisRequest.GetBidder(Convert.ToInt32(hfDetailId.Value)).BidderItemDetails[e.Item.DataSetIndex] != null)
                            {
                                if (_presenter.CurrentBidAnalysisRequest.GetBidder(Convert.ToInt32(hfDetailId.Value)).BidderItemDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                                {
                                    ListItem liI = ddlItemAcc.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.GetBidder(Convert.ToInt32(hfDetailId.Value)).BidderItemDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                                    if (liI != null)
                                        liI.Selected = true;
                                }
                            }
                        }
                        else
                        {
                            if (_presenter.CurrentBidAnalysisRequest.Bidders[Convert.ToInt32(hfDetailId.Value)].BidderItemDetails[e.Item.DataSetIndex] != null)
                            {
                                if (_presenter.CurrentBidAnalysisRequest.Bidders[Convert.ToInt32(hfDetailId.Value)].BidderItemDetails[e.Item.DataSetIndex].ItemAccount.Id != 0)
                                {
                                    ListItem liI = ddlItemAcc.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.Bidders[Convert.ToInt32(hfDetailId.Value)].BidderItemDetails[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                                    if (liI != null)
                                        liI.Selected = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void BindItems(DropDownList ddlItems)
        {
            ddlItems.DataSource = _presenter.GetItemAccounts();
            ddlItems.DataBind();
        }
        private void PopBidAnalysisRequesters()
        {
            if (_presenter.CurrentBidAnalysisRequest.PurchaseRequest != null)
            {

                txtRequester.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;

                txtRequestDate.Text = _presenter.GetPurchaseRequest(_presenter.CurrentBidAnalysisRequest.PurchaseRequest.Id).RequestedDate.ToShortDateString();

                txtAnalyzedDate.Text = DateTime.Now.ToShortDateString();

                ddlProject.SelectedValue = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails[0].Project.Id.ToString();
                if (_presenter.CurrentBidAnalysisRequest.TotalPrice != 0)
                {
                    txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                }
                ddlGrant.DataSource = _presenter.GetGrantbyprojectId(Convert.ToInt32(ddlProject.SelectedValue));
                ddlGrant.DataBind();
                ddlGrant.SelectedValue = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails[0].Grant.Id.ToString();

                //    GridView1.DataSource = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails;
                //  GridView1.DataBind();
            }

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
        protected void dgBidders_ItemDataBound1(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlFSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
                BindSupplierType(ddlFSupplierType);
                DropDownList ddlFSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
                BindSupplier(ddlFSupplier, int.Parse(ddlFSupplierType.SelectedValue));
                //DropDownList ddlFItemAcc = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                //BindItems(ddlFItemAcc);
            }
            else
            {


                if (_presenter.CurrentBidAnalysisRequest.Bidders != null)
                {


                    DropDownList ddlSupplierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                    if (ddlSupplierType != null)
                    {
                        BindSupplierType(ddlSupplierType);
                        if ((_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].SupplierType.Id != null))
                        {
                            ListItem li = ddlSupplierType.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].SupplierType.Id.ToString());
                            if (li != null)
                                li.Selected = true;
                        }

                    }

                    DropDownList ddlSupplier = e.Item.FindControl("ddlSupplier") as DropDownList;
                    if (ddlSupplierType != null)
                    {
                        BindSupplier(ddlSupplier, int.Parse(ddlSupplierType.SelectedValue));
                        if ((_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].Supplier.Id != null))
                        {
                            ListItem liI = ddlSupplier.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].Supplier.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }
                    /*   DropDownList ddlItemAcc = e.Item.FindControl("ddlItemAcc") as DropDownList;
                       if (ddlItemAcc != null)
                       {
                           BindItems(ddlItemAcc);
                           if ((_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].ItemAccount.Id != null))
                           {
                               ListItem liI = ddlItemAcc.Items.FindByValue(_presenter.CurrentBidAnalysisRequest.Bidders[e.Item.DataSetIndex].ItemAccount.Id.ToString());
                               if (liI != null)
                                   liI.Selected = true;
                           }

                      }*/

                }

            }
        }
        protected void dgBidders_CancelCommand1(object source, DataGridCommandEventArgs e)
        {
            this.dgBidders.EditItemIndex = -1;
        }
        protected void dgBidders_DeleteCommand1(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBidders.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Requests.Bidder bidder = _presenter.CurrentBidAnalysisRequest.GetBidder(id);
            try
            {
                _presenter.DeleteBidder(bidder);
                BindBidder();

                Master.ShowMessage(new AppMessage("Bidder was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bidder. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBidders_ItemCommand1(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Requests.Bidder bidder = new Chai.WorkflowManagment.CoreDomain.Requests.Bidder();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlSupplierType = e.Item.FindControl("ddlFSupplierType") as DropDownList;
                    bidder.SupplierType = _presenter.GetSupplierType(Convert.ToInt32(ddlSupplierType.SelectedValue));
                    DropDownList ddlSupplier = e.Item.FindControl("ddlFSupplier") as DropDownList;
                    bidder.Supplier = _presenter.GetSupplier(Convert.ToInt32(ddlSupplier.SelectedValue));
                    TextBox txtFLeadTimefromSupplier = e.Item.FindControl("txtFLeadTimefromSupplier") as TextBox;
                    bidder.LeadTimefromSupplier = txtFLeadTimefromSupplier.Text;
                    TextBox txtFContactDetails = e.Item.FindControl("txtFContactDetails") as TextBox;
                    bidder.ContactDetails = txtFContactDetails.Text;
                    TextBox txtFSpecialTermsDelivery = e.Item.FindControl("txtFSpecialTermsDeliveryy") as TextBox;
                    bidder.SpecialTermsDelivery = txtFSpecialTermsDelivery.Text;
                    TextBox txtFRank = e.Item.FindControl("txtFRank") as TextBox;
                    bidder.Rank = Convert.ToInt32(txtFRank.Text);


                    _presenter.CurrentBidAnalysisRequest.Bidders.Add(bidder);
                    Master.ShowMessage(new AppMessage("Bidder Detail Successfully Added", RMessageType.Info));
                    dgBidders.EditItemIndex = -1;
                    BindBidder();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Bidder " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void BindBidItem(Bidder Bidddet)
        {
            bidd = Session["bidd"] as Bidder;
            dgItemDetail.DataSource = bidd.BidderItemDetails;
            dgItemDetail.DataBind();
        }
        protected void dgBidders_EditCommand1(object source, DataGridCommandEventArgs e)
        {
            this.dgBidders.EditItemIndex = e.Item.ItemIndex;

            BindBidder();
        }
        protected void dgItemDetail_CancelCommand(object source, DataGridCommandEventArgs e)
        {

        }
        protected void dgItemDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            bidd = Session["bidd"] as Bidder;

            if (e.CommandName == "AddNew")
            {
                try
                {
                    BidderItemDetail biditemdet = new BidderItemDetail();
                    biditemdet.Bidder = bidd;
                    DropDownList ddlItem = e.Item.FindControl("ddlFItemAcc") as DropDownList;
                    biditemdet.ItemAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlItem.SelectedValue));

                    TextBox txtItemDescription = e.Item.FindControl("txtFDescription") as TextBox;
                    biditemdet.ItemDescription = txtItemDescription.Text;
                    TextBox txtQty = e.Item.FindControl("txtQty") as TextBox;
                    biditemdet.Qty = Convert.ToInt32(txtQty.Text);
                    TextBox txtUnitCost = e.Item.FindControl("txtUnitCost") as TextBox;
                    biditemdet.UnitCost = Convert.ToDecimal(txtUnitCost.Text);


                    biditemdet.TotalCost = Convert.ToInt32(txtQty.Text) * Convert.ToDecimal(txtUnitCost.Text);


                    if (_presenter.CurrentBidAnalysisRequest.Id > 0)
                        _presenter.CurrentBidAnalysisRequest.GetBidder(Convert.ToInt32(hfDetailId.Value)).BidderItemDetails.Add(biditemdet);
                    else
                        _presenter.CurrentBidAnalysisRequest.Bidders[Convert.ToInt32(hfDetailId.Value)].BidderItemDetails.Add(biditemdet);

                    dgItemDetail.EditItemIndex = -1;

                    BindItemdetailGrid(biditemdet.Bidder);
                    decimal cost = 0;
                    if (_presenter.CurrentBidAnalysisRequest.Bidders.Count > 0)
                    {

                        foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.Bidders)
                        {
                            if (detail.Rank == 1)
                            {
                                foreach (BidderItemDetail biddetail in detail.BidderItemDetails)
                                {

                                    cost = cost + biddetail.TotalCost;
                                }
                            };
                        }
                    }
                    _presenter.CurrentBidAnalysisRequest.TotalPrice = cost;
                    txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                    pnlBidItem_ModalPopupExtender.Show();
                    Master.ShowMessage(new AppMessage("Bidder Item Successfully Added", Chai.WorkflowManagment.Enums.RMessageType.Info));
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add BidderItem " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgItemDetail_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            bidd = Session["bidd"] as Bidder;
            int id = (int)dgItemDetail.DataKeys[e.Item.ItemIndex];
            BidderItemDetail biddet;
            // Chai.WorkflowManagment.CoreDomain.Requests.BidderItemDetail detail = _presenter.CurrentBidAnalysisRequest.GetBidder(bidd.Id).GetBidderItemDetail(id);
            if (id > 0)
                biddet = bidd.GetBidderItemDetail(id);
            else

                biddet = bidd.BidderItemDetails[e.Item.ItemIndex];

            try
            {
                biddet.Bidder = biddet.Bidder;
                DropDownList ddlItem = e.Item.FindControl("ddlItemAcc") as DropDownList;
                ItemAccount itemEdtAccount = _presenter.GetItemAccount(Convert.ToInt32(ddlItem.SelectedValue));
                biddet.ItemAccount = itemEdtAccount;

                TextBox txtItemDescription = e.Item.FindControl("txtDescription") as TextBox;
                biddet.ItemDescription = txtItemDescription.Text;
                TextBox txtQty = e.Item.FindControl("txtEdtQty") as TextBox;
                biddet.Qty = Convert.ToInt32(txtQty.Text);
                TextBox txtUnitCost = e.Item.FindControl("txtEdtUnitCost") as TextBox;
                biddet.UnitCost = Convert.ToDecimal(txtUnitCost.Text);
                biddet.TotalCost = Convert.ToInt32(txtQty.Text) * Convert.ToDecimal(txtUnitCost.Text);


                dgItemDetail.EditItemIndex = -1;

                BindItemdetailGrid(biddet.Bidder);

                decimal cost = 0;
                if (_presenter.CurrentBidAnalysisRequest.Bidders.Count > 0)
                {

                    foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.Bidders)
                    {
                        if (detail.Rank == 1)
                        {
                            foreach (BidderItemDetail biddetail in detail.BidderItemDetails)
                            {

                                cost = cost + biddetail.TotalCost;
                            }
                        };
                    }
                }
                _presenter.CurrentBidAnalysisRequest.TotalPrice = cost;
                txtTotal.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                pnlBidItem_ModalPopupExtender.Show();
                Master.ShowMessage(new AppMessage("Bidder Item Successfully Updated", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Add BidderItem " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgItemDetail_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            bidd = Session["bidd"] as Bidder;

            int id = (int)dgItemDetail.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Requests.BidderItemDetail bidderItem = _presenter.CurrentBidAnalysisRequest.GetBidder(bidd.Id).GetBidderItemDetail(id);
            try
            {
                _presenter.DeleteBidderItemDetail(bidderItem);
                BindItemDetails();

                Master.ShowMessage(new AppMessage("Bidder Item was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bidder Item. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgItemDetail_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemDetail.EditItemIndex = e.Item.ItemIndex;

            BindItemDetails();
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
        }
        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            foreach (PurchaseRequestDetail prd in _presenter.CurrentBidAnalysisRequest.PurchaseRequest.PurchaseRequestDetails)
            {



                Label lbl = e.Item.FindControl("lblName") as Label;
                prd.ItemAccount.AccountName = lbl.Text;
                Label lbl1 = e.Item.FindControl("lblName1") as Label;
                prd.AccountCode = lbl1.Text;
                Label lbl2 = e.Item.FindControl("lblName2") as Label;
                prd.EstimatedCost = Convert.ToDecimal(lbl2.Text);





            }
        }
        protected void grvBidAnlysisRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {

            //ClearForm();
            BindBidAnalysisRequestFields();
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();

            if (_presenter.CurrentBidAnalysisRequest.CurrentStatus != null)
            {
                btnRequest.Visible = false;

            }
            else
            {
                btnRequest.Visible = true;

            }
        }
        private void PrintTransaction()
        {
            lblRequester.Text = _presenter.CurrentBidAnalysisRequest.AppUser.UserName.ToString();
            lblRequestDate0.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
            lblSpecialNeed.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;
            lblpaytypeRes.Text = _presenter.CurrentBidAnalysisRequest.PaymentMethod;
            lblTot.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();

            txtselectionfor.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
            lblReasonForSelection.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;

            grvprtBidders.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders;
            grvprtBidders.DataBind();

            grvStatuses.DataSource = _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses;
            grvStatuses.DataBind();


            foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.Bidders)
            {
                grvprtBidderItemDetails.DataSource = detail.BidderItemDetails;
                grvprtBidderItemDetails.DataBind();
            }
        }
        protected void grvBidAnalysisRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBidAnalysisRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver != 0)
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void dgItemDetail_EditCommand1(object source, DataGridCommandEventArgs e)
        {
            bidd = Session["bidd"] as Bidder;
            this.dgItemDetail.EditItemIndex = e.Item.ItemIndex;
            int BIDId = (int)dgItemDetail.DataKeys[e.Item.ItemIndex];


            BidderItemDetail biditem;

            if (BIDId > 0)
                biditem = _presenter.GetBiderItemDet(BIDId);
            else
                biditem = (BidderItemDetail)bidd.BidderItemDetails[e.Item.ItemIndex];
            BindBidItem(biditem.Bidder);
            pnlBidItem_ModalPopupExtender.Show();

        }
        protected void dgBidders_SelectedIndexChanged1(object sender, EventArgs e)
        {


            int BidderId = (int)dgBidders.DataKeys[dgBidders.SelectedItem.ItemIndex];
            int Id = dgBidders.SelectedItem.ItemIndex;


            if (BidderId > 0)
                Session["bidd"] = _presenter.CurrentBidAnalysisRequest.GetBidder(BidderId);

            else
                Session["bidd"] = _presenter.CurrentBidAnalysisRequest.Bidders[dgBidders.SelectedItem.ItemIndex];


            int recordId = (int)dgBidders.SelectedIndex;
            if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            {
                hfDetailId.Value = BidderId.ToString();
            }
            else
            {
                hfDetailId.Value = dgBidders.SelectedItem.ItemIndex.ToString();
            }


            BindBidItem(bidd);
            pnlBidItem_ModalPopupExtender.Show();
        }
        protected void btncancelCost_Click(object sender, EventArgs e)
        {

        }
    }
}
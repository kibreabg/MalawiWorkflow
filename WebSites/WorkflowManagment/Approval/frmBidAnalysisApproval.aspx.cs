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
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Approval;
using log4net;
using System.Reflection;
using log4net.Config;
using System.IO;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmBidAnalysisApproval : POCBasePage, IBidAnalysisView
    {
        private BidAnalysisApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private BidAnalysisRequest _bidanalysisrequest;
        private int reqid;
        private int PurchaseId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
                if (_presenter.CurrentBidAnalysisRequest != null)
                {
                    if (_presenter.CurrentBidAnalysisRequest.Id > 0)
                    {
                        PopApprovalStatus();
                        PrintTransaction();
                        BindBidAnalysisRequestforprint();
                    }
                }
            }
            this._presenter.OnViewLoaded();
            BindSearchPurchaseRequestGrid();
            ReturnFromBidAnalysis();
            //PrintTransaction();
            BindBidAnalysisRequestforprint();
        
        }
        protected void BindJS()
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "workflowscripts", String.Format("<script language=\"JavaScript\" src=\"http://localhost/WorkflowManagment/WorkflowManagment.js\"></script>\n"));
        }
        [CreateNew]
        public BidAnalysisApprovalPresenter Presenter
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
                return "{BF5943CA-07E5-4817-93EC-DE39D67D5562}";
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
            get { return txtRequestNosearch.Text; }
        }
        public string RequestDate
        {
            get { return txtRequestDatesearch.Text; }
        }
        public int BidAnalysisRequestId
        {
            get
            {
                if (grvPurchaseRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvPurchaseRequestList.SelectedDataKey.Value);


                }
                else if (Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]) != 0)
                {
                    return Convert.ToInt32(Request.QueryString["BidAnalysisRequestId"]);
                }
                else if (Convert.ToInt32(Session["ReqID"]) != 0)
                {
                    return Convert.ToInt32(Session["ReqID"]);
                }

                else
                {
                    return 0;
                }
            }
            set
            {
                reqid = value;
            }

        }
        public string PnlStatus
        {
            get
            {
                if (Convert.ToString(Request.QueryString["PnlStatus"]) != null)
                    return Convert.ToString(Request.QueryString["PnlStatus"]);
                else
                    return "Disabled";
            }
        }
        private void PopApprovalStatus()
        {
            ddlApprovalStatus.Items.Clear();
            ddlApprovalStatus.Items.Add(new ListItem("Select Status", "0"));
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 3) == s[i].Substring(0, 3))
                {
                    ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

                }

            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
           
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.Bid_Analysis_Request.ToString().Replace('_', ' ').ToString(), _presenter.CurrentBidAnalysisRequest.TotalPrice);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentBidAnalysisRequest.CurrentLevel == 1)
                {
                    will = "Approve";
                    break;
                    

                }
                else if (_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;

                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }


        }
        private void BindPurchaseRequestStatus()
        {
            foreach (BidAnalysisRequestStatus PRS in _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses)
            {
                if (PRS.WorkflowLevel == _presenter.CurrentBidAnalysisRequest.CurrentLevel && PRS.WorkflowLevel == _presenter.CurrentBidAnalysisRequest.CurrentLevel)
                {
                    ddlApprovalStatus.SelectedValue = PRS.ApprovalStatus;
                    txtRejectedReason.Text = PRS.RejectedReason;
                    if (_presenter.CurrentBidAnalysisRequest.ProgressStatus == ProgressStatus.Completed.ToString())
                    {
                        btnApprove.Enabled = false;
                      //  btnPrint0.Enabled = true;
                    }
                    else
                    {
                        btnApprove.Enabled = true;
                    }

                }
                //if (_presenter.CurrentBidAnalysisRequest.CurrentLevel == _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count && (PRS.ApprovalStatus != null) && _presenter.CurrentBidAnalysisRequest.ProgressStatus == ProgressStatus.Completed.ToString())
                //{
                //    btnPurchaseOrder.Enabled = true;
                //}
            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentBidAnalysisRequest.CurrentLevel == _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count && _presenter.CurrentBidAnalysisRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                //btnPurchaseOrder.Visible = true;
                btnPrint0.Enabled = true;
                SendEmailToRequester();
                //btnPurchaseOrder.Enabled = true;
             

            }

        }
        private void SavePurchaseRequestStatus()
        {

            foreach (BidAnalysisRequestStatus PRS in _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses)
            {
                if ((PRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(PRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo : 0)) && PRS.WorkflowLevel == _presenter.CurrentBidAnalysisRequest.CurrentLevel)
                {
                    PRS.ApprovalDate = DateTime.Now;
                    PRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    PRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(PRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(PRS.Approver).AppUser.FullName : "";
                    PRS.RejectedReason = txtRejectedReason.Text;
                    if (PRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentBidAnalysisRequest.CurrentLevel == _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count)
                            _presenter.CurrentBidAnalysisRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                     GetNextApprover();
                        PRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(PRS.Approver).FullName + " has " + PRS.ApprovalStatus + " Bid Analysis Request made by " + _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName);
                       // btnPurchaseOrder.Enabled = true;
                    }
                    else
                    {
                        _presenter.CurrentBidAnalysisRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        PRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(PRS);
                        Log.Info(_presenter.GetUser(PRS.Approver).FullName + " has " + PRS.ApprovalStatus + " Bid Analysis Request made by " + _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName);
                        //btnPurchaseOrder.Enabled = true;
                    }
                    break;
                }

            }

        }
        private void SendEmailRejected(BidAnalysisRequestStatus PRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).Email, "Bid Analysis Request Rejection", "Your Purchase Request with Request No. - '" + _presenter.CurrentBidAnalysisRequest.RequestNo + "' was Rejected for this reason - '" + PRS.RejectedReason + "'");

            if (PRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < PRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[i].Approver).Email, "Purchase Request Rejection", "Purchase Request with Request No. - '" + _presenter.CurrentBidAnalysisRequest.RequestNo + "' made by " + _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName + " was Rejected for this reason - '" + PRS.RejectedReason + "'");
                }
            }
        }
        private void GetNextApprover()
        {
            foreach (BidAnalysisRequestStatus PRS in _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses)
            {
                if (PRS.ApprovalStatus == null)
                {
                    SendEmail(PRS);
                    _presenter.CurrentBidAnalysisRequest.CurrentApprover = PRS.Approver;
                    _presenter.CurrentBidAnalysisRequest.CurrentLevel = PRS.WorkflowLevel;
                    _presenter.CurrentBidAnalysisRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    _presenter.CurrentBidAnalysisRequest.CurrentStatus = PRS.ApprovalStatus;
                    break;

                }
            }
        }
        private void SendEmail(BidAnalysisRequestStatus PRS)
        {
            if (_presenter.GetUser(PRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(PRS.Approver).Email, "Purchase Request", (_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName).ToUpper() + " Requests for Purchase with Purchase No. - '" + (_presenter.CurrentBidAnalysisRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(PRS.Approver).AssignedTo).Email, "Purchase Request", (_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName).ToUpper() + " Requests for Purchase with Purchase No '" + (_presenter.CurrentBidAnalysisRequest.RequestNo).ToUpper() + "'");
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).Email, "Bid Analysis  Request ", "Your Bid Analysis Request with Bid Analysis Request No. - '" + (_presenter.CurrentBidAnalysisRequest.RequestNo).ToUpper() + "' was Completed.");
        }
        private void EnableControls()
        {
            if (_presenter.CurrentBidAnalysisRequest.CurrentLevel == _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count) 
            {
                if (_presenter.CurrentBidAnalysisRequest.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    // btnPurchaseOrder.Enabled = true;
                    // SendEmailToRequester();
                    btnPrint0.Enabled = true;
                }
                else
                {
                    btnPrint0.Enabled = false;
                }
            }
        }
        private void BindBidAnalysisRequestforprint()
        {
            if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            {
                lblRequestNoResult.Text = _presenter.CurrentBidAnalysisRequest.RequestNo;
                lblRequestedDateResult.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
                lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName; 
                lblTotalPriceResult.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();
                lblpaytypeRes.Text = _presenter.CurrentBidAnalysisRequest.PaymentMethod;
                lblCommentResult.Text = _presenter.CurrentBidAnalysisRequest.ReasonforSelection;
                lblRequireddateofdeliveryResult.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;

              //  lblApprovalStatusResult.Text = _presenter.CurrentBidAnalysisRequest.CurrentStatus;
                


                grvStatuses.DataSource = _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses;
                grvStatuses.DataBind();
            }
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (_presenter.CurrentBidAnalysisRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SavePurchaseRequestStatus();
                    Session["PurchaseId"] = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.Id;
                    _presenter.SaveOrUpdateBidAnalysisRequest(_presenter.CurrentBidAnalysisRequest);
                   ShowPrint();
                    EnableControls();
                    if (ddlApprovalStatus.SelectedValue != "Rejected")
                    {
                        Master.ShowMessage(new AppMessage("Purchase  Approval Processed ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    else
                    {
                        Master.ShowMessage(new AppMessage("Purchase  Approval Rejected ", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    }
                    btnApprove.Enabled = false;
                    BindSearchPurchaseRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }

                BindBidAnalysisRequestforprint();
                btnApprove.Enabled = false;
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


                
            //}
            //catch (Exception ex)
            //{
            //    Master.ShowMessage(new AppMessage("There is an error approving Purchase Request", Chai.WorkflowManagment.Enums.RMessageType.Error));
            //}


        }
        private void BindBAAttachments()
        {
            if (_presenter.CurrentBidAnalysisRequest != null)
            {
                grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
                grvAttachments.DataBind();
            }
        }
        protected void grvPurchaseRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {

            _presenter.OnViewLoaded();
            PopApprovalStatus();
            grvAttachments.DataSource = _presenter.CurrentBidAnalysisRequest.BAAttachments;
            grvAttachments.DataBind();
            txtRejectedReason.Visible = false;
            lblRejectedReason.Enabled = false;
            BindPurchaseRequestStatus();
            BindBAAttachments();
            pnlApproval_ModalPopupExtender.Show();
            //ChangeBidAnalysisLink();

            Session["PurchaseId"] = _presenter.CurrentBidAnalysisRequest.Id;



          
           
          
           

        }
        private void ReturnFromBidAnalysis()
        {
            if (PnlStatus == "Enabled")
            {
                pnlApproval_ModalPopupExtender.Show();
                _presenter.OnViewLoaded();
                BindBAAttachments();
            }
        }
        private void BindPurchaseRequestDetails()
        {
            dgPurchaseRequestDetail.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders;
            dgPurchaseRequestDetail.DataBind();
        }
        protected void grvPurchaseRequestList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {


        }
        protected void grvPurchaseRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Button btnStatus = e.Row.FindControl("btnStatus") as Button;
            BidAnalysisRequest pr = e.Row.DataItem as BidAnalysisRequest;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (_presenter.GetUser(pr.CurrentApprover).EmployeePosition.PositionName == "Admin/HR Assisitance (Driver)" && pr.CurrentLevel == pr.BidAnalysisRequestStatuses.Count && pr.ProgressStatus == ProgressStatus.InProgress.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#112552");
                    
                   
                }
                else if (pr.ProgressStatus == ProgressStatus.InProgress.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF6C");

                }
                else if (pr.ProgressStatus == ProgressStatus.Completed.ToString())
                {
                    btnStatus.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF7251");

                }
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
            if (pr != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(pr.AppUser.Id).FullName;
                }
            }
        }
        protected void grvPurchaseRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPurchaseRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {

            BindSearchPurchaseRequestGrid();

            // pnlPopUpSearch_ModalPopupExtender.Show();
        }
        private void BindSearchPurchaseRequestGrid()
        {
            grvPurchaseRequestList.DataSource = _presenter.ListBidAnalysisRequests(txtRequestNosearch.Text, txtRequestDatesearch.Text, ddlProgressStatus.SelectedValue);
            grvPurchaseRequestList.DataBind();
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlApproval_ModalPopupExtender.Hide();
            Response.Redirect("frmPurchaseApproval.aspx");
            // pnlApproval.Visible = false;
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                txtRejectedReason.Visible = true;

            }
            pnlApproval_ModalPopupExtender.Show();
        }
        /*    
        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }
        private void EnableControls()
        {
            if (_presenter.CurrentBidAnalysisRequest.CurrentLevel == _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count)
            {
                btnPurchaseOrder.Enabled = true;
               // SendEmailToRequester();
                btnPrint0.Enabled = true;
            }
        }      
        protected void dgPurchaseRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        /*  protected void lnkbidanalysis_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("frmPurchaseApprovalDetail.aspx?PurchaseRequestId={0}", _presenter.CurrentBidAnalysisRequest.Id));
        }*/
        protected void grvPurchaseRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                reqid = (int)grvPurchaseRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                Session["ReqID"] = reqid;
                _presenter.CurrentBidAnalysisRequest = _presenter.GetBidAnalysisRequestById(reqid);
                if (e.CommandName == "ViewItem")
                {
                    reqid = Convert.ToInt32(grvPurchaseRequestList.DataKeys[0].Value);
                   
                    pnlDetail_ModalPopupExtender.Show();

                   
                    BindItemDetal();

                }
            }
        }
        #region Field Getters
      

   
        #endregion
        private void BindItemDetal()
        {
           // grvPurchaseRequestList.DataSource = _presenter.CurrentBidAnalysisRequest.Bidders;
            //grvPurchaseRequestList.DataBind();
            IList<BidderItemDetail> biddetail = new List<BidderItemDetail>();
            foreach (Bidder bider in _presenter.CurrentBidAnalysisRequest.Bidders)
            {
                foreach (BidderItemDetail biderdetail in bider.BidderItemDetails)
                {
                    biddetail.Add(biderdetail);
                }
               

            }
            dgPurchaseRequestDetail.DataSource = biddetail;
            dgPurchaseRequestDetail.DataBind();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        protected void btnPurchaseOrder_Click(object sender, EventArgs e)
        {
            int purchaseId = _presenter.CurrentBidAnalysisRequest.PurchaseRequest.Id;
                Response.Redirect(String.Format("frmPurchaseOrder.aspx?BidAnalysisRequestId={0}", purchaseId));
        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        private void PrintTransaction()
        {
            if (_presenter.CurrentBidAnalysisRequest.Id > 0)
            {
                pnlApproval_ModalPopupExtender.Hide();

                lblRequestNoResult.Text = _presenter.CurrentBidAnalysisRequest.RequestNo.ToString();
                lblRequestedDateResult.Text = _presenter.CurrentBidAnalysisRequest.RequestDate.ToString();
                lblRequesterResult.Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.AppUser.Id).FullName;
                lblCommentResult.Text = _presenter.CurrentBidAnalysisRequest.SelectedBy.ToString();
              //  lblApprovalStatusResult.Text = _presenter.CurrentBidAnalysisRequest.ProgressStatus.ToString();
                lblRequireddateofdeliveryResult.Text = _presenter.CurrentBidAnalysisRequest.SpecialNeed;
                lblTotalPriceResult.Text = _presenter.CurrentBidAnalysisRequest.TotalPrice.ToString();

                grvStatuses.DataSource = _presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses;
                grvStatuses.DataBind();
                IList<BidderItemDetail> biddetail = new List<BidderItemDetail>();
                foreach (Bidder bider in _presenter.CurrentBidAnalysisRequest.GetBidderbyRankone())
                {
                    
                    foreach (BidderItemDetail biderdetail in bider.BidderItemDetails)
                    {
                        //if (bider.Rank == 1)
                        //{
                            biddetail.Add(biderdetail);
                        //}
                        //else
                        //{
                        //    biddetail = null;
                        //}
                    }
                    
                   

                }

                dgPurchaseRequestDetail.DataSource = biddetail;
                dgPurchaseRequestDetail.DataBind();
            }
           // grvStatuses.DataSource = _presenter.CurrentPaymentReimbursementRequest.PaymentReimbursementRequestStatuses;
            //grvStatuses.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgPurchaseRequestDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item)
            //{
            //    foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.Bidders)
            //    {

            //        foreach (DataGridItem gvr in dgPurchaseRequestDetail.Items)
            //        {
            //            // String Rank = ((Label)gvr.FindControl("lblRank")).Text;
            //            Label Reason = (Label)gvr.FindControl("lblReason");
            //            Reason.Text = detail.GetSelectionReason();


            //        }
            //    }

            //}





        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (_presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver) != null)
                    {
                        e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBidAnalysisRequest.BidAnalysisRequestStatuses[e.Row.RowIndex].Approver).FullName;
                    }
                }
            }
        }
        protected void dgPurchaseRequestDetail_ItemCommand(object source, DataGridCommandEventArgs e)
        {
           
        }
        protected void dgPurchaseRequestDetail_ItemCreated(object sender, DataGridItemEventArgs e)
        {


            //foreach (Bidder detail in _presenter.CurrentBidAnalysisRequest.Bidders)
            //{
             
            //    foreach (DataGridItem gvr in dgPurchaseRequestDetail.Items)
            //    {
            //        String Rank = ((Label)gvr.FindControl("lblRank")).Text;
            //        String Reason = ((Label)gvr.FindControl("lblReason")).Text;
            //        if (Rank == "1" && e.Item.ItemType != ListItemType.Header)
            //        {
            //            Reason = detail.BidAnalysisRequest.ReasonforSelection;
            //        }
            //        else if (Rank != "1" && e.Item.ItemType != ListItemType.Header)
            //        {
            //            Reason = "";
            //        }
            //    }
            //}
        }
    }
}
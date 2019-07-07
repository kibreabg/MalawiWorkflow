using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class PurchaseOrderPresenter : Presenter<IPurchaseOrderView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Approval.ApprovalController _controller;
        private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private Chai.WorkflowManagment.Modules.Admin.AdminController _admincontroller;
        private BidAnalysisRequest _purchaserequest;
      
        public PurchaseOrderPresenter([CreateNew] Chai.WorkflowManagment.Modules.Approval.ApprovalController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller, [CreateNew] Chai.WorkflowManagment.Modules.Admin.AdminController admincontroller)
         {
             _controller = controller;
             _settingcontroller = settingcontroller;
             _admincontroller = admincontroller;
         }
    
         public override void OnViewLoaded()
         {
            
                 if (View.BidAnalysisRequestId > 0)
                 {
                     _controller.CurrentObject = _controller.GetBidAnalysisRequest(View.BidAnalysisRequestId);
                 }
                 CurrentBidAnalysisRequest = _controller.CurrentObject as BidAnalysisRequest;
            
         }
         public BidAnalysisRequest CurrentBidAnalysisRequest
         {
             get
             {
                 if (_purchaserequest == null)
                 {
                     int id = View.BidAnalysisRequestId;
                     if (id > 0)
                         _purchaserequest = _controller.GetBidAnalysisRequest(id);
                     else
                         _purchaserequest = new BidAnalysisRequest();
                 }
                 return _purchaserequest;
             }
             set { _purchaserequest = value; }
         }
        
         public override void OnViewInitialized()
         {
            
                 if (_purchaserequest == null)
                 {
                     int id = View.BidAnalysisRequestId;
                     if (id > 0)
                         _controller.CurrentObject = _controller.GetBidAnalysisRequest(id);
                     else
                         _controller.CurrentObject = new BidAnalysisRequest();
                 }
            
         }
         public IList<ItemAccount> GetItemAccounts()
         {
             return _settingcontroller.GetItemAccounts();

         }
         public ItemAccount GetItemAccount(int Id)
         {
             return _settingcontroller.GetItemAccount(Id);

         }
        
         public AppUser Approver(int Position)
         {
             return _controller.Approver(Position);
         }
         public AppUser GetUser(int UserId)
         {
             return _admincontroller.GetUser(UserId);
         }
         public void SaveOrUpdateBidAnalysisRequest(BidAnalysisRequest BidAnalysisRequest)
         {
             _controller.SaveOrUpdateEntity(BidAnalysisRequest);
         }
        
         
         public void CancelPage()
         {
             _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
         }

         public void DeletePurchaseRequest(PurchaseRequest PurchaseRequest)
         {
             _controller.DeleteEntity(PurchaseRequest);
         }
         public void DeleteSoleVendorRequest(SoleVendorRequest SoleVendorRequest)
         {
             _controller.DeleteEntity(SoleVendorRequest);
         }

         public BidAnalysisRequest GetPurchaseRequestById(int id)
         {
             return _controller.GetBidAnalysisRequest(id);
         }
      
         public ApprovalSetting GetApprovalSetting(string RequestType, int value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public IList<BidAnalysisRequest> ListPurchaseRequests(string requestNo, string RequestDate, string ProgressStatus)
         {
             return _controller.ListBidAnalysisRequests(requestNo, RequestDate, ProgressStatus);

         }
         
         public IList<Supplier> GetSuppliers()
         {
            return _settingcontroller.GetSuppliers();
         }
         public Supplier GetSupplier(int Id)
         {
             return _settingcontroller.GetSupplier(Id);
         }
         public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
         public void DeleteBidAnalysis(BidAnalysisRequest BidAnalysis)
         {
             _controller.DeleteEntity(BidAnalysis);
         }
       
         public void DeleteBidder(Bidder Bidder)
         {
             _controller.DeleteEntity(Bidder);
         }

         public int GetLastPurchaseOrderId()
         {
            return  _controller.GetLastPurchaseOrderId();
         }
         public void DeleteBidderItemDetail(BidderItemDetail BidderItemDetail)
         {
             _controller.DeleteEntity(BidderItemDetail);
         }
         public void Commit()
         {
             _controller.Commit();
         }
    }
}





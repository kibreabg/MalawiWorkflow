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
    public class PurchaseOrderSoleVendorPresenter : Presenter<IPurchaseOrderSoleVendorView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Approval.ApprovalController _controller;
        private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private Chai.WorkflowManagment.Modules.Admin.AdminController _admincontroller;
    
        private SoleVendorRequest _solevendorrequest;
        public PurchaseOrderSoleVendorPresenter([CreateNew] Chai.WorkflowManagment.Modules.Approval.ApprovalController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller, [CreateNew] Chai.WorkflowManagment.Modules.Admin.AdminController admincontroller)
         {
             _controller = controller;
             _settingcontroller = settingcontroller;
             _admincontroller = admincontroller;
         }
    
         public override void OnViewLoaded()
         {
            
                 if (View.SoleVendorRequestId > 0)
                 {
                     _controller.CurrentObject = _controller.GetSoleVendorRequest(View.SoleVendorRequestId);
                 }
                 CurrentSoleVendorRequest = _controller.CurrentObject as SoleVendorRequest;
            
         }
      
         public SoleVendorRequest CurrentSoleVendorRequest
         {
             get
             {
                 if (_solevendorrequest == null)
                 {
                     int id = View.SoleVendorRequestId;
                     if (id > 0)
                         _solevendorrequest = _controller.GetSoleVendorRequest(id);
                     else
                         _solevendorrequest = new SoleVendorRequest();
                 }
                 return _solevendorrequest;
             }
             set { _solevendorrequest = value; }
         }
         public override void OnViewInitialized()
         {
            
                 if (_solevendorrequest == null)
                 {
                     int id = View.SoleVendorRequestId;
                     if (id > 0)
                         _controller.CurrentObject = _controller.GetSoleVendorRequest(id);
                     else
                         _controller.CurrentObject = new SoleVendorRequest();
                
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
     
         public void SaveOrUpdateSoleVendorRequest(SoleVendorRequest SoleVendorRequest)
         {
             _controller.SaveOrUpdateEntity(SoleVendorRequest);
         }
         
         public void CancelPage()
         {
             _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
         }

      
         public void DeleteSoleVendorRequest(SoleVendorRequest SoleVendorRequest)
         {
             _controller.DeleteEntity(SoleVendorRequest);
         }

         public BidAnalysisRequest GetPurchaseRequestById(int id)
         {
             return _controller.GetBidAnalysisRequest(id);
         }
         public SoleVendorRequest GetSoleVendorRequestById(int id)
         {
             return _controller.GetSoleVendorRequest(id);
         }
         public ApprovalSetting GetApprovalSetting(string RequestType, int value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
        
         public IList<SoleVendorRequest> ListSoleVendorRequests(string requestNo, string RequestDate, string ProgressStatus)
         {
             return _controller.ListSoleVendorRequests(requestNo, RequestDate, ProgressStatus);

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
         public void DeleteSoleVendor(SoleVendorRequest SoleVendor)
         {
             _controller.DeleteEntity(SoleVendor);
         }
         public void DeleteBidder(Bidder Bidder)
         {
             _controller.DeleteEntity(Bidder);
         }
         public int GetLastPurchaseOrderSoleVendorId()
         {
            return  _controller.GetLastPurchaseOrderSoleVendorId();
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





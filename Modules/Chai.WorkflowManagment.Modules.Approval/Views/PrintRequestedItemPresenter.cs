using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public class PrintRequestedItemPresenter : Presenter<IPrintRequestedItemView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Approval.ApprovalController _controller;
        private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private Chai.WorkflowManagment.Modules.Admin.AdminController _admincontroller;
        private BidAnalysisRequest _bidanalysisrequest;
        public PrintRequestedItemPresenter([CreateNew] Chai.WorkflowManagment.Modules.Approval.ApprovalController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller, [CreateNew] Chai.WorkflowManagment.Modules.Admin.AdminController admincontroller)
         {
             _controller = controller;
             _settingcontroller = settingcontroller;
             _admincontroller = admincontroller;
         }
    
         public override void OnViewLoaded()
         {
             if (View.BidAnalysisRequestId > 0)
             {
                 _controller.CurrentObject = _controller.GetLeaveRequest(View.BidAnalysisRequestId);
             }
             CurrentBidAnalysisRequest = _controller.CurrentObject as BidAnalysisRequest;
         }
         public BidAnalysisRequest CurrentBidAnalysisRequest
         {
             get
             {
                 if (_bidanalysisrequest == null)
                 {
                     int id = View.BidAnalysisRequestId;
                     if (id > 0)
                         _bidanalysisrequest = _controller.GetBidAnalysisRequest(id);
                     else
                         _bidanalysisrequest = new BidAnalysisRequest();
                 }
                 return _bidanalysisrequest;
             }
             set { _bidanalysisrequest = value; }
         }
         public override void OnViewInitialized()
         {
             if (_bidanalysisrequest == null)
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
         public void SaveOrUpdatePurchaseRequest(PurchaseRequest PurchaseRequest)
         {
             _controller.SaveOrUpdateEntity(PurchaseRequest);
         }
         
         public void CancelPage()
         {
             _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
         }

         public void DeletePurchaseRequest(PurchaseRequest PurchaseRequest)
         {
             _controller.DeleteEntity(PurchaseRequest);
         }

         public BidAnalysisRequest GetBidAnalysisRequestById(int id)
         {
             return _controller.GetBidAnalysisRequest(id);
         }

         public ApprovalSetting GetApprovalSetting(string RequestType, int value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public IList<BidAnalysisRequest> ListBidAnalysisRequests(string requestNo, string RequestDate, string ProgressStatus)
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





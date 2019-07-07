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
    public class PurchaseApprovalDetailPresenter : Presenter<IPurchaseApprovalDetailView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Approval.ApprovalController _controller;
        private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
        private Chai.WorkflowManagment.Modules.Admin.AdminController _admincontroller;
        private PurchaseRequest _purchaserequest;
        public PurchaseApprovalDetailPresenter([CreateNew] Chai.WorkflowManagment.Modules.Approval.ApprovalController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller, [CreateNew] Chai.WorkflowManagment.Modules.Admin.AdminController admincontroller)
         {
             _controller = controller;
             _settingcontroller = settingcontroller;
             _admincontroller = admincontroller;
         }
    
         public override void OnViewLoaded()
         {
             if (View.PurchaseRequestId > 0)
             {
                 _controller.CurrentObject = _controller.GetPurchaseRequest(View.PurchaseRequestId);
             }
             CurrentPurchaseRequest = _controller.CurrentObject as PurchaseRequest;
         }
         public PurchaseRequest CurrentPurchaseRequest
         {
             get
             {
                 if (_purchaserequest == null)
                 {
                     int id = View.PurchaseRequestId;
                     if (id > 0)
                         _purchaserequest = _controller.GetPurchaseRequest(id);
                     else
                         _purchaserequest = new PurchaseRequest();
                 }
                 return _purchaserequest;
             }
             set { _purchaserequest = value; }
         }
         public override void OnViewInitialized()
         {
             if (_purchaserequest == null)
             {
                 int id = View.PurchaseRequestId;
                 if (id > 0)
                     _controller.CurrentObject = _controller.GetPurchaseRequest(id);
                 else
                     _controller.CurrentObject = new PurchaseRequest();
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
         public AssignJob GetAssignedJobbycurrentuser(int userId)
         {
             return _controller.GetAssignedJobbycurrentuser(userId);
         }
         public int GetAssignedUserbycurrentuser()
         {
             return _controller.GetAssignedUserbycurrentuser();
         }
         
         public void CancelPage()
         {
             _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
         }

         public void DeletePurchaseRequest(PurchaseRequest PurchaseRequest)
         {
             _controller.DeleteEntity(PurchaseRequest);
         }

         public PurchaseRequest GetPurchaseRequestById(int id)
         {
             return _controller.GetPurchaseRequest(id);
         }

         public ApprovalSetting GetApprovalSetting(string RequestType, int value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public IList<PurchaseRequest> ListPurchaseRequests(string requestNo, string RequestDate, string ProgressStatus)
         {
             return _controller.ListPurchaseRequests(requestNo, RequestDate, ProgressStatus);

         }
         public IList<Supplier> GetSuppliers()
         {
            return _settingcontroller.GetSuppliers();
         }
         public IList<Supplier> GetSuppliers(int SupplierTypeId)
         {
             return _settingcontroller.GetSuppliers(SupplierTypeId);
         }
         public IList<SupplierType> GetSupplierTypes()
         {
             return _settingcontroller.GetSupplierTypes();
         }
         public Supplier GetSupplier(int Id)
         {
             return _settingcontroller.GetSupplier(Id);
         }
         public SupplierType GetSupplierType(int Id)
         {
             return _settingcontroller.GetSupplierType(Id);
         }
         public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
        
        
         public void Commit()
         {
             _controller.Commit();
         }
    }
}





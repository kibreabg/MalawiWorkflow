using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Request;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class PurchaseRequestPresenter : Presenter<IPurchaseRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
         private Chai.WorkflowManagment.Modules.Request.RequestController _controller;
         private Chai.WorkflowManagment.Modules.Setting.SettingController _settingcontroller;
         private PurchaseRequest _purchaserequest;
         public PurchaseRequestPresenter([CreateNew] Chai.WorkflowManagment.Modules.Request.RequestController controller, [CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController settingcontroller)
         {
         		_controller = controller;
                _settingcontroller = settingcontroller;
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
         public IList<PurchaseRequest> GetPurchaseRequests()
         {
             return _controller.GetPurchaseRequests();
         }
        
         public AppUser Approver(int Position)
         {
             return _controller.Approver(Position);
         }
         public AppUser GetUser(int UserId)
         {
             return _controller.GetSuperviser(UserId);
         }
         public AppUser GetSuperviser(int superviser)
         {
             return _controller.GetSuperviser(superviser);
         }
         public void SaveOrUpdateLeavePurchase(PurchaseRequest PurchaseRequest)
         {
             _controller.SaveOrUpdateEntity(PurchaseRequest);
         }
         public int GetLastPurchaseRequestId()
         {
             return _controller.GetLastPurchaseRequestId();
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

         public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforProcess(RequestType, value);
         }
         public ApprovalSetting GetApprovalSettingforPurchaseProcess(string RequestType, decimal value)
         {
             return _settingcontroller.GetApprovalSettingforPurchaseProcess(RequestType, value);
         }
         public AssignJob GetAssignedJobbycurrentuser()
         {
             return _controller.GetAssignedJobbycurrentuser();
         }
         public AssignJob GetAssignedJobbycurrentuser(int UserId)
         {
             return _controller.GetAssignedJobbycurrentuser(UserId);
         }
         public IList<PurchaseRequest> ListPurchaseRequests(string requestNo,string RequestDate)
         {
             return _controller.ListPurchaseRequests(requestNo, RequestDate);

         }
         public IList<ItemAccount> GetItemAccounts()
         {
             return _settingcontroller.GetItemAccounts();

         }
         public ItemAccount GetItemAccount(int Id)
         {
             return _settingcontroller.GetItemAccount(Id);

         }
         
         public IList<Project> GetProjects()
         {
             return _settingcontroller.GetProjects();

         }
         public Project GetProject(int Id)
         {
             return _settingcontroller.GetProject(Id);

         }
         public IList<Grant> GetGrants()
         {
             return _settingcontroller.GetGrants();

         }
       
         public Grant GetGrant(int Id)
         {
             return _settingcontroller.GetGrant(Id);

         }
         public PurchaseRequestDetail GetPurchaseRequestDetail(int Id)
         {
             return _controller.GetPurchaseRequestDetail(Id);

         }
         public AppUser CurrentUser()
         {
             return _controller.GetCurrentUser();
         }
         public void DeletePurchaseRequestDetail(PurchaseRequestDetail PurchaseRequestDetail)
         {
             _controller.DeleteEntity(PurchaseRequestDetail);
         }
         public void Commit()
         {
             _controller.Commit();
         }

         public IList<Grant> GetGrantbyprojectId(int projectId)
         {
             return _settingcontroller.GetProjectGrantsByprojectId(projectId);

         }
    }
}





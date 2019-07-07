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
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class BidAnalysisRequestPresenter : Presenter<IBidAnalysisRequestView>
    {

        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private BidAnalysisRequest _bidAnalysisRequest;
        private decimal Totalamount = 0;
        public BidAnalysisRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetBARequestId > 0)
            {
                _controller.CurrentObject = _controller.GetBidAnalysisRequest(View.GetBARequestId);
            }
            CurrentBidAnalysisRequest = _controller.CurrentObject as BidAnalysisRequest;
        }

        public override void OnViewInitialized()
        {
            if (_bidAnalysisRequest == null)
            {
                int id = View.GetBidAnalysisRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetBidAnalysisRequest(id);
                else
                    _controller.CurrentObject = new BidAnalysisRequest();
            }
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _settingController.GetItemAccounts();

        }


        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AppUser GetUser(int UserId)
        {
            return _adminController.GetUser(UserId);
        }
        public BidAnalysisRequest CurrentBidAnalysisRequest
        {
            get
            {
                if (_bidAnalysisRequest == null)
                {
                    int id = View.GetBidAnalysisRequestId;
                    if (id > 0)
                        _bidAnalysisRequest = _controller.GetBidAnalysisRequest(id);
                    else
                        _bidAnalysisRequest = new BidAnalysisRequest();
                }
                return _bidAnalysisRequest;
            }
            set
            {
                _bidAnalysisRequest = value;
            }
        }
        public IList<BidAnalysisRequest> GetBidAnalysisRequests()
        {
            return _controller.GetBidAnalysisRequests();
        }
        public int GetLastBidAnalysisRequestId()
        {
            return _controller.GetLastBidAnalysisRequestId();
        }
        private void SaveBidAnalysisRequestStatus()
        {
            if (GetApprovalSetting(RequestType.Bid_Analysis_Request.ToString().Replace('_', ' '), CurrentBidAnalysisRequest.TotalPrice) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.Bid_Analysis_Request.ToString().Replace('_', ' '), CurrentBidAnalysisRequest.TotalPrice).ApprovalLevels)
                {
                    BidAnalysisRequestStatus BARS = new BidAnalysisRequestStatus();
                    BARS.BidAnalysisRequest = CurrentBidAnalysisRequest;
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            BARS.Approver = CurrentUser().Superviser.Value;
                        else

                            BARS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentBidAnalysisRequest.Project.Id != 0)
                        {
                            BARS.Approver = GetProject(CurrentBidAnalysisRequest.Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                            BARS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else

                            BARS.Approver = 0;
                    }
                    BARS.WorkflowLevel = i;
                    i++;
                    CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Add(BARS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentBidAnalysisRequest.BidAnalysisRequestStatuses != null)
            {
                foreach (BidAnalysisRequestStatus SVRS in CurrentBidAnalysisRequest.BidAnalysisRequestStatuses)
                {
                    if (SVRS.ApprovalStatus == null)
                    {
                        SendEmail(SVRS);
                        CurrentBidAnalysisRequest.CurrentApprover = SVRS.Approver;
                        CurrentBidAnalysisRequest.CurrentLevel = SVRS.WorkflowLevel;
                        CurrentBidAnalysisRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateBidAnalysisRequest()
        {
            BidAnalysisRequest BidAnalysisRequest = CurrentBidAnalysisRequest;
            BidAnalysisRequest.PurchaseRequest = _controller.GetPurchaseRequest(View.GetPurchaseRequestId); 
            BidAnalysisRequest.RequestNo = View.GetRequestNo;
            BidAnalysisRequest.RequestDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            BidAnalysisRequest.AnalyzedDate = Convert.ToDateTime(View.GetAnalysedDate.ToShortDateString());
            BidAnalysisRequest.PaymentMethod = View.GetPaymentMethod;
            BidAnalysisRequest.SpecialNeed = View.GetSpecialNeed;


            //  BidAnalysisRequest.Supplier.Id=View.GetSupplierId;
             BidAnalysisRequest.ReasonforSelection = View.GetReasonForSelection;
            //   BidAnalysisRequest.SelectedBy = View.GetSelectedBy;

            BidAnalysisRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
            if (View.GetProjectId != 0)
                BidAnalysisRequest.Project = _settingController.GetProject(View.GetProjectId);
            if (View.GetGrantId != 0)
                BidAnalysisRequest.Grant = _settingController.GetGrant(View.GetGrantId);
            BidAnalysisRequest.AppUser = _adminController.GetUser(CurrentUser().Id);

            decimal price = 0;
            foreach (Bidder bider in CurrentBidAnalysisRequest.Bidders)
            {


                if (CurrentBidAnalysisRequest.GetBidderbyRank().Rank == 1)
                {

                    foreach (BidderItemDetail biditemdet in bider.BidderItemDetails)
                    {

                        price = price + biditemdet.TotalCost;
                    }
                }
               BidAnalysisRequest.TotalPrice  = price;
                break;
            }
          
            SaveBidAnalysisRequestStatus();
            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(BidAnalysisRequest);
            _controller.CurrentObject = null;
            if (CurrentBidAnalysisRequest.BidAnalysisRequestStatuses.Count == 0 && CurrentBidAnalysisRequest.GetBidderbyRank().Rank == 1)
                foreach (Bidder bider in CurrentBidAnalysisRequest.Bidders)
                {




                    foreach (BidderItemDetail detail in bider.BidderItemDetails)
                    {
                        Totalamount = Totalamount + detail.TotalCost;
                    }

                }

        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteBidAnalysisRequest(BidAnalysisRequest BidAnalysisRequest)
        {
            _controller.DeleteEntity(BidAnalysisRequest);
        }
        public BidAnalysisRequest GetBidAnalysisRequest(int id)
        {
            return _controller.GetBidAnalysisRequest(id);
        }
        public IList<BidAnalysisRequest> ListBidAnalysisRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListBidAnalysisRequests(RequestNo, RequestDate);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            return _controller.GetAssignedJobbycurrentuser(UserId);
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public IList<Project> GetProjects()
        {
            return _settingController.GetProjects();
        }
        public IList<Grant> GetGrants()
        {
            return _settingController.GetGrants();
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public Grant GetGrantprojectId(int projectId)
        {
            return _settingController.GetGrant(projectId);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, decimal value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(BidAnalysisRequestStatus SVRS)
        {
            if (GetSuperviser(SVRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(SVRS.Approver).Email, "Bid Analysis Request", (CurrentBidAnalysisRequest.AppUser.FullName).ToUpper() + "' Request for Bid Analysis No '" + (CurrentBidAnalysisRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(SVRS.Approver).AssignedTo).Email, "Bid Analysis Request", (CurrentBidAnalysisRequest.AppUser.FullName).ToUpper() + "' Request  for Bid Analysis");
            }
        }
        public IList<Supplier> GetSuppliers()

        {
            return _settingController.GetSuppliers();
        }
        public IList<Supplier> GetSuppliers(int SupplierTypeId)
        {
            return _settingController.GetSuppliers(SupplierTypeId);
        }
        public IList<SupplierType> GetSupplierTypes()
        {
            return _settingController.GetSupplierTypes();
        }
        public Supplier GetSupplier(int Id)
        {
            return _settingController.GetSupplier(Id);
        }
        public SupplierType GetSupplierType(int Id)
        {
            return _settingController.GetSupplierType(Id);
        }
        public ItemAccount GetItemAccount(int Id)
        {
            return _settingController.GetItemAccount(Id);
        }

        public BidderItemDetail GetBiderItemDet(int id)
        {
            return _controller.GetBiderItem(id);
        }
        public PurchaseRequest GetPurchaseRequest(int purchaseRequestId)
        {
            return _controller.GetPurchaseRequest(purchaseRequestId);
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





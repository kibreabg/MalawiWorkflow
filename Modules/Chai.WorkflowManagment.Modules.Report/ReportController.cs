using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Report;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Report
{
    public class ReportController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public ReportController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        #region Objectreport
        public IList<LeaveReport> GetLeaveReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT AppUsers.FirstName + ' ' +  AppUsers.LastName as StaffName, " +
                               " [Forward] as Leave_Days_Opening_Balance, " +
                               " LeaveTypes.LeaveTypeName as Leave_Type, " +
                               " CAST(LeaveRequests.DateFrom as nvarchar) + ' ' + CAST(LeaveRequests.DateTo as nvarchar) as Period_Leave_Taken, " +
                               " RequestedDays as Number_of_Leave_Taken , " +
                               " [Balance] as Leave_Balance_Carried_Forward " +
                               " FROM [WorkflowManagment].[dbo].[LeaveRequests] Inner join AppUsers on AppUsers.Id = [LeaveRequests].Requester " +
                               " Inner Join LeaveTypes on LeaveTypes.Id = [LeaveRequests].LeaveType_Id " +
                               " where 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When LeaveRequests.RequestedDate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END AND " +
                               " [LeaveRequests].ProgressStatus = 'Completed' " ;

            return WorkspaceFactory.CreateReadOnly().Queryable<LeaveReport>(filterExpression).ToList();

        }

        public IList<PurchaseReport> GetPurchaseReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT PODate as Date,RequestNo as Purchase_Request_Ref,PoNumber as Purchase_Order, ItemAccounts.AccountCode,ItemAccounts.AccountName as Description,PurchaseOrderDetails.TotalCost,Projects.ProjectCode as Project_ID ,Grants.GrantCode as Grant_ID " +
                                                 " From  [WorkflowManagment].[dbo].PurchaseRequests  " +
                                                 " Inner join PurchaseOrders on PurchaseOrders.Id = PurchaseRequests.Id " +
                                                 " Inner join PurchaseRequestDetails on PurchaseRequestDetails.PurchaseRequest_Id = PurchaseRequests.Id " +
                                                 " Inner Join PurchaseOrderDetails on PurchaseOrderDetails.PurchaseOrder_Id = PurchaseOrders.Id " +
                                                 " Inner Join ItemAccounts on ItemAccounts.Id = PurchaseOrderDetails.ItemAccount_Id " +
                                                 " Inner Join Projects on Projects.Id = PurchaseRequestDetails.Project_Id " +
                                                 " Inner Join Grants on Grants.Id = PurchaseRequestDetails.Grant_Id " +
                                                 " where PurchaseRequests.ProgressStatus = 'Completed' AND " +
                                                 " 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When PurchaseOrders.PODate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END ";
            return WorkspaceFactory.CreateReadOnly().Queryable<PurchaseReport>(filterExpression).ToList();

        }
        public IList<VehicleReport> GetVehicleReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT VehicleRequests.RequestDate as Date,AppUsers.FirstName + ' ' + AppUsers.LastName as Name_of_Requester,VehicleRequests.DepartureDate,VehicleRequests.ReturningDate,Driver.FirstName + '' + Driver.LastName as Driver,CarRentals.Name as Car_Rental,VehicleRequests.RequestNo as Vehicle_Reg_Number,VehicleRequests.NoOfPassengers " +
                               " From  [WorkflowManagment].[dbo].VehicleRequests " +
                                                 " Inner join AppUsers on AppUsers.Id = VehicleRequests.AppUser_Id " +
                                                 " Inner join VehicleRequestStatuses on VehicleRequests.Id = VehicleRequestStatuses.VehicleRequest_Id " +
                                                 " Left Join AppUsers as Driver on Driver.Id =VehicleRequestStatuses.AppUser_Id " +
                                                 " Left Join CarRentals on CarRentals.Id = VehicleRequestStatuses.CarRental_Id " +
                                                 " where VehicleRequests.ProgressStatus = 'Completed' AND " +
                                                 " 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When VehicleRequests.RequestDate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END ";
            return WorkspaceFactory.CreateReadOnly().Queryable<VehicleReport>(filterExpression).ToList();

        }
        public IList<LiquidationReport> GetLiquidationReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT ExpenseLiquidationRequests.RequestDate as Date,TravelAdvanceRequests.TravelAdvanceNo as Travel_ID , ItemAccounts.AccountName as Description,ItemAccounts.AccountCode,Projects.ProjectCode as Project_ID,Grants.GrantCode as Grant_ID " +
                               " From  [WorkflowManagment].[dbo].TravelAdvanceRequests " +
                                               "  Inner join ExpenseLiquidationRequests on TravelAdvanceRequests.Id = ExpenseLiquidationRequests.TravelAdvanceRequest_Id " +
                                               "   Inner join TravelAdvanceRequestDetails on TravelAdvanceRequests.Id = TravelAdvanceRequestDetails.TravelAdvanceRequest_Id " +
                                               "  Inner join TravelAdvanceCosts on TravelAdvanceRequestDetails.Id = TravelAdvanceCosts.TravelAdvanceRequestDetail_Id " +
                                               " Inner Join ItemAccounts on ItemAccounts.Id= TravelAdvanceCosts.ItemAccount_Id " +
                                               "   Inner join Projects on Projects.Id = TravelAdvanceRequests.Project_Id " +
                                               "  Inner join Grants on Grants.Id = TravelAdvanceRequests.Grant_Id " +
                                               "  where ExpenseLiquidationRequests.ProgressStatus = 'Completed' AND " +
                                               " 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When ExpenseLiquidationRequests.RequestDate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END ";
            return WorkspaceFactory.CreateReadOnly().Queryable<LiquidationReport>(filterExpression).ToList();

        }
        public IList<TravelAdvanceReport> GetTravelAdvanceReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT ExpenseLiquidationRequests.RequestDate as Date,TravelAdvanceRequests.TravelAdvanceNo as Travel_ID , ItemAccounts.AccountName as Description,ItemAccounts.AccountCode,Projects.ProjectCode as Project_ID,Grants.GrantCode as Grant_ID " +
                               " From  [WorkflowManagment].[dbo].TravelAdvanceRequests " +
                                               "  Inner join TravelAdvanceRequestDetails on TravelAdvanceRequests.Id = TravelAdvanceRequestDetails.TravelAdvanceRequest_Id " +
                                               "  Inner join TravelAdvanceCosts on TravelAdvanceRequestDetails.Id = TravelAdvanceCosts.TravelAdvanceRequestDetail_Id " +
                                               "  Inner Join ItemAccounts on ItemAccounts.Id= TravelAdvanceCosts.ItemAccount_Id " +
                                               "  Inner join Projects on Projects.Id = TravelAdvanceRequests.Project_Id " +
                                               "  Inner join Grants on Grants.Id = TravelAdvanceRequests.Grant_Id " +
                                               "  where ExpenseLiquidationRequests.ProgressStatus = 'Completed' AND " +
                                               " 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When TravelAdvanceRequests.RequestDate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END ";
            return WorkspaceFactory.CreateReadOnly().Queryable<TravelAdvanceReport>(filterExpression).ToList();

        }
        public IList<CashPaymentReport> GetCashPaymentReporto(string DateFrom, string DateTo)
        {
            string filterExpression = "";

            filterExpression = " SELECT CashPaymentRequests.RequestDate as Date,CashPaymentRequests.RequestNo as Reference_No ,ItemAccounts.AccountName as Description,ItemAccounts.AccountCode,Projects.ProjectCode as Project_ID,Grants.GrantCode as Grant_ID " +
                               " From  [WorkflowManagment].[dbo].CashPaymentRequests " +
                                               "  Inner join CashPaymentRequestDetails on CashPaymentRequests.Id = CashPaymentRequestDetails.CashPaymentRequest_Id " +
                                               "   Inner Join ItemAccounts on ItemAccounts.Id= CashPaymentRequestDetails.ItemAccount_Id " +
                                               "   Inner join Projects on Projects.Id = CashPaymentRequestDetails.Project_Id " +
                                               "   Inner join Grants on Grants.Id = CashPaymentRequestDetails.Grant_Id " +
                                               "   where CashPaymentRequests.ProgressStatus = 'Completed' AND " +
                                               " 1 = Case when '" + DateFrom + "' = '' and  '" + DateTo + "' ='' Then 1 When CashPaymentRequests.RequestDate between '" + DateFrom + "' and '" + DateTo + "' Then 1 END ";
            return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentReport>(filterExpression).ToList();

        }
        #endregion
        public DataSet GetLeaveReport(int EmployeeName, int LeaveType)
        {
            ReportDao re = new ReportDao();
            return re.LeaveReport(EmployeeName, LeaveType);
        }
        public DataSet GetPurchaseReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.PurchaseReport(datefrom, dateto);
        }
        public DataSet GetVehicleReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.VehicleReport(datefrom, dateto);
        }
        public DataSet GetLiquidationReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.LiquidationReport(datefrom, dateto);
        }
        public DataSet GetTravelAdvancenReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.TravelAdvanceReport(datefrom, dateto);
        }
        public DataSet GetCashPaymentReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.CashPaymentReport(datefrom, dateto);
        }
        public DataSet GetBankPaymentReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.BankPaymentPaymentReport(datefrom, dateto);
        }
        public DataSet GetCostSharingPaymentReport(string datefrom, string dateto)
        {
            ReportDao re = new ReportDao();
            return re.CostSharingPaymentReport(datefrom, dateto);
        }
        public DataSet ExportBankPayment(string datefrom, string dateto,string ExportType)
        {
            ReportDao re = new ReportDao();
            return re.ExportBankPayment(datefrom, dateto, ExportType);
        }
        public DataSet ExportCashPayment(string datefrom, string dateto, string ExportType)
        {
            ReportDao re = new ReportDao();
            return re.ExportCashPayment(datefrom, dateto, ExportType);
        }
        public DataSet ExportCostSharingPayment(string datefrom, string dateto, string ExportType)
        {
            ReportDao re = new ReportDao();
            return re.ExportCostSharingPayment(datefrom, dateto, ExportType);
        }
        public DataSet ExportTravelAdvance(string datefrom, string dateto, string ExportType)
        {
            ReportDao re = new ReportDao();
            return re.ExportTravelAdvance(datefrom, dateto, ExportType);
        }
        public CashPaymentRequest GetCashPaymentRequest(string RequestId)
        {
            return _workspace.Single<CashPaymentRequest>(x => x.RequestNo == RequestId);
        }
        public CostSharingRequest GetCostSharingRequest(string RequestId)
        {
            return _workspace.Single<CostSharingRequest>(x => x.RequestNo == RequestId);
        }
        public TravelAdvanceRequest GetTravelAdvanceRequestRequest(string RequestId)
        {
            return _workspace.Single<TravelAdvanceRequest>(x => x.TravelAdvanceNo == RequestId);
        }
        public OperationalControlRequest GetOperationalControlRequest(string RequestId)
        {
            return _workspace.Single<OperationalControlRequest>(x => x.RequestNo == RequestId);
        }
        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}

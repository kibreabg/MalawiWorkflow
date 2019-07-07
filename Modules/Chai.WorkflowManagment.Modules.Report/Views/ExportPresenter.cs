using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Report;
using System.Data;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public class ExportPresenter : Presenter<IExportView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Report.ReportController _controller;
        public ExportPresenter([CreateNew] Chai.WorkflowManagment.Modules.Report.ReportController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        public DataSet ExportBankPayment(string DateFrom, string DateTo, string ExportType)
        {
            return _controller.ExportBankPayment(DateFrom, DateTo,ExportType);
        }
        public DataSet ExportCashPayment(string DateFrom, string DateTo,string ExportType)
        {
            return _controller.ExportCashPayment(DateFrom, DateTo, ExportType);
        }
        public CashPaymentRequest GetCashPaymentRequestRequest(string RequestId)
        {
            return _controller.GetCashPaymentRequest(RequestId);
        }
        public CostSharingRequest GetCostSharingRequest(string RequestId)
        {
            return _controller.GetCostSharingRequest(RequestId);
        }
        public TravelAdvanceRequest GetTravelAdvanceRequestRequest(string RequestId)
        {
            return _controller.GetTravelAdvanceRequestRequest(RequestId);
        }
        public OperationalControlRequest GetOperationalControlRequest(string RequestId)
        {
            return _controller.GetOperationalControlRequest(RequestId);
        }
        public void UpdateCashPaymentRequestExportStatus(CashPaymentRequest CashPaymentRequest)
        {
            _controller.SaveOrUpdateEntity(CashPaymentRequest);
        }
        public void UpdateCostSharingPaymentRequestExportStatus(CostSharingRequest CostSharingRequest)
        {
            _controller.SaveOrUpdateEntity(CostSharingRequest);
        }
        public void UpdateTravelAdvanceRequestExportStatus(TravelAdvanceRequest TravelAdvanceRequest)
        {
            _controller.SaveOrUpdateEntity(TravelAdvanceRequest);
        }
        public void UpdateOperationalRequestExportStatus(OperationalControlRequest OperationalControlRequest)
        {
            _controller.SaveOrUpdateEntity(OperationalControlRequest);
        }
        public DataSet ExportCostSharingPayment(string DateFrom, string DateTo,string ExportType)
        {
            return _controller.ExportCostSharingPayment(DateFrom, DateTo, ExportType);
        }
        public DataSet ExportTravelAdvance(string DateFrom, string DateTo, string ExportType)
        {
            return _controller.ExportTravelAdvance(DateFrom, DateTo, ExportType);
        }
        // TODO: Handle other view events and set state in the view
    }
}





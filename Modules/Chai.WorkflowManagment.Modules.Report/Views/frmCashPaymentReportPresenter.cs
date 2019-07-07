using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Report;
using System.Data;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public class frmCashPaymentReportPresenter : Presenter<IfrmCashPaymentReportView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Report.ReportController _controller;
        public frmCashPaymentReportPresenter([CreateNew] Chai.WorkflowManagment.Modules.Report.ReportController controller)
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
        public IList<CashPaymentReport> GetCashPaymentReporto(string DateFrom, string DateTo)
        {
           return _controller.GetCashPaymentReporto(DateFrom, DateTo);
        }
        public DataSet GetCashPaymentReport(string DateFrom, string DateTo)
        {
            return _controller.GetCashPaymentReport(DateFrom, DateTo);
        }
        // TODO: Handle other view events and set state in the view
    }
}





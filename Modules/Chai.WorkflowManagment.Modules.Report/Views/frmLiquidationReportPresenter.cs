using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Report;
using System.Data;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public class frmLiquidationReportPresenter : Presenter<IfrmLiquidationReportView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Report.ReportController _controller;
        public frmLiquidationReportPresenter([CreateNew] Chai.WorkflowManagment.Modules.Report.ReportController controller)
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
        public IList<LiquidationReport> GetLiquidationReporto(string DateFrom,string DateTo)
        {
            return _controller.GetLiquidationReporto(DateFrom, DateTo);
        }
        public DataSet GetLiquidationReport(string DateFrom, string DateTo)
        {
            return _controller.GetLiquidationReport(DateFrom, DateTo);
        }
        // TODO: Handle other view events and set state in the view
    }
}





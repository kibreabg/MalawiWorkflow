using System;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmLeaveReport : POCBasePage, IfrmLeaveReportView
	{
		private frmLeaveReportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                PopUsers();
                PopLeaveType();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
        public frmLeaveReportPresenter Presenter
		{
			get
			{
				return this._presenter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");

				this._presenter = value;
				this._presenter.View = this;
			}
		}
        public override string PageID
        {
            get
            {
                return "{8FD3CC34-9213-497B-A316-F59D40EADB44}";
            }
        }
        private void PopUsers()
        {
            ddlEmployeeName.DataSource = _presenter.GetAllUsers();
            ddlEmployeeName.DataBind();
        }
        private void PopLeaveType()
        {
            ddlLeaveType.DataSource = _presenter.GetLeaveTypes();
            ddlLeaveType.DataBind();

        }
        private void ViewLeaveReport()
        {

            var path = Server.MapPath("LeaveReport.rdlc");
            var datasource = _presenter.GetLeaveReport(Convert.ToInt32(ddlEmployeeName.SelectedValue), Convert.ToInt32(ddlLeaveType.SelectedValue));
            ReportDataSource s = new ReportDataSource("DataSet1", datasource.Tables[0]);
            ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(s);
            ReportViewer1.LocalReport.ReportPath = path;
            var EmployeeName = ddlEmployeeName.SelectedValue != "" ? ddlEmployeeName.SelectedValue : " ";
            var LeaveType = ddlLeaveType.SelectedValue != "" ? ddlLeaveType.SelectedValue : " ";
            var param4 = new ReportParameter("EmployeeName", EmployeeName);
            var param5 = new ReportParameter("LeaveType", LeaveType);
            var parameters = new List<ReportParameter>();
            
            parameters.Add(param4);
            parameters.Add(param5);
            ReportViewer1.LocalReport.SetParameters(parameters);


        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            ViewLeaveReport();
        }
}
}


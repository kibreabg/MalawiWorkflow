using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Data;
using OfficeOpenXml;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmExportTraveladvance : POCBasePage, IExportView
	{
		private ExportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                grvTravelAdvanceRequestList.DataSource = _presenter.ExportTravelAdvance(txtDateFrom.Text, txtDateTo.Text,ddlExportType.SelectedValue);
                grvTravelAdvanceRequestList.DataBind();
			}
			this._presenter.OnViewLoaded();
            
		}

		[CreateNew]
        public ExportPresenter Presenter
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
                return "{5E178312-553F-480B-9A3B-9FF7F30F2182}";
            }
        }
        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            grvTravelAdvanceRequestList.DataSource = _presenter.ExportTravelAdvance(txtDateFrom.Text, txtDateTo.Text, ddlExportType.SelectedValue);
            grvTravelAdvanceRequestList.DataBind();
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            try
            {
               

                dt1 = _presenter.ExportTravelAdvance(txtDateFrom.Text, txtDateTo.Text, ddlExportType.SelectedValue).Tables[0];

                // mySqlDataAdapter.Fill(dt1);

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Travel Advance Payment");


                    ws.Cells["A1"].LoadFromDataTable(dt1, true);


                    //Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Travel Advance Data.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.Flush();
                    UpdateExportStatus(dt1);
                    Response.End();
                }
                UpdateExportStatus(dt1);
            }
            catch (Exception ex)
            {
                UpdateExportStatus(dt1);
            }
        }
        private void UpdateExportStatus(DataTable dt)
        {
            foreach (DataRow DR in dt.Rows)
            {
                
              TravelAdvanceRequest Request =   _presenter.GetTravelAdvanceRequestRequest(DR.Field<string>("RefNumber"));
              Request.ExportStatus = "Exported";
              _presenter.UpdateTravelAdvanceRequestExportStatus(Request);
            }
        }
}
}


using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Data;
using OfficeOpenXml;
using Chai.WorkflowManagment.CoreDomain.Requests;

namespace Chai.WorkflowManagment.Modules.Report.Views
{
    public partial class frmExportBankPayment : POCBasePage, IExportView
	{
		private ExportPresenter _presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                grvBankPaymentRequestList.DataSource = _presenter.ExportBankPayment(txtDateFrom.Text, txtDateTo.Text, ddlExportType.SelectedValue);
                grvBankPaymentRequestList.DataBind();
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
                return "{228BA9DE-58B1-438B-B3A2-2FC8A96757D4}";
            }
        }
        

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx");
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            grvBankPaymentRequestList.DataSource = _presenter.ExportBankPayment(txtDateFrom.Text, txtDateTo.Text, ddlExportType.SelectedValue);
            grvBankPaymentRequestList.DataBind();
        }
        
        private void UpdateExportStatus(DataTable dt)
        {
            foreach (DataRow DR in dt.Rows)
            {
             OperationalControlRequest Request =  _presenter.GetOperationalControlRequest(DR.Field<string>("RefNumber"));
             Request.ExportStatus = "Exported";
             _presenter.UpdateOperationalRequestExportStatus(Request);
            }
        }
        protected void btnExport_Click1(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            try
            {
                

                dt1 = _presenter.ExportBankPayment(txtDateFrom.Text, txtDateTo.Text, ddlExportType.SelectedValue).Tables[0];

                // mySqlDataAdapter.Fill(dt1);

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Bank Payment");


                    ws.Cells["A1"].LoadFromDataTable(dt1, true);


                    //Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Bank Payment Data .xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.Flush();
                    UpdateExportStatus(dt1);
                    Response.End();
                }
               
            }
            catch (Exception ex)
            {
               // UpdateExportStatus(dt1);
            }
        }
}
}


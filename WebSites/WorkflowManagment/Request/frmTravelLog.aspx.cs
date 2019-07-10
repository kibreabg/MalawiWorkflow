using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.TravelLogs;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmTravelLog : POCBasePage, ITravelLogView
    {
        private TravelLogPresenter _presenter;
        private IList<TravelLog> _TravelLogs;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindTravelLogs();

            }

            this._presenter.OnViewLoaded();
            BindVehicleRequest();
        }

        [CreateNew]
        public TravelLogPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        public override string PageID
        {
            get
            {
                return "{79FFA014-4057-4C71-9519-FF519DA2E2E2}";
            }
        }

        #region Field Getters
        public int GetRequestId
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["requestId"]))
                    return Convert.ToInt32(Request.QueryString["requestId"]);
                else
                    return 0;
            }
        }
        public IList<TravelLog> TravelLogs
        {
            get
            {
                return _TravelLogs;
            }
            set
            {
                _TravelLogs = value;
            }
        }
        #endregion
        void BindTravelLogs()
        {
            dgTravelLog.DataSource = _presenter.ListTravelLogs(GetRequestId);
            dgTravelLog.DataBind();
        }
        protected void dgTravelLog_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgTravelLog.EditItemIndex = -1;
        }
        protected void dgTravelLog_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgTravelLog.DataKeys[e.Item.ItemIndex];
            TravelLog TravelLog = _presenter.GetTravelLogById(id);
            try
            {
                //LinkButton lnkbtn = e.Item.FindControl("lnkDelete") as LinkButton;
                //if (lnkbtn != null)
                //    lnkbtn.Attributes.Add("OnClientClick", "javascript:return confirm('Are you sure you want to delete Supplier?');");
                _presenter.DeleteTravelLog(TravelLog);
                BindTravelLogs();

                Master.ShowMessage(new AppMessage("Travel Log was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Travel Log. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgTravelLog_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            TravelLog TravelLog = new TravelLog();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    //TextBox txtRequestNo = e.Item.FindControl("txtRequestNo") as TextBox;
                    //TravelLog.RequestNo = Convert.ToInt32(txtRequestNo.Text);
                    TextBox txtDeparturePlace = e.Item.FindControl("txtDeparturePlace") as TextBox;
                    TravelLog.DeparturePlace = txtDeparturePlace.Text;
                    TextBox txtArrivalPlace = e.Item.FindControl("txtArrivalPlace") as TextBox;
                    TravelLog.ArrivalPlace = txtArrivalPlace.Text;
                    TextBox txtDepartureTime = e.Item.FindControl("txtDepartureTime") as TextBox;
                    TravelLog.DepartureTime = Convert.ToDateTime(txtDepartureTime.Text);
                    TextBox txtArrivalTime = e.Item.FindControl("txtArrivalTime") as TextBox;
                    TravelLog.ArrivalTime = Convert.ToDateTime(txtArrivalTime.Text);
                    TextBox txtStartKmReading = e.Item.FindControl("txtStartKmReading") as TextBox;
                    TravelLog.StartKmReading = Convert.ToDecimal(txtStartKmReading.Text);
                    TextBox txtEndKmReading = e.Item.FindControl("txtEndKmReading") as TextBox;
                    TravelLog.EndKmReading = Convert.ToDecimal(txtEndKmReading.Text);
                    TextBox txtFuelPrice = e.Item.FindControl("txtFuelPrice") as TextBox;
                    TravelLog.FuelPrice = Convert.ToInt32(txtFuelPrice.Text);

                    int VehicleRequestId = Convert.ToInt32(Request.QueryString["requestId"]);

                    SaveTravelLog(TravelLog, VehicleRequestId);
                    dgTravelLog.EditItemIndex = -1;
                    BindTravelLogs();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Travel Log " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveTravelLog(TravelLog TravelLog, int VehicleRequestId)
        {
            try
            {
                if (TravelLog.Id <= 0)
                {
                    _presenter.SaveOrUpdateTravelLog(TravelLog, VehicleRequestId);
                    Master.ShowMessage(new AppMessage("Travel Log saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateTravelLog(TravelLog, VehicleRequestId);
                    Master.ShowMessage(new AppMessage("Travel Log Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgTravelLog_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgTravelLog.EditItemIndex = e.Item.ItemIndex;
            BindTravelLogs();
        }
        protected void dgTravelLog_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgTravelLog_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgTravelLog.DataKeys[e.Item.ItemIndex];
            TravelLog TravelLog = _presenter.GetTravelLogById(id);

            try
            {
                //TextBox txtRequestNo = e.Item.FindControl("txtRequestNo") as TextBox;
                //TravelLog.RequestNo = Convert.ToInt32(txtRequestNo.Text);
                TextBox txtDeparturePlace = e.Item.FindControl("txtEdtDeparturePlace") as TextBox;
                TravelLog.DeparturePlace = txtDeparturePlace.Text;
                TextBox txtArrivalPlace = e.Item.FindControl("txtEdtArrivalPlace") as TextBox;
                TravelLog.ArrivalPlace = txtArrivalPlace.Text;
                TextBox txtDepartureTime = e.Item.FindControl("txtEdtDepartureTime") as TextBox;
                TravelLog.DepartureTime = Convert.ToDateTime(txtDepartureTime.Text);
                TextBox txtArrivalTime = e.Item.FindControl("txtEdtArrivalTime") as TextBox;
                TravelLog.ArrivalTime = Convert.ToDateTime(txtArrivalTime.Text);
                TextBox txtStartKmReading = e.Item.FindControl("txtEdtStartKmReading") as TextBox;
                TravelLog.StartKmReading = Convert.ToDecimal(txtStartKmReading.Text);
                TextBox txtEndKmReading = e.Item.FindControl("txtEdtEndKmReading") as TextBox;
                TravelLog.EndKmReading = Convert.ToDecimal(txtEndKmReading.Text);
                TextBox txtFuelPrice = e.Item.FindControl("txtEdtFuelPrice") as TextBox;
                TravelLog.FuelPrice = Convert.ToInt32(txtFuelPrice.Text);

                int VehicleRequestId = Convert.ToInt32(Request.QueryString["requestId"]);

                SaveTravelLog(TravelLog, VehicleRequestId);
                dgTravelLog.EditItemIndex = -1;
                BindTravelLogs();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Travel Log. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        private void BindVehicleRequest()
        {
            VehicleRequest VR = _presenter.GetVehicleRequest(GetRequestId);

            lblRequestedDateResult.Text = VR.RequestDate.ToString();
            lblRequestNoResult.Text = VR.RequestNo.ToString();
            lblRequesterResult.Text = VR.AppUser.FullName;// _presenter.GetUser(VR.AppUser.Id).FullName;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmVehicle : POCBasePage, IVehicleView
    {
        private VehiclePresenter _presenter;
        private IList<Vehicle> _Vehicles;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindVehicles();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public VehiclePresenter Presenter
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
                return "{7B51A5EE-B7D6-449B-ABE6-BD6DBE0C337E}";
            }
        }

        #region Field Getters
        public string GetPlateNo
        {
            get { return txtSrchVehicleName.Text; }
        }
        public IList<Vehicle> Vehicles
        {
            get
            {
                return _Vehicles;
            }
            set
            {
                _Vehicles = value;
            }
        }
        #endregion
        void BindVehicles()
        {
            dgVehicle.DataSource = _presenter.ListVehicles(GetPlateNo);
            dgVehicle.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //_presenter.ListVehicles(GetName);
            BindVehicles();
        }
        protected void dgVehicle_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgVehicle.EditItemIndex = -1;
        }
        protected void dgVehicle_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgVehicle.DataKeys[e.Item.ItemIndex];
            Vehicle Vehicle = _presenter.GetVehicleById(id);
            try
            {
                _presenter.DeleteVehicle(Vehicle);
                BindVehicles();

                Master.ShowMessage(new AppMessage("Vehicle was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Vehicle. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgVehicle_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Vehicle Vehicle = new Vehicle();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtPlateNo = e.Item.FindControl("txtPlateNo") as TextBox;
                    Vehicle.PlateNo = txtPlateNo.Text;
                    DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    Vehicle.Status = ddlFStatus.SelectedValue;
                    Vehicle.Status = "Active";
                    SaveVehicle(Vehicle);
                    dgVehicle.EditItemIndex = -1;
                    BindVehicles();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Vehicle " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveVehicle(Vehicle Vehicle)
        {
            try
            {
                if (Vehicle.Id <= 0)
                {
                    _presenter.SaveOrUpdateVehicle(Vehicle);
                    Master.ShowMessage(new AppMessage("Vehicle saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateVehicle(Vehicle);
                    Master.ShowMessage(new AppMessage("Vehicle Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgVehicle_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgVehicle.EditItemIndex = e.Item.ItemIndex;

            BindVehicles();
        }
        protected void dgVehicle_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgVehicle_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgVehicle.DataKeys[e.Item.ItemIndex];
            Vehicle Vehicle = _presenter.GetVehicleById(id);

            try
            {
                TextBox txtEdtPlateNo = e.Item.FindControl("txtEdtPlateNo") as TextBox;
                Vehicle.PlateNo = txtEdtPlateNo.Text;
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                Vehicle.Status = ddlStatus.SelectedValue;
                
                SaveVehicle(Vehicle);
                dgVehicle.EditItemIndex = -1;
                BindVehicles();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Vehicle. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }        
    }
}
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
    public partial class frmCarRental : POCBasePage, ICarRentalView
    {
        private CarRentalPresenter _presenter;
        private IList<CarRental> _CarRentals;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindCarRentals();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public CarRentalPresenter Presenter
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
                return "{94C874DF-15C7-4978-A8C7-80221EE83AAD}";
            }
        }

        #region Field Getters
        public string GetName
        {
            get { return txtSrchCarRentalName.Text; }
        }
        public IList<CarRental> CarRentals
        {
            get
            {
                return _CarRentals;
            }
            set
            {
                _CarRentals = value;
            }
        }
        #endregion
        void BindCarRentals()
        {
            dgCarRental.DataSource = _presenter.ListCarRentals(GetName);
            dgCarRental.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //_presenter.ListCarRentals(GetName);
            BindCarRentals();
        }
        protected void dgCarRental_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCarRental.EditItemIndex = -1;
        }
        protected void dgCarRental_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgCarRental.DataKeys[e.Item.ItemIndex];
            CarRental CarRental = _presenter.GetCarRentalById(id);
            try
            {
                CarRental.Status = "InActive";
                _presenter.SaveOrUpdateCarRental(CarRental);
                
                BindCarRentals();

                Master.ShowMessage(new AppMessage("Car Rental was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Car Rental. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgCarRental_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            CarRental CarRental = new CarRental();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtName = e.Item.FindControl("txtCarRentalName") as TextBox;
                    CarRental.Name = txtName.Text;
                    TextBox txtPhone = e.Item.FindControl("txtPhoneNo") as TextBox;
                    CarRental.PhoneNo = txtPhone.Text;
                    TextBox txtContact = e.Item.FindControl("txtContactAddress") as TextBox;
                    CarRental.ContactAddress = txtContact.Text;
                    SaveCarRental(CarRental);
                    dgCarRental.EditItemIndex = -1;
                    BindCarRentals();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Car Rental " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveCarRental(CarRental CarRental)
        {
            try
            {
                if (CarRental.Id <= 0)
                {
                    _presenter.SaveOrUpdateCarRental(CarRental);
                    Master.ShowMessage(new AppMessage("Car Rental saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateCarRental(CarRental);
                    Master.ShowMessage(new AppMessage("Car Rental Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgCarRental_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgCarRental.EditItemIndex = e.Item.ItemIndex;

            BindCarRentals();
        }
        protected void dgCarRental_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgCarRental_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgCarRental.DataKeys[e.Item.ItemIndex];
            CarRental CarRental = _presenter.GetCarRentalById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtEdtCarRentalName") as TextBox;
                CarRental.Name = txtName.Text;
                TextBox txtPhone = e.Item.FindControl("txtEdtPhoneNo") as TextBox;
                CarRental.PhoneNo = txtPhone.Text;
                TextBox txtContact = e.Item.FindControl("txtEdtContactAddress") as TextBox;
                CarRental.ContactAddress = txtContact.Text;
                CarRental.Status = "Active";
                SaveCarRental(CarRental);
                dgCarRental.EditItemIndex = -1;
                BindCarRentals();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Car Rental. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }        
    }
}
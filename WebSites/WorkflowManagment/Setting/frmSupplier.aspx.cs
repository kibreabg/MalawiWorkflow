using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Microsoft.Practices.ObjectBuilder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public partial class frmSupplier : POCBasePage, ISupplierView
    {
        private SupplierPresenter _presenter;
        private IList<Supplier> _Suppliers;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindSupplier();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public SupplierPresenter Presenter
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
                return "{8E5FAA6E-E9E9-4B21-888D-11539FA9BBA8}";
            }
        }

        void BindSupplier()
        {
            dgSupplier.DataSource = _presenter.ListSuppliers(txtSupplierName.Text);
            dgSupplier.DataBind();
        }
        private void BindSupplierTypes(DropDownList ddlSupplierTypes)
        {
            if (ddlSupplierTypes != null)
            {
                ddlSupplierTypes.DataSource = _presenter.GetSupplierTypes();
                ddlSupplierTypes.DataValueField = "Id";
                ddlSupplierTypes.DataTextField = "SupplierTypeName";
                ddlSupplierTypes.DataBind();
            }
        }

        #region interface
        public IList<CoreDomain.Setting.Supplier> Supplier
        {
            get
            {
                return _Suppliers;
            }
            set
            {
                _Suppliers = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListSuppliers(SupplierName);
            BindSupplier();
        }
        protected void dgSupplier_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplier.EditItemIndex = -1;
        }
        protected void dgSupplier_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSupplier.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Supplier Supplier = _presenter.GetSupplierById(id);
            try
            {
                Supplier.Status = "InActive";
                _presenter.SaveOrUpdateSupplier(Supplier);
                
                BindSupplier();

                Master.ShowMessage(new AppMessage("Supplier was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Supplier. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgSupplier_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.Supplier supplier = new Chai.WorkflowManagment.CoreDomain.Setting.Supplier();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    DropDownList ddlSuppliertype = e.Item.FindControl("ddlSupplierType") as DropDownList;
                    supplier.SupplierType = _presenter.GetSupplierTypeById(Convert.ToInt32(ddlSuppliertype.SelectedValue));
                    TextBox txtFSupplierName = e.Item.FindControl("txtFSupplierName") as TextBox;
                    supplier.SupplierName = txtFSupplierName.Text;
                    TextBox txtFSupplierAddress = e.Item.FindControl("txtFSupplierAddress") as TextBox;
                    supplier.SupplierAddress = txtFSupplierAddress.Text;
                    TextBox txtFSupplierContact = e.Item.FindControl("txtFSupplierContact") as TextBox;
                    supplier.SupplierContact = txtFSupplierContact.Text;
                    TextBox txtFSupplierphoneContact = e.Item.FindControl("txtFSupplierphoneContact") as TextBox;
                    supplier.ContactPhone = txtFSupplierphoneContact.Text;
                    TextBox txtFSupplierEmail = e.Item.FindControl("txtFSupplierEmail") as TextBox;
                    supplier.Email = txtFSupplierEmail.Text;
                    supplier.Status = "Active";
                    SaveSupplier(supplier);
                    dgSupplier.EditItemIndex = -1;
                    BindSupplier();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Supplier " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveSupplier(Chai.WorkflowManagment.CoreDomain.Setting.Supplier Supplier)
        {
            try
            {
                if (Supplier.Id <= 0)
                {
                    _presenter.SaveOrUpdateSupplier(Supplier);
                    Master.ShowMessage(new AppMessage("Supplier saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateSupplier(Supplier);
                    Master.ShowMessage(new AppMessage("Supplier Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgSupplier_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplier.EditItemIndex = e.Item.ItemIndex;

            BindSupplier();
        }
        protected void dgSupplier_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlSupplierType = e.Item.FindControl("ddlSupplierType") as DropDownList;
                BindSupplierTypes(ddlSupplierType);
            }
            else
            {
               
                    DropDownList ddlEdtSupplierType = e.Item.FindControl("ddlEdtSupplierType") as DropDownList;
                    BindSupplierTypes(ddlEdtSupplierType);
               
                    
                
            }
        }
        protected void dgSupplier_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgSupplier.DataKeys[e.Item.ItemIndex];
            Supplier Supplier = _presenter.GetSupplierById(id);

            try
            {
                DropDownList ddlSuppliertype = e.Item.FindControl("ddledtSupplierType") as DropDownList;
                Supplier.SupplierType = _presenter.GetSupplierTypeById(Convert.ToInt32(ddlSuppliertype.SelectedValue));
                TextBox txtName = e.Item.FindControl("txtSupplierName") as TextBox;
                Supplier.SupplierName = txtName.Text;
                TextBox txtSupplierAddress = e.Item.FindControl("txtSupplierAddress") as TextBox;
                Supplier.SupplierAddress = txtSupplierAddress.Text;
                TextBox txtSupplierContact = e.Item.FindControl("txtSupplierContact") as TextBox;
                Supplier.SupplierContact = txtSupplierContact.Text;
                TextBox txtSupplierphoneContact = e.Item.FindControl("txtSupplierphoneContact") as TextBox;
                Supplier.ContactPhone = txtSupplierphoneContact.Text;
                TextBox txtFSupplierEmail = e.Item.FindControl("txtSupplierEmail") as TextBox;
                Supplier.Email = txtFSupplierEmail.Text;
                SaveSupplier(Supplier);
                dgSupplier.EditItemIndex = -1;
                BindSupplier();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Supplier. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


        public string SupplierName
        {
            get { return txtSupplierName.Text; }
        }

    }
}
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
    public partial class frmSupplierType : POCBasePage, ISupplierTypeView
    {
        private SupplierTypePresenter _presenter;
        private IList<SupplierType> _SupplierTypes;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindSupplierType();
            }
            
            this._presenter.OnViewLoaded();           

        }

        [CreateNew]
        public SupplierTypePresenter Presenter
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
                return "{C89D5298-958A-4808-9213-0462754C2A9E}";
            }
        }
        public string SupplierTypeEmail
        {
            get { return txtSupplierTypeEmail.Text; }
        }
        
        #region interface
        public IList<SupplierType> SupplierType
        {
            get
            {
                return _SupplierTypes;
            }
            set
            {
                _SupplierTypes = value;
            }
        }
        #endregion
        void BindSupplierType()
        {
            dgSupplierType.DataSource = _presenter.ListSupplierTypes(SupplierTypeEmail);
            dgSupplierType.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            dgSupplierType.DataSource = _presenter.ListSupplierTypes(SupplierTypeEmail);
            dgSupplierType.DataBind();
        }
        protected void dgSupplierType_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplierType.EditItemIndex = -1;
        }
        protected void dgSupplierType_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgSupplierType.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.SupplierType SupplierType = _presenter.GetSupplierTypeById(id);
            try
            {
                SupplierType.Status = "InActive";
                _presenter.SaveOrUpdateSupplierType(SupplierType);
                
                BindSupplierType();

                Master.ShowMessage(new AppMessage("Supplier Type was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Supplier Type. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgSupplierType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            SupplierType sType = new SupplierType();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtSupplierTypeName = e.Item.FindControl("txtSupplierTypeName") as TextBox;
                    sType.SupplierTypeName = txtSupplierTypeName.Text;
                    sType.Status = "Active";

                    SaveSupplierType(sType);
                    dgSupplierType.EditItemIndex = -1;
                    BindSupplierType();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Supplier Type " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveSupplierType(SupplierType SupplierType)
        {
            try
            {
                if(SupplierType.Id  <= 0)
                {
                    _presenter.SaveOrUpdateSupplierType(SupplierType);
                    Master.ShowMessage(new AppMessage("Supplier Type Saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateSupplierType(SupplierType);
                    Master.ShowMessage(new AppMessage("Supplier Type Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgSupplierType_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgSupplierType.EditItemIndex = e.Item.ItemIndex;

            BindSupplierType();
        }
        protected void dgSupplierType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgSupplierType_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgSupplierType.DataKeys[e.Item.ItemIndex];
            SupplierType SupplierType = _presenter.GetSupplierTypeById(id);

            try
            {
                TextBox txtEdtSupplierTypeName = e.Item.FindControl("txtEdtSupplierTypeName") as TextBox;
                SupplierType.SupplierTypeName = txtEdtSupplierTypeName.Text;
                

                SaveSupplierType(SupplierType);
                dgSupplierType.EditItemIndex = -1;
                BindSupplierType();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update SupplierType. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
    }
}
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
    public partial class frmExpenseType : POCBasePage, IExpenseTypeView
    {
        private ExpenseTypePresenter _presenter;
        private IList<ExpenseType> _ExpenseTypes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindExpenseType();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public ExpenseTypePresenter Presenter
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
                return "{990C732F-25EE-4287-A093-423026F33676}";
            }
        }

        void BindExpenseType()
        {
            dgExpenseType.DataSource = _presenter.ListExpenseTypes();
            dgExpenseType.DataBind();
        }
        #region interface


        public IList<CoreDomain.Setting.ExpenseType> ExpenseType
        {
            get
            {
                return _ExpenseTypes;
            }
            set
            {
                _ExpenseTypes = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListExpenseTypes();
            BindExpenseType();
        }
        protected void dgExpenseType_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgExpenseType.EditItemIndex = -1;
        }
        protected void dgExpenseType_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgExpenseType.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.ExpenseType ExpenseType = _presenter.GetExpenseTypeById(id);
            try
            {
                ExpenseType.Status = "InActive";
                _presenter.SaveOrUpdateExpenseType(ExpenseType);
                
                BindExpenseType();

                Master.ShowMessage(new AppMessage("Expense Type was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Expense Type. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgExpenseType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.ExpenseType ExpenseType = new Chai.WorkflowManagment.CoreDomain.Setting.ExpenseType();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFExpenseTypeName = e.Item.FindControl("txtFExpenseTypeName") as TextBox;
                    ExpenseType.ExpenseTypeName = txtFExpenseTypeName.Text;
                    ExpenseType.Status = "Active";

                    SaveExpenseType(ExpenseType);
                    dgExpenseType.EditItemIndex = -1;
                    BindExpenseType();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Expense Type " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveExpenseType(Chai.WorkflowManagment.CoreDomain.Setting.ExpenseType ExpenseType)
        {
            try
            {
                if(ExpenseType.Id  <= 0)
                {
                    _presenter.SaveOrUpdateExpenseType(ExpenseType);
                    Master.ShowMessage(new AppMessage("Expense Type saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateExpenseType(ExpenseType);
                    Master.ShowMessage(new AppMessage("Expense Type Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgExpenseType_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgExpenseType.EditItemIndex = e.Item.ItemIndex;

            BindExpenseType();
        }
        protected void dgExpenseType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgExpenseType_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgExpenseType.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.ExpenseType ExpenseType = _presenter.GetExpenseTypeById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtExpenseTypeName") as TextBox;
                ExpenseType.ExpenseTypeName = txtName.Text;
                SaveExpenseType(ExpenseType);
                dgExpenseType.EditItemIndex = -1;
                BindExpenseType();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Expense Type. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        } 


     
    }
}
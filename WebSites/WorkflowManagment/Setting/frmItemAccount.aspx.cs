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
    public partial class frmItemAccount : POCBasePage, IItemAccountView
    {
        private ItemAccountPresenter _presenter;
        private IList<ItemAccount> _ItemAccounts;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindItemAccount();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public ItemAccountPresenter Presenter
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
                return "{969246EF-65E7-4E32-87A9-86B481C365B1}";
            }
        }

        void BindItemAccount()
        {
            dgItemAccount.DataSource = _presenter.ListItemAccounts(txtItemAccountName.Text,txtItemAccountCode.Text);
            dgItemAccount.DataBind();
        }
        #region interface
        

        public IList<CoreDomain.Setting.ItemAccount> ItemAccount
        {
            get
            {
                return _ItemAccounts;
            }
            set
            {
                _ItemAccounts = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListItemAccounts(ItemAccountName,ItemAccountCode);
            BindItemAccount();
        }
        protected void dgItemAccount_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemAccount.EditItemIndex = -1;
        }
        protected void dgItemAccount_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgItemAccount.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.ItemAccount ItemAccount = _presenter.GetItemAccountById(id);
            try
            {
                ItemAccount.Status = "InActive";
                _presenter.SaveOrUpdateItemAccount(ItemAccount);
                BindItemAccount();

                Master.ShowMessage(new AppMessage("Item Account was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Item Account. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgItemAccount_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.ItemAccount ItemAccount = new Chai.WorkflowManagment.CoreDomain.Setting.ItemAccount();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFItemAccountName = e.Item.FindControl("txtFItemAccountName") as TextBox;
                    ItemAccount.AccountName = txtFItemAccountName.Text;
                    TextBox txtFItemAccountCode = e.Item.FindControl("txtFItemAccountCode") as TextBox;
                    ItemAccount.AccountCode = txtFItemAccountCode.Text;
                    ItemAccount.Status = "Active";

                    SaveItemAccount(ItemAccount);
                    dgItemAccount.EditItemIndex = -1;
                    BindItemAccount();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Item Account " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveItemAccount(Chai.WorkflowManagment.CoreDomain.Setting.ItemAccount ItemAccount)
        {
            try
            {
                if(ItemAccount.Id  <= 0)
                {
                    _presenter.SaveOrUpdateItemAccount(ItemAccount);
                    Master.ShowMessage(new AppMessage("Item Account saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateItemAccount(ItemAccount);
                    Master.ShowMessage(new AppMessage("Item Account Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgItemAccount_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgItemAccount.EditItemIndex = e.Item.ItemIndex;

            BindItemAccount();
        }
        protected void dgItemAccount_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgItemAccount_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgItemAccount.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.ItemAccount ItemAccount = _presenter.GetItemAccountById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtItemAccountName") as TextBox;
                ItemAccount.AccountName = txtName.Text;
                TextBox txtCode = e.Item.FindControl("txtItemAccountCode") as TextBox;
                ItemAccount.AccountCode = txtCode.Text;
                SaveItemAccount(ItemAccount);
                dgItemAccount.EditItemIndex = -1;
                BindItemAccount();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Item Account. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


        public string ItemAccountName
        {
            get { return txtItemAccountName.Text; }
        }

        public string ItemAccountCode
        {
            get { return txtItemAccountCode.Text; }
        }
    }
}
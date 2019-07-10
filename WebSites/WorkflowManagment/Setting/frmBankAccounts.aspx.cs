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
    public partial class frmBankAccount : POCBasePage, IBankAccountView
    {
        private BankAccountPresenter _presenter;
        private IList<Account> _BankAccounts;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindBankAccounts();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public BankAccountPresenter Presenter
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
                return "{9F2504F1-4BC8-4945-BAFA-0ADFB417E390}";
            }
        }

        #region Field Getters
        public string GetName
        {
            get { return txtSrchBankAccountName.Text; }
        }
        public IList<Account> BankAccounts
        {
            get
            {
                return _BankAccounts;
            }
            set
            {
                _BankAccounts = value;
            }
        }
        #endregion
        void BindBankAccounts()
        {
            dgBankAccount.DataSource = _presenter.ListBankAccounts(GetName);
            dgBankAccount.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //_presenter.ListBankAccounts(GetName);
            BindBankAccounts();
        }
        protected void dgBankAccount_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBankAccount.EditItemIndex = -1;
        }
        protected void dgBankAccount_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBankAccount.DataKeys[e.Item.ItemIndex];
            Account BankAccount = _presenter.GetBankAccountById(id);
            try
            {
                 BankAccount.Status = "InActive";
                _presenter.SaveOrUpdateBankAccount(BankAccount);
                BindBankAccounts();

                Master.ShowMessage(new AppMessage("Bank Account was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Bank Account. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBankAccount_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Account BankAccount = new Account();
            if (e.CommandName == "AddNew")
            {
                try
                {
                    TextBox txtName = e.Item.FindControl("txtBankAccountName") as TextBox;
                    BankAccount.Name = txtName.Text;
                    TextBox txtAccountNo = e.Item.FindControl("txtAccountNo") as TextBox;
                    BankAccount.AccountNo = txtAccountNo.Text;
                    BankAccount.Status = "Active";
                    SaveBankAccount(BankAccount);
                    dgBankAccount.EditItemIndex = -1;
                    BindBankAccounts();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Bank Account " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveBankAccount(Account BankAccount)
        {
            try
            {
                if (BankAccount.Id <= 0)
                {
                    _presenter.SaveOrUpdateBankAccount(BankAccount);
                    Master.ShowMessage(new AppMessage("Bank Account Saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateBankAccount(BankAccount);
                    Master.ShowMessage(new AppMessage("Bank Account Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgBankAccount_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBankAccount.EditItemIndex = e.Item.ItemIndex;
            BindBankAccounts();
        }
        protected void dgBankAccount_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgBankAccount_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgBankAccount.DataKeys[e.Item.ItemIndex];
            Account BankAccount = _presenter.GetBankAccountById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtEdtBankAccountName") as TextBox;
                BankAccount.Name = txtName.Text;
                TextBox txtAccountNo = e.Item.FindControl("txtEdtAccountNo") as TextBox;
                BankAccount.AccountNo = txtAccountNo.Text;
                SaveBankAccount(BankAccount);
                dgBankAccount.EditItemIndex = -1;
                BindBankAccounts();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Bank Account. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }        
    }
}
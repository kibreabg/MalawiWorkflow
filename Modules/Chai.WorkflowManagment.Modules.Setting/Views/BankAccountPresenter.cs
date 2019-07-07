using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class BankAccountPresenter : Presenter<IBankAccountView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public BankAccountPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }
        public override void OnViewLoaded()
        {
            View.BankAccounts = _controller.ListBankAccounts(View.GetName);
        }
        public override void OnViewInitialized()
        {
            
        }
        public IList<Account> GetBankAccounts()
        {
            return _controller.GetAccounts();
        }
        public void SaveOrUpdateBankAccount(Account BankAccount)
        {
            _controller.SaveOrUpdateEntity(BankAccount);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteBankAccount(Account BankAccount)
        {
            _controller.DeleteEntity(BankAccount);
        }
        public Account GetBankAccountById(int id)
        {
            return _controller.GetAccount(id);
        }
        public IList<Account> ListBankAccounts(string BankAccountName)
        {
            return _controller.ListBankAccounts(BankAccountName);          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





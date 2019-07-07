using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ItemAccountPresenter : Presenter<IItemAccountView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public ItemAccountPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.ItemAccount = _controller.ListItemAccounts(View.ItemAccountName, View.ItemAccountCode);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _controller.GetItemAccounts();
        }

        public void SaveOrUpdateItemAccount(ItemAccount ItemAccount)
        {
            _controller.SaveOrUpdateEntity(ItemAccount);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteItemAccount(ItemAccount ItemAccount)
        {
            _controller.DeleteEntity(ItemAccount);
        }
        public ItemAccount GetItemAccountById(int id)
        {
            return _controller.GetItemAccount(id);
        }

        public IList<ItemAccount> ListItemAccounts(string ItemAccountName, string ItemAccountCode)
        {
            return _controller.ListItemAccounts(ItemAccountName, ItemAccountCode);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





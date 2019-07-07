using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ExpenseTypePresenter : Presenter<IExpenseTypeView>
    {

        
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public ExpenseTypePresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.ExpenseType = _controller.GetExpenseTypes();
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<ExpenseType> GetExpenseTypes()
        {
            return _controller.GetExpenseTypes();
        }

        public void SaveOrUpdateExpenseType(ExpenseType ExpenseType)
        {
            _controller.SaveOrUpdateEntity(ExpenseType);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteExpenseType(ExpenseType ExpenseType)
        {
            _controller.DeleteEntity(ExpenseType);
        }
        public ExpenseType GetExpenseTypeById(int id)
        {
            return _controller.GetExpenseType(id);
        }

        public IList<ExpenseType> ListExpenseTypes()
        {
            return _controller.ListExpenseTypes();
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





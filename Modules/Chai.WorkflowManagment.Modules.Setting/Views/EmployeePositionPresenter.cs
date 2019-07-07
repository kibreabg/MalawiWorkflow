using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class EmployeePositionPresenter : Presenter<IEmployeePositionView>
    {

        
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        public EmployeePositionPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.EmployeePosition = _controller.GetEmployeePositions();
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<EmployeePosition> GetEmployeePositions()
        {
            return _controller.GetEmployeePositions();
        }

        public void SaveOrUpdateEmployeePosition(EmployeePosition EmployeePosition)
        {
            _controller.SaveOrUpdateEntity(EmployeePosition);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteEmployeePosition(EmployeePosition EmployeePosition)
        {
            _controller.DeleteEntity(EmployeePosition);
        }
        public EmployeePosition GetEmployeePositionById(int id)
        {
            return _controller.GetEmployeePosition(id);
        }

        public IList<EmployeePosition> ListEmployeePositions()
        {
            return _controller.ListEmployeePositions();
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





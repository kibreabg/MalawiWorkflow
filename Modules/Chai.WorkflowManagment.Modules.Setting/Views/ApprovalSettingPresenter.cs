using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class ApprovalSettingPresenter : Presenter<IApprovalSettingView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private Chai.WorkflowManagment.Modules.Setting.SettingController _controller;
        private ApprovalSetting _approvalsetting;
        public ApprovalSettingPresenter([CreateNew] Chai.WorkflowManagment.Modules.Setting.SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            if (View.ApprovalSettingId > 0)
            {
                _controller.CurrentObject = _controller.GetApprovalSetting(View.ApprovalSettingId);
            }
            CurrentApprovalSetting = _controller.CurrentObject as ApprovalSetting;
        }
        public ApprovalSetting CurrentApprovalSetting
        {
            get
            {
                if (_approvalsetting == null)
                {
                    int id = View.ApprovalSettingId;
                    if (id > 0)
                        _approvalsetting = _controller.GetApprovalSetting(id);
                    else
                        _approvalsetting = new ApprovalSetting();
                }
                return _approvalsetting;
            }
            set { _approvalsetting = value; }
        }
        public override void OnViewInitialized()
        {
            if (_approvalsetting == null)
            {
                int id = View.ApprovalSettingId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetApprovalSetting(id);
                else
                    _controller.CurrentObject = new ApprovalSetting();
            }
        }
        public IList<ApprovalSetting> GetApprovalSettings()
        {
            return _controller.GetApprovalSettings();
        }

        public void SaveOrUpdateApprovalSetting(ApprovalSetting ApprovalSetting)
        {
            _controller.SaveOrUpdateEntity(ApprovalSetting);
        }
        public ApprovalLevel GetApprovalLevel(int ApprovalLevelId)
        {
            return _controller.GetApprovalLevel(ApprovalLevelId);
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteApprovalSetting(ApprovalSetting approvalsetting)
        {
            _controller.DeleteEntity(approvalsetting);
        }
        public void DeleteApprovalLevel(ApprovalLevel approvallevel)
        {
            _controller.DeleteEntity(approvallevel);
        }
        public ApprovalSetting GetApprovalSettingById(int id)
        {
            return _controller.GetApprovalSetting(id);
        }

        public IList<ApprovalSetting> ListApprovalSettings(string RequestType)
        {
            return _controller.ListApprovalSettings(RequestType);
          
        }
        public IList<EmployeePosition> ListEmployeePosition()
        {
            return _controller.GetEmployeePositions();
        
        }
        public EmployeePosition GetEmployeePosition(int EmployeePositionId)
        {
            return _controller.GetEmployeePosition(EmployeePositionId);
           
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public class BeneficiaryPresenter : Presenter<IBeneficiaryView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private SettingController _controller;
        public BeneficiaryPresenter([CreateNew] SettingController controller)
        {
            _controller = controller;
        }

        public override void OnViewLoaded()
        {
            View.Beneficiaries = _controller.ListBeneficiaries(View.GetName);
        }

        public override void OnViewInitialized()
        {
            
        }
        public IList<Beneficiary> GetBeneficiaries()
        {
            return _controller.GetBeneficiaries();
        }

        public void SaveOrUpdateBeneficiary(Beneficiary beneficiary)
        {
            _controller.SaveOrUpdateEntity(beneficiary);
        }

        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Setting/Default.aspx?{0}=3", AppConstants.TABID));
        }

        public void DeleteBeneficiary(Beneficiary beneficiary)
        {
            _controller.DeleteEntity(beneficiary);
        }
        public Beneficiary GetBeneficiaryById(int id)
        {
            return _controller.GetBeneficiary(id);
        }

        public IList<Beneficiary> ListBeneficiaries(string BeneficiaryName)
        {
            return _controller.ListBeneficiaries(BeneficiaryName);
          
        }
        public void Commit()
        {
            _controller.Commit();
        }
    }
}





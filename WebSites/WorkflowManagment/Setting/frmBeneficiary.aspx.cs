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
    public partial class frmBeneficiary : POCBasePage, IBeneficiaryView
    {
        private BeneficiaryPresenter _presenter;
        private IList<Beneficiary> _Beneficiaries;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindBeneficiaries();
            }

            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public BeneficiaryPresenter Presenter
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
                return "{04515AEA-B1B2-4D02-BD91-0B74E4C75CD4}";
            }
        }

        #region Field Getters
        public string GetName
        {
            get { return txtSrchBeneficiaryName.Text; }
        }
        public IList<Beneficiary> Beneficiaries
        {
            get
            {
                return _Beneficiaries;
            }
            set
            {
                _Beneficiaries = value;
            }
        }
        #endregion
        void BindBeneficiaries()
        {
            dgBeneficiary.DataSource = _presenter.ListBeneficiaries(GetName);
            dgBeneficiary.DataBind();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            //_presenter.ListCarRentals(GetName);
            BindBeneficiaries();
        }
        protected void dgBeneficiary_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = -1;
        }
        protected void dgBeneficiary_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);
            try
            {
                beneficiary.Status = "InActive";
                _presenter.SaveOrUpdateBeneficiary(beneficiary);
                
                BindBeneficiaries();

                Master.ShowMessage(new AppMessage("beneficiary was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete beneficiary. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgBeneficiary_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Beneficiary beneficiary = new Beneficiary();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtName = e.Item.FindControl("txtBeneficiaryName") as TextBox;
                    beneficiary.BeneficiaryName = txtName.Text;
                    TextBox txtBranchName = e.Item.FindControl("txtBranchName") as TextBox;
                    beneficiary.BranchName = txtBranchName.Text;
                    TextBox txtBankName = e.Item.FindControl("txtBankName") as TextBox;
                    beneficiary.BankName = txtBankName.Text;
                    TextBox txtSortCode = e.Item.FindControl("txtSortCode") as TextBox;
                    beneficiary.SortCode = txtSortCode.Text;
                    TextBox txtAccountNumber = e.Item.FindControl("txtAccountNumber") as TextBox;
                    beneficiary.AccountNumber = txtAccountNumber.Text;
                    beneficiary.Status = "Active";
                    SaveBeneficiary(beneficiary);
                    dgBeneficiary.EditItemIndex = -1;
                    BindBeneficiaries();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Beneficiary " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveBeneficiary(Beneficiary beneficiary)
        {
            try
            {
                if (beneficiary.Id <= 0)
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateBeneficiary(beneficiary);
                    Master.ShowMessage(new AppMessage("Beneficiary Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgBeneficiary_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgBeneficiary.EditItemIndex = e.Item.ItemIndex;

            BindBeneficiaries();
        }
        protected void dgBeneficiary_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgBeneficiary_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgBeneficiary.DataKeys[e.Item.ItemIndex];
            Beneficiary beneficiary = _presenter.GetBeneficiaryById(id);

            try
            {
                TextBox txtName = e.Item.FindControl("txtEdtBeneficiaryName") as TextBox;
                beneficiary.BeneficiaryName = txtName.Text;
                TextBox txtBranchName = e.Item.FindControl("txtEdtBranchName") as TextBox;
                beneficiary.BranchName = txtBranchName.Text;
                TextBox txtBankName = e.Item.FindControl("txtEdtBankName") as TextBox;
                beneficiary.BankName = txtBankName.Text;
                TextBox txtSortCode = e.Item.FindControl("txtEdtSortCode") as TextBox;
                beneficiary.SortCode = txtSortCode.Text;
                TextBox txtAccountNumber = e.Item.FindControl("txtEdtAccountNumber") as TextBox;
                beneficiary.AccountNumber = txtAccountNumber.Text;
                SaveBeneficiary(beneficiary);
                dgBeneficiary.EditItemIndex = -1;
                BindBeneficiaries();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Beneficiary. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }        
    }
}
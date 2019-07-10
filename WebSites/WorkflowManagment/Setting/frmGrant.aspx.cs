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
    public partial class frmGrant : POCBasePage, IGrantView
    {
        private GrantPresenter _presenter;
        private IList<Grant> _grants;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindGrant();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public GrantPresenter Presenter
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

        void BindGrant()
        {
            dgGrant.DataSource = _presenter.ListGrants(txtGrantName.Text,txtGrantCode.Text);
            dgGrant.DataBind();
        }
        #region interface
        

        public IList<CoreDomain.Setting.Grant> grant
        {
            get
            {
                return _grants;
            }
            set
            {
                _grants = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListGrants(GrantName,GrantCode);
            BindGrant();
        }
        protected void dgGrant_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgGrant.EditItemIndex = -1;
        }
        protected void dgGrant_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgGrant.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Grant grant = _presenter.GetGrantById(id);
            try
            {
                grant.Status = "InActive";
                _presenter.SaveOrUpdateGrant(grant);
                
                BindGrant();

                Master.ShowMessage(new AppMessage("Grant was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Grant. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgGrant_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.Grant grant = new Chai.WorkflowManagment.CoreDomain.Setting.Grant();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFGrantName = e.Item.FindControl("txtFGrantName") as TextBox;
                    grant.GrantName = txtFGrantName.Text;
                    TextBox txtFGrantCode = e.Item.FindControl("txtFGrantCode") as TextBox;
                    grant.GrantCode = txtFGrantCode.Text;
                    TextBox txtFDonor = e.Item.FindControl("txtFDonor") as TextBox;
                    grant.Donor = txtFDonor.Text;
                    grant.Status = "Active";
                    SaveGrant(grant);
                    dgGrant.EditItemIndex = -1;
                    BindGrant();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Grant " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveGrant(Chai.WorkflowManagment.CoreDomain.Setting.Grant grant)
        {
            try
            {
                if(grant.Id  <= 0)
                {
                    _presenter.SaveOrUpdateGrant(grant);
                    Master.ShowMessage(new AppMessage("Grant saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateGrant(grant);
                    Master.ShowMessage(new AppMessage("Grant Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgGrant_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgGrant.EditItemIndex = e.Item.ItemIndex;

            BindGrant();
        }
        protected void dgGrant_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgGrant_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgGrant.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Grant grant = _presenter.GetGrantById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtGrantName") as TextBox;
                grant.GrantName = txtName.Text;
                TextBox txtCode = e.Item.FindControl("txtGrantCode") as TextBox;
                grant.GrantCode = txtCode.Text;
                TextBox txtDonor = e.Item.FindControl("txtDonor") as TextBox;
                grant.Donor = txtDonor.Text;
                SaveGrant(grant);
                dgGrant.EditItemIndex = -1;
                BindGrant();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Grant. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }


        public string GrantName
        {
            get { return txtGrantName.Text; }
        }

        public string GrantCode
        {
            get { return txtGrantCode.Text; }
        }
    }
}
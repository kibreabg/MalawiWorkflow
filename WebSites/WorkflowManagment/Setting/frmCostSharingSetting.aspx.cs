using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
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
    public partial class frmCostSharingSetting : POCBasePage, ICostSharingSetting
    {
        private CostSharingSettingPresenter _presenter;
        decimal totalPercentage = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindCostSharing();
               
            }
            
            this._presenter.OnViewLoaded();
         

        }

        [CreateNew]
        public CostSharingSettingPresenter Presenter
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
                return "{FAC4E85E-A360-4F0E-B1A6-099B38F4D615}";
            }
        }
        private void AddProjects()
        {
            foreach (Project PR in _presenter.GetProjects())
            {
                if (_presenter.GetProjectfromCostSharing(PR.Id) == null)
                {
                    CostSharingSetting CSS = new CostSharingSetting();
                    CSS.Project = _presenter.GetProject(PR.Id);
                    _presenter.SaveOrUpdateCostSharing(CSS);
                }
            }
        }
        private void BindCostSharing()
        {
            AddProjects();
           
            dgItemDetail.DataSource = _presenter.GetCostSharingSettings();
            dgItemDetail.DataBind();
        }
        private void BindGrant(DropDownList ddlGrant, int ProjectId)
        {
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataValueField = "Id";
            ddlGrant.DataTextField = "GrantCode";
            ddlGrant.DataBind();
        }
        private decimal GetTotalPercentage()
        {
            int index = 0;

            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];

                TextBox txtPercentage = dgi.FindControl("txtPercentage") as TextBox;

                totalPercentage = totalPercentage + Convert.ToDecimal(txtPercentage.Text);

                index++;
            }
            return totalPercentage;
        }
        private void SetCostSharing()
        {
            int index = 0;
            
            foreach (DataGridItem dgi in dgItemDetail.Items)
            {
                int id = (int)dgItemDetail.DataKeys[dgi.ItemIndex];

                CostSharingSetting detail;
                
                detail = _presenter.GetProjectfromCostSharing(id);
                if (detail != null)
                {
                    TextBox txtPercentage = dgi.FindControl("txtPercentage") as TextBox;
                    detail.Percentage = Convert.ToDecimal(txtPercentage.Text);
                    DropDownList ddlGrant = dgi.FindControl("ddlGrant") as DropDownList;
                    detail.Grant = _presenter.GetGrant(Convert.ToInt32(ddlGrant.SelectedValue));
                    _presenter.SaveOrUpdateCostSharing(detail);
                }
                index++;

            }
           
            Master.ShowMessage(new AppMessage("Cost Sharing successfully saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        private void DeleteInActiveProjects()
        {
            foreach (CostSharingSetting CSS in _presenter.GetCostSharingSettings())
            {
                if (_presenter.GetProjectforCostSharing(CSS.Project.Id) == null)
                {
                    _presenter.DeleteCostSharingSetting(CSS);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DeleteInActiveProjects();
            if (GetTotalPercentage() != 100)
            {
                Master.ShowMessage(new AppMessage("Total Percentage is not equal to 100 ", Chai.WorkflowManagment.Enums.RMessageType.Error));

            }
            else 
            { 
            SetCostSharing();
            }
        }
        protected void dgItemDetail_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
               
                if (_presenter.GetCostSharingSettings() != null)
                {
                    
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    if (ddlGrant != null)
                    {
                        BindGrant(ddlGrant, Convert.ToInt32(_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Project.Id));
                        if (_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant != null)
                        {
                            if (_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant.Id != null)
                            {
                                ListItem liI = ddlGrant.Items.FindByValue(_presenter.GetCostSharingSettings()[e.Item.DataSetIndex].Grant.Id.ToString());
                                if (liI != null)
                                    liI.Selected = true;
                            }
                        }

                    }
                  
                }
            }
        }
}
}
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
    public partial class frmProjects : POCBasePage, IProjectView
    {
        private ProjectPresenter _presenter;
        private IList<Project> _Projects;
        private Project _Project;
        private int selectedProjectId;
        private int ProjectId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindProjects();
            }

            this._presenter.OnViewLoaded();
            _Project = Session["Project"] as Project;
            //  ProgramId = (int)Session["ProgramId"] ;
        }

        [CreateNew]
        public ProjectPresenter Presenter
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
                return "{B428E389-9CD4-4B56-BEEE-3E4C48F746DA}";
            }
        }

        private void BindProjects()
        {
            dgProject.DataSource = _presenter.ListProjects(txtProjectCode.Text);
            dgProject.DataBind();
            //_presenter.Commit();
        }
        #region interface


        public IList<CoreDomain.Setting.Project> Projects
        {
            get
            {
                return _Projects;
            }
            set
            {
                _Projects = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListProjects(ProjectCode);
            BindProjects();
        }
        protected void dgProject_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProject.EditItemIndex = -1;
            this.BindProjects();
        }
        protected void dgProject_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgProject.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.Project project = _presenter.GetProject(id);
            try
            {
                project.Status = "InActive";
                _presenter.SaveOrUpdateProject(project);
                BindProjects();

                Master.ShowMessage(new AppMessage("Project was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Project. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgProject_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.Project project = new Chai.WorkflowManagment.CoreDomain.Setting.Project();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFProjectDescription = e.Item.FindControl("txtFProjectDescription") as TextBox;
                    project.ProjectDescription = txtFProjectDescription.Text;
                    TextBox txtFProjectCode = e.Item.FindControl("txtFProjectCode") as TextBox;
                    project.ProjectCode = txtFProjectCode.Text;
                    TextBox txtStartingDate = e.Item.FindControl("txtStartingDate") as TextBox;
                    project.StartingDate = Convert.ToDateTime(txtStartingDate.Text);
                    TextBox txtEndDate = e.Item.FindControl("txtEndDate") as TextBox;
                    project.EndDate = Convert.ToDateTime(txtEndDate.Text);
                    TextBox txtFTotalBudget = e.Item.FindControl("txtFTotalBudget") as TextBox;
                    project.TotalBudget = txtFTotalBudget.Text != "" ? Convert.ToDecimal(txtFTotalBudget.Text) : 0;
                    TextBox txtFRemainingBudget = e.Item.FindControl("txtFRemainingBudget") as TextBox;
                    project.RemainingBudget = txtFRemainingBudget.Text != "" ? Convert.ToDecimal(txtFRemainingBudget.Text) : 0;
                    DropDownList ddlProgramManager = e.Item.FindControl("ddlProgramManager") as DropDownList;
                    project.AppUser = _presenter.GetUser(Convert.ToInt32(ddlProgramManager.SelectedValue));
                    DropDownList ddlStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
                    project.Status = ddlStatus.SelectedValue;


                    SaveProject(project);
                    dgProject.EditItemIndex = -1;
                    BindProjects();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Project " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        private void SaveProject(Chai.WorkflowManagment.CoreDomain.Setting.Project project)
        {
            try
            {
                if (project.Id <= 0)
                {
                    _presenter.SaveOrUpdateProject(project);
                    Master.ShowMessage(new AppMessage("project saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateProject(project);
                    Master.ShowMessage(new AppMessage("project Updated", RMessageType.Info));
                    // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgProject_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProject.EditItemIndex = e.Item.ItemIndex;

            BindProjects();
        }
        protected void dgProject_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlProgramManager = e.Item.FindControl("ddlProgramManager") as DropDownList;
                PopProgramManagers(ddlProgramManager);
                DropDownList ddlFStatus = e.Item.FindControl("ddlFStatus") as DropDownList;
            }
            else
            {
                if (_Projects != null)
                {
                    DropDownList ddlEdtProgramManager = e.Item.FindControl("ddlEdtProgramManager") as DropDownList;
                    if (ddlEdtProgramManager != null)
                    {
                        PopProgramManagers(ddlEdtProgramManager);
                        int projectId = Convert.ToInt32(_Projects[e.Item.DataSetIndex].Id);
                        if (_presenter.GetProject(projectId).AppUser.Id != 0)
                        {
                            ListItem li = ddlEdtProgramManager.Items.FindByValue(_presenter.GetProject(projectId).AppUser.Id.ToString());
                            if (li != null)
                                li.Selected = true;
                        }
                    }

                    DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                    if (ddlStatus != null)
                    {
                        if (_Projects[e.Item.DataSetIndex].Status != null)
                        {
                            ListItem liI = ddlStatus.Items.FindByValue(_Projects[e.Item.DataSetIndex].Status.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }
                    }
                }
            }
        }
        private void PopProgramManagers(DropDownList ddlProgramManager)
        {
            ddlProgramManager.DataSource = _presenter.GetProgramManagers();
            ddlProgramManager.DataBind();
        }
        protected void dgProject_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgProject.DataKeys[e.Item.ItemIndex];
            Project project = _presenter.GetProject(id);
            try
            {
                TextBox txtProjectDescription = e.Item.FindControl("txtProjectDescription") as TextBox;
                project.ProjectDescription = txtProjectDescription.Text;
                TextBox txtProjectCode = e.Item.FindControl("txtProjectCode") as TextBox;
                project.ProjectCode = txtProjectCode.Text;
                TextBox txtEdtStartingDate = e.Item.FindControl("txtEdtStartingDate") as TextBox;
                project.StartingDate = Convert.ToDateTime(txtEdtStartingDate.Text);
                TextBox txtEdtEndDate = e.Item.FindControl("txtEdtEndDate") as TextBox;
                project.EndDate = Convert.ToDateTime(txtEdtEndDate.Text);
                TextBox txtTotalBudget = e.Item.FindControl("txtTotalBudget") as TextBox;
                project.TotalBudget = Convert.ToDecimal(txtTotalBudget.Text);

                TextBox txtRemainingBudget = e.Item.FindControl("txtRemainingBudget") as TextBox;
                project.RemainingBudget = Convert.ToDecimal(txtRemainingBudget.Text);
                DropDownList ddlEditProgramManager = e.Item.FindControl("ddlEdtProgramManager") as DropDownList;
                project.AppUser = _presenter.GetUser(Convert.ToInt32(ddlEditProgramManager.SelectedValue));
                DropDownList ddlStatus = e.Item.FindControl("ddlStatus") as DropDownList;
                project.Status = ddlStatus.SelectedValue;
                _presenter.SaveOrUpdateProject(project);
                Master.ShowMessage(new AppMessage("Project Updated Successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                dgProject.EditItemIndex = -1;
                BindProjects();
            }

            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Project Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgProject_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int ProjectId = (int)dgProject.DataKeys[dgProject.SelectedIndex];
            Session["ProjectId"] = ProjectId;
            dgProject.SelectedItemStyle.BackColor = System.Drawing.Color.BurlyWood;
            propan.Visible = true;
            _Project = _presenter.GetProject(ProjectId);
            Session["Project"] = _Project;
            PnlProGrant.Visible = true;
            BindProjectGrants();
        }

        #region ProjectGrant
        private void BindProjectGrants()
        {
            dgProjectGrant.DataSource = _Project.ProGrants;
            dgProjectGrant.DataBind();
        }
        protected void dgProjectGrant_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProjectGrant.EditItemIndex = -1;
            BindProjectGrants();
        }
        protected void dgProjectGrant_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgProjectGrant.DataKeys[e.Item.ItemIndex];

            try
            {
                _Project.RemoveProjectGrant(id);
                _presenter.DeleteProjectGrant(_presenter.GetProjectGrant(id));
                _presenter.SaveOrUpdateProject(_Project);

                BindProjectGrants();

                Master.ShowMessage(new AppMessage("Project Grant was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Project Grant. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgProjectGrant_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgProjectGrant.EditItemIndex = e.Item.ItemIndex;
            BindProjectGrants();
        }
        protected void dgProjectGrant_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    ProGrant projectgrant = new ProGrant();
                    DropDownList ddlFGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                    projectgrant.Grant = _presenter.GetGrant(Convert.ToInt32(ddlFGrant.SelectedValue));
                    TextBox txtFGrantDate = e.Item.FindControl("txtFGrantDate") as TextBox;
                    projectgrant.GrantDate = Convert.ToDateTime(txtFGrantDate.Text);
                    projectgrant.Project = _Project;
                    _Project.ProGrants.Add(projectgrant);
                    _presenter.SaveOrUpdateProject(_Project);
                    Master.ShowMessage(new AppMessage("Project Grant Added Successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    dgProjectGrant.EditItemIndex = -1;
                    BindProjectGrants();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Project Grant. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgProjectGrant_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DropDownList ddlGrant = e.Item.FindControl("ddlFGrant") as DropDownList;
                BindGrant(ddlGrant);
            }
            else
            {
                if (_Project.ProGrants != null)
                {
                    DropDownList ddlGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                    if (ddlGrant != null)
                    {
                        BindGrant(ddlGrant);
                        if (_Project.ProGrants[e.Item.DataSetIndex].Grant.Id != null)
                        {
                            ListItem lig = ddlGrant.Items.FindByValue(_Project.ProGrants[e.Item.DataSetIndex].Grant.Id.ToString());
                            if (lig != null)
                                lig.Selected = true;
                        }
                    }
                }
            }
        }
        protected void dgProjectGrant_UpdateCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgProjectGrant.DataKeys[e.Item.ItemIndex];
            ProGrant projectGrant = _Project.GetProjectGrant(id);
            try
            {
                DropDownList ddlFGrant = e.Item.FindControl("ddlGrant") as DropDownList;
                projectGrant.Grant = _presenter.GetGrant(Convert.ToInt32(ddlFGrant.SelectedValue));
                TextBox txtFGrantDate = e.Item.FindControl("txtGrantDate") as TextBox;
                projectGrant.GrantDate = Convert.ToDateTime(txtFGrantDate.Text);
                _presenter.SaveOrUpdateProject(_Project);
                Master.ShowMessage(new AppMessage("Project Grant Updated Successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                dgProjectGrant.EditItemIndex = -1;
                BindProjectGrants();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Project Grant . " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        private void BindGrant(DropDownList ddlGrant)
        {
            ddlGrant.DataSource = _presenter.ListGrant();
            ddlGrant.DataBind();
        }
        #endregion
        public string ProjectCode
        {
            get { return txtProjectCode.Text; }
        }
        protected void btnCancedetail_Click(object sender, EventArgs e)
        {
            PnlProGrant.Visible = false;
        }
    }
}
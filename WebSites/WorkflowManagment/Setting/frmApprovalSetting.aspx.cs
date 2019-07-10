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
    public partial class frmApprovalSetting : POCBasePage, IApprovalSettingView
    {
        private ApprovalSettingPresenter _presenter;
        private ApprovalSetting _approvalsetting;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                PopRequestType();
                PopRequestTypeforSearch();
                BindApprovalLevels();
            }
            
            this._presenter.OnViewLoaded();
            
            BindSearchApprovalSettingGrid();
            //BindApprovalLevels(); 
            

        }

        [CreateNew]
        public ApprovalSettingPresenter Presenter
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
                return "{CBF21161-18DD-434A-9B0B-E6316AB62637}";
            }
        }


        public ApprovalSetting ApprovalSetting
        {
            get
            {
                return _approvalsetting;
            }
            set
            {
                _approvalsetting = value;
            }
        }

        public string RequestType
        {
            get { return ddlRequestTypesrch.SelectedValue; }
        }


        private void PopRequestType()
        {
            string[] s = Enum.GetNames(typeof(RequestType));

            for (int i = 0; i < s.Length; i++)
            {
                ddlRequestType.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }

        
        }
        private void PopRequestTypeforSearch()
        {
            string[] s = Enum.GetNames(typeof(RequestType));

            for (int i = 0; i < s.Length; i++)
            {
                ddlRequestTypesrch.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }


        }
        private void PopEmployeePosition(DropDownList ddlPosition)
        {

            
            ddlPosition.DataSource = _presenter.ListEmployeePosition();
            ddlPosition.DataBind();
        }
        private void PopWill(DropDownList ddlWill)
        {

            string[] s = Enum.GetNames(typeof(Will));

            for (int i = 0; i < s.Length; i++)
            {
                ddlWill.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));

            }
            
        }
        private void BindApprovalSetting(int ApprovalSettingId)
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentApprovalSetting != null)
            {
                ddlRequestType.SelectedValue = _presenter.CurrentApprovalSetting.RequestType;
                ddlCriteria.SelectedValue = _presenter.CurrentApprovalSetting.CriteriaCondition;
                txtValue.Text = _presenter.CurrentApprovalSetting.Value.ToString();
                txtValue2.Text = _presenter.CurrentApprovalSetting.Value2.ToString();
                //txtLevel.Text = _presenter.CurrentApprovalSetting.ApprovalLevel.ToString();
                DisplayCondition();
                BindApprovalLevels();        
                
            }
           
        }
        private void DisplayCondition()
        {
            if (_presenter.CurrentApprovalSetting.CriteriaCondition == "None")
            {
                txtValue.Visible = false;
                lblValue.Visible = false;
                txtValue2.Visible = false;
                lblValue2.Visible = false;
            }
            else if (_presenter.CurrentApprovalSetting.CriteriaCondition == "Between")
            {
                txtValue.Visible = true;
                lblValue.Visible = true;
                txtValue2.Visible = true;
                lblValue2.Visible = true;
            }
            else
            {
                txtValue2.Visible = false;
                lblValue2.Visible = false;
                txtValue.Visible = true;
                lblValue.Visible = true;
            }
        }
        private void BindSearchApprovalSettingGrid()
        {
            grvApprovalSettingList.DataSource = _presenter.ListApprovalSettings(ddlRequestTypesrch.SelectedValue); ;
            grvApprovalSettingList.DataBind();
        }
        private void SaveApprovalSetting()
        {
            try
            {
                if (_presenter.CurrentApprovalSetting.ApprovalLevels.Count > 0)
                {
                    _presenter.CurrentApprovalSetting.RequestType = ddlRequestType.SelectedValue;
                    _presenter.CurrentApprovalSetting.CriteriaCondition = ddlCriteria.SelectedValue;
                    _presenter.CurrentApprovalSetting.Value = txtValue.Text != "" ? Convert.ToInt32(txtValue.Text) : 0;
                    if(ddlCriteria.SelectedValue == "Between")
                    {
                    _presenter.CurrentApprovalSetting.Value2 = txtValue2.Text != "" ? Convert.ToInt32(txtValue2.Text) : 0;
                    _presenter.CurrentApprovalSetting.CriteriaQuery = ddlCriteria.SelectedValue + " " + _presenter.CurrentApprovalSetting.Value + " " + "And" + " "+ _presenter.CurrentApprovalSetting.Value2;
                     }
                    else
                    {
                        _presenter.CurrentApprovalSetting.CriteriaQuery = ddlCriteria.SelectedValue + " " + _presenter.CurrentApprovalSetting.Value;
                    }
                }

                else
                {
                    lblError.Text = "You have to add atleast one Approval Level for this approval setting";
                }
               // _presenter.CurrentApprovalSetting.ApprovalLevel = Convert.ToInt32(txtLevel.Text);
               // SetIssueDetailValues();
            }

            catch(Exception ex)
            {
                  
               
            
            }
        
        }
       
      
        //private void GenerateApprovalLevelByWorkflowLevel()
        //{

        //    for (int i = 1; i <= int.Parse(txtLevel.Text);i++ )
        //    {
                
        //            ApprovalLevel iDetail = new ApprovalLevel();
        //            iDetail.ApprovalSetting = _presenter.CurrentApprovalSetting;
        //            _presenter.CurrentApprovalSetting.ApprovalLevels.Add(iDetail);
                
        //    }
        //}
        //private void SetIssueDetailValues()
        //{
        //    int index = 0;
        //    foreach (DataGridItem dgi in dgApprovalLevel.Items)
        //    {
        //        int id = (int)dgApprovalLevel.DataKeys[dgi.ItemIndex];

        //        ApprovalLevel iDetail;
        //        if (id > 0)
        //            iDetail = _presenter.CurrentApprovalSetting.GetApprovalLevel(id);
        //        else
        //            iDetail = (ApprovalLevel)_presenter.CurrentApprovalSetting.ApprovalLevels[index];

        //        DropDownList ddl = dgi.FindControl("ddlPosition") as DropDownList;
        //        iDetail.EmployeePosition = _presenter.GetEmployeePosition(int.Parse(ddl.SelectedValue));

        //        TextBox txt = dgi.FindControl("txtLevel") as TextBox;
        //        iDetail.WorkflowLevel = Convert.ToInt32(txt.Text);

                
        //        index++;
        //    }
        //}
        
        private void BindApprovalLevels()
        {
            this.dgApprovalLevel.DataSource = _presenter.CurrentApprovalSetting.ApprovalLevels;
            this.dgApprovalLevel.DataBind();
        }
        protected void dgApprovalLevel_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgApprovalLevel.EditItemIndex = -1;
            BindApprovalLevels();
        }
        protected void dgApprovalLevel_DeleteCommand1(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgApprovalLevel.DataKeys[e.Item.ItemIndex];

            try
            {
               
                _presenter.DeleteApprovalLevel(_presenter.GetApprovalLevel(id));
                _presenter.CurrentApprovalSetting.RemoveApprovalLevel(id);
                _presenter.SaveOrUpdateApprovalSetting(_presenter.CurrentApprovalSetting);

                BindApprovalLevels();

                Master.ShowMessage(new AppMessage("Approval Level was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Approval Level " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }

        }
        protected void dgApprovalLevel_EditCommand1(object source, DataGridCommandEventArgs e)
        {
            this.dgApprovalLevel.EditItemIndex = e.Item.ItemIndex;

            BindApprovalLevels();
        }
       
        protected void dgApprovalLevel_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "AddNew")
            {
                try
                {
                    ApprovalLevel approvallevel = new ApprovalLevel();
                    DropDownList ddlFPositionName = e.Item.FindControl("ddlFPosition") as DropDownList;
                    approvallevel.EmployeePosition = _presenter.GetEmployeePosition(Convert.ToInt32(ddlFPositionName.SelectedValue));
                    DropDownList ddlFWill = e.Item.FindControl("ddlFWill") as DropDownList;
                    approvallevel.Will = ddlFWill.SelectedValue;
                    approvallevel.ApprovalSetting = _approvalsetting;
                    _presenter.CurrentApprovalSetting.ApprovalLevels.Add(approvallevel);
                    Master.ShowMessage(new AppMessage("Approval Level added successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    dgApprovalLevel.EditItemIndex = -1;
                    BindApprovalLevels();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Approval Level. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }
        protected void dgApprovalLevel_ItemDataBound(object sender, DataGridItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Footer)
            {
               DropDownList ddlFEmployeePosition = e.Item.FindControl("ddlFPosition") as DropDownList;
               PopEmployeePosition(ddlFEmployeePosition);
               DropDownList ddlFWill = e.Item.FindControl("ddlFWill") as DropDownList;
               PopWill(ddlFWill);

            }
            else
            {


                if (_presenter.CurrentApprovalSetting.ApprovalLevels != null)
                {


                    DropDownList ddlEmployeePosition = e.Item.FindControl("ddlPosition") as DropDownList;
                    if (ddlEmployeePosition != null)
                    {
                        PopEmployeePosition(ddlEmployeePosition);
                        if (_presenter.CurrentApprovalSetting.ApprovalLevels[e.Item.DataSetIndex].EmployeePosition.Id != null)
                        {
                            ListItem liI = ddlEmployeePosition.Items.FindByValue(_presenter.CurrentApprovalSetting.ApprovalLevels[e.Item.DataSetIndex].EmployeePosition.Id.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }

                   
                }
                if (_presenter.CurrentApprovalSetting.ApprovalLevels != null)
                {


                    DropDownList ddlWill = e.Item.FindControl("ddlWill") as DropDownList;
                    if (ddlWill != null)
                    {
                        PopWill(ddlWill);
                        if (_presenter.CurrentApprovalSetting.ApprovalLevels[e.Item.DataSetIndex].Will != null)
                        {
                            ListItem liI = ddlWill.Items.FindByValue(_presenter.CurrentApprovalSetting.ApprovalLevels[e.Item.DataSetIndex].Will.ToString());
                            if (liI != null)
                                liI.Selected = true;
                        }

                    }


                }
            }


        }
        protected void dgApprovalLevel_UpdateCommand1(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgApprovalLevel.DataKeys[e.Item.ItemIndex];
            ApprovalLevel approvalLevel = _presenter.CurrentApprovalSetting.GetApprovalLevel(id);
            try
            {
                
                DropDownList ddlPositionName = e.Item.FindControl("ddlPosition") as DropDownList;
                approvalLevel.EmployeePosition = _presenter.GetEmployeePosition(Convert.ToInt32(ddlPositionName.SelectedValue));
                DropDownList ddlWill = e.Item.FindControl("ddlWill") as DropDownList;
                approvalLevel.Will = ddlWill.SelectedValue;
               // approvalLevel.ApprovalSetting = _approvalsetting;
                _presenter.SaveOrUpdateApprovalSetting(_presenter.CurrentApprovalSetting);
                Master.ShowMessage(new AppMessage("Approval Level  Updated successfully.", Chai.WorkflowManagment.Enums.RMessageType.Info));
                dgApprovalLevel.EditItemIndex = -1;
                BindApprovalLevels();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Approval Level Detail. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
      
        private void ClearForm()
        {
            //txtLevel.Text = string.Empty;
            txtValue.Text = string.Empty;
            PopRequestType();
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {         
           BindSearchApprovalSettingGrid();
           ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void grvApprovalSettingList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ApprovalLevel"] = true;
            ClearForm();
            BindApprovalSetting(Convert.ToInt32(grvApprovalSettingList.SelectedDataKey.Value));
        }
        protected void grvApprovalSettingList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            _presenter.DeleteApprovalSetting(_presenter.GetApprovalSettingById(Convert.ToInt32(grvApprovalSettingList.DataKeys[e.RowIndex].Value)));
            
              btnFind_Click(sender, e);
              Master.ShowMessage(new AppMessage("Approval Setting Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
            
        }
        protected void grvApprovalSettingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvApprovalSettingList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvApprovalSettingList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
            
        }
        public int ApprovalSettingId
        {
            get
            {
                if (grvApprovalSettingList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvApprovalSettingList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveApprovalSetting();
            _presenter.SaveOrUpdateApprovalSetting(_presenter.CurrentApprovalSetting);
            ClearForm();
            Master.ShowMessage(new AppMessage("Approval Setting Successfully Saved", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
     
        protected void ddlCriteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCriteria.SelectedValue == "None")
            {
                txtValue.Visible = false;
                lblValue.Visible = false;
                txtValue2.Visible = false;
                lblValue2.Visible = false;
            }
            else if (ddlCriteria.SelectedValue == "Between")
            {
                txtValue.Visible = true;
                lblValue.Visible = true;
                txtValue2.Visible = true;
                lblValue2.Visible = true;
            }
            else 
            {
                txtValue2.Visible = false;
                lblValue2.Visible = false;
                txtValue.Visible = true;
                lblValue.Visible = true;
            }
        }



        protected void dgApprovalLevel_CancelCommand1(object source, DataGridCommandEventArgs e)
        {
            this.dgApprovalLevel.EditItemIndex = -1;
            BindApprovalLevels();
        }
}
}
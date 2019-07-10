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
    public partial class frmLeaveType : POCBasePage, ILeaveTypeView
    {
        private LeaveTypePresenter _presenter;
        private IList<LeaveType> _LeaveTypes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindLeaveType();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public LeaveTypePresenter Presenter
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
                return "{990C732F-25EE-4287-A093-423026F33676}";
            }
        }

        void BindLeaveType()
        {
            dgLeaveType.DataSource = _presenter.ListLeaveTypes();
            dgLeaveType.DataBind();
        }
        #region interface


        public IList<CoreDomain.Setting.LeaveType> leavetype
        {
            get
            {
                return _LeaveTypes;
            }
            set
            {
                _LeaveTypes = value;
            }
        }
        #endregion
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListLeaveTypes();
            BindLeaveType();
        }
        protected void dgLeaveType_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgLeaveType.EditItemIndex = -1;
        }
        protected void dgLeaveType_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgLeaveType.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.LeaveType LeaveType = _presenter.GetLeaveTypeById(id);
            try
            {
                LeaveType.Status = "InActive";
                _presenter.SaveOrUpdateLeaveType(LeaveType);
                BindLeaveType();

                Master.ShowMessage(new AppMessage("Leave Type was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Leave Type. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgLeaveType_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.LeaveType LeaveType = new Chai.WorkflowManagment.CoreDomain.Setting.LeaveType();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFLeaveTypeName = e.Item.FindControl("txtFLeaveTypeName") as TextBox;
                    LeaveType.LeaveTypeName = txtFLeaveTypeName.Text;
                    LeaveType.Status = "Active";

                    SaveLeaveType(LeaveType);
                    dgLeaveType.EditItemIndex = -1;
                    BindLeaveType();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Leave Type " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveLeaveType(Chai.WorkflowManagment.CoreDomain.Setting.LeaveType LeaveType)
        {
            try
            {
                if(LeaveType.Id  <= 0)
                {
                    _presenter.SaveOrUpdateLeaveType(LeaveType);
                    Master.ShowMessage(new AppMessage("Leave Type saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateLeaveType(LeaveType);
                    Master.ShowMessage(new AppMessage("Leave Type Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgLeaveType_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgLeaveType.EditItemIndex = e.Item.ItemIndex;

            BindLeaveType();
        }
        protected void dgLeaveType_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgLeaveType_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgLeaveType.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.LeaveType LeaveType = _presenter.GetLeaveTypeById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtLeaveTypeName") as TextBox;
                LeaveType.LeaveTypeName = txtName.Text;
                SaveLeaveType(LeaveType);
                dgLeaveType.EditItemIndex = -1;
                BindLeaveType();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Leave Type. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        } 


     
    }
}
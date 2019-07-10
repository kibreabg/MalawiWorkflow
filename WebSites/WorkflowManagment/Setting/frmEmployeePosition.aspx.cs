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
    public partial class frmEmployeePosition : POCBasePage, IEmployeePositionView
    {
        private EmployeePositionPresenter _presenter;
        private IList<EmployeePosition> _EmployeePositions;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                BindEmployeePosition();
            }
            
            this._presenter.OnViewLoaded();
            

        }

        [CreateNew]
        public EmployeePositionPresenter Presenter
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
                return "{F1DA0019-C467-4C97-A28E-C4496931A6B7}";
            }
        }

        void BindEmployeePosition()
        {
            dgEmployeePosition.DataSource = _presenter.ListEmployeePositions();
            dgEmployeePosition.DataBind();
        }
        
        protected void btnFind_Click(object sender, EventArgs e)
        {
            _presenter.ListEmployeePositions();
            BindEmployeePosition();
        }
        protected void dgEmployeePosition_CancelCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgEmployeePosition.EditItemIndex = -1;
        }
        protected void dgEmployeePosition_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int id = (int)dgEmployeePosition.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.EmployeePosition EmployeePosition = _presenter.GetEmployeePositionById(id);
            try
            {
                EmployeePosition.Status = "InActive";
                _presenter.SaveOrUpdateEmployeePosition(EmployeePosition);
                
                BindEmployeePosition();

                Master.ShowMessage(new AppMessage("Employee Position was Removed Successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Employee Position. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }
        protected void dgEmployeePosition_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Chai.WorkflowManagment.CoreDomain.Setting.EmployeePosition EmployeePosition = new Chai.WorkflowManagment.CoreDomain.Setting.EmployeePosition();
            if (e.CommandName == "AddNew")
            {
                try
                {

                    TextBox txtFEmployeePositionName = e.Item.FindControl("txtFPositionName") as TextBox;
                    EmployeePosition.PositionName = txtFEmployeePositionName.Text;
                    EmployeePosition.Status = "Active";
                    SaveEmployeePosition(EmployeePosition);
                    dgEmployeePosition.EditItemIndex = -1;
                    BindEmployeePosition();
                }
                catch (Exception ex)
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to Add Employee Position " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
        }

        private void SaveEmployeePosition(Chai.WorkflowManagment.CoreDomain.Setting.EmployeePosition EmployeePosition)
        {
            try
            {
                if(EmployeePosition.Id  <= 0)
                {
                    _presenter.SaveOrUpdateEmployeePosition(EmployeePosition);
                    Master.ShowMessage(new AppMessage("Employee Position saved", RMessageType.Info));
                    //_presenter.CancelPage();
                }
                else
                {
                    _presenter.SaveOrUpdateEmployeePosition(EmployeePosition);
                    Master.ShowMessage(new AppMessage("Employee Position Updated", RMessageType.Info));
                   // _presenter.CancelPage();
                }
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage(ex.Message, RMessageType.Error));
            }
        }
        protected void dgEmployeePosition_EditCommand(object source, DataGridCommandEventArgs e)
        {
            this.dgEmployeePosition.EditItemIndex = e.Item.ItemIndex;

            BindEmployeePosition();
        }
        protected void dgEmployeePosition_ItemDataBound(object sender, DataGridItemEventArgs e)
        {

        }
        protected void dgEmployeePosition_UpdateCommand(object source, DataGridCommandEventArgs e)
        {

            int id = (int)dgEmployeePosition.DataKeys[e.Item.ItemIndex];
            Chai.WorkflowManagment.CoreDomain.Setting.EmployeePosition EmployeePosition = _presenter.GetEmployeePositionById(id);

            try
            {


                TextBox txtName = e.Item.FindControl("txtPositionName") as TextBox;
                EmployeePosition.PositionName = txtName.Text;
                SaveEmployeePosition(EmployeePosition);
                dgEmployeePosition.EditItemIndex = -1;
                BindEmployeePosition();
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to Update Employee Position. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }




        public IList<EmployeePosition> EmployeePosition
        {
            get
            {
                return _EmployeePositions;
            }
            set
            {
                _EmployeePositions = value;
            }
        }
    }
}
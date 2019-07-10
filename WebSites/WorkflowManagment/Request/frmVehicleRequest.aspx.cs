using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public partial class frmVehicleRequest : POCBasePage, IVehicleRequestView
    {
        private VehicleRequestPresenter _presenter;
        private IList<VehicleRequest> _VehicleRequests;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                CheckApprovalSettings();
                PopRequestPersonnels();
                PopProjects();
                BindVehicleRequests();
                if (_presenter.CurrentVehicleRequest.Id <= 0)
                {
                    AutoNumber();
                    btnDelete.Visible = false;
                }
            }
            txtRequestDate.Text = DateTime.Today.Date.ToShortDateString();
            this._presenter.OnViewLoaded();

        }
        [CreateNew]
        public VehicleRequestPresenter Presenter
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
                return "{D1B7939C-7154-4403-B535-B4D33684CE21}";
            }
        }

        #region Field Getters
        public int GetVehicleRequestId
        {
            get
            {
                if (grvVehicleRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvVehicleRequestList.SelectedDataKey.Value);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string GetRequestNo
        {
            get { return AutoNumber(); }
        }
        public DateTime GetDepartureDate
        {
            get { return Convert.ToDateTime(txtDepartureDate.Text); }
        }
        public DateTime GetReturningDate
        {
            get { return Convert.ToDateTime(txtReturningDate.Text); }
        }
        public string GetDepartureTime
        {
            get { return timepicker.Text; }
        }
     
        public string GetPurposeOfTravel
        {
            get { return txtPurposeOfTravel.Text; }
        }
        public string GetDestination
        {
            get { return txtDestination.Text; }
        }
        public string GetComment
        {
            get { return txtComment.Text; }
        }
        public int GetNoOfPassengers
        {
            get { return Convert.ToInt32(txtNoOfPassangers.Text); }
        }
        public int GetProjectId
        {
            get { return Convert.ToInt32(ddlProject.SelectedValue); }
        }
        public int GetGrantId
        {
            get { return Convert.ToInt32(ddlGrant.SelectedValue); }
        }
        private void PopRequestPersonnels()
        {
            txtRequestingPersonnel.Text = _presenter.CurrentUser().FirstName + " " + _presenter.CurrentUser().LastName;
        }
        public IList<VehicleRequest> VehicleRequests
        {
            get
            {
                return _VehicleRequests;
            }
            set
            {
                _VehicleRequests = value;
            }
        }
        #endregion
        private string AutoNumber()
        {
            return "VR-" + (_presenter.GetLastVehicleRequestId() + 1).ToString();
        }
        private void CheckApprovalSettings()
        {
            if (_presenter.GetApprovalSetting(RequestType.Vehicle_Request.ToString().Replace('_', ' '), 0) == null)
            {
                pnlWarning.Visible = true;
            }
        }
        private void PopProjects()
        {
            ddlProject.DataSource = _presenter.GetProjects();
            ddlProject.DataBind();

            ddlProject.Items.Insert(0, new ListItem("---Select Project---", "0"));
            ddlProject.SelectedIndex = 0;
        }
        private void PopGrants(int ProjectId)
        {
            ddlGrant.DataSource = _presenter.GetGrantbyprojectId(ProjectId);
            ddlGrant.DataBind();

            ddlGrant.Items.Insert(0, new ListItem("---Select Grant---", "0"));
            ddlGrant.SelectedIndex = 0;
        }
        private void ClearFormFields()
        {
            //txtRequestNo.Text = String.Empty;
            txtDepartureDate.Text = String.Empty;
            txtReturningDate.Text = String.Empty;
            txtPurposeOfTravel.Text = String.Empty;
            txtNoOfPassangers.Text = String.Empty;
            txtDestination.Text = String.Empty;
            txtComment.Text = String.Empty;
        }
        private void BindVehicleRequests()
        {
            grvVehicleRequestList.DataSource = _presenter.ListVehicleRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text);
            grvVehicleRequestList.DataBind();
        }
        private void BindVehicleRequestFields()
        {
            _presenter.OnViewLoaded();
            if (_presenter.CurrentVehicleRequest != null)
            {
                //txtRequestNo.Text = _presenter.CurrentVehicleRequest.RequestNo.ToString();
                txtDepartureDate.Text = _presenter.CurrentVehicleRequest.DepartureDate.Value.ToShortDateString();
                txtReturningDate.Text = _presenter.CurrentVehicleRequest.ReturningDate.Value.ToShortDateString();
                timepicker.Text = _presenter.CurrentVehicleRequest.DepartureTime;
              
                txtPurposeOfTravel.Text = _presenter.CurrentVehicleRequest.PurposeOfTravel.ToString();
                txtDestination.Text = _presenter.CurrentVehicleRequest.Destination;
                txtComment.Text = _presenter.CurrentVehicleRequest.Comment;
                txtNoOfPassangers.Text = _presenter.CurrentVehicleRequest.NoOfPassengers.ToString();
                ddlProject.SelectedValue = _presenter.CurrentVehicleRequest.Project.Id.ToString();
                PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
                ddlGrant.SelectedValue = _presenter.CurrentVehicleRequest.Grant.Id.ToString();
                BindVehicleRequests();
            }
        }
        protected void grvVehicleRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["VehicleRequest"] = true;
            //ClearForm();
            BindVehicleRequestFields();
            btnDelete.Enabled = true;
            if (_presenter.CurrentVehicleRequest.CurrentStatus != null)
            {
                btnSave.Visible = false;
                btnDelete.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }
        }
        protected void grvVehicleRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //LinkButton db = (LinkButton)e.Row.Cells[5].Controls[0];
                //db.OnClientClick = "return confirm('Are you sure you want to delete this Recieve?');";
            }
        }
        protected void grvVehicleRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvVehicleRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.SaveOrUpdateVehicleRequest();
                if (_presenter.CurrentVehicleRequest.VehicleRequestStatuses.Count != 0)
                {
                    BindVehicleRequests();
                    Master.ShowMessage(new AppMessage("Successfully did a Vehicle  Request, Reference No - <b>'" + _presenter.CurrentVehicleRequest.RequestNo + "'</b>", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    Log.Info(_presenter.CurrentUser().FullName + " has requested a Vehicle");
                    btnSave.Visible = false;
                }
                else
                {
                    Master.ShowMessage(new AppMessage("There was an error constructing the approval process", Chai.WorkflowManagment.Enums.RMessageType.Error));
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("Violation of UNIQUE KEY"))
                    {
                        Master.ShowMessage(new AppMessage("Please Click Request button Again,There is a duplicate Number", Chai.WorkflowManagment.Enums.RMessageType.Error));
                        //AutoNumber();
                    }
                }
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            _presenter.DeleteVehicleRequest(_presenter.CurrentVehicleRequest);
            ClearFormFields();
            BindVehicleRequests();
            btnDelete.Enabled = false;
            Master.ShowMessage(new AppMessage("Vehicle Request Successfully Deleted", Chai.WorkflowManagment.Enums.RMessageType.Info));
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindVehicleRequests();
            //pnlSearch_ModalPopupExtender.Show();
            ScriptManager.RegisterStartupScript(this, GetType(), "showSearch", "showSearch();", true);
        }
        protected void btnCancelPopup_Click(object sender, EventArgs e)
        {
            pnlWarning.Visible = false;
            _presenter.CancelPage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("frmVehicleRequest.aspx");
        }
        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopGrants(Convert.ToInt32(ddlProject.SelectedValue));
        }


      
    }
}
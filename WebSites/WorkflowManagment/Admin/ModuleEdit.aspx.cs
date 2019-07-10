using System;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
	public partial class ModuleEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, IModuleEditView
	{
		private ModuleEditPresenter _presenter;
        
		protected void Page_Load(object sender, EventArgs e)
		{        
			if (!this.IsPostBack)
			{
				this._presenter.OnViewInitialized();
                BindControls();
			}
			this._presenter.OnViewLoaded();
		}

		[CreateNew]
		public ModuleEditPresenter Presenter
		{
			get
			{
				return this._presenter;
			}
			set
			{
				if(value == null)
					throw new ArgumentNullException("value");

				this._presenter = value;
				this._presenter.View = this;
			}
		}
        public string GetModuleId
        {
            get { return Request.QueryString[AppConstants.MODULEID]; }
        }
        private void BindControls()
        {
            PocModule module = _presenter.CurrentPocModule;
            this.txtName.Text = module.Name;
            this.txtFolderPath.Text = module.FolderPath;
            this.btnDelete.Visible = (module.Id >0);
            this.btnDelete.Attributes.Add("onclick", "return confirm(\"Are you sure?\")");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.IsValid)
            {
                try
                {
                    _presenter.SaveOrUpdateModule();

                    Master.ShowMessage(new AppMessage("Module was saved successfully", RMessageType.Info));
                }
                catch
                {
                    Master.ShowMessage(new AppMessage("Error: Unable to save Module", RMessageType.Error));
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelPage();
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.DeleteModule();
                Master.TransferMessage(new AppMessage("Module was deleted successfully", RMessageType.Info));
                _presenter.CancelPage();
            }
            catch
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete Module", RMessageType.Error));
            }
        }


        public string GetName
        {
            get { return txtName.Text; }
        }

        public string GetFolderPath
        {
            get { return txtFolderPath.Text; }
        }
    }
}


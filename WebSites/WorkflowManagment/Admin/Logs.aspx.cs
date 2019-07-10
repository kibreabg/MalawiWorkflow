using System;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class Logs : Microsoft.Practices.CompositeWeb.Web.UI.Page, ILogsView
    {
        private LogsPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                UploadFile();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public LogsPresenter Presenter
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

        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }
        private void UploadFile()
        {
            try
            {
                string AuditLog = Server.MapPath("~/AuditTrail.Log");
                string Errorlog = Server.MapPath("~/ErrorExceptions.Log");
                string FailedLoginslog = Server.MapPath("~/FailedLogins.Log");

                IList<Log> attachments = new List<Log>();
                Log l = new Log();
                l.FilePath = AuditLog;
                l.fileName = "Audit Trail";
                attachments.Add(l);
                Log l2 = new Log();
                l2.FilePath = Errorlog;
                l2.fileName = "Exceptions";
                attachments.Add(l2);
                Log l3 = new Log();
                l3.FilePath = FailedLoginslog;
                l3.fileName = "Failed Login Attempts";
                attachments.Add(l3);

                grvAttachments.DataSource = attachments;
                grvAttachments.DataBind();

            }
            catch (Exception ex)
            { 
              
            }
            
        }

        public class Log
        {
            private string _fileName;
            private string _filePath;
            public  string fileName
            {
                get { return _fileName; }
                set { _fileName = value; }
            }
            public string FilePath
            {
                get { return _filePath; }
                set { _filePath = value; }
            }
        }
     
  
}
}


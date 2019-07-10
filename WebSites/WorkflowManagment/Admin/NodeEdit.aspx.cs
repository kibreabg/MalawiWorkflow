using System;
using System.Web.Compilation;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Microsoft.Practices.ObjectBuilder;

using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Admin.Util;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class NodeEdit : Microsoft.Practices.CompositeWeb.Web.UI.Page, INodeEditView
    {
        private NodeEditPresenter _presenter;
        private string _vertualPath;
        private string _pageId;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                btnDelete.Attributes.Add("onclick", "return confirm(\"Ary you sure?\")");
                imbMoveup.ImageUrl = Page.ResolveUrl("~/Admin/Images/up.png");
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public NodeEditPresenter Presenter
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

        private ListOfDirectoryItems GetDirItems(int rootlen, string lPath)
        {
            ListOfDirectoryItems dirItems = new ListOfDirectoryItems(rootlen, lPath);

            if (Directory.Exists(lPath))
            {
                DirectoryInfo dr = new DirectoryInfo(lPath);

                foreach (DirectoryInfo sdr in dr.GetDirectories())
                {
                    dirItems.Add(new DirectoryItem(sdr.FullName, sdr.Name, DirectoryItemType.DIRECTORY));
                }

                foreach (FileInfo fi in dr.GetFiles("*.aspx"))
                {
                    dirItems.Add(new DirectoryItem(fi.FullName, fi.Name, DirectoryItemType.FILE));
                }
            }
            return dirItems;
        }

        private string ReadPageId(string pagePath)
        {
            Type type = BuildManager.GetCompiledType(pagePath);

            // if type is null, could not determine page type
            if (type == null)
                throw new ApplicationException("Page " + pagePath + " not found");

            POCBasePage page = (POCBasePage)Activator.CreateInstance(type);
            return page.PageID;
        }

        public string GetTabId
        {
            get { return Request.QueryString[AppConstants.TABID]; }
        }

        public string GetNodeId
        {
            get { return Request.QueryString[AppConstants.NODEID]; }
        }

        public string GetPageID { get { return _pageId; } }
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!txtFolderpath.Text.Contains(".aspx"))
            {
                Master.ShowMessage(new AppMessage("Error: You must select ASPX page." , Chai.WorkflowManagment.Enums.RMessageType.Error));
                return;
            }

            try
            {
                _pageId = ReadPageId(GetVirtualPath());
                _presenter.SaveOrUpdateNode();
                Master.ShowMessage(new AppMessage("Node was saved successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to save node. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            _presenter.CancelIt();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.DeleteIt();
                Master.ShowMessage(new AppMessage("Node was deleted successfully", Chai.WorkflowManagment.Enums.RMessageType.Info));
            }
            catch (Exception ex)
            {
                Master.ShowMessage(new AppMessage("Error: Unable to delete node. " + ex.Message, Chai.WorkflowManagment.Enums.RMessageType.Error));
            }
        }

        #region INodeEditView Members

        public void BindNode()
        {
            PopModules();
            Node node = _presenter.CurrentNode;

            if (node.Id > 0)
            {
                this.txtTitle.Text = node.Title;
                this.txtDescription.Text = node.Description;
                this.txtFolderpath.Text = node.FilePath.Replace('/', '\\');
                this.txtImagePath.Text = node.ImagePath;
                ddlModule.SelectedValue = node.PocModule.Id.ToString();
                ddlModule.Enabled = false;
                btnDelete.Visible = true;
                _vertualPath = node.NodeUrl;

                string path = _presenter.ServerPath("~/" + _presenter.GetModuleById(int.Parse(ddlModule.SelectedValue)).FolderPath);
                int pathlen = path.Length;
                if (node.FilePath.Contains("/"))
                {
                    int i = node.FilePath.LastIndexOf('/');
                    path += "/" + node.FilePath.Substring(0, i);
                }
                _presenter.DirectoryItems = GetDirItems(pathlen, path);
                PopDirectoryItems();
            }
            else
            {
                btnDelete.Visible = false;
                imbMoveup.Enabled = false;
            }
        }

        private void PopModules()
        {
            ddlModule.DataSource = _presenter.GetModules;
            ddlModule.DataBind();

            ddlModule.Items.Insert(0, new ListItem("---Select Module---", "0"));
            ddlModule.SelectedIndex = 0;
        }

        public void BindRoles()
        {
            this.rptRoles.DataSource = _presenter.GetRoles;
            this.rptRoles.DataBind();
        }

        #endregion

        public void SetRoles(Node node)
        {
            //foreach (NodeRole np in node.NodeRoles)
            //{
            //    node.RemoveNoderole(np.Id);
            //}

            foreach (RepeaterItem ri in rptRoles.Items)
            {
                CheckBox chkView = (CheckBox)ri.FindControl("chkViewAllowed");
                CheckBox chkEdit = (CheckBox)ri.FindControl("chkEditAllowed");
                if (chkView.Checked || chkEdit.Checked)
                {
                    if (!node.Exists(Convert.ToInt32(ViewState[ri.UniqueID])))
                    {
                        NodeRole np = new NodeRole();
                        np.Node = node;
                        np.Role = _presenter.GetRole((int)ViewState[ri.UniqueID]);
                        np.ViewAllowed = chkView.Checked;
                        np.EditAllowed = chkEdit.Checked;

                        node.NodeRoles.Add(np);
                    }
                }
            }
        }

        protected void rptRoles_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Role role = e.Item.DataItem as Role;
            if (role != null)
            {
                CheckBox chkView = (CheckBox)e.Item.FindControl("chkViewAllowed");
                if (_presenter.CurrentNode != null)
                    chkView.Checked = this._presenter.CurrentNode.ViewAllowed(role);

                CheckBox chkEdit = (CheckBox)e.Item.FindControl("chkEditAllowed");
                if (_presenter.CurrentNode != null)
                    chkEdit.Checked = this._presenter.CurrentNode.EditAllowed(role);

                this.ViewState[e.Item.UniqueID] = role.Id;
            }

        }

        #region INodeEditView Members

        public string GetModuleId
        {
            get { return ddlModule.SelectedValue; }
        }

        public string GetTitle
        {
            get { return txtTitle.Text; }
        }

        public string GetDescription
        {
            get { return txtDescription.Text; }
        }

        public string GetFolderPath
        {
            get { return txtFolderpath.Text; }
        }

        public string GetImagePath
        {
            get { return txtImagePath.Text; }
        }

        #endregion

        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlModule.SelectedValue != "0")
            {
                string path = _presenter.ServerPath("~/" + _presenter.GetModuleById(int.Parse(ddlModule.SelectedValue)).FolderPath);
                _presenter.DirectoryItems = GetDirItems(path.Length, path);
            }
            else
            {
                _presenter.DirectoryItems = null;
            }

            PopDirectoryItems();
        }

        private void PopDirectoryItems()
        {
            lstDirectory.Items.Clear();
            imbMoveup.Enabled = false;

            ListOfDirectoryItems ldi = _presenter.DirectoryItems;
            if (ldi != null)
            {
                foreach (DirectoryItem di in ldi.DirectoryItems)
                {
                    if (di.ItemType == DirectoryItemType.DIRECTORY)
                    {
                        lstDirectory.Items.Add(new ListItem(di.FileName));
                    }
                    else
                    {
                        lstDirectory.Items.Add(new ListItem("--> " + di.FileName));
                    }
                }

                if (ldi.FullName.Length > ldi.RootDirLen)
                    imbMoveup.Enabled = true;
            }
        }

        protected void lstDirectory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DirectoryItem di = _presenter.DirectoryItems.DirectoryItems[lstDirectory.SelectedIndex];

            if (di.ItemType == DirectoryItemType.DIRECTORY)
            {
                _presenter.DirectoryItems = GetDirItems(_presenter.DirectoryItems.RootDirLen, di.FolderPath);
                txtFolderpath.Text = di.FolderPath.Substring(_presenter.DirectoryItems.RootDirLen + 1);
                PopDirectoryItems();
            }
            else
            {
                txtFolderpath.Text = di.FolderPath.Substring(_presenter.DirectoryItems.RootDirLen + 0);
            }

        }

        private string GetVirtualPath()
        {
            PocModule m= _presenter.GetModuleById(int.Parse(ddlModule.SelectedValue));
            return "~/" + m.FolderPath + "/" + txtFolderpath.Text;
        }

        protected void imbMoveup_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            ListOfDirectoryItems di = _presenter.DirectoryItems;
            int len = di.FullName.LastIndexOf('\\');
            string str = di.FullName.Substring(0, len);

            _presenter.DirectoryItems = GetDirItems(di.RootDirLen, str);
            txtFolderpath.Text = str.Substring(_presenter.DirectoryItems.RootDirLen);
            PopDirectoryItems();
        }
    }
}


using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Admin.Views;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Modules.Shell;

namespace Chai.WorkflowManagment.Modules.Admin.Views
{
    public partial class Navigation : Microsoft.Practices.CompositeWeb.Web.UI.UserControl
    {

        private BaseMaster GetMaster()
        {
            return Page.Master as BaseMaster;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {        
                hplNewTab.NavigateUrl = Page.ResolveUrl(String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}=0", AppConstants.TABID, AppConstants.NODEID));
            }

            BuildNavigation();
        }

        private void BuildNavigation()
        {
            HtmlGenericControl mainList = ultabs;

            foreach (Tab tab in GetMaster().Presenter.GetListOfAllTabs())
            {
                mainList.Controls.Add(BuildListItemFromTab(tab));
            }
            
          
            //this.plhTabs.Controls.Add(mainList);
        }

        private HtmlControl BuildListItemFromTab(Tab tab)
        {
            HtmlGenericControl listItem = litabs;
            HyperLink hpl = new HyperLink();

            hpl.NavigateUrl = this.Page.ResolveUrl(String.Format("~/Admin/TabEdit.aspx?{0}=0&{1}={2}", AppConstants.TABID,AppConstants.NODEID, tab.Id));
            hpl.Text = tab.TabName;
            listItem.Controls.Add(hpl);
            return listItem;
        }

    }
}


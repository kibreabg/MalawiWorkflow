using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Linq;

using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class NodeNavigation : Microsoft.Practices.CompositeWeb.Web.UI.UserControl
    {
        private Tab _tab;

        private BaseMaster GetMaster()
        {
            return Page.Master as BaseMaster;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _tab = GetMaster().Presenter.ActiveTab;
            BindPanel();
        }
   

        public void BindPanel()
        {
            if (_tab != null)
            {
                this.rptPanel.DataSource = _tab.TaskPans;
                this.rptPanel.DataBind();
            }
        }

        protected void rptPanel_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            TaskPan pan  = e.Item.DataItem as TaskPan;
            if (pan != null)
            {
                Literal ltr = e.Item.FindControl("ltrTitle") as Literal;
                ltr.Text = pan.Title;

                PlaceHolder plh = e.Item.FindControl("plhNode") as PlaceHolder;
                BuildNavigation(pan.TaskPanNodes.ToList(), plh);
            }
        }

        private void BuildNavigation(IList<TaskPanNode> iList, PlaceHolder plh)
        {
            HtmlGenericControl mainlist = new HtmlGenericControl("ul");
            mainlist.Attributes.Add("class", "side-menu");
            foreach (TaskPanNode pn in iList)
            {
                if (pn.Node.ViewAllowed(GetMaster().Presenter.CurrentUser))
                {
                    mainlist.Controls.Add(BuildListItemFromNode(pn.Node));
                }
            }

            plh.Controls.Add(mainlist);
        }

        
        private HtmlControl BuildListItemFromNode(Node node)
        {
            HtmlGenericControl listItem = new HtmlGenericControl("li");
            HyperLink hpl = new HyperLink();
            hpl.NavigateUrl = this.Page.ResolveUrl(String.Format("{0}?{1}={2}", node.NodeUrl, AppConstants.TABID, _tab.Id));
            hpl.Text = node.Title;
            listItem.Controls.Add(hpl);
            return listItem;
        }

      
    }
}


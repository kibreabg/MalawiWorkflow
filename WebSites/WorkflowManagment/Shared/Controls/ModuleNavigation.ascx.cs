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
using Chai.WorkflowManagment.Services;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class ModuleNavigation : Microsoft.Practices.CompositeWeb.Web.UI.UserControl
    {
        private BaseMaster GetMaster()
        {
            return Page.Master as BaseMaster;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BuildNavigation();
        }
        

        private void BuildNavigation()
        {
            HtmlGenericControl mainList = new HtmlGenericControl("ul");

            mainList.Attributes["Id"] = "navigation";
            mainList.Attributes["class"] = "sf-navbar sf-js-enabled";

            HtmlGenericControl listItem = new HtmlGenericControl("li");
            HyperLink hpl = new HyperLink();
            hpl.NavigateUrl = this.Page.ResolveUrl(string.Format("~/Default.aspx?{0}=0", AppConstants.TABID));
            //hpl.Text = "Dashboard";
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "menu-item-parent";
            span.InnerText = "Dashboard";
            HtmlGenericControl inline = new HtmlGenericControl("i");
            inline.Attributes["class"] = "fa fa-lg fa-fw fa-home";
            hpl.Controls.Add(inline);
            hpl.Controls.Add(span);

            listItem.Controls.Add(hpl);
            mainList.Controls.Add(listItem);

            foreach(Tab tab in  GetMaster().Presenter.GetListOfAllTabs())
            {
                if (tab.ViewAllowed(GetMaster().Presenter.CurrentUser))
                {
                    mainList.Controls.Add(BuildListItemFromTab(tab,mainList));
                }
            }

            if (GetMaster().Presenter.CurrentUser != null && GetMaster().Presenter.CurrentUser.HasPermission(AccessLevel.Administrator))
                
            {
                //HtmlGenericControl listItem = new HtmlGenericControl("li");
                //HyperLink hpl = new HyperLink();
                //hpl.NavigateUrl = this.Page.ResolveUrl(string.Format("~/Admin/Default.aspx?{0}=0", AppConstants.TABID));
                //hpl.Text = "Admin";
                //listItem.Controls.Add(hpl);
                //mainList.Controls.Add(listItem);
            }
          
            this.plhNavigation.Controls.Add(mainList);
        }

        private HtmlControl BuildListItemFromTab(Tab tab,HtmlGenericControl mainlist)
        {
             HtmlGenericControl listItem = new HtmlGenericControl("li");
             HyperLink hpl = new HyperLink();

             HtmlGenericControl inline = new HtmlGenericControl("i");
             inline.Attributes["class"] = "fa fa-lg fa-fw fa-home";

            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes["class"] = "menu-item-parent";
            span.InnerText = tab.TabName;

            hpl.Controls.Add(inline);
            
            hpl.Controls.Add(span);

            listItem.Controls.Add(hpl);
            DataBind(tab, listItem);
            mainlist.Controls.Add(listItem);
            
            if (tab.Id.ToString() == GetMaster().TabId)
            {
                listItem.Attributes.Add("class", "open");
                HtmlGenericControl b = new HtmlGenericControl("b");
                HtmlGenericControl em = new HtmlGenericControl("em");
                b.Attributes.Add("class", "collapse-sign");
                em.Attributes.Add("class", "fa fa-minus-square-o");
                b.Controls.Add(em);
                hpl.Controls.Add(b);
            }

            //hpl.NavigateUrl = this.Page.ResolveUrl(String.Format("~/{0}/Default.aspx?{1}={2}", tab.PocModule.FolderPath, AppConstants.TABID, tab.Id));
            //hpl.Text = tab.TabName;
          
            //listItem.Controls.Add(hpl);

            
            if (tab.PopupMenus.Count > 0)
            {
                listItem.Controls.Add(BuildListFromPopupMenus(tab.PopupMenus.ToList()));
            }
            return listItem;
        }

        private HtmlControl BuildListFromPopupMenus(IList<PopupMenu> pmenus)
        {
            HtmlGenericControl list = new HtmlGenericControl("ul");
            foreach (PopupMenu p in pmenus)
            {
                list.Controls.Add(BuildListItemFromPopupMenu(p));
            }

            return list;
        }
   
        private HtmlControl BuildListItemFromPopupMenu(PopupMenu pm)
        {            
            HtmlGenericControl listItem = new HtmlGenericControl("li");
            HyperLink hpl = new HyperLink();
            hpl.NavigateUrl = this.Page.ResolveUrl(String.Format("{0}?{1}={2}", pm.Node.NodeUrl, AppConstants.TABID, pm.Tab.Id));
            hpl.Text = pm.Node.Title;
            listItem.Controls.Add(hpl);
            
            return listItem;
        }

        // public void BindPanel()
        //{
        //    if (_tab != null)
        //    {
        //        this.rptPanel.DataSource = _tab.TaskPans;
        //        this.rptPanel.DataBind();
        //    }
        //}
                

        private void BuildSubMenuNavigation(IList<TaskPanNode> iList,HtmlGenericControl parentlistItem)
        {
            HtmlGenericControl mainlist = new HtmlGenericControl("ul");
          
            foreach (TaskPanNode pn in iList)
            {
                if (pn.Node.ViewAllowed(GetMaster().Presenter.CurrentUser))
                {
                    BuildSubMenuListItemFromNode(pn.Node, pn.TaskPan.Tab.Id, mainlist);
                }
            }
           parentlistItem.Controls.Add(mainlist);
            //parentlistItem.Controls.Add(mainlist);
        }
        
        private HtmlControl BuildSubMenuListItemFromNode(Node node, int TabId, HtmlGenericControl parentlistItem)
        {
            HtmlGenericControl listItem = new HtmlGenericControl("li");
            HyperLink hpl = new HyperLink();
            hpl.NavigateUrl = this.Page.ResolveUrl(String.Format("{0}?{1}={2}&{3}={4}", node.NodeUrl, AppConstants.TABID, TabId, AppConstants.NODEID, node.Id));
            hpl.Text = node.Title;
            if (node.Id.ToString() == GetMaster().NodeId)
            {
                listItem.Attributes.Add("class", "active");
            }
            listItem.Controls.Add(hpl);
            parentlistItem.Controls.Add(listItem);
            return parentlistItem;
        }

        private void DataBind(Tab tab, HtmlGenericControl li)
        {
          foreach(TaskPan pan in tab.TaskPans)
            if (pan != null)
            {
                //Literal ltr = ltrTitle;
                //ltr.Text = pan.Title;

                //PlaceHolder plh = plhNode;
                BuildSubMenuNavigation(pan.TaskPanNodes.ToList(),li);
            }
        }
    }
}


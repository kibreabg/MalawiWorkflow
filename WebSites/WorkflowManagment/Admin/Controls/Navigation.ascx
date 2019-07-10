<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Navigation.ascx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.Navigation" %>


<ul>
					<li>
						<a href="#" title="Admin"><i class="fa fa-lg fa-fw fa-home"></i> <span class="menu-item-parent">Admin</span></a>
                        <ul style="display:block">
                            
                            <li><asp:hyperlink id="hplModules" navigateurl="../Modules.aspx" runat="server">Manage Modules</asp:hyperlink></li>
                            <li><a href="#">Tabs</a>
                               <ul runat="server" id="ultabs">
                                   <li runat="server" id="litabs" ></li>
                                   <li><asp:hyperlink id="hplNewTab" navigateurl="../TabEdit.aspx" runat="server">Add New Tab</asp:hyperlink></li>
                               </ul></li> 
                            <li><asp:hyperlink id="hplNodes" navigateurl="../Nodes.aspx" runat="server">Manage Nodes</asp:hyperlink></li>
   	                        <li><asp:hyperlink id="hplRoles" navigateurl="../Roles.aspx" runat="server">Manage Roles</asp:hyperlink></li>
         	                <li><asp:hyperlink id="hplUsers" navigateurl="../Users.aspx" runat="server">Manage Users</asp:hyperlink></li>
                            <li><asp:hyperlink id="hplLogs" navigateurl="../Logs.aspx" runat="server">Manage Logs</asp:hyperlink></li>
                        </ul>

		 
    </ul>

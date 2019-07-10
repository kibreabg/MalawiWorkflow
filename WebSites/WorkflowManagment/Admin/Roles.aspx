
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.Roles"
    Title="Roles" MasterPageFile="~/Shared/AdminMaster.master" %>
   <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
 <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Roles</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
                                    </fieldset>
                                    <footer>
                                       <asp:button id="btnNew" runat="server" text="Add new role" onclick="btnNew_Click" class="btn btn-primary"></asp:button>
                     </footer>
                                 </div>
                                </div>  </div>
     <div class="table-responsive">
        <table class="table table-striped table-bordered table-hover" >
				<asp:repeater id="rptRoles" runat="server" 
                    onitemdatabound="rptRoles_ItemDataBound">
					<headertemplate>
						<tr>
						    <th></th>
							<th>Role name</th>
							<th>Permission level(s)</th>
							<th></th>
						</tr>
					</headertemplate>
					<itemtemplate>
						<tr>
						    <td><asp:Image Width="14" Height="12" ImageAlign="Middle" runat="server" ID="imgRole" /></td>
							<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
							<td><asp:label id="lblPermissions" runat="server"></asp:label></td>
							<td>
								<asp:hyperlink id="hplEdit" runat="server">Edit</asp:hyperlink>
							</td>
						</tr>
					</itemtemplate>
				</asp:repeater>
			</table>
			</div>
			<div>
				
			</div>
</asp:Content>

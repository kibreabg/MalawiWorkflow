<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NodeNavigation.ascx.cs" Inherits="Chai.WorkflowManagment.Modules.Shell.Views.NodeNavigation" %>
<asp:Repeater ID="rptPanel" runat="server" onitemdatabound="rptPanel_ItemDataBound">

    <ItemTemplate>
       <%-- <div class="navsection">
            <div style="background-color: Navy; color: White">
            <table width="100%" >
                <tr>
                    <td >--%>
                      <div class="portlet ui-widget ui-widget-content ui-helper-clearfix ui-corner-all">
                    <div class="portlet-header ui-widget-header"><asp:Literal ID="ltrTitle" runat="server" />
				   <span class="ui-icon ui-icon-circle-arrow-s"></span>
					  </div>
                        
                   <%-- </td>
                    <td width="20px" align="center">
                       &nbsp;
                    </td>
                </tr>
                
                </table>
            </div>
            <div id="nav">--%>
                <asp:PlaceHolder ID="plhNode" runat="server"></asp:PlaceHolder>
          <%--  </div>
        </div>
        <br />--%>
        </div>
    </ItemTemplate>
</asp:Repeater>

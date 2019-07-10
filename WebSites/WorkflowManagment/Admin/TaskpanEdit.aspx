
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TaskpanEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.TaskpanEdit"
    Title="TaskpanEdit" MasterPageFile="~/Shared/AdminMaster.master" %>
    <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>   Task Pan Property</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">   
   <label class="label">Tab Name</label> 
 <label class="input">
       
         
                    <asp:Label ID="lblTabname" runat="server" Text="Label"></asp:Label></label></section></div>
             <div class="row">
									<section class="col col-6">   
   <label class="label">Task Pan Title</label> 
 <label class="input">
          
                    
            
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                        ID="rfvTitle" runat="server" ErrorMessage="Title is required" Display="Dynamic"
                        CssClass="validator" ControlToValidate="txtTitle" EnableClientScript="False"></asp:RequiredFieldValidator></label></section></div>
              <div class="row">
									<section class="col col-6">   
   <label class="label">Image Path</label> 
 <label class="input">
                    
                    <asp:TextBox ID="txtImage" runat="server"></asp:TextBox></label></section></div>
               </fieldset></div></div></div></div>
     
      <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>   Task Pan Nodes</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
    <table class="tbl">
        <tr>
        <td>List of Nodes</td>
            <td>
                <label class="select">
                <asp:DropDownList ID="ddlNodes" runat="server">
                </asp:DropDownList><i></i></label>
                
            </td>
            <td><asp:Button ID="butAddaction" runat="server" Text="Add" Cssclass="btn btn-primary"
                    OnClick="butAddaction_Click" Width="60px" Enabled="False" /></td>
        </tr>
        
    </table>
   
    <asp:GridView ID="grvNodes" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
        GridLines="Horizontal"  OnRowDataBound="grvNodes_RowDataBound"
        PageSize="10" onrowcommand="grvNodes_RowCommand">
        
        <Columns>
            <asp:TemplateField HeaderText="Node Title" ItemStyle-Width="300px">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem,"Node.Title") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ASPX PAge">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Node.NodeUrl")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Node.Description")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtDelete"  runat="server" CommandName="Delete" Text="Remove" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:ImageButton ID="ibtUp" runat="server" ImageAlign="AbsMiddle" Width="16px" 
                        Height="16px" CommandName="MoveUp" ImageUrl="~/Admin/Images/up.png"/>
                    <asp:ImageButton ID="ibtDown" runat="server" ImageAlign="AbsMiddle" 
                        Width="16px" Height="16px" CommandName="MoveDown" 
                        ImageUrl="~/Admin/Images/down.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
         <PagerStyle Cssclass="paginate_button active" HorizontalAlign="Center" />
    </asp:GridView>
           
    
        <footer>
        <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" Cssclass="btn btn-primary"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" Cssclass="btn btn-primary"
            Text="Cancel" onclick="btnCancel_Click"></asp:Button>
        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" Cssclass="btn btn-primary"
            Text="Delete" onclick="btnDelete_Click" Enabled="False"></asp:Button>
        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" 
            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
        </cc1:ConfirmButtonExtender>
   </footer></div>
     </div></div></div>
</asp:Content>

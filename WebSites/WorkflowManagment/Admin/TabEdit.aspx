<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TabEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.TabEdit"
    Title="TabEdit" MasterPageFile="~/Shared/AdminMaster.master" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
  <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Module Edit</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">   
   <label class="label">Tab Module</label> 
 <label class="select">
       
                    
                
                    <asp:DropDownList ID="ddlModule" runat="server" DataValueField="Id" 
                        DataTextField="Name">
                    </asp:DropDownList><i></i></label></section></div>
<div class="row">
               <section class="col col-6">   
               <label class="label"> Tab Title</label> 
 <label class="input">
       
                   
               
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                        ID="rfvTitle" runat="server" ErrorMessage="Title is required" Display="Dynamic"
                        CssClass="validator" ControlToValidate="txtTitle" EnableClientScript="False"></asp:RequiredFieldValidator></label></section></div>
                <div class="row">
               <section class="col col-6">   
               <label class="label"> Description</label> 
 <label class="input">
                    
                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></label></section></div>
              <div class="row">
               <section class="col col-6">   
               <label class="label">Position</label> 
  

                <asp:ImageButton ID="ibtUp" runat="server" AlternateText="Up" 
                        ImageUrl="~/Admin/Images/up.png" ToolTip="Move Up" onclick="ibtUp_Click" 
                        Height="24px" Width="24px" />
                &nbsp;<asp:ImageButton ID="ibtDown" runat="server" AlternateText="Down" 
                        ImageUrl="~/Admin/Images/down.png" ToolTip="Move Down" 
                        onclick="ibtDown_Click" Height="24px" Width="24px" /></section></div>
</fieldset></div></div></div></div>
 <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>  Task Pans [ <asp:HyperLink ID="hplAddtaskpan" Cssclass="btn btn-primary"
                runat="server">Add New Task Pan</asp:HyperLink>&nbsp;]</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
										

    <asp:GridView ID="grvTaskpans" runat="server" AutoGenerateColumns="False" CellPadding="3" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
        ForeColor="#333333" GridLines="Horizontal"  OnRowDataBound="grvNodes_RowDataBound"
        PageSize="10" onrowcommand="grvTaskpans_RowCommand">
       
        <Columns>
            <asp:TemplateField HeaderText="Task Pan Title">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem,"Title") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem,"ImagePath") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:HyperLink ID="hplEdit" runat="server">Edit</asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate >
                    <asp:ImageButton ID="ibtUp" runat="server" ImageAlign="AbsMiddle" Width="16px" Height="16px" CommandName="MoveUp" ImageUrl="~/Admin/Images/up.png"/>
                    <asp:ImageButton ID="ibtDown" runat="server" ImageAlign="AbsMiddle" Width="16px" Height="16px" CommandName="MoveDown" ImageUrl="~/Admin/Images/down.png" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
         <PagerStyle Cssclass="paginate_button active" HorizontalAlign="Center" />
    </asp:GridView>
    </div>
    </div>

    

    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2> Popup Menus </h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
           
            
    <table class="tbl">
        <tr>
            <td>
                <label class="select">
                <asp:DropDownList ID="ddlNodes" runat="server">
                </asp:DropDownList><i></i></label>
                <asp:Button ID="butAddaction" runat="server" Text="Add" OnClick="butAddaction_Click" Cssclass="btn btn-primary"/>
                <asp:Button ID="butRemoveaction" runat="server" OnClick="butRemoveaction_Click" Text="Remove" Cssclass="btn btn-primary"
                     />
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lsbNodes" runat="server" Height="147px" Width="350px"></asp:ListBox>
            </td>
        </tr>
    </table>
    </div>
</div></div>
   <h4> Authorization</h4>      
    <div class="table-responsive">
       
        <table class="table table-striped table-bordered table-hover">
          
        
            <asp:Repeater ID="rptRoles" runat="server" 
                onitemdatabound="rptRoles_ItemDataBound">
                <HeaderTemplate>
                    <tr>
                        <th>
                            Role
                        </th>
                        <th>
                            View allowed
                        </th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "Name") %>
                        </td>
                        <td style="text-align: center">
                            <asp:CheckBox ID="chkViewAllowed" runat="server"></asp:CheckBox>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
        </div>

       <footer>
        <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" class="btn btn-primary"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" CausesValidation="False" class="btn btn-primary"
            Text="Cancel" onclick="btnCancel_Click"></asp:Button>
        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
            Text="Delete" onclick="btnDelete_Click" Enabled="False"></asp:Button>
        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server" 
            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
        </cc1:ConfirmButtonExtender>
  </footer>
   
</asp:Content>

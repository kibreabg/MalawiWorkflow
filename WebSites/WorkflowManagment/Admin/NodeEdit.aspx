<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NodeEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.NodeEdit"
    Title="NodeEdit" MasterPageFile="~/Shared/AdminMaster.master" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
 
    <div class="alert alert-block alert-success">
		<h4 class="alert-heading">Information</h4>
	<p>
        Manage the properties of the node (page). Use the buttons on the bottom of the page
        to save or delete the page.</p>
        </div>
     <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Node</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">   
    
                    <label class="label">Node Module</label> 
                    <label class="select">
           
        
                
                    <asp:DropDownList ID="ddlModule" runat="server" DataValueField="Id" 
                        DataTextField="Name" 
                        onselectedindexchanged="ddlModule_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList><i></i></label></section></div>
               <div class="row">
									<section class="col col-6">   
    
                    <label class="label">Node Title</label> 
                    <label class="input">
                     
               
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                        ID="rfvTitle" runat="server" ErrorMessage="Title is required" Display="Dynamic"
                        CssClass="validator" ControlToValidate="txtTitle" EnableClientScript="False"></asp:RequiredFieldValidator></label></section></div>
<div class="row">
									<section class="col col-6">   
    
                    <label class="label">Description</label> 
                    <label class="input">
                  
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></label></section></div>
     <div class="row">
									<section class="col col-6">   
    
                    <label class="label">ASPX Page</label> 
<label class="input">          
                    
                    <asp:TextBox ID="txtFolderpath" runat="server" ReadOnly="True"></asp:TextBox>
                              
                    <asp:ListBox ID="lstDirectory" runat="server"
                        onselectedindexchanged="lstDirectory_SelectedIndexChanged" Width="520px" 
                        AutoPostBack="True">
                    </asp:ListBox> </label></section><asp:ImageButton ID="imbMoveup" runat="server" 
                        onclick="imbMoveup_Click" ToolTip="Move Up" /></div>
       <div class="row">
									<section class="col col-6">   
         <label class="label">Image Path</label>  
         <label class="input">        
                   
               
              
                    <asp:TextBox ID="txtImagePath" runat="server"></asp:TextBox></label></section></div>
               </fieldset>

      <h4>
              Authorization</h4>      
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
                        <th>
                            Edit allowed
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
                        <td style="text-align: center">
                            <asp:CheckBox ID="chkEditAllowed" runat="server"></asp:CheckBox>
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
   </footer></div></div></div></div>
</asp:Content>

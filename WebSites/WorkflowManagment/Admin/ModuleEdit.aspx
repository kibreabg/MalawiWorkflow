
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ModuleEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.ModuleEdit"
    Title="ModuleEdit" MasterPageFile="~/Shared/AdminMaster.master" %>
    <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
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
   
                    <label class="label">Module Name</label>
                    <label class="input">
            
           
						<asp:textbox id="txtName" runat="server"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator" display="Dynamic" enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator></label></section></div>
					<div class="row">
									<section class="col col-6">   
   
                    <label class="label">Folder Path</label>
                    <label class="input">
				
					<asp:textbox id="txtFolderPath" runat="server"></asp:textbox></label></section></div></fieldset>
						<footer>
			<asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click" class="btn btn-primary"></asp:button>
			<asp:Button id="btnCancel" runat="server" Text="Cancel" causesvalidation="false" onclick="btnCancel_Click" class="btn btn-primary"></asp:Button>
			<asp:Button id="btnDelete" runat="server" Text="Delete" causesvalidation="false" class="btn btn-primary"
                    onclick="btnDelete_Click"></asp:Button> </footer></div></div></div></div>
		
</asp:Content>

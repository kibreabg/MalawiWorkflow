
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RoleEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.RoleEdit"
    Title="RoleEdit" MasterPageFile="~/Shared/AdminMaster.master" %>
    <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
 
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" Runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Edit Role</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">       
						<asp:Label ID="lblName" runat="server" Text="Name" CssClass="label"></asp:Label>
             <label class="input">
						<asp:textbox id="txtName" runat="server"></asp:textbox>
							<asp:requiredfieldvalidator id="rfvName" runat="server" errormessage="Name is required" cssclass="validator" display="Dynamic" enableclientscript="False" controltovalidate="txtName"></asp:requiredfieldvalidator>
					</label>
					</section></div>
<div class="row">
                        <section class="col col-6">
						<asp:Label ID="lblPermission" runat="server" Text="Permission" CssClass="label"></asp:Label>
					
					<asp:checkboxlist id="cblRoles" runat="server" repeatlayout="Flow"></asp:checkboxlist>

					</section>
                         </div>
                          </fieldset>
                                                       <footer>
			
			<asp:button id="btnSave" runat="server" text="Save" onclick="btnSave_Click" Cssclass="btn btn-primary"></asp:button>
			<asp:Button id="btnCancel" runat="server" Text="Cancel" causesvalidation="false" onclick="btnCancel_Click" Cssclass="btn btn-primary"></asp:Button>
			<asp:Button id="btnDelete" runat="server" Text="Delete" causesvalidation="false" Cssclass="btn btn-primary" OnClick="btnDelete_Click"></asp:Button>
			</footer>
        </div>
                                </div>
                                </div></div>
</asp:Content>

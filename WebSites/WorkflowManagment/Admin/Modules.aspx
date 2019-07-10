
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Modules.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.Modules"
    Title="Modules"MasterPageFile="~/Shared/AdminMaster.master" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
       <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Modules</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								
                
                          </fieldset>
                          <footer>    
                                <asp:HyperLink ID="hplNewnode" runat="server" Cssclass="btn btn-primary">Register New Module</asp:HyperLink>             
        
                              </footer>
  </div>
   </div>
                                </div>
      
      
       
        <asp:GridView ID="grvNodes" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
            CellPadding="3" ForeColor="#333333" GridLines="Horizontal" Width="100%" 
           OnRowDataBound="grvNodes_RowDataBound" PageSize="10">
            
            
            <Columns>
                <asp:TemplateField HeaderText="Module Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"Name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Folder Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"FolderPath") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:HyperLink ID="hplEdit" runat="server">Edit</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    <PagerStyle Cssclass="paginate_button active" HorizontalAlign="Center" />
        </asp:GridView>
      
        </div>
</asp:Content>
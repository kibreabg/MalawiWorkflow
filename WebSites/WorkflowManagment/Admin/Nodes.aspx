<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Nodes.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.Nodes"
    Title="Nodes" MasterPageFile="~/Shared/AdminMaster.master" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
 <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Nodes</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">       
       <label class="label"> Module Name</label> 
 <label class="select">
                   
                    <asp:DropDownList ID="ddlModule" runat="server" 
                        DataTextField="Name" DataValueField="Id">
       </asp:DropDownList><i></i></label></section></div>
                               </fieldset><footer> 
                                   <asp:HyperLink ID="hplNewnode" runat="server" Cssclass="btn btn-primary">Register New Node</asp:HyperLink>
       <asp:Button ID="butFiliter" runat="server" Text="List" onclick="butFiliter_Click" Cssclass="btn btn-primary" />
    
                   
                </footer>
  </div>
   </div>
                                </div>
       
        <asp:GridView ID="grvNodes" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
            CellPadding="3" ForeColor="#333333" GridLines="Horizontal"
           OnRowDataBound="grvNodes_RowDataBound" PageSize="10">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:TemplateField HeaderText="File Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"FilePath") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Node Title">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"Title") %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"ImagePath") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem,"Description") %>
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

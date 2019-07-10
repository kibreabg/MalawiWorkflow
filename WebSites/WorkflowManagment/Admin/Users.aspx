<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.Users"
    Title="Users"  MasterPageFile="~/Shared/AdminMaster.master" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
     <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
                     <header>
					        <span class="widget-icon"> <i class="fa fa-edit"></i> </span>
					        <h2>Find Users</h2>				
				    </header>
                    <div>								
					<div class="jarviswidget-editbox"></div>	
						<div class="widget-body no-padding">
                         <div class="smart-form">
                           <fieldset>					
								<div class="row">
									<section class="col col-6">       
       <label class="label">Username</label> 
 <label class="input">
        <asp:TextBox ID="txtUsername" runat="server" ></asp:TextBox></label></section>
                </div>
                          </fieldset>
                          <footer>                     
         <asp:Button ID="btnNew" runat="server" Text="Add New User" onclick="btnNew_Click" Cssclass="btn btn-primary" ></asp:Button>
        <asp:Button ID="btnFind" runat="server" Text="Find" onclick="btnFind_Click" Cssclass="btn btn-primary"></asp:Button>
                              </footer>
  </div>
   </div>
                                </div>
 
<asp:GridView ID="grvUser" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover"  PagerStyle-CssClass="paginate_button active" AlternatingRowStyle-CssClass=""
            AutoGenerateColumns="False" CellPadding="3" ForeColor="#333333" 
            GridLines="Horizontal" Width="100%"  OnRowDataBound="grvUser_RowDataBound"          
             onpageindexchanging="grvUser_PageIndexChanging"  PageSize="10">
            <PagerSettings FirstPageImageUrl="~/Images/arrow_beg.gif" 
                LastPageImageUrl="~/Images/arrow_end.gif" 
                NextPageImageUrl="~/Images/arrow_right.gif" 
                PreviousPageImageUrl="~/Images/arrow_left.gif" />
           
            <Columns>
                <asp:TemplateField HeaderText="Username">
                <ItemTemplate>                
                <asp:Label ID="lblUsername" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"Username") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Firstname">
                <ItemTemplate>                
                <asp:Label ID="lblFirstname" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"firstname") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lastname">
                <ItemTemplate>                
                <asp:Label ID="lblLastname" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"lastname") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                <ItemTemplate>                
                <asp:Label ID="lblEmail" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"email") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last login date">
                <ItemTemplate>                
                <asp:Label ID="lblLastlogin" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"LastLogin") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Last login from">
                <ItemTemplate>                
                <asp:Label ID="lblloginfrom" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"LastIp") %>'></asp:Label>
                </ItemTemplate>                
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                <ItemTemplate>               
                <asp:Label ID="lblStatus" runat ="server" Text ='<%# DataBinder.Eval(Container.DataItem,"IsActive") %>'></asp:Label>
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

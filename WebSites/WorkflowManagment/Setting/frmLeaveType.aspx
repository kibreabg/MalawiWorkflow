<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmLeaveType.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmLeaveType" %>
 <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" Runat="Server">
      <asp:ValidationSummary ID="NewValidationSummary1" runat="server" 
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph" 
        ValidationGroup="2" ForeColor="" />
        <asp:ValidationSummary ID="EditValidationSummary2" runat="server" 
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph" 
        ValidationGroup="1" ForeColor="" />
   <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
          
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Leave Types</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    
                    <footer>
                        
                     <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    
                    </footer>
                </div>
            </div>
        </div>
		

    <asp:DataGrid ID="dgLeaveType" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" 
        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" 
         GridLines="None"
        oncancelcommand="dgLeaveType_CancelCommand" ondeletecommand="dgLeaveType_DeleteCommand" oneditcommand="dgLeaveType_EditCommand" 
        onitemcommand="dgLeaveType_ItemCommand" onitemdatabound="dgLeaveType_ItemDataBound" onupdatecommand="dgLeaveType_UpdateCommand" 
         ShowFooter="True">
   
        <Columns>
            <asp:TemplateColumn HeaderText="Leave Type">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "LeaveTypeName")%>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtLeaveTypeName" runat="server" Cssclass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "LeaveTypeName")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ControlToValidate="txtLeaveTypeName" ErrorMessage="LeaveType Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:TextBox ID="txtFLeaveTypeName" runat="server" Cssclass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFLeaveTypeName" ErrorMessage="Leave Type Name Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                </FooterTemplate>
            </asp:TemplateColumn>
            
            <asp:TemplateColumn  HeaderText="Actions">
                <EditItemTemplate>
                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" Cssclass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                   <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Cssclass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                </EditItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="2" Cssclass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                </FooterTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" Cssclass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" Cssclass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" ><i class="fa fa-times"></i></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
           <PagerStyle  Cssclass="paginate_button active" HorizontalAlign="Center" />
    </asp:DataGrid>
    </div>
		
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
</asp:Content>


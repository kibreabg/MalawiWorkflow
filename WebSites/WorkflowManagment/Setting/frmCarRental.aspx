<%@ Page Title="Car Rental Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmCarRental.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmCarRental" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ValidationSummary ID="NewValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="2" ForeColor="" />
    <asp:ValidationSummary ID="EditValidationSummary2" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="1" ForeColor="" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Car Rental</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblCarRentalName" runat="server" Text="Car Rental Name" CssClass="label"></asp:Label>

                                <label class="input">

                                    <asp:TextBox ID="txtSrchCarRentalName" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                                                <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    
                    </footer>
                </div>
            </div>
        </div>


        <asp:DataGrid ID="dgCarRental" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
            GridLines="None"
            OnCancelCommand="dgCarRental_CancelCommand" OnDeleteCommand="dgCarRental_DeleteCommand" OnEditCommand="dgCarRental_EditCommand"
            OnItemCommand="dgCarRental_ItemCommand" OnItemDataBound="dgCarRental_ItemDataBound" OnUpdateCommand="dgCarRental_UpdateCommand"
            ShowFooter="True">

            <Columns>
                <asp:TemplateColumn HeaderText="Car Rental Name">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "Name")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtCarRentalName" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEdtCarRentalName" ErrorMessage="Car Rental Name Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCarRentalName" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtCarRentalName" ErrorMessage="Car Rental Name Required" ValidationGroup="2">*</asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Car Rental Phone Number">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "PhoneNo")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtPhoneNo" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "PhoneNo")%>'></asp:TextBox>
                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtPhoneNo" ID="txtEdtPhoneNo_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtPhoneNo" ID="txtPhoneNo_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Car Rental Contact Address">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ContactAddress")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtContactAddress" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ContactAddress")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtContactAddress" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Actions">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="2" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
        </asp:DataGrid>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

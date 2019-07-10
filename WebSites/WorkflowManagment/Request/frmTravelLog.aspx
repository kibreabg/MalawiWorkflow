<%@ Page Title="Travel Log List" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmTravelLog.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmTravelLog" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
     <script type="text/javascript">
         function Clickheretoprint(theid) {
             var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
             disp_setting += "scrollbars=yes,width=750, height=600, left=100, top=25";
             var content_vlue = document.getElementById(theid).innerHTML;

             var docprint = window.open("", "", disp_setting);
             docprint.document.open();
             docprint.document.write('<html><head><title>CHAI Zimbabwe</title>');
             docprint.document.write('</head><body onLoad="self.print()"><center>');
             docprint.document.write(content_vlue);
             docprint.document.write('</center></body></html>');
             docprint.document.close();
             docprint.focus();
         }
    </script>
    <asp:ValidationSummary ID="NewValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="2" ForeColor="" />
    <asp:ValidationSummary ID="EditValidationSummary2" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="1" ForeColor="" />
    <asp:Button ID="btnPrint" runat="server" OnClientClick="javascript:Clickheretoprint('divprint')" Text="Print Travel Log"  CssClass="btn btn-primary" />
    <asp:Button ID="btnBack" runat="server" Text="Back"  CssClass="btn btn-primary" OnClick="btnBack_Click" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Travel Logs List</h2>
        </header>
        <asp:DataGrid ID="dgTravelLog" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
            GridLines="None"
            OnCancelCommand="dgTravelLog_CancelCommand" OnDeleteCommand="dgTravelLog_DeleteCommand" OnEditCommand="dgTravelLog_EditCommand"
            OnItemCommand="dgTravelLog_ItemCommand" OnItemDataBound="dgTravelLog_ItemDataBound" OnUpdateCommand="dgTravelLog_UpdateCommand"
            ShowFooter="True">

            <Columns>
                <asp:TemplateColumn HeaderText="Request No">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "RequestNo") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtRequestNo" runat="server" ReadOnly="true" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "RequestNo") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtRequestNo" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Departure Place">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DeparturePlace") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtDeparturePlace" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "DeparturePlace") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDeparturePlace" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Arrival Place">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ArrivalPlace") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtArrivalPlace" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ArrivalPlace") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtArrivalPlace" runat="server" CssClass="form-control"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Departure Time">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "DepartureTime") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtDepartureTime" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "DepartureTime") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEdtDepartureTime" runat="server" ControlToValidate="txtEdtDepartureTime"
                            ErrorMessage="Departure Time is Required"
                            SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtDepartureTime" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtDepartureTime" runat="server" ControlToValidate="txtDepartureTime"
                            ErrorMessage="Departure Time is Required"
                            SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Arrival Time">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "ArrivalTime") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtArrivalTime" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "ArrivalTime") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEdtArrivalTime" runat="server" ControlToValidate="txtEdtArrivalTime"
                            ErrorMessage="Arrival Time is Required"
                            SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtArrivalTime" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtArrivalTime" runat="server" ControlToValidate="txtArrivalTime"
                            ErrorMessage="Arrival Time is Required"
                            SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Start Km Reading">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "StartKmReading") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtStartKmReading" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "StartKmReading") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEdtStartKmReading" runat="server" ControlToValidate="txtEdtStartKmReading"
                            ErrorMessage="Start Km Reading is Required"
                            SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtStartKmReading" ID="txtEdtStartKmReading_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtStartKmReading" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtStartKmReading" runat="server" ControlToValidate="txtStartKmReading"
                            ErrorMessage="Start Km Reading is Required"
                            SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtStartKmReading" ID="txtStartKmReading_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="End Km Reading">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "EndKmReading") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtEndKmReading" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "EndKmReading") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEdtEndKmReading" runat="server" ControlToValidate="txtEdtEndKmReading"
                            ErrorMessage="End Km Reading is Required"
                            SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtEndKmReading" ID="txtEdtEndKmReading_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtEndKmReading" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEndKmReading" runat="server" ControlToValidate="txtEndKmReading"
                            ErrorMessage="End Km Reading is Required"
                            SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEndKmReading" ID="txtEndKmReading_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Fuel Price">
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "FuelPrice") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEdtFuelPrice" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "FuelPrice") %>'></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtEdtFuelPrice" runat="server" ControlToValidate="txtEdtFuelPrice"
                            ErrorMessage="Fuel Price is Required"
                            SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtFuelPrice" ID="txtEdtFuelPrice_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtFuelPrice" runat="server" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RfvtxtFuelPrice" runat="server" ControlToValidate="txtFuelPrice"
                            ErrorMessage="Fuel Price is Required"
                            SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFuelPrice" ID="txtFuelPrice_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                    </FooterTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Actions">
                    <EditItemTemplate>
                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" ValidationGroup="edit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="lnkAddNew" runat="server" CausesValidation="true" CommandName="AddNew" ValidationGroup="save" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
        </asp:DataGrid>
        <br />
    </div>

    <div id="divprint" style="display:none">

        <table border="2" cellspacing="4" style="width: 100%; border-style: solid;">
            <tr>
                <td>
                    <strong>
                        <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                    </strong></td>
                <td colspan="6">
                    <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>
                        <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                    </strong></td>
                <td colspan="6">
                    <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>
                        <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                    </strong></td>
                <td colspan="6">
                    <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                </td>
            </tr>
            <tr style="font-weight: bold; border-style: solid;">
                <td>Departure Place</td>
                <td>Arrival Place</td>
                <td>Departure Time</td>
                <td>Arrival Time</td>
                <td>Start Km Reading</td>
                <td>End Km Reading</td>
                <td>Fuel Price</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>

    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>

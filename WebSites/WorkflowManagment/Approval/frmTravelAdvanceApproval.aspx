<%@ Page Title="Travel Advance Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmTravelAdvanceApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmTravelAdvanceApproval" EnableEventValidation="false" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
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
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Travel Advance Requests</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestNo" runat="server" Text="Request No" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchRequestNo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestDate" runat="server" Text="Request Date" CssClass="label"></asp:Label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchProgressStatus" runat="server" Text="Status" CssClass="label"></asp:Label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlSrchProgressStatus" runat="server">
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnPop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvTravelAdvanceRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvTravelAdvanceRequestList_RowCommand"
            OnRowDataBound="grvTravelAdvanceRequestList_RowDataBound" OnSelectedIndexChanged="grvTravelAdvanceRequestList_SelectedIndexChanged"
            AllowPaging="True" OnPageIndexChanging="grvTravelAdvanceRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="TravelAdvanceNo" HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Requester" SortExpression="AppUser.FullName" />
                <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="VisitingTeam" HeaderText="Visiting Team" SortExpression="VisitingTeam" />
                <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose of Travel" SortExpression="PurposeOfTravel" />
                <asp:BoundField DataField="TotalTravelAdvance" HeaderText="Total Travel Advance" SortExpression="TotalTravelAdvance" />
                <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project ID" SortExpression="Project.ProjectCode" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnStatus" Text="" BorderStyle="None" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
        <div>
            <asp:Button runat="server" ID="btnInProgress" Text="" BorderStyle="None" BackColor="#FFFF6C" />
            <b>In Progress</b><br />
            <asp:Button runat="server" ID="btnComplete" Text="" BorderStyle="None" BackColor="#FF7251" />
            <b>Completed</b>

        </div>
        <br />
    </div>
    <asp:Panel ID="pnlApproval" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Process Travel Advance Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblReferenceNo" Visible="false" runat="server" CssClass="label">Reference No.</asp:Label>
                                                <asp:Label ID="lblReference" Visible="false" runat="server" CssClass="select">
                                                    <asp:TextBox ID="txtReference" runat="server" ReadOnly="true" Visible="false"></asp:TextBox>
                                                </asp:Label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-4">
                                                <asp:Label ID="lblAccount" Visible="false" runat="server" CssClass="label">Payment Type</asp:Label>
                                                <asp:Label ID="lblAccountdd" Visible="false" runat="server" CssClass="select">
                                                    <asp:DropDownList ID="ddlAccount" runat="server" AppendDataBoundItems="True" DataTextField="Name" DataValueField="Id">
                                                        <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="rfvddlaccount" runat="server" CssClass="validator" ValidationGroup="save" ErrorMessage="Account Required" InitialValue="0" ControlToValidate="ddlAccount"></asp:RequiredFieldValidator>
                                                </asp:Label>
                                            </section>
                                            <section class="col col-4">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>
                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged">
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" runat="server" CssClass="validator" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-4">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="input">
                                                    <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>
                                        <asp:Button ID="btnBankPayment" runat="server" CssClass="btn btn-primary" OnClick="btnBankPayment_Click" Text="Bank Payment" Visible="False" />
                                    </footer>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" PopupControlID="pnlApproval" TargetControlID="btnPop"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>

    <asp:Panel ID="pnlDetail" runat="server">
        <div class="modal-body no-padding">
            <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                <header>
                    <span class="widget-icon"><i class="fa fa-edit"></i></span>
                    <h2>Travel Advance Details</h2>
                </header>
                <div>
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <asp:DataGrid ID="dgTravelAdvanceRequestDetail" runat="server"
                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True" OnSelectedIndexChanged="dgTravelAdvanceRequestDetail_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="City From">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "CityFrom")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="City To">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "CityTo")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="From Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "FromDate","{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="To Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ToDate","{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Mode of Travel">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ModeOfTravel")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="View Costs"></asp:ButtonColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
                <br />
                <div>
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <asp:GridView ID="grvTravelAdvanceCosts"
                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                AllowPaging="True" OnPageIndexChanging="grvTravelAdvanceCosts_PageIndexChanging"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                <RowStyle CssClass="rowstyle" />
                                <Columns>
                                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" />
                                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Request Date" />
                                    <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" />
                                    <asp:BoundField DataField="Days" HeaderText="Days" />
                                    <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" />
                                    <asp:BoundField DataField="NoOfUnits" HeaderText="No Of Units" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" />
                                </Columns>
                                <FooterStyle CssClass="FooterStyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <PagerStyle CssClass="PagerStyle" />
                                <RowStyle CssClass="rowstyle" />
                            </asp:GridView>
                            <footer>
                                <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
                            </footer>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop2" PopupControlID="pnlDetail" CancelControlID="btnCancelPopup2"
        ID="pnlDetail_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE<br />
                            TRAVEL ADVANCE REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%;">

                <tr>
                    <td align="right" style="width: 848px">&nbsp;</td>
                    <td align="right" style="width: 390px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Reference No:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPostingRef" runat="server" Text="Posting Ref:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        _______________
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">
                        <strong>
                            <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px">
                        <asp:Label ID="lblCommentsResult" runat="server"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblVisitingTeam" runat="server" Text="Visiting Team:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblVisitingTeamResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPurposeOfTravel" runat="server" Text="Purpose of Travel:"></asp:Label></strong></td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblPurposeOfTravelResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 390px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblTotalTravelAdvance" runat="server" Text="Total Travel Advance:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblTotalTravelAdvanceResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblGrantId" runat="server" Text="Grant ID:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblGrantIdResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <strong>
                            <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblProjectId" runat="server" Text="Project ID:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblProjectIdResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="CityFrom" HeaderText="City From" SortExpression="CityFrom" />
                    <asp:BoundField DataField="CityTo" HeaderText="City To" SortExpression="CityTo" />
                    <asp:BoundField DataField="HotelBooked" HeaderText="Hotel Booked" SortExpression="HotelBooked" />
                    <asp:TemplateField HeaderText="From Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("FromDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="To Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ToDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="ModeOfTravel" HeaderText="Mode of Travel" SortExpression="ModeOfTravel" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvCost"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseType.ExpenseTypeName" />
                    <asp:BoundField DataField="AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                    <asp:BoundField DataField="Days" HeaderText="Days" SortExpression="Days" />
                    <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                    <asp:BoundField DataField="NoOfUnits" HeaderText="No Of Units" SortExpression="NoOfUnits" />
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses" OnRowDataBound="grvStatuses_RowDataBound"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Name" />
                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <table style="width: 100%;">
                <tr>
                    <td></td>
                    <td>Signature</td>
                    <td></td>
                    <td></td>
                    <td style="text-align:right; padding-right:12%;">Recieved By </td>
                </tr>
                <tr>
                    <td></td>
                    <td>___________________</td>
                    <td></td>
                    <td></td>
                    <td style="text-align:right;">___________________</td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>

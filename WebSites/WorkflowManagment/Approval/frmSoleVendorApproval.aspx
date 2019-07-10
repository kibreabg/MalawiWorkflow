<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmSoleVendorApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmSoleVendorApproval" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script type="text/javascript" language="javascript">
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
            <h2>Search Sole Vendor Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                <label class="input">
                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="label"></asp:Label>

                                <label class="select">
                                    <asp:DropDownList ID="ddlProgressStatus" runat="server"></asp:DropDownList><i></i>

                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnPop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

        <asp:GridView ID="grvSoleVendorRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            OnRowDataBound="grvSoleVendorRequestList_RowDataBound" OnRowDeleting="grvSoleVendorRequestList_RowDeleting"
            OnSelectedIndexChanged="grvSoleVendorRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvSoleVendorRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30" OnRowCommand="grvSoleVendorRequestList_RowCommand">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ContactPersonNumber" HeaderText="Contact Person & Number" SortExpression="ContactPersonNumber" />
                <asp:BoundField DataField="ProposedPurchasedPrice" HeaderText="Proposed Purchased Price" SortExpression="ProposedPurchasedPrice" />
                <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Proposed Supplier" SortExpression="Supplier.SupplierName" />
                <asp:BoundField DataField="SoleVendorJustificationType" HeaderText="Sole Vendor JustificationType" SortExpression="SoleVendorJustificationType" />
                <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project ID" SortExpression="Project.ProjectCode" />
                <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant ID" SortExpression="Grant.GrantCode" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ShowSelectButton="True" SelectText="Process Request" ButtonType="Button" />
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
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>

                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Process Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>
                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged" ValidationGroup="Approve" AutoPostBack="True">
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus" ValidationGroup="Approve"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="textarea">
                                                    <asp:TextBox ID="txtRejectedReason" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblAttachments" runat="server" Text="Attachments" CssClass="label"></asp:Label>
                                                <asp:GridView ID="grvAttachments"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle CssClass="FooterStyle" />
                                                    <HeaderStyle CssClass="headerstyle" />
                                                    <PagerStyle CssClass="PagerStyle" />
                                                    <RowStyle CssClass="rowstyle" />
                                                </asp:GridView>
                                            </section>
                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary" ValidationGroup="Approve"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>
                                        <%--<asp:Button ID="btnPurchaseOrder" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="btnPurchaseOrder_Click" Text="Purchase Order" />--%>
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
        Enabled="True" TargetControlID="btnPop" PopupControlID="pnlApproval" CancelControlID="btnCancelPopup"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlDetail" runat="server">
        <div class="modal-content">
            <div class="modal-body no-padding">
                <div role="content">

                    <!-- widget edit box -->
                    <div class="jarviswidget-editbox">
                        <!-- This area used as dropdown edit box -->

                    </div>
                    <!-- end widget edit box -->

                    <!-- widget content -->
                    <div class="widget-body">
                        <div class="tab-content">
                            <div class="tab-pane" id="hr1">
                                <div class="tabbable tabs-below">
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane" id="AA">
                                        </div>
                                    </div>
                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a data-toggle="tab" href="#AA">Tab 1</a>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                            <div class="tab-pane active" id="hr2">
                                <ul class="nav nav-tabs">
                                    <li class="active">
                                        <a href="#iss1" data-toggle="tab">Item Details</a>
                                    </li>
                                </ul>
                                <div class="tab-content padding-10">
                                    <div class="tab-pane active" id="iss1">
                                        <asp:DataGrid ID="dgSoleVendorRequestDetail" runat="server"
                                            AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                            DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Account Name">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account Code">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Item Description">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Unit Cost">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Total Cost">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                            </Columns>
                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                        </asp:DataGrid>
                                    </div>
                                </div>
                            </div>
                            <footer>
                                <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
                            </footer>
                        </div>

                    </div>
                    <!-- end widget content -->

                </div>
            </div>
        </div>

    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop2" PopupControlID="pnlDetail" CancelControlID="btnCancelPopup2"
        ID="pnlDetail_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <div id="divprint" style="display: none; width: 942px;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            SOLE VENDOR REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%">

                <tr>
                    <td align="right" style="">&nbsp;</td>
                    <td align="right" style="width: 244px" class="inbox-data-from">&nbsp;</td>
                    <td align="right" style="width: 271px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Request No.:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequestNoresult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequestedDateresult" runat="server"></asp:Label>
                    </td>
                    <td align="right" style="width: 334px">&nbsp;</td>
                    <td align="right" style="width: 335px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequesterres" runat="server"></asp:Label>
                    </td>
                    <td style="width: 334px">&nbsp;</td>
                    <td style="width: 335px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 682px; height: 18px;">&nbsp;</td>
                    <td style="width: 244px; height: 18px;">&nbsp;</td>
                    <td style="width: 334px; height: 18px;">&nbsp;</td>
                    <td style="width: 335px; height: 18px;">&nbsp;</td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblContactPersonNumber" runat="server" Text="Contact Person & Number:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblContactPersonNumberRes" runat="server"></asp:Label>
                    </td>
                    <td style="width: 334px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblProposedPurchasedprice" runat="server" Text="Proposed Purchased price:"></asp:Label>
                        </strong></td>
                    <td style="width: 335px; height: 18px;">
                        <asp:Label ID="lblProposedPurchasedpriceres" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 682px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblProposedSupplier" runat="server" Text="Proposed Supplier:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblProposedSupplierresp" runat="server"></asp:Label>
                    </td>
                    <td style="width: 334px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblRequestedSupplier" runat="server" Text="Requested Supplier"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 335px; height: 18px;">
                        <asp:Label ID="lblRequestedSupplierres" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 682px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblSoleSourceJustificationPreparedBy" runat="server" Text="Sole Source Justification PreparedBy:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblSoleSourceJustificationPreparedByresp" runat="server"></asp:Label>
                    </td>
                    <td style="width: 334px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblSoleVendorJustificationType" runat="server" Text="Sole Vendor JustificationType"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 335px; height: 18px;">
                        <asp:Label ID="lblSoleVendorJustificationTyperes" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 18px">&nbsp;</td>
                     <td style="width: 334px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPaymet" runat="server" Text="Payment Method"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblPaymentMethRes" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                    <td style="width: 334px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblApprovalStatusp" runat="server" Text="Approval Status"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblapprovalstatusres" runat="server"></asp:Label>
                    </td>
                    
                </tr>
            </table>
            <br />
            <br />
            <asp:GridView ID="grvStatuses" CellPadding="5" CellSpacing="3"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDataBound="grvStatuses_RowDataBound"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ApprovalDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField HeaderText="Approver" />
                    <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                    <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


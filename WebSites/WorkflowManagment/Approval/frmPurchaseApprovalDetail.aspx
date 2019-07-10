<%@ Page Title="Purchase Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseApprovalDetail.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPurchaseApprovalDetail" EnableEventValidation="false" %>

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
            <h2>Search Purchase Requests</h2>
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
                                    <asp:TextBox ID="txtSrchRequestNo" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestDate" runat="server" Text="Request Date" CssClass="label"></asp:Label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
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
                        <asp:Button ID="btnpop" runat="server" />
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvPurchaseRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvPurchaseRequestList_RowCommand"
            OnRowDataBound="grvPurchaseRequestList_RowDataBound" OnSelectedIndexChanged="grvPurchaseRequestList_SelectedIndexChanged"
            AllowPaging="True" OnPageIndexChanging="grvPurchaseRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:BoundField DataField="Requester" HeaderText="Requester" SortExpression="Requester" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Required Date of Delivery">
                    <ItemTemplate>
                        <asp:Label ID="lblDateDelivery" runat="server" Text='<%# Eval("Requireddateofdelivery", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              
                <asp:BoundField DataField="SuggestedSupplier" HeaderText="Suggested Supplier" SortExpression="PurposeOfTravel" />
                <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPurchase" />

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
                            <h2>Process Purchase Request</h2>
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
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged">
                                                        <asp:ListItem Value="0">Select Status</asp:ListItem>
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" runat="server" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="input">
                                                    <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:LinkButton runat="server" ID="lnkBidRequest" Visible="false" Text="Prepare Bid Analysis" OnClick="lnkBidRequest_Click" CssClass="btn btn-primary"></asp:LinkButton><br />
                                                <br />
                                                <asp:LinkButton runat="server" ID="lnkSoleVendor" Visible="false" Text="Prepare Sole Vendor Verification" OnClick="lnkSoleVendor_Click" CssClass="btn btn-primary"></asp:LinkButton>
                                            </section>
                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>
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
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground" Enabled="True" PopupControlID="pnlApproval"
        TargetControlID="btnPop" CancelControlID="btnCancelPopup" ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlDetail" runat="server">
        <div class="modal-body no-padding">
            <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                <header>
                    <span class="widget-icon"><i class="fa fa-edit"></i></span>
                    <h2>Purchase Request Details</h2>
                </header>
                <div>
                    <div class="jarviswidget-editbox"></div>
                    <div class="widget-body no-padding">
                        <div class="smart-form">
                            <asp:GridView ID="grvPurchaseRequestDetails" CellPadding="5" CellSpacing="3"
                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                CssClass="table table-striped table-bordered table-hover">
                                <RowStyle CssClass="rowstyle" />
                                <Columns>
                                    <asp:BoundField DataField="Item" HeaderText="Item" SortExpression="Item" />
                                    <asp:BoundField DataField="Priceperunit" HeaderText="Price Per Unit" SortExpression="Priceperunit" />
                                    <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                                    <asp:BoundField DataField="EstimatedCost" HeaderText="Estimated Cost" SortExpression="EstimatedCost" />
                                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
                                </Columns>
                                <FooterStyle CssClass="FooterStyle" />
                                <HeaderStyle CssClass="headerstyle" />
                                <PagerStyle CssClass="PagerStyle" />
                                <RowStyle CssClass="rowstyle" />
                            </asp:GridView>
                            <footer>
                                <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
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
        <table style="width: 100%;">
            <tr>
                <td style="width: 17%; text-align: left;">
                    <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                <td style="font-size: large; text-align: center;">
                    <strong>CHAI ZIMBABWE
                            <br />
                        PURCHASE REQUEST FORM</strong></td>
            </tr>
        </table>
        <table style="width: 100%;">

            <tr>
                <td align="right" style="width: 576px">&nbsp;</td>
                <td align="right" style="width: 490px" class="modal-sm">&nbsp;</td>
                <td align="right" style="width: 280px" class="modal-sm">&nbsp;</td>
                <td align="right" style="width: 389px">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                </td>

                <td style="width: 389px">
                    <asp:Label ID="lblRequireddateofdeliveryResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">
                    <strong>
                        <asp:Label ID="lblSuggestedSupplier" runat="server" Text="Suggested Supplier:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblSuggestedSupplierResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblSpecialNeed" runat="server" Text="Special Need:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblSpecialNeedResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">
                    <strong>
                        <asp:Label ID="Label1" runat="server" Text="Deliver To:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 389px">
                    <asp:Label ID="lblDelivertoResult" runat="server"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">&nbsp;</td>
                <td style="width: 389px"></td>
                <td>&nbsp;</td>
            </tr>
             <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblPaymentMeth" runat="server" Text="Payment Method:"></asp:Label>
                    </strong></td>
                <td style="width: 490px" class="modal-sm">
                    <asp:Label ID="lblPayMethRes" runat="server"></asp:Label>
                </td>
                <td style="width: 280px" class="modal-sm">&nbsp;</td>
                <td style="width: 389px"></td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">
                    <strong>
                        <asp:Label ID="lblReqDate" runat="server" Text="Required Date Of Delivery:"></asp:Label>
                    </strong>
                </td>
                <td style="width: 490px; height: 18px;">
                    <asp:Label ID="lblReqDateResult" runat="server"></asp:Label>
                </td>

            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">&nbsp;</td>
                <td style="width: 490px; height: 18px;">&nbsp;</td>
                <td style="width: 280px; height: 18px;">&nbsp;</td>
                <td style="width: 389px; height: 18px;">&nbsp;</td>
                <td style="height: 18px"></td>
            </tr>
            <tr>
                <td style="width: 576px; height: 18px; padding-left: 15%;">&nbsp;</td>
                <td style="width: 490px; height: 18px;">&nbsp;</td>
                <td style="height: 18px; width: 280px;">&nbsp;</td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="AccountName" SortExpression="ItemAccount.AccountName" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                    <asp:BoundField DataField="Priceperunit" HeaderText="Price Per Unit" SortExpression="Priceperunit" />
                    <asp:BoundField DataField="EstimatedCost" HeaderText="Estimated Cost" SortExpression="EstimatedCost" />
                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
        <br />
        <asp:GridView ID="grvStatuses" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
            CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ApprovalDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField HeaderText="Approver" />
                <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                <asp:BoundField DataField="ApprovalStatus" HeaderText="Approval Status" SortExpression="ApprovalStatus" />

            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
    </div>

</asp:Content>

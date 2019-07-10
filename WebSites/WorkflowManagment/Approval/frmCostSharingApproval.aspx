<%@ Page Title="Cost Sharing Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmCostSharingApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmCostSharingApproval" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript" language="javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#detail').modal('show');
            });
        }
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
            <h2>Search Cost Sharing Requests</h2>
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
                        <asp:Button ID="btnPop3" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvCostSharingRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvCostSharingRequestList_RowCommand"
            OnRowDataBound="grvCostSharingRequestList_RowDataBound" OnSelectedIndexChanged="grvCostSharingRequestList_SelectedIndexChanged"
            AllowPaging="True" OnPageIndexChanging="grvCostSharingRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Requester" SortExpression="AppUser.FullName" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Payee" HeaderText="Payee" SortExpression="Payee" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" SortExpression="ItemAccount.AccountName" />
                <asp:BoundField DataField="EstimatedTotalAmount" HeaderText="Estimated Total Amount" SortExpression="EstimatedTotalAmount" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                <asp:ButtonField ButtonType="Button" CommandName="Retire" Text="Retire" />
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
                            <h2>Process Cost Sharing Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblAccount" Visible="false" runat="server" CssClass="label">Payment Type</asp:Label>
                                                <asp:Label ID="lblAccountdd" Visible="false" runat="server" CssClass="select">
                                                    <asp:DropDownList ID="ddlAccount" runat="server">
                                                        <asp:ListItem Value=" ">Select Payment Type</asp:ListItem>
                                                        <asp:ListItem>Petty Cash</asp:ListItem>
                                                        <asp:ListItem>Bank Payment</asp:ListItem>
                                                    </asp:DropDownList><i></i>
                                                </asp:Label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>
                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged">
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" runat="server" CssClass="validator" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-12">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="input">
                                                    <asp:TextBox ID="txtRejectedReason" Visible="false" runat="server"></asp:TextBox>
                                                </label>
                                            </section>
                                        </div>

                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint');return false;" Enabled="False"></asp:Button>
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
        Enabled="True" TargetControlID="btnPop" PopupControlID="pnlApproval" CancelControlID="btnCancelPopup"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlReimbursement" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Process Retirement</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>

                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblEstimatedAmount" Visible="true" runat="server" CssClass="label">Estimated Amount</asp:Label>
                                                <asp:Label ID="lblEstimatedAmountresult" Visible="true" runat="server" CssClass="label"></asp:Label>

                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblActualExpenditure" runat="server" Text="Actual Expenditure" CssClass="label"></asp:Label>
                                                <label class="input">
                                                    <asp:TextBox ID="txtActualExpenditure" runat="server"></asp:TextBox>
                                                    <cc1:FilteredTextBoxExtender ID="txtActualExpenditure_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtActualExpenditure" ValidChars="&quot;.&quot;">
                                                    </cc1:FilteredTextBoxExtender>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">

                                            <section class="col col-6">
                                                <label class="label">Attach Reciepts</label>
                                                <asp:FileUpload ID="fuReciept" runat="server" />
                                                <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                                            </section>

                                        </div>
                                        <div class="row">
                                            <section class="col col-12">
                                                <asp:GridView ID="grvAttachments"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" OnSelectedIndexChanged="grvAttachments_SelectedIndexChanged">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DeleteFile" />
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
                                        <asp:Button ID="btnReimburse" runat="server" ValidationGroup="Reimburse" Text="Retire" Enabled="true" CssClass="btn btn-primary" OnClick="btnReimburse_Click"></asp:Button>
                                        <asp:Button ID="btnCloseReimburse" runat="server" Text="Close" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnPrintReimburse" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>
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
        Enabled="True" TargetControlID="btnPop3" PopupControlID="pnlReimbursement" CancelControlID="btnCloseReimburse"
        ID="pnlReimbursement_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <div class="modal fade" id="detail" tabindex="-1" role="dialog">
        <div class="modal-dialog">
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
                                        <li>
                                            <a href="#iss2" data-toggle="tab">Attachment</a>
                                        </li>

                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <asp:DataGrid ID="dgCostSharingRequestDetail" runat="server"
                                                AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                                DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True"
                                                OnEditCommand="dgCostSharingRequestDetail_EditCommand" OnItemDataBound="dgCostSharingRequestDetail_ItemDataBound"
                                                OnUpdateCommand="dgCostSharingRequestDetail_UpdateCommand">
                                                <Columns>

                                                    <asp:TemplateColumn HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "SharedAmount")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Project ID">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEdtProject" CssClass="form-control" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlEdtProject_SelectedIndexChanged">
                                                                <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <i></i>
                                                            <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" ControlToValidate="ddlEdtProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Grant ID">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEdtGrant" runat="server" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvGrant" runat="server" ControlToValidate="ddlEdtGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Project Description">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Project.ProjectDescription")%>
                                                        </ItemTemplate>

                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Actions">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                </Columns>
                                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                            </asp:DataGrid>
                                        </div>
                                        <div class="tab-pane" id="iss2">
                                            <asp:GridView ID="grvdetailAttachments"
                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" OnSelectedIndexChanged="grvdetailAttachments_SelectedIndexChanged">
                                                <RowStyle CssClass="rowstyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDownload2" Text="Download" CommandArgument='<%# Eval("FilePath") %>' runat="server" OnClick="DownloadFile2"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle CssClass="FooterStyle" />
                                                <HeaderStyle CssClass="headerstyle" />
                                                <PagerStyle CssClass="PagerStyle" />
                                                <RowStyle CssClass="rowstyle" />
                                            </asp:GridView>


                                        </div>
                                    </div>
                                </div>
                                <footer>
                                    <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
                                </footer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>













    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            CASH PAYMENT REQUEST FORM</strong></td>
                </tr>
            </table>

            <table style="width: 100%;">
                <tr>
                    <td align="right" style="width: 1009px" class="modal-lg">&nbsp;</td>
                    <td align="right" style="width: 244px" class="inbox-data-from">&nbsp;</td>
                    <td align="right" style="width: 271px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblVoucherNo" runat="server" Text="Voucher No:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px" class="inbox-data-from">
                        <asp:Label ID="lblVoucherNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px" class="inbox-data-from">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px">&nbsp;</td>
                    <td style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px; height: 18px;">
                        <label class="input" __designer:mapid="15fe">
                            <asp:RequiredFieldValidator ID="rfvActualExpenditure" runat="server" CssClass="validator" ValidationGroup="Reimburse" ErrorMessage="Actual Expenditure Required" InitialValue="" ControlToValidate="txtActualExpenditure"></asp:RequiredFieldValidator>
                        </label>
                    </td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblPayee" runat="server" Text="Payee:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblPayeeResult" runat="server"></asp:Label>
                    </td>

                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblDescriptionP" runat="server" Text="Description :"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblDescResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblTotalAmountResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;"><strong>
                        <asp:Label ID="lblActualExpendture" runat="server" Text="Actual Expenditure:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblActualExpendtureRes" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px; height: 18px;"><strong>
                        <asp:Label ID="lblReimberseStatus" runat="server" Text="Retirement Status:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblReimbersestatusRes" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;"><strong>
                        <asp:Label ID="lblAccountName" runat="server" Text="Account Code:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblAccountNameResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 271px; height: 18px;"><strong>
                        <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                 <tr>
                    
                    <td style="width: 848px">
                        <strong>
                            <asp:Label ID="lblPaytype" runat="server" Text="Payment Method:"></asp:Label>
                        </strong></td>
                      <td style="width: 390px">
                        <asp:Label ID="lblpaytypeRes" runat="server" Text="" class="label"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 1009px; height: 18px; padding-left: 20%;">&nbsp;</td>
                    <td style="width: 244px; height: 18px;">&nbsp;</td>
                    <td style="width: 271px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>

            </table>

            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project Code" />
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant Code" />
                    <asp:BoundField DataField="SharedAmount" HeaderText="Shared Amount" SortExpression="SharedAmount" />


                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date", "{0:dd/MM/yyyy}")%>'></asp:Label>
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
            <br />
            <table style="width: 100%;">
                <tr>
                    <td></td>
                    <td>Signature</td>
                    <td></td>
                    <td></td>
                    <td style="text-align: right; padding-right: 12%;">Recieved By </td>
                </tr>
                <tr>
                    <td></td>
                    <td>___________________</td>
                    <td></td>
                    <td></td>
                    <td style="text-align: right;">___________________</td>
                </tr>
            </table>
        </fieldset>
    </div>

</asp:Content>

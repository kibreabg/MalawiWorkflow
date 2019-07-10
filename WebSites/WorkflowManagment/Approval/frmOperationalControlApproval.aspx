<%@ Page Title="Bank Payment Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmOperationalControlApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmOperationalControlApproval" EnableEventValidation="false" %>

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
            <h2>Search Bank Payment Requests</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblSrchRequestNo" runat="server" Text="Voucher No" CssClass="label"></asp:Label>
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
        <asp:GridView ID="grvOperationalControlRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvOperationalControlRequestList_RowCommand"
            OnRowDataBound="grvOperationalControlRequestList_RowDataBound" OnSelectedIndexChanged="grvOperationalControlRequestList_SelectedIndexChanged"
            AllowPaging="True" OnPageIndexChanging="grvOperationalControlRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="VoucherNo" HeaderText="Voucher No" SortExpression="VoucherNo" />
                <asp:BoundField DataField="AppUser.FullName" HeaderText="Requester" SortExpression="AppUser.FullName" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Beneficiary.BeneficiaryName" HeaderText="Beneficiary Name" SortExpression="Beneficiary.BeneficiaryName" />
                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                <asp:BoundField DataField="BranchCode" HeaderText="Branch Code" SortExpression="BranchCode" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
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
                            <h2>Process Bank Payment Request</h2>
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
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ValidationGroup="save" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                        <div class="row">
                                            <section class="col col-12">
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
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>

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
                                    <li>
                                        <a href="#iss2" data-toggle="tab">Attachment</a>
                                    </li>

                                </ul>
                                <div class="tab-content padding-10">
                                    <div class="tab-pane active" id="iss1">
                                        <asp:DataGrid ID="dgOperationalControlRequestDetail" runat="server"
                                            AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover"
                                            DataKeyField="Id" GridLines="None" PagerStyle-CssClass="paginate_button active" ShowFooter="True"
                                            OnEditCommand="dgOperationalControlRequestDetail_EditCommand" OnItemDataBound="dgOperationalControlRequestDetail_ItemDataBound"
                                            OnUpdateCommand="dgOperationalControlRequestDetail_UpdateCommand">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="Account Name">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlEdtAccountDescription" CssClass="form-control" OnSelectedIndexChanged="ddlEdtAccountDescription_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">
                                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <i></i>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Account Code">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEdtAccountCode" ReadOnly="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container.DataItem, "Amount")%>
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
                                                        <asp:RequiredFieldValidator ID="rfvddlEdtProject" runat="server" CssClass="validator" ControlToValidate="ddlEdtProject" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
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

                                    </div>

                                </div>
                            </div>
                            <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
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

    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            BANK PAYMENT REQUEST FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%;">
                <tr>
                    <td align="right" style="width: 848px">&nbsp;</td>
                    <td align="right" style="width: 357px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblVoucherNo" runat="server" Text="Voucher No:"></asp:Label>
                        </strong></td>
                    <td style="width: 357px">
                        <asp:Label ID="lblVoucherNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPostingRef" runat="server" Text="Posting Ref:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">__________
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 357px">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px"><strong>
                        <asp:Label ID="lblBranchCode" runat="server" Text="Bank:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 389px">
                        <asp:Label ID="lblBranchCodeResult" runat="server"></asp:Label></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 357px; height: 18px;">
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;"><strong>
                        <asp:Label ID="lblBankAccountNo" runat="server" Text="Bank Account No:"></asp:Label>
                    </strong></td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblBankAccountNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>

                
               
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 357px; height: 18px;">
                        <asp:Label ID="lblBeneficiaryNameResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 629px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblReimberseStatus" runat="server" Text="Retirement Status:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblReimbersestatusRes" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;"><strong>
                        <asp:Label ID="lblTotalAmount" runat="server" Text="Total Amount:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 357px; height: 18px;">
                        <asp:Label ID="lblTotalAmountResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;"><strong>
                        <asp:Label ID="lblActualExpendture" runat="server" Text="Actual Expenditure:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 357px; height: 18px;">
                        <asp:Label ID="lblActualExpendtureRes" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
                        </strong>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblDescriptionResult" runat="server"></asp:Label>
                    </td>
                     <td style="width: 629px; height: 18px; padding-left: 10%;">
                        <strong>
                            <asp:Label ID="lblPayMeth" runat="server" Text="Payment Method:"></asp:Label>
                        </strong>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblPayMethRes" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 10%;"><strong>
                        <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 357px; height: 18px;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
            </table>

            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="AccountName" SortExpression="ItemAccount.AccountName" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="ItemAccount.AccountCode" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="ActualExpendture" HeaderText="Actual Expendture" SortExpression="ActualExpendture" />
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

        </fieldset>
    </div>

</asp:Content>

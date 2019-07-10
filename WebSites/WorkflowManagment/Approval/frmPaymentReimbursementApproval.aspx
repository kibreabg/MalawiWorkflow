<%@ Page Title="Payment Reimbursement Request Approval Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPaymentReimbursementApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPaymentReimbursementApproval" EnableEventValidation="false" %>

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
            <h2>Search Payment Reimbursement</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblExpenseType" runat="server" Text="Expense Type" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtSrchExpenseType" runat="server" Visible="true"></asp:TextBox>
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
                        <asp:Button ID="btnFind" runat="server" Text="Find" CssClass="btn btn-primary" OnClick="btnFind_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
        <asp:GridView ID="grvPaymentReimbursementRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="grvPaymentReimbursementRequestList_RowCommand"
            OnRowDataBound="grvPaymentReimbursementRequestList_RowDataBound" OnSelectedIndexChanged="grvPaymentReimbursementRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPaymentReimbursementRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
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
                            <h2>Process Payment Reimbursement Request</h2>
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
                                                <asp:Label ID="lblAttachments" runat="server" Text="Attachments" CssClass="label"></asp:Label>
                                                <asp:GridView ID="grvAttachments"
                                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" OnSelectedIndexChanged="grvAttachments_SelectedIndexChanged">
                                                    <RowStyle CssClass="rowstyle" />
                                                    <Columns>
                                                        <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                                                        <asp:BoundField DataField="ContentType" HeaderText="Content Type" SortExpression="ContentType" />
                                                        <asp:BoundField DataField="Data" HeaderText="Data" Visible="false" />
                                                        <asp:CommandField ButtonType="Link" SelectText="View Attachment" ShowSelectButton="True" />
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
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground" Enabled="True"
        PopupControlID="pnlApproval" TargetControlID="btnPop"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlDetail" Visible="false" Style="position: absolute; top: 10%; left: 10%;" runat="server">
        <div class="modal-content">
            <div class="modal-body no-padding">
                <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                    <header>
                        <span class="widget-icon"><i class="fa fa-edit"></i></span>
                        <h2>Reimbursement Details</h2>
                    </header>
                    <div>
                        <div class="jarviswidget-editbox"></div>
                        <div class="widget-body no-padding">
                            <div class="smart-form">
                                <asp:DataGrid ID="dgReimbursementDetail" runat="server"
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
                                        <asp:TemplateColumn HeaderText="Amount Advanced">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Actual Expenditure">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Variance">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Variance")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Project ID">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                            </ItemTemplate>

                                        </asp:TemplateColumn>


                                    </Columns>
                                    <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                </asp:DataGrid>
                                <footer>
                                    <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>
                                </footer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            CASH PAYMENT REIMBURSEMENT TRANSACTION FORM</strong></td>
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
                    <td style="width: 848px">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px">
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
                    <td style="width: 848px">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px">
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblEmployeeNo" runat="server" Text="Employee No:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblEmpNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                 <tr>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPaymentMeth" runat="server" Text="Payment Method:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblPayMethRes" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>
                        <asp:Label ID="lblCommentPrint" runat="server" Text="Comment:"></asp:Label>
                    </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblCommentResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
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
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Account Name" />
                    <asp:BoundField DataField="AmountAdvanced" HeaderText="Amount Advanced" />
                    <asp:BoundField DataField="ActualExpenditure" HeaderText="Actual Expenditure" />
                    <asp:BoundField DataField="Variance" HeaderText="Variance" />
                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project" />
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
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                    <asp:BoundField DataField="Approver" HeaderText="Approver" SortExpression="Approver" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
        </fieldset>
    </div>

</asp:Content>

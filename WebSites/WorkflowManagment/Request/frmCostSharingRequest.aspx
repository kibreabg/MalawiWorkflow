<%@ Page Title="Cost Sharing Request" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmCostSharingRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmCostSharingRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Cost Sharing Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <%-- <section class="col col-6">
                                <label class="label">Request No</label>
                                <label class="input">
                                    <asp:TextBox ID="txtVoucherNo" runat="server" ReadOnly="true"></asp:TextBox>
                                </label>
                            </section>--%>

                            <section class="col col-6">
                                <label class="label">Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                             <section class="col col-6">
                                <label class="label">Payment Methods</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPayMethods" AutoPostBack="false" AppendDataBoundItems="true" 
                                        runat="server" DataValueField="Id" DataTextField="Name" CssClass="form-control">
                                        <asp:ListItem Text="--Select Payment Method--" Value="0"></asp:ListItem>
                                        <asp:ListItem>RTGs</asp:ListItem>
                                        <asp:ListItem>USD</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="RequiredFieldValidator3" runat="server" ErrorMessage="Payment Method must be selected" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlPayMethods"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">

                                <label class="label">Amount Type</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlAmountType" runat="server">
                                        <asp:ListItem Value="">Select Amount Type</asp:ListItem>
                                        <asp:ListItem>Estimated Amount</asp:ListItem>
                                        <asp:ListItem>Actual Amount</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="RfvAmountType" runat="server" ControlToValidate="ddlAmountType" CssClass="validator" Display="Dynamic" ErrorMessage="Amount Type Required" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>


                            <section class="col col-6">
                                <label class="label">Cost</label>
                                <label class="input">

                                    <asp:TextBox ID="txtEstimatedCost" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="txtEstimatedCost_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEstimatedCost" ValidChars="&quot;.&quot;">
                                    </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="rfvtxtEstimatedCost" runat="server" ControlToValidate="txtEstimatedCost" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Payee</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPayee" runat="server" DataValueField="SupplierName" DataTextField="SupplierName" AppendDataBoundItems="True">
                                        <asp:ListItem Value="">--Select Payee--</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                                <asp:RequiredFieldValidator ID="rfvPayee" runat="server" ControlToValidate="ddlPayee" CssClass="validator" Display="Dynamic" ErrorMessage="Payee Required" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                            </section>
                            <section class="col col-6">
                                <label class="label">Description</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">

                            <section class="col col-6">
                                <label class="label">Item Account</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">Select Account</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="rfvddlAccountDescription" runat="server" ControlToValidate="ddlAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account Name must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                    </fieldset>

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
                                            <a href="#iss2" data-toggle="tab">Attach Reciept</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">

                                        <div class="tab-pane active" id="iss2">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Attach Reciepts</label>
                                                        <asp:FileUpload ID="fuReciept" runat="server" />
                                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                                                    </section>
                                                </div>
                                            </fieldset>
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

                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- end widget content -->

                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" class="btn btn-primary" ValidationGroup="request"></asp:Button>
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" />--%>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="false"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure you want to delete this record?" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

    </div>
    <%--<cc1:ModalPopupExtender ID="pnlSearch_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Enabled="True"
        PopupControlID="pnlSearch" CancelControlID="btnCancelSearch" TargetControlID="btnSearch">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlSearch" runat="server">--%>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Cost Sharing Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Voucher No.</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Voucher No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestDate">Request Date</label>
                                <label class="input" style="position: relative; display: block; font-weight: 400;">
                                    <i class="icon-append fa fa-calendar" style="position: absolute; top: 5px; width: 22px; height: 22px; font-size: 14px; line-height: 22px; text-align: center; right: 5px; padding-left: 3px; border-left-width: 1px; border-left-style: solid; color: #A2A2A2;"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" ToolTip="Request Date" runat="server"></asp:TextBox>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: right;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button Text="Close" ID="btnCancelSearch" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvCashPaymentRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvCashPaymentRequestList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvCashPaymentRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                                            <asp:TemplateField HeaderText="Request Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Payee" HeaderText="Payee" SortExpression="Payee" />
                                            <asp:BoundField DataField="VoucherNo" HeaderText="VoucherNo" SortExpression="VoucherNo" />

                                            <asp:CommandField ShowSelectButton="True" />
                                        </Columns>
                                        <FooterStyle CssClass="FooterStyle" />
                                        <HeaderStyle CssClass="headerstyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <RowStyle CssClass="rowstyle" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--</asp:Panel>--%>

    <asp:Panel ID="pnlWarning" Visible="false" Style="position: absolute; top: 55px; left: 108px;" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <div class="alert alert-block alert-danger">
                                        <h4 class="alert-heading">Warning</h4>
                                        <p>
                                            The current Request has no Approval Settings defined. Please contact your administrator!
                                        </p>
                                    </div>
                                    <footer>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
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
</asp:Content>

<%@ Page Title="Payment Reimbursement Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPaymentReimbursementRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmPaymentReimbursementRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Payment Reimbursement</h2>
        </header>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="well well-sm well-primary">
                        <asp:GridView ID="grvCashPayments"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                            OnRowDataBound="grvCashPayments_RowDataBound" OnSelectedIndexChanged="grvCashPayments_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvCashPayments_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="RequestNo" />
                                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" />
                                <asp:BoundField DataField="VoucherNo" HeaderText="Voucher No" />
                                <asp:BoundField DataField="Payee" HeaderText="Payee" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" />
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
        <asp:Panel ID="pnlInfo" runat="server">
            <div class="alert alert-info fade in">
                <button class="close" data-dismiss="alert">
                    ×
                </button>
                <i class="fa-fw fa fa-info"></i>
                <strong>Info!</strong> Please select the Cash Payment Transaction to perform Payment Reimbursement for!
            </div>
        </asp:Panel>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
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
                                            <div class="tab-pane" id="BB">
                                            </div>
                                        </div>
                                        <ul class="nav nav-tabs">
                                            <li class="active">
                                                <a data-toggle="tab" href="#AA">Tab 1</a>
                                            </li>
                                            <li class="">
                                                <a data-toggle="tab" href="#BB">Tab 2</a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="tab-pane active" id="hr2">

                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Expense Form</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Attach Invoice</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Request Date</label>
                                                        <label class="input">
                                                            <i class="icon-append fa fa-calendar"></i>
                                                            <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                       <section class="col col-6">
                                                        <label class="label">Expense Type</label>
                                                        <label class="select">
                                                          <asp:DropDownList ID="ddlExpenseType" runat="server" AppendDataBoundItems="True">
                                                           <asp:ListItem Value=" ">Select Expense Type</asp:ListItem>
                                                           <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                                           <asp:ListItem Value="Claim">Claim</asp:ListItem>
                                                         </asp:DropDownList><i></i>
                                                            <asp:RequiredFieldValidator ID="RfvExpenseType" runat="server" ControlToValidate="ddlExpenseType" ErrorMessage="Expense Type Required" InitialValue=" " SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                      </label>
                                                    </section>
                                                    </div>
                                                 <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Comment</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                </div>
                                                   <div class="row">
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
                            </section></div>
                                                <asp:DataGrid ID="dgPaymentReimbursementDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                    GridLines="None" OnItemDataBound="dgPaymentReimbursementDetail_ItemDataBound" ShowFooter="True">

                                                    <Columns>
                                                        <asp:TemplateColumn HeaderText="Requested Items">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Project ID">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Amount Advanced">
                                                            <ItemTemplate>
                                                                <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                                                                <asp:HiddenField ID="hfAmountAdvanced" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Actual Expenditure">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtActualExpenditure" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtActualExpenditure_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>'></asp:TextBox>
                                                                <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtActualExpenditure" ID="txtActualExpenditure_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="."></cc1:FilteredTextBoxExtender>
                                                                <asp:RequiredFieldValidator ID="rfvActualExpenditure" runat="server" ControlToValidate="txtActualExpenditure" ErrorMessage="Actual Expenditure is required" ValidationGroup="save" InitialValue="0">*</asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>
                                                        <asp:TemplateColumn HeaderText="Variance">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtVariance" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Variance")%>'></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn>

                                                    </Columns>
                                                    <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                </asp:DataGrid>
                                            </fieldset>
                                        </div>
                                        <div class="tab-pane" id="iss2">
                                            <div class="jarviswidget-editbox"></div>
                                            <div class="widget-body no-padding">
                                                <div class="smart-form">
                                                    <fieldset>
                                                        <div class="row">
                                                            <section class="col col-6">
                                                                <label class="label">Invoice Reciept</label>
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
                                                            <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
                                                            <asp:BoundField DataField="ContentType" HeaderText="Content Type" SortExpression="ContentType" />
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
                        <!-- end widget content -->
                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="save" Enabled="false" Text="Request" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" />
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                          <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

    <%-- Modal --%>

    <asp:Panel ID="pnlSearch" runat="server">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                    &times;</button>
                <h4 class="modal-title" id="myModalLabel">Search Payment Reimbursement Requests</h4>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <div class="input-group">
                                <label for="txtSrchRequestDate">Requested Date</label>
                                <i class="icon-append fa fa-calendar"></i>
                                <asp:TextBox ID="txtSrchRequestDate" CssClass="datepicker form-control"
                                    data-dateformat="mm/dd/yy" ToolTip="Request Date" runat="server"></asp:TextBox>
                            </div>

                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="ddlSrchExpenseType">Expense Type</label><br />
                            <asp:DropDownList ID="ddlSrchExpenseType" CssClass="form-control" runat="server">
                                <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                <asp:ListItem Value="Claim">Claim</asp:ListItem>
                            </asp:DropDownList><i></i>
                        </div>
                    </div>

                </div>
                <div class="row" style="text-align: right;">
                    <div class="col-md-12">
                        <div class="form-group">
                            <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="well well-sm well-primary">
                                <asp:GridView ID="grvPaymentReimbursementRequestList"
                                    runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                    OnRowDataBound="grvPaymentReimbursementRequestList_RowDataBound" OnSelectedIndexChanged="grvPaymentReimbursementRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPaymentReimbursementRequestList_PageIndexChanging"
                                    CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                    <RowStyle CssClass="rowstyle" />
                                    <Columns>
                                        <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                        <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
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
            <div class="modal-footer">
                <asp:Button ID="btnCancelSearch" runat="server" CssClass="btn btn-primary" Text="Cancel" />
            </div>
        </div>
    </asp:Panel>

    <cc1:ModalPopupExtender runat="server" Enabled="True" TargetControlID="btnSearch" CancelControlID="btnCancelSearch"
        PopupControlID="pnlSearch" ID="pnlSearch_ModalPopupExtender" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
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

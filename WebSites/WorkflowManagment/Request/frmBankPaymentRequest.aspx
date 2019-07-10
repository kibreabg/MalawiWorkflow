<%@ Page Title="Bank Payment Request" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmBankPaymentRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmBankPaymentRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Bank Payment Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Reference No</label>
                                <label class="input">
                                    <asp:TextBox ID="txtVoucherNo" runat="server"></asp:TextBox>
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
                            <section class="col col-6">
                                <label class="label">Process Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server"></asp:TextBox>
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
                                            <a href="#iss1" data-toggle="tab">Add Details</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <asp:DataGrid ID="dgBankPaymentDetail" runat="server" AutoGenerateColumns="false"
                                                CellPadding="0" CssClass="table table-striped table-bordered table-hover" DataKeyField="Id"
                                                GridLines="None" OnCancelCommand="dgBankPaymentDetail_CancelCommand" OnDeleteCommand="dgBankPaymentDetail_DeleteCommand"
                                                OnEditCommand="dgBankPaymentDetail_EditCommand" OnItemCommand="dgBankPaymentDetail_ItemCommand" OnUpdateCommand="dgBankPaymentDetail_UpdateCommand"
                                                PagerStyle-CssClass="paginate_button active" ShowFooter="True" Visible="true" OnItemDataBound="dgBankPaymentDetail_ItemDataBound">
                                                <Columns>
                                                    <asp:TemplateColumn HeaderText="Process Date">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "ProcessDate")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Bank Code (Must be 5 digits)">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "BankCode")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtBankCode" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "BankCode")%>'></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtBankCode" ID="txtEdtBankCode_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtEdtBankCode" runat="server" ControlToValidate="txtEdtBankCode"
                                                                CssClass="validator" Display="Dynamic" ErrorMessage="Bank Code is required"
                                                                SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtBankCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtBankCode" ID="txtBankCode_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtBankCode" runat="server" ControlToValidate="txtBankCode"
                                                                CssClass="validator" Display="Dynamic" ErrorMessage="Bank Code is required"
                                                                SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Account No">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Account.AccountNo")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlEdtAccount" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                                <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <i></i>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                                                <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <i></i>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Name">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Name")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtName" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Name")%>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Amount">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Amount")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtAmount" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Amount")%>'></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtEdtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtEdtAmount" ValidChars="&quot;.&quot;">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtEdtAmount" runat="server" ControlToValidate="txtEdtAmount" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtAmount" ValidChars="&quot;.&quot;">
                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvtxtAmount" runat="server" ControlToValidate="txtAmount" CssClass="validator" Display="Dynamic" ErrorMessage="Amount is required" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Currency (Only in CAPS)">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Currency")%>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtEdtCurrency" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Currency")%>'></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtCurrency" ID="txtEdtCurrency_FilteredTextBoxExtender" FilterType="UppercaseLetters"></cc1:FilteredTextBoxExtender>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtCurrency" ID="txtCurrency_FilteredTextBoxExtender" FilterType="UppercaseLetters"></cc1:FilteredTextBoxExtender>
                                                        </FooterTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Reference">
                                                        <ItemTemplate>
                                                            <%# DataBinder.Eval(Container.DataItem, "Reference")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateColumn>
                                                    <asp:TemplateColumn HeaderText="Actions">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lnkAddNew" runat="server" CausesValidation="true" CommandName="AddNew" CssClass="btn btn-sm btn-success" ValidationGroup="save"><i class="fa fa-save"></i></asp:LinkButton>
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
                                        <div class="tab-pane" id="iss2">
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- end widget content -->

                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" class="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" class="btn btn-primary" />
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
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
    <cc1:ModalPopupExtender ID="pnlSearch_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" Enabled="True"
        PopupControlID="pnlSearch" CancelControlID="btnCancelSearch" TargetControlID="btnSearch">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlSearch" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Bank Payment Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Reference No.</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Voucher No" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="input">
                                    <label for="txtSrchRequestDate">Process Date</label>
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" ToolTip="Process Date" runat="server"></asp:TextBox>
                                </label>
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
                                    <asp:GridView ID="grvBankPaymentRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnSelectedIndexChanged="grvBankPaymentRequestList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvBankPaymentRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                            <asp:BoundField DataField="ProcessDate" HeaderText="Process Date" SortExpression="ProcessDate" />
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
                    <asp:Button Text="Close" ID="btnCancelSearch" runat="server" class="btn btn-default"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>

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

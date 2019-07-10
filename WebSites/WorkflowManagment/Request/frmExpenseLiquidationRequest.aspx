<%@ Page Title="Expense Liquidation Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmExpenseLiquidationRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmExpenseLiquidationRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
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

        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }

        function IsOneDecimalPoint(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode; // restrict user to type only one . point in number
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 1 && charCode == 46)
                return false;
            return true;
        }
    </script>
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Expense Liquidation</h2>
        </header>
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="well well-sm well-primary">
                        <asp:GridView ID="grvTravelAdvances"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                            OnRowDataBound="grvTravelAdvances_RowDataBound" OnSelectedIndexChanged="grvTravelAdvances_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvTravelAdvances_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="TravelAdvanceNo" HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                                <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose Of Travel" SortExpression="PurposeOfTravel" />
                                <asp:BoundField DataField="TotalTravelAdvance" HeaderText="Total Travel Advance" SortExpression="TotalTravelAdvance" />
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
                <strong>Info!</strong> Please select the Travel Advance Transaction to perform Expense Liquidation for
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
                                            <a href="#iss2" data-toggle="tab">Attachments</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content padding-10">
                                        <div class="tab-pane active" id="iss1">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Expense Type</label>
                                                        <label class="select">
                                                            <asp:DropDownList ID="ddlPExpenseType" runat="server">
                                                                <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                                            </asp:DropDownList><i></i>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Purpose of Advance </label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtComment" runat="server"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="rfvPurpose" runat="server" ErrorMessage="Purpose of Advance Required" ForeColor="Red" ControlToValidate="txtComment" ValidationGroup="request"></asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Total Actual Expenditure </label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtTotActual" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Total Travel Advance</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtTotalAdvance" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Travel Advance Request Date </label>
                                                        <label class="input">
                                                            <i class="icon-append fa fa-calendar"></i>
                                                            <asp:TextBox ID="txtTravelAdvReqDate" runat="server" ReadOnly="true"></asp:TextBox>
                                                        </label>
                                                    </section>
                                                    <section class="col col-6">
                                                        <label class="label">Comment</label>
                                                        <label class="input">
                                                            <asp:TextBox ID="txtAdditionalComment" runat="server"></asp:TextBox>
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
                                                <asp:UpdatePanel ID="upLiquidationDetail" runat="server">
                                                    <ContentTemplate>
                                                        <asp:DataGrid ID="dgExpenseLiquidationDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                                            GridLines="None" OnItemDataBound="dgExpenseLiquidationDetail_ItemDataBound" ShowFooter="True" OnItemCommand="dgExpenseLiquidationDetail_ItemCommand">

                                                            <Columns>
                                                                <asp:TemplateColumn HeaderText="Ref No.">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control" AutoPostBack="true" Text='<%# DataBinder.Eval(Container.DataItem, "RefNo")%>'></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtRefNo" runat="server" ControlToValidate="txtRefNo" CssClass="validator" ErrorMessage="Ref No. is required" ValidationGroup="request" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFRefNo" runat="server" CssClass="form-control" AutoPostBack="true"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtFRefNo" runat="server" CssClass="validator" ControlToValidate="txtFRefNo" ErrorMessage="Ref No. is required" ValidationGroup="save" InitialValue=""></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Account Name">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlAccountDescription" runat="server" ControlToValidate="ddlAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                                                                        <i></i>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlFAccountDescription" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAccountDescription_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvFddlAccountDescription" runat="server" ControlToValidate="ddlFAccountDescription" CssClass="validator" Display="Dynamic" ErrorMessage="Account must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                        <i></i>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Account Code">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountCode")%>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Expense Type">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "ExpenseType.ExpenseTypeName")%>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlExpenseType" runat="server" Enabled="true" DataValueField="Id" DataTextField="ExpenseTypeName" AppendDataBoundItems="True">
                                                                            <asp:ListItem Value="0">--Select Expense Type--</asp:ListItem>
                                                                        </asp:DropDownList><i></i>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Project ID">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlProject" runat="server" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlProject" runat="server" ControlToValidate="ddlProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="request"></asp:RequiredFieldValidator>
                                                                        <i></i>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlFProject" runat="server" AutoPostBack="true" CssClass="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlFProject_SelectedIndexChanged">
                                                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvddlFProject" runat="server" ControlToValidate="ddlFProject" CssClass="validator" Display="Dynamic" ErrorMessage="Project must be selected" InitialValue="0" SetFocusOnError="true" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                        <i></i>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Grant ID">
                                                                    <ItemTemplate>
                                                                        <asp:DropDownList ID="ddlGrant" runat="server" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RfvGrant" runat="server" CssClass="validator" ControlToValidate="ddlGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="request"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:DropDownList ID="ddlFGrant" runat="server" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id" EnableViewState="true">
                                                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server" CssClass="validator" ControlToValidate="ddlFGrant" Display="Dynamic" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>

                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Amount Advanced">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>
                                                                        <asp:HiddenField ID="hfAmountAdvanced" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "AmountAdvanced")%>'></asp:HiddenField>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFAmount" runat="server" ReadOnly="true" CssClass="form-control">0</asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Actual Expenditure">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtActualExpenditure" runat="server" CssClass="form-control" AutoPostBack="true" onkeypress="return IsOneDecimalPoint(event);" OnTextChanged="txtActualExpenditure_TextChanged" Text='<%# DataBinder.Eval(Container.DataItem, "ActualExpenditure")%>'></asp:TextBox>
                                                                        <%--<asp:RegularExpressionValidator runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$" ControlToValidate="txtActualExpenditure" CssClass="validator" ValidationGroup="request" ErrorMessage="Please enter a decimal value"></asp:RegularExpressionValidator>--%>
                                                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtActualExpenditure" ID="txtActualExpenditure_FilteredTextBoxExtender" FilterType="Custom" ValidChars="0123456789."></cc1:FilteredTextBoxExtender>
                                                                        <%--<cc1:MaskedEditExtender runat="server" TargetControlID="txtActualExpenditure" Mask="9{9}.99" MaskType="Number" ClearMaskOnLostFocus="false"></cc1:MaskedEditExtender>--%>
                                                                        <asp:RequiredFieldValidator ID="rfvActualExpenditure" runat="server" ControlToValidate="txtActualExpenditure" CssClass="validator" ErrorMessage="Actual Expenditure is required" ValidationGroup="request" InitialValue="-1"></asp:RequiredFieldValidator>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFActualExpenditure" runat="server" CssClass="form-control" AutoPostBack="true" Text="0" OnTextChanged="txtFActualExpenditure_TextChanged"></asp:TextBox>
                                                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFActualExpenditure" ID="txtFActualExpenditure_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="."></cc1:FilteredTextBoxExtender>
                                                                        <asp:RequiredFieldValidator ID="rfvtxtFActualExpenditure" runat="server" CssClass="validator" ControlToValidate="txtFActualExpenditure" ErrorMessage="Actual Expenditure is required" ValidationGroup="save" InitialValue="0"></asp:RequiredFieldValidator>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Variance">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtVariance" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Variance")%>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:TextBox ID="txtFVariance" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                                <asp:TemplateColumn HeaderText="Actions">
                                                                    <EditItemTemplate>
                                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CausesValidation="true" CommandName="Update" CssClass="btn btn-xs btn-default" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                                    </EditItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:LinkButton ID="lnkAddNew" runat="server" CausesValidation="true" CommandName="AddNew" CssClass="btn btn-sm btn-success" ValidationGroup="save"><i class="fa fa-save"></i></asp:LinkButton>
                                                                    </FooterTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                                        </asp:DataGrid>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>

                                            </fieldset>
                                        </div>
                                        <div class="tab-pane" id="iss2">
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
                        <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="request" Visible="false" Text="Request" OnClick="btnSave_Click" class="btn btn-primary"></asp:Button>
                        <%--<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" class="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="False"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Search Expense Liquidation Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestDate">Requested Date</label>
                                <label class="input" style="position: relative; display: block; font-weight: 400;">
                                    <i class="icon-append fa fa-calendar" style="position: absolute; top: 5px; width: 22px; height: 22px; font-size: 14px; line-height: 22px; text-align: center; right: 5px; padding-left: 3px; border-left-width: 1px; border-left-style: solid; color: #A2A2A2;"></i>
                                    <asp:TextBox ID="txtSrchRequestDate" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" ToolTip="Request Date" runat="server"></asp:TextBox>
                                </label>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="ddlSrchExpenseType">Expense Type</label><br />
                                <asp:DropDownList ID="ddlSrchExpenseType" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="Advance">Advance</asp:ListItem>
                                </asp:DropDownList><i></i>
                            </div>
                        </div>

                    </div>
                    <div class="row" style="text-align: right;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button Text="Close" ID="Button1" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvExpenseLiquidationRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnRowDataBound="grvExpenseLiquidationRequestList_RowDataBound" OnSelectedIndexChanged="grvExpenseLiquidationRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvExpenseLiquidationRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                            <asp:BoundField DataField="ExpenseType" HeaderText="Expense Type" SortExpression="ExpenseType" />
                                            <asp:CommandField ShowSelectButton="True" />
                                        </Columns>
                                        <FooterStyle CssClass="FooterStyle" />
                                        <HeaderStyle CssClass="headerstyle" />
                                        <PagerStyle CssClass="PagerStyle" />
                                        <RowStyle CssClass="rowstyle" />
                                    </asp:GridView>
                                    <div class="alert alert-info fade in">
                                        <button class="close" data-dismiss="alert">
                                            ×
                                        </button>
                                        <i class="fa-fw fa fa-info"></i>
                                        <strong>Info!</strong> Rejected Expense Liquidations are highlighted in <span style="color:red">Red Color</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>
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
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            LIQUIDATION EXPENSE TRANSACTION FORM</strong></td>
                </tr>
            </table>
            <table style="width: 100%;">

                <tr>
                    <td align="right" style="width: 738px">&nbsp;</td>
                    <td align="right">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td align="right" style="width: 389px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 738px">
                        <strong>
                            <asp:Label ID="lblRequestNo" runat="server" Text="Request No:"></asp:Label>
                        </strong></td>
                    <td>
                        <asp:Label ID="lblRequestNoResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 738px">
                        <strong>
                            <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date:"></asp:Label>
                        </strong></td>
                    <td>
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 738px">
                        <strong>
                            <asp:Label ID="lblRequester" runat="server" Text="Requester:"></asp:Label>
                        </strong></td>
                    <td>
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px">&nbsp;</td>
                    <td style="width: 389px"></td>
                    <td>&nbsp;</td>
                </tr>

                <tr>
                    <%-- <td style="width: 738px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblExpenseTypePrint" runat="server" Text="Expense Type:"></asp:Label>
                        </strong>
                    </td>
                    <td style="height: 18px;">
                        <asp:Label ID="lblExpenseTypeResult" runat="server"></asp:Label>
                    </td>--%>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 389px; height: 18px;"><strong>
                        <asp:Label ID="lblCommentPrint" runat="server" Text="Purpose of Advance:"></asp:Label>
                    </strong>
                    </td>
                    <td style="height: 18px;">
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
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" />
                    <asp:BoundField DataField="AmountAdvanced" HeaderText="Amount Advanced" />
                    <asp:BoundField DataField="ActualExpenditure" HeaderText="Actual Expenditure" />
                    <asp:BoundField DataField="Variance" HeaderText="Variance" />
                    <asp:BoundField DataField="Project.ProjectCode" HeaderText="Project" />
                    <asp:BoundField DataField="Grant.GrantCode" HeaderText="Grant" />
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

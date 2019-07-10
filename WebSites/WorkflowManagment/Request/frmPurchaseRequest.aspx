<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmPurchaseRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <asp:ValidationSummary ID="VSPurchaseRequest" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:ValidationSummary ID="VSDetailAdd" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proadd" ForeColor="" />
    <asp:ValidationSummary ID="VSEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proedit" ForeColor="" />
    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Purchase Request</h2>
        </header>


        <!-- widget div-->
        <div>

            <!-- widget edit box -->
            <div class="jarviswidget-editbox">
                <!-- This area used as dropdown edit box -->

            </div>
            <!-- end widget edit box -->

            <!-- widget content -->
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>

                        <div class="row">
                            <%--<section class="col col-4">
                                <label class="label">
                                    Request No.</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequestNo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>--%>
                            <section class="col col-6">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label id="lblRequestDate" runat="server" class="label" visible="true">
                                    Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>

                        </div>

                        <div class="row">

                            <section class="col col-4">
                                <label class="label">
                                    Required date of delivery</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtdeliveryDate" runat="server" Visible="true" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvdeliveryDate" CssClass="validator" runat="server" ControlToValidate="txtdeliveryDate" ErrorMessage="Delivery Date Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label class="label">
                                    Deliver to</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDeliverto" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvDeliverto" CssClass="validator" runat="server" ControlToValidate="txtDeliverto" ErrorMessage="Deliver To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                             <section class="col col-4">
                                <label class="label">
                                    Special Need</label>
                                <label class="input">
                                    <asp:TextBox ID="txtSpecialNeed" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-4">
                                <label class="label">
                                    Suggested Suppliers (if any)</label>
                                <label class="input">
                                    <asp:TextBox ID="txtSuggestedSupplier" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label id="lblapplyfor" runat="server" class="label" visible="true">
                                    Specifications</label>
                                <label class="input">
                                    <asp:TextBox ID="txtComment" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                             <section class="col col-4">
                                <label class="label">Payment Methods</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlPayMethods" AppendDataBoundItems="true" 
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
                            <section class="col col-4">
                                <label class="label">Total Purchase </label>
                                <label class="input">
                                    <asp:TextBox ID="txtTotal" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                          
                                <section class="col col-4">
                                    <asp:CheckBox ID="chkBudgeted" runat="server" Text="Budgeted" /><i></i>
                                </section>
                                
                                                
                          
                           
                        </div>
                        <asp:DataGrid ID="dgPurchaseRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            GridLines="None"
                            OnCancelCommand="dgPurchaseRequestDetail_CancelCommand" OnDeleteCommand="dgPurchaseRequestDetail_DeleteCommand" OnEditCommand="dgPurchaseRequestDetail_EditCommand"
                            OnItemCommand="dgPurchaseRequestDetail_ItemCommand" OnItemDataBound="dgPurchaseRequestDetail_ItemDataBound" OnUpdateCommand="dgPurchaseRequestDetail_UpdateCommand"
                            ShowFooter="True">

                            <Columns>
                                <asp:TemplateColumn HeaderText="Account Name">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlAccount" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id"
                                            ValidationGroup="proedit">
                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvAccount" runat="server" CssClass="validator"
                                            ControlToValidate="ddlAccount" ErrorMessage="Account Description Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFAccount" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd" AutoPostBack="True" OnSelectedIndexChanged="ddlFAccount_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Account</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFAccount" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFAccount" Display="Dynamic"
                                            ErrorMessage="Account Description Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Account Code">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAccountCode" runat="server" CssClass="form-control" Enabled="false" Text=' <%# DataBinder.Eval(Container.DataItem, "AccountCode")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFAccountCode" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Item")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Item")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFItem" runat="server" CssClass="form-control"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Qty">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtQty" ID="txtQty_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvQty" CssClass="validator" runat="server" ControlToValidate="txtQty" ErrorMessage="Qty Required" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFQty" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFQty" ID="txtFQty_FilteredTextBoxExtender" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvFQty" CssClass="validator" runat="server" ControlToValidate="txtFQty" ErrorMessage="Qty Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Price per unit">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Priceperunit")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPriceperunit" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Priceperunit")%>' Width="81px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtPriceperunit" ID="txtPriceperunit_FilteredTextBoxExtender" FilterType="Numbers, Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvtxtPriceperunit" CssClass="validator" runat="server" ControlToValidate="txtPriceperunit" ErrorMessage="Price per unit Required" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFPriceperunit" runat="server" CssClass="form-control" Width="66px"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtFPriceperunit" ID="txtFPriceperunit_FilteredTextBoxExtender" FilterType="Numbers, Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator ID="RfvFPriceperunit" CssClass="validator" runat="server" ControlToValidate="txtFPriceperunit" ErrorMessage="Price per unit Required" ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Estimated Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "EstimatedCost")%>
                                    </ItemTemplate>

                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Project ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="ProjectCode" DataValueField="Id"
                                            ValidationGroup="proedit" AutoPostBack="True" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvProject" runat="server" CssClass="validator"
                                            ControlToValidate="ddlProject" ErrorMessage="Project Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFProject" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="ProjectCode" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd" AutoPostBack="True" OnSelectedIndexChanged="ddlFProject_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Project</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFProjectCode" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFProject" Display="Dynamic"
                                            ErrorMessage="Project Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Grant ID">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlGrant" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                            ValidationGroup="proedit" OnSelectedIndexChanged="ddlGrant_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvGrant" runat="server" CssClass="validator"
                                            ControlToValidate="ddlGrant" ErrorMessage="Grant Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFGrant" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                            EnableViewState="true" ValidationGroup="proadd">
                                            <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFGrant" Display="Dynamic"
                                            ErrorMessage="Grant Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="proadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    </fieldset>

                    <footer>
                        <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" />
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <%--<asp:Button ID="btnsearch2" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" TabIndex="9" />

                        <%--<asp:Button ID="btnSearch" data-toggle="modal" runat="server" OnClientClick="#myModal" Text="Search" ></asp:Button>--%>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>

                </div>
            </div>
            <!-- end widget content -->

        </div>
        <!-- end widget div -->

    </div>

    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-colorbutton="false"
                        data-widget-editbutton="false"
                        data-widget-togglebutton="false"
                        data-widget-deletebutton="false"
                        data-widget-fullscreenbutton="false"
                        data-widget-custombutton="false"
                        data-widget-sortable="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Search Purchase Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                                <label class="input">
                                                    <i class="icon-append fa fa-calendar"></i>
                                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker"
                                                        data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                                </label>
                                            </section>
                                        </div>
                                    </fieldset>

                                    <footer>
                                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="Button1" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>
                        <asp:GridView ID="grvPurchaseRequestList"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            OnRowDataBound="grvPurchaseRequestList_RowDataBound" OnRowDeleting="grvPurchaseRequestList_RowDeleting"
                            OnSelectedIndexChanged="grvPurchaseRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPurchaseRequestList_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                <asp:TemplateField HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Required date of delivery">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Requireddateofdelivery", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="DeliverTo" HeaderText="Deliver To" SortExpression="DeliverTo" />
                                <asp:BoundField DataField="SuggestedSupplier" HeaderText="Suggested Supplier" SortExpression="SuggestedSupplier" />
                                <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" />
                                <asp:CommandField ShowSelectButton="True" />
                                <%-- <asp:CommandField ShowDeleteButton="True" />--%>
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
    <!-- /.modal-content -->
    <%--<asp:ModalPopupExtender runat="server" Enabled="True" PopupControlID="pnlSearch"
        TargetControlID="btnsearch2" CancelControlID="Button1" BackgroundCssClass="modalBackground" ID="pnlSearch_ModalPopupExtender">
    </asp:ModalPopupExtender>--%>

    <asp:Panel ID="pnlWarning" Visible="false" Style="position: absolute; top: 370px; left: 84px;" runat="server">
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
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


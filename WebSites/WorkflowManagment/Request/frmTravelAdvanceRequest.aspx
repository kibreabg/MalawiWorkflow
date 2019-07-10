<%@ Page Title="Travel Advance Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmTravelAdvanceRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmTravelAdvanceRequest" %>

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
    <div class="jarviswidget" id="wid-id-0" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Travel Advance Request</h2>
        </header>
        <div role="content">
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">                            
                           <%-- <section class="col col-6">
                                <label class="label">Travel Advance Number</label>
                                <label class="input">
                                    <asp:TextBox ID="txtTravelAdvanceNo" runat="server" ReadOnly="true"></asp:TextBox>
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
                                <label class="label">Purpose of Travel</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPurposeOfTravel" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">                            
                            <section class="col col-6">
                                <label class="label">Project</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlProject" AutoPostBack="true" runat="server" DataValueField="Id" DataTextField="ProjectCode" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged">
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="rfvddlProject" runat="server" ErrorMessage="Project is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlProject"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Grant</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlGrant" runat="server" DataValueField="Id" DataTextField="GrantCode">
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator
                                        ID="rfvGrant" runat="server" ErrorMessage="Grant is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="saveMain" InitialValue="0"
                                        SetFocusOnError="true" ControlToValidate="ddlGrant"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Visiting Team</label>
                                <label class="input">
                                    <asp:TextBox ID="txtVisitingTeam" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Additional Comments</label>
                                <label class="input">
                                    <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Travel Advance Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Total Travel Advance </label>
                                <label class="input">
                                    <asp:TextBox ID="txtTotal" ReadOnly="true" runat="server"></asp:TextBox>
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
                        <asp:DataGrid ID="dgTravelAdvanceRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            GridLines="None"
                            OnCancelCommand="dgTravelAdvanceRequestDetail_CancelCommand" OnDeleteCommand="dgTravelAdvanceRequestDetail_DeleteCommand" OnEditCommand="dgTravelAdvanceRequestDetail_EditCommand"
                            OnItemCommand="dgTravelAdvanceRequestDetail_ItemCommand" OnItemDataBound="dgTravelAdvanceRequestDetail_ItemDataBound" OnUpdateCommand="dgTravelAdvanceRequestDetail_UpdateCommand"
                            ShowFooter="True" OnSelectedIndexChanged="dgTravelAdvanceRequestDetail_SelectedIndexChanged">

                            <Columns>
                                <asp:TemplateColumn HeaderText="City From">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "CityFrom")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCityFrom" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "CityFrom")%>' Width="118px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCityFrom" runat="server" CssClass="form-control" Width="118px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="City To">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "CityTo")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtCityTo" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "CityTo")%>' Width="118px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtCityTo" runat="server" CssClass="form-control" Width="118px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Hotel Booked">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "HotelBooked")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:RadioButtonList ID="rblEdtHotelBooked" RepeatDirection="Horizontal" runat="server">
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:RadioButtonList ID="rblHotelBooked" RepeatDirection="Horizontal" runat="server">
                                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                            <asp:ListItem Value="No">No</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="From Date">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "FromDate", "{0:M/d/yyyy}")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtFromDate" runat="server" CssClass="form-control datepicker"
                                            data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "FromDate", "{0:M/d/yyyy}")%>' Width="81px"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvEdtFromDate" runat="server" ErrorMessage="From Date is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="edit"
                                            SetFocusOnError="true" ControlToValidate="txtEdtFromDate"></asp:RequiredFieldValidator><br />
                                        <asp:CompareValidator ID="cvEdtFromToDates" runat="server" ErrorMessage="Less Date"
                                            ControlToCompare="txtEdtToDate" ControlToValidate="txtEdtFromDate" CssClass="validator"
                                            ValidationGroup="edit" Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control datepicker"
                                            data-dateformat="mm/dd/yy" Width="81px"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtFromDate" runat="server" ErrorMessage="From Date is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="save"
                                            SetFocusOnError="true" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator><br />
                                        <asp:CompareValidator ID="cvFromToDates" runat="server" ErrorMessage="Less Date"
                                            ControlToCompare="txtToDate" ControlToValidate="txtFromDate" CssClass="validator"
                                            ValidationGroup="save" Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="To Date">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ToDate", "{0:M/d/yyyy}")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtToDate" runat="server" CssClass="form-control datepicker"
                                            data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "ToDate", "{0:M/d/yyyy}")%>' Width="81px"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtEdtToDate" runat="server" ErrorMessage="To Date is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="edit"
                                            SetFocusOnError="true" ControlToValidate="txtEdtToDate"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control datepicker"
                                            data-dateformat="mm/dd/yy" Width="81px"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvttxtToDate" runat="server" ErrorMessage="To Date is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="save"
                                            SetFocusOnError="true" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Mode Of Travel">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtModeOfTravel" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True"
                                            ValidationGroup="3">
                                            <asp:ListItem Value="">Select Mode Of Travel</asp:ListItem>
                                            <asp:ListItem Value="Chai Car/Hired Car">Chai Car/Hired Car</asp:ListItem>
                                            <asp:ListItem Value="Bus">Bus</asp:ListItem>
                                            <asp:ListItem Value="Air">Air</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvStatus" runat="server"
                                            ControlToValidate="ddlEdtModeOfTravel" ErrorMessage="Mode of Travel is required"
                                            InitialValue="" SetFocusOnError="True" ValidationGroup="edit" CssClass="validator"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlModeOfTravel" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" EnableViewState="true" ValidationGroup="save">
                                            <asp:ListItem Value="">Select Mode Of Travel</asp:ListItem>
                                            <asp:ListItem Value="Chai Car/Hired Car">Chai Car/Hired Car</asp:ListItem>
                                            <asp:ListItem Value="Bus">Bus</asp:ListItem>
                                            <asp:ListItem Value="Air">Air</asp:ListItem>

                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFStatus" runat="server"
                                            ControlToValidate="ddlModeOfTravel" Display="Dynamic" CssClass="validator"
                                            ErrorMessage="Mode of Travel is required" InitialValue="" SetFocusOnError="True"
                                            ValidationGroup="save"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ModeOfTravel")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>

                                <asp:TemplateColumn HeaderText="Air Fare">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "AirFare")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtAirFare" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "AirFare")%>' Width="67px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAirFare" runat="server" CssClass="form-control" Width="67px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="edit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="save" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="Add Cost"></asp:ButtonColumn>
                            </Columns>
                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                        </asp:DataGrid>
                    </fieldset>

                    <footer>
                        <asp:Button ID="btnSave" ValidationGroup="saveMain" runat="server" Text="Request" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" />--%>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-primary"
                            Text="Delete" OnClick="btnDelete_Click" Visible="false"></asp:Button>
                        <cc1:ConfirmButtonExtender ID="btnDelete_ConfirmButtonExtender" runat="server"
                            ConfirmText="Are you sure" Enabled="True" TargetControlID="btnDelete">
                        </cc1:ConfirmButtonExtender>
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" />
                          <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                        <asp:Button ID="btnHiddenPopupp" runat="server" />
                        <asp:HiddenField ID="hfDetailId" runat="server" />
                        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" Enabled="false" OnClientClick="javascript:Clickheretoprint('divprint')"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>

    </div>
    <%-- Modal --%>

    <%--<asp:Panel ID="pnlSearch" Visible="true" runat="server">--%>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>
                    <h4 class="modal-title" id="myModalLabel">Search Travel Advance Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Travel Advance Number</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Request No" runat="server"></asp:TextBox>
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
                                <asp:Button Text="Close" ID="Button1" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvTravelAdvanceRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnSelectedIndexChanged="grvTravelAdvanceRequestList_SelectedIndexChanged"
                                        AllowPaging="True" OnPageIndexChanging="grvTravelAdvanceRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="TravelAdvanceNo" HeaderText="Travel Advance No" SortExpression="TravelAdvanceNo" />
                                             <asp:TemplateField HeaderText="Request Date">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:BoundField DataField="VisitingTeam" HeaderText="Visiting Team" SortExpression="VisitingTeam" />
                                            <asp:BoundField DataField="PurposeOfTravel" HeaderText="Purpose Of Travel" SortExpression="PurposeOfTravel" />
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
    <%--<cc1:ModalPopupExtender runat="server" Enabled="True" PopupControlID="pnlSearch" CancelControlID="btnCancelSearch"
        TargetControlID="btnSearch" BackgroundCssClass="modalBackground" ID="pnlSearch_ModalPopupExtender">
    </cc1:ModalPopupExtender>--%>
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
    <asp:Panel ID="pnlTACost" Visible="true" runat="server">
        <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
            <header>
                <span class="widget-icon"><i class="fa fa-edit"></i></span>
                <h2>Travel Advance Costs</h2>
            </header>
            <div>
                <div class="jarviswidget-editbox"></div>
                <div class="widget-body no-padding">
                    <div class="smart-form">
                        <asp:DataGrid ID="dgTravelAdvanceRequestCost" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                            GridLines="None"
                            OnCancelCommand="dgTravelAdvanceRequestCost_CancelCommand" OnDeleteCommand="dgTravelAdvanceRequestCost_DeleteCommand" OnEditCommand="dgTravelAdvanceRequestCost_EditCommand"
                            OnItemCommand="dgTravelAdvanceRequestCost_ItemCommand" OnItemDataBound="dgTravelAdvanceRequestCost_ItemDataBound" OnUpdateCommand="dgTravelAdvanceRequestCost_UpdateCommand"
                            ShowFooter="True">

                            <Columns>
                                <asp:TemplateColumn HeaderText="Account Name">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlEdtAccountDescription" runat="server" Enabled="false" DataValueField="Id" DataTextField="AccountName" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Account Name--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                        <asp:RequiredFieldValidator
                                            ID="rfvddlEdtAccountDescription" runat="server" ErrorMessage="Account Description is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="editCost" InitialValue="0"
                                            SetFocusOnError="true" ControlToValidate="ddlEdtAccountDescription"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlAccountDescription" runat="server" Enabled="false" DataValueField="Id" DataTextField="AccountName" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Account Name--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                        <asp:RequiredFieldValidator
                                            ID="rfvddlAccountDescription" runat="server" ErrorMessage="Account Description is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="saveCost" InitialValue="0"
                                            SetFocusOnError="true" ControlToValidate="ddlAccountDescription"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Expense Type">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ExpenseType.ExpenseTypeName")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                          <asp:DropDownList ID="ddlEdtExpenseType" runat="server" Enabled="true" DataValueField="Id" DataTextField="ExpenseTypeName" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Expense Type--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                     
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                         <asp:DropDownList ID="ddlExpenseType" runat="server" Enabled="true" DataValueField="Id" DataTextField="ExpenseTypeName" AppendDataBoundItems="True">
                                            <asp:ListItem Value="0">--Select Expense Type--</asp:ListItem>
                                        </asp:DropDownList><i></i>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Days">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Days")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtDays" Text='<%# DataBinder.Eval(Container.DataItem, "Days")%>' runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtDays" ID="txtEdtDays_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtEdtDays" runat="server" ErrorMessage="No of days is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="editCost"
                                            SetFocusOnError="true" ControlToValidate="txtEdtDays"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtDays" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtDays" ID="txtDays_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtDays" runat="server" ErrorMessage="No of days is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="saveCost"
                                            SetFocusOnError="true" ControlToValidate="txtDays"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtUnitCost" Text='<%# DataBinder.Eval(Container.DataItem, "UnitCost")%>' runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtUnitCost" ID="txtEdtUnitCost_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtEdtUnitCost" runat="server" ErrorMessage="Unit cost is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="editCost"
                                            SetFocusOnError="true" ControlToValidate="txtEdtUnitCost"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUnitCost" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtUnitCost" ID="txtUnitCost_FilteredTextBoxExtender" FilterType="Custom, Numbers" ValidChars='"."'></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtUnitCost" runat="server" ErrorMessage="Unit cost is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="saveCost"
                                            SetFocusOnError="true" ControlToValidate="txtUnitCost"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="No of Units">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "NoOfUnits")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtNoOfUnits" Text='<%# DataBinder.Eval(Container.DataItem, "NoOfUnits")%>' runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtEdtNoOfUnits" ID="txtEdtNoOfUnits_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtEdtNoOfUnits" runat="server" ErrorMessage="No Of Units is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="editCost"
                                            SetFocusOnError="true" ControlToValidate="txtEdtNoOfUnits"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtNoOfUnits" runat="server"></asp:TextBox>
                                        <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtNoOfUnits" ID="txtNoOfUnits_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                        <asp:RequiredFieldValidator
                                            ID="rfvtxtNoOfUnits" runat="server" ErrorMessage="No Of Units is required" Display="Dynamic"
                                            CssClass="validator" ValidationGroup="saveCost"
                                            SetFocusOnError="true" ControlToValidate="txtNoOfUnits"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Total")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="editCost" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="saveCost" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                        </asp:DataGrid>
                        <footer>
                            <asp:Button ID="btnCancelCost" runat="server" Text="Close" class="btn btn-primary"></asp:Button>
                        </footer>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <cc1:ModalPopupExtender runat="server" Enabled="True" CancelControlID="btnCancelCost"
        ID="pnlTACost_ModalPopupExtender" TargetControlID="btnHiddenPopupp" BackgroundCssClass="modalBackground"
        PopupControlID="pnlTACost">
    </cc1:ModalPopupExtender>
    <div id="divprint" style="display: none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align: left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE<br />
                            TRAVEL ADVANCE REQUEST FORM</strong></td>
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
                    <td style="width: 848px; height: 18px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblVisitingTeam" runat="server" Text="Visiting Team:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblVisitingTeamResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblPurposeOfTravel" runat="server" Text="Purpose of Travel:"></asp:Label></strong></td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblPurposeOfTravelResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 390px; padding-left: 11%;">
                        <strong>
                            <asp:Label ID="lblTotalTravelAdvance" runat="server" Text="Total Travel Advance:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblTotalTravelAdvanceResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblComments" runat="server" Text="Comments:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblCommentsResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 848px; height: 18px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <strong>
                            <asp:Label ID="lblApprovalStatusPrint" runat="server" Text="Approval Status:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblApprovalStatusResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblProjectId" runat="server" Text="Project ID:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 389px; height: 18px;">
                        <asp:Label ID="lblProjectIdResult" runat="server"></asp:Label>
                    </td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
            </table>
            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="CityFrom" HeaderText="City From" SortExpression="CityFrom" />
                    <asp:BoundField DataField="CityTo" HeaderText="City To" SortExpression="CityTo" />
                    <asp:BoundField DataField="HotelBooked" HeaderText="Hotel Booked" SortExpression="HotelBooked" />
                    <asp:BoundField DataField="FromDate" HeaderText="From Date" SortExpression="FromDate" />
                    <asp:BoundField DataField="ToDate" HeaderText="To Date" SortExpression="ToDate" />
                    <asp:BoundField DataField="ModeOfTravel" HeaderText="Mode of Travel" SortExpression="ModeOfTravel" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvCost"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ExpenseType.ExpenseTypeName" HeaderText="Expense Type" SortExpression="ExpenseType.ExpenseTypeName" />
                    <asp:BoundField DataField="AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                    <asp:BoundField DataField="Days" HeaderText="Days" SortExpression="Days" />
                    <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />
                    <asp:BoundField DataField="NoOfUnits" HeaderText="No Of Units" SortExpression="NoOfUnits" />
                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses" OnRowDataBound="grvStatuses_RowDataBound"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                    <asp:BoundField HeaderText="Approver" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>


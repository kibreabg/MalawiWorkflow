<%@ Page Title="Vehicle Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmVehicleRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmVehicleRequest" %>

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
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="true" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Vehicle Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <%--<section class="col col-6">
                                <label class="label">Request Number</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequestNo" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvtxtRequestNo" runat="server" ErrorMessage="Request number is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="save" SetFocusOnError="true" ControlToValidate="txtRequestNo"></asp:RequiredFieldValidator>
                                </label>
                            </section>--%>
                            <section class="col col-6">
                                <label class="label">Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Departure Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDepartureDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvtxtDepartureDate" runat="server" ErrorMessage="Departure Date is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="save" ControlToValidate="txtDepartureDate"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" CssClass="validator" runat="server" ErrorMessage="Departure Date must be less than Returning Date" ControlToCompare="txtReturningDate" ControlToValidate="txtDepartureDate" ValidationGroup="save" Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Returning Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtReturningDate" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvtxtReturningDate" runat="server" ErrorMessage="Returning Date is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="save" ControlToValidate="txtReturningDate"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Departure Time</label>
                                <label class="input">
                                    <i class="icon-append fa fa-clock-o"></i>
                                    <asp:TextBox ID="timepicker" CssClass="form-control timepicker-orient-top" Text="" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Vehicle Requesting Personnel</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequestingPersonnel" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                       
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Purpose of Travel</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPurposeOfTravel" TextMode="MultiLine" Width="100%" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator
                                        ID="rfvtxtPurposeOfTravel" runat="server" ErrorMessage="Purpose of Travel is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="save" ControlToValidate="txtPurposeOfTravel"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Number of Passengers</label>
                                <label class="input">
                                    <asp:TextBox ID="txtNoOfPassangers" runat="server"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtNoOfPassangers" ID="txtNoOfPassangers_FilteredTextBoxExtender" FilterType="Numbers"></cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator
                                        ID="rfvtxtNoOfPassangers" runat="server" ErrorMessage="Number of passengers is required" Display="Dynamic"
                                        CssClass="validator" ValidationGroup="save" ControlToValidate="txtNoOfPassangers"></asp:RequiredFieldValidator>
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
                                <label class="label">Destination</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Comment </label>
                                <label style="color: green;">(Please add additional info about your travel)</label>
                                <label class="input">
                                    <asp:TextBox ID="txtComment" TextMode="MultiLine" Rows="5" Width="100%" runat="server"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                    </fieldset>

                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Request" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="save" CssClass="btn btn-primary"></asp:Button>
                        <%--<asp:Button ID="btnSearch" data-toggle="modal" href="#myModal" Text="Search" CssClass="btn btn-primary"></asp:Button>--%>
                        <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CssClass="btn btn-primary" Text="Delete" OnClick="btnDelete_Click"></asp:Button>

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
    <%--<cc1:ModalPopupExtender runat="server" DynamicServicePath="" PopupControlID="myModal"
        Enabled="True" TargetControlID="btnSearch" CancelControlID="btnCancelSearch" BackgroundCssClass="modalBackground"
        ID="pnlSearch_ModalPopupExtender">
    </cc1:ModalPopupExtender>--%>
    <%--<asp:Panel ID="pnlSearch" runat="server">--%>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Vehicle Requests</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="txtSrchRequestNo">Request Number</label>
                                <asp:TextBox ID="txtSrchRequestNo" CssClass="form-control" ToolTip="Request No" runat="server"></asp:TextBox>
                            </div>
                        </div>
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
                    </div>
                    <div class="row" style="text-align: right;">
                        <div class="col-md-12">
                            <div class="form-group">
                                <asp:Button ID="btnFind" runat="server" OnClick="btnFind_Click" Text="Find" CssClass="btn btn-primary"></asp:Button>
                                <asp:Button ID="btnCancelSearch" Text="Close" runat="server" class="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="form-group">
                                <div class="well well-sm well-primary">
                                    <asp:GridView ID="grvVehicleRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        OnRowDataBound="grvVehicleRequestList_RowDataBound" OnSelectedIndexChanged="grvVehicleRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvVehicleRequestList_PageIndexChanging"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                            <asp:TemplateField HeaderText="Request Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequestDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Departure Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReportedDate" runat="server" Text='<%# Eval("DepartureDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DepartureTime" HeaderText="Departure Time" SortExpression="DepartureTime" />
                                            <asp:TemplateField HeaderText="Returning Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReturningDate" runat="server" Text='<%# Eval("ReturningDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

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
    <%-- </asp:Panel>--%>


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
                                            The current Request has no Approval Settings defined. Please contact your administrator
                                        </p>
                                    </div>
                                    <footer>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
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

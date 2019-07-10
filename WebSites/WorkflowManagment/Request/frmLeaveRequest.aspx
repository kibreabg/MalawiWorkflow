<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmLeaveRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmLeaveRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#searchModal').modal('show');
            });
        }
    </script>
    <asp:ValidationSummary ID="VSLeaveRequest" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Leave Request</h2>
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
                           <%-- <section class="col col-3">
                                <label class="label">
                                    Request No.</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequestNo" runat="server" Visible="true" ReadOnly="true"></asp:TextBox>
                                </label>
                            </section>--%>
                            <section class="col col-4">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label class="label">
                                    Employee No.
                                </label>
                                <label class="input">
                                    <asp:TextBox ID="txtEmployeeNo" runat="server" Visible="true"></asp:TextBox></label>
                            </section>
                            <section class="col col-4">
                                <label id="lblRequestDate" runat="server" class="label" visible="true">
                                    Request Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" ReadOnly="true" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-3">
                                <label class="label">
                                    Leave Type</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlLeaveType" runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataTextField="LeaveTypeName" DataValueField="Id" OnSelectedIndexChanged="ddlLeaveType_SelectedIndexChanged1">
                                        <asp:ListItem Value="0">Select Leave Type</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="RfvRequestType" runat="server" CssClass="validator" ControlToValidate="ddlLeaveType" ErrorMessage="Leave Type Required" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-2">
                                <label cssclass="label" id="lblAddress" runat="server" visible="false">
                                    Address while on leave
                                </label>
                                <label class="input">
                                    <asp:TextBox ID="txtAddress" runat="server" Visible="false" TabIndex="1"></asp:TextBox></label>
                            </section>
                            <section class="col col-2">
                                <label cssclass="label" id="lblCompReason" runat="server" visible="false">
                                    Reason</label>
                                <label class="input">
                                    <asp:TextBox ID="txtCompReason" runat="server" Visible="false" TabIndex="2"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Date From</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateFrom" runat="server" Visible="true" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" TabIndex="3" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvDateFrom" runat="server" ControlToValidate="txtDateFrom" CssClass="validator" ErrorMessage="Date From Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Date From must be less than Date To" ControlToCompare="txtDateTo" ControlToValidate="txtDateFrom" ValidationGroup="Save" Type="Date" Operator="LessThanEqual"></asp:CompareValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">
                                    Date To</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateTo" runat="server" Visible="true" CssClass="form-control datepicker"
                                        data-dateformat="mm/dd/yy" TabIndex="4" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvDateTo" runat="server" ControlToValidate="txtDateTo" CssClass="validator" ErrorMessage="Date To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-4">
                                <label id="lblapplyfor" runat="server" class="label" visible="true">
                                    I wish to apply for
                                </label>
                                <label class="input">
                                    <asp:TextBox ID="txtapplyfor" runat="server" Visible="true" placeholder="Days" TabIndex="6" AutoPostBack="True" OnTextChanged="txtapplyfor_TextChanged"></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="txtapplyfor_FilteredTextBoxExtender" runat="server" FilterType="Custom, Numbers" TargetControlID="txtapplyfor" ValidChars=".">
                                </cc1:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="Rfvapplyfor" runat="server" CssClass="validator" ControlToValidate="txtapplyfor" ErrorMessage="I wish to apply for Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-4">
                                <asp:Label runat="server" ID="lblforward" class="label">
                                    Leave day's brought forward</asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtforward" runat="server" ReadOnly="true" Visible="true" OnTextChanged="txtforward_TextChanged" placeholder="Days" TabIndex="5" AutoPostBack="True"></asp:TextBox>
                                    <asp:Label ID="lblnoempleavesetting" runat="server" Text="" ForeColor="Red" Visible="true"></asp:Label>
                                    <asp:Label ID="lblOpeningBalance" runat="server" Text="Opening Balance:-" ForeColor="Green" Visible="false"></asp:Label><asp:Label ID="lblOBValue" runat="server" Text="" ForeColor="Green" Visible="false"></asp:Label>
                                    <%--<asp:RequiredFieldValidator ID="RfvForward" runat="server" ControlToValidate="txtforward" ErrorMessage="Leave day's brought forward Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>--%>
                                </label>
                            </section>
                            <section class="col col-4">
                                <asp:Label runat="server" ID="lblBalance" class="label">
                                    Leave days balance
                                </asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtbalance" runat="server" Visible="true" placeholder="Days" TabIndex="7" Enabled="False"></asp:TextBox></label>
                                <%--<asp:RequiredFieldValidator ID="rfvbalance" runat="server" ControlToValidate="txtbalance" ErrorMessage="Leave days balance Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>--%>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" TabIndex="8" />
                        <%--<asp:Button ID="btnsearch2" runat="server" CssClass="btn btn-primary" Text="Search" TabIndex="10" OnClick="btnsearch2_Click" />--%>
                        <a class="btn btn-primary" data-toggle="modal" href="#searchModal"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');" TabIndex="9" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="New" TabIndex="10" />
                        <asp:Button ID="btnClosepage" runat="server" Text="Close"  CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
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
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-deletebutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Search Leave Request</h2>
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
                                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary" CommandName="Find"></asp:Button>
                                        <asp:Button ID="Button1" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>

                        <asp:GridView ID="grvLeaveRequestList"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            OnRowDataBound="grvLeaveRequestList_RowDataBound" OnRowDeleting="grvLeaveRequestList_RowDeleting"
                            OnSelectedIndexChanged="grvLeaveRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvLeaveRequestList_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                                <asp:TemplateField HeaderText="Request Date">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                <asp:BoundField DataField="LeaveType.LeaveTypeName" HeaderText="Leave Type" SortExpression="LeaveType.LeaveTypeName" />
                                 <asp:TemplateField HeaderText="Date From">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("DateFrom", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Date To">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("DateTo", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
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
        <!-- /.modal-content -->
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
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


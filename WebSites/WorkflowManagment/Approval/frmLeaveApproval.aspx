<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmLeaveApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmLeaveApproval" %>

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
            <h2>Search Leave Request</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-3">
                                <asp:Label ID="lblRequestNosearch" runat="server" Text="Request No" CssClass="label"></asp:Label>

                                <label class="input">
                                    <asp:TextBox ID="txtRequestNosearch" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblRequestDatesearch" runat="server" Text="Request Date" CssClass="label"></asp:Label>

                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDatesearch" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-3">
                                <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="label"></asp:Label>

                                <label class="select">
                                    <asp:DropDownList ID="ddlProgressStatus" runat="server"></asp:DropDownList><i></i>

                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnPop" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

        <asp:GridView ID="grvLeaveRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            OnRowDataBound="grvLeaveRequestList_RowDataBound" OnRowDeleting="grvLeaveRequestList_RowDeleting"
            OnSelectedIndexChanged="grvLeaveRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvLeaveRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:BoundField HeaderText="Requester" />
                 <asp:TemplateField HeaderText="Request Date">
                                            <ItemTemplate>
                                              <asp:Label ID="lblRequestedDate" runat="server" Text='<%# Eval("RequestedDate", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                <asp:BoundField DataField="LeaveType.LeaveTypeName" HeaderText="LeaveType" SortExpression="LeaveType.LeaveTypeName" />
                <asp:TemplateField HeaderText="Date From">
                                            <ItemTemplate>
                                              <asp:Label ID="lblFrom" runat="server" Text='<%# Eval("DateFrom", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                    <asp:TemplateField HeaderText="Date To">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDateTo" runat="server" Text='<%# Eval("DateTo", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                
                <asp:CommandField ShowSelectButton="True" SelectText="Process Request" ButtonType="Button" />
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
            <asp:Button runat="server" ID="btnInProgress" Text="" BorderStyle="None"  BackColor="#FFFF6C"/>  <B>In Progress</B><br />
            <asp:Button runat="server" ID="btnComplete" Text="" BorderStyle="None" BackColor="#FF7251"/>  <B>Completed</B>

        </div>
        <br />

    </div>
    <asp:Panel ID="pnlApproval"  runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;</button>

                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Process Request</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">

                                            <section class="col col-3">
                                                <asp:Label ID="lblLeaveType" runat="server" Text="Leave Type" CssClass="label"></asp:Label>

                                                <asp:Label ID="lblLeaveTyperes" runat="server" Text=" " CssClass="label"></asp:Label>
                                            </section>
                                            <section class="col col-3">
                                                <asp:Label ID="lblRequestedDays" runat="server" Text="Requested Days" CssClass="label"></asp:Label>

                                                <asp:Label ID="lblrequesteddaysres" runat="server" Text="" CssClass="label"></asp:Label>
                                            </section>
                                             <section class="col col-3">
                                                <asp:Label ID="lblViewBalance" runat="server" Text="Employee Leave Balance" CssClass="label" Visible="false"></asp:Label>

                                                <asp:Label ID="lblViewBalRes" runat="server" Text="" CssClass="label" Visible="false"></asp:Label>
                                            </section>
                                            
                                        </div>
                                        <div class="row">

                                            <section class="col col-6">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>

                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged" ValidationGroup="Approve" AutoPostBack="True">
                                                        
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus" ValidationGroup="Approve"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>
                                                <label class="textarea">
                                                    <asp:TextBox ID="txtRejectedReason" runat="server" TextMode="MultiLine" Visible="false"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRejectedReason" runat="server" Enabled="false" CssClass="validator" ValidationGroup="save" ErrorMessage="Must Enter Rejection Reason" ControlToValidate="txtRejectedReason"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" Text="Save" OnClick="btnApprove_Click" Enabled="false" CssClass="btn btn-primary" ValidationGroup="Approve"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
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
    <div id="divprint" style="display: none; width: 942px;">
        <fieldset>
             <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align:left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            EMPLOYEE LEAVE REQUEST FORM</strong></td>
                </tr>
            </table>
        <table style="width: 100%">
            
            <tr>
                <td align="right" style="width: 682px; height: 17px;"></td>
                <td align="right" style="width: 390px; height: 17px;"></td>
                <td align="right" style="width: 334px; height: 17px;"></td>
                <td align="right" style="width: 335px; height: 17px;"></td>
                <td style="height: 17px"></td>
            </tr>
            <tr>
                <td align="right" style="width: 682px">
                    <strong>
                        <asp:Label ID="lblRequestNo" runat="server" Text="Request No."></asp:Label>
                        :</strong></td>
                <td align="right" style="width: 390px">
                <asp:Label ID="lblRequestNoresult" runat="server"></asp:Label>
                </td>
                <td align="right" style="width: 334px">&nbsp;</td>
                <td align="right" style="width: 335px">
                    &nbsp;</td>
                <td>:
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 682px">
                    <strong>
                        <asp:Label ID="lblRequestedDate" runat="server" Text="Requested Date"></asp:Label>
                        :</strong></td>
                <td align="right" style="width: 390px">
                    <asp:Label ID="lblRequestedDateresult" runat="server"></asp:Label>
                </td>
                <td align="right" style="width: 334px">&nbsp;</td>
                <td align="right" style="width: 335px">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="width: 682px">
                    <strong>
                        <asp:Label ID="lblRequester" runat="server" Text="Requester"></asp:Label>
                        :</strong></td>
                <td align="right" style="width: 390px">
                    <asp:Label ID="lblRequesterres" runat="server"></asp:Label>
                </td>
                <td align="right" style="width: 334px">&nbsp;</td>
                <td align="right" style="width: 335px">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="width: 682px; height: 18px;">
                    <strong>
                        <asp:Label ID="lblEmployeeNo" runat="server" Text="Employee No"></asp:Label>
                        :</strong></td>
                <td align="right" style="width: 390px; height: 18px;">
                    <asp:Label ID="lblEmpNoRes" runat="server"></asp:Label>
                </td>
                <td align="right" style="width: 334px; height: 18px;">&nbsp;</td>
                <td align="right" style="width: 335px; height: 18px;">
                    &nbsp;</td>
                <td style="height: 18px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="right" style="width: 682px; height: 18px;">&nbsp;</td>
                <td align="right" style="width: 390px; height: 18px;">&nbsp;</td>
                <td align="right" style="width: 334px; height: 18px;">&nbsp;</td>
                <td align="right" style="width: 335px; height: 18px;">
                    &nbsp;</td>
                <td style="height: 18px">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 682px; height: 18px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                <strong>
                    <asp:Label ID="lblDateFfrom" runat="server" Text="Date From"></asp:Label>
                </strong>
                </td>
                <td style="width: 390px; height: 18px;">
                    <asp:Label ID="lblDatefromres" runat="server"></asp:Label>
                </td>
                <td style="width: 334px; height: 18px;">
                    <strong>
                        <asp:Label ID="lblDateTo" runat="server" Text="Date To"></asp:Label>
                        :</strong></td>
                <td style="width: 335px; height: 18px;">
                    <asp:Label ID="lblDatetores" runat="server"></asp:Label>
                </td>
                <td style="height: 18px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 682px; height: 18px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <strong>
                    <asp:Label ID="lblRequesteddaysP" runat="server" Text="Requested Days"></asp:Label>
                </strong>
                </td>
                <td style="width: 390px; height: 18px;">
                    <asp:Label ID="lblrequesteddaysresp" runat="server"></asp:Label>
                </td>
                <td style="width: 334px; height: 18px;">
                    <strong>
                        <asp:Label ID="lblBalance" runat="server" Text="Balance"></asp:Label>
                    </strong>
                </td>
                <td style="width: 335px; height: 18px;">
                    <asp:Label ID="lblbalanceres" runat="server"></asp:Label>
                </td>
                <td style="height: 18px">&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 682px; height: 18px;">
                    <strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblLeavetypep" runat="server" Text="Leave Type"></asp:Label>
                    </strong>
                </td>
                <td style="width: 390px; height: 18px;">
                    <asp:Label ID="lblleavetyperesp" runat="server"></asp:Label>
                </td>
                <td style="width: 334px; height: 18px;">
                    <strong>
                        <asp:Label ID="lblApprovalStatusp" runat="server" Text="Approval Status"></asp:Label>
                    </strong>
                </td>
                <td style="width: 335px; height: 18px;">
                    <asp:Label ID="lblapprovalstatusres" runat="server"></asp:Label>
                </td>
                <td style="height: 18px">&nbsp;</td>
            </tr>
            
        </table>
            <br />
         <br />
        <asp:GridView ID="grvStatuses" CellPadding="5" CellSpacing="3"
            runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDataBound="grvStatuses_RowDataBound"
            CssClass="table table-striped table-bordered table-hover">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                   <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ApprovalDate", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                
                <asp:BoundField  HeaderText="Approver"  />
                <asp:BoundField DataField="AssignedBy" HeaderText="Assignee Approver" SortExpression="AssignedBy" />
                <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus"/>
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
            </fieldset>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


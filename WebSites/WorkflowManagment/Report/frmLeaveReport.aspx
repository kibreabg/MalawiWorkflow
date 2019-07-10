<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLeaveReport.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmLeaveReport"
    Title="Leave Report" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Leave Report</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Employee Name</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlEmployeeName" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Leave Type</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlLeaveType" runat="server" AppendDataBoundItems="True" DataTextField="LeaveTypeName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Leave Type</asp:ListItem>
                                    </asp:DropDownList><i></i>                                    
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>
    </div>
    <asp:Panel ID="Panel1" runat="server" BackColor="White" Visible="false">
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="100%">
            <LocalReport ReportPath="Report\LeaveReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="GetLeaveReport" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </asp:Panel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetLeaveReport" TypeName="Chai.WorkflowManagment.Modules.Report.ReportController">
        <SelectParameters>
            <asp:FormParameter FormField="txtDateFrom" Name="DateFrom" Type="String" />
            <asp:FormParameter FormField="txtDateTo" Name="DateTo" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

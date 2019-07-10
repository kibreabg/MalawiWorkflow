<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmBankPaymentReport.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmBankPaymentReport"
    Title="Payment Report" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>payment Report</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Date From</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox></label>
                            </section>
                            <section class="col col-6">
                                <label class="label">Date To</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox></label>
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

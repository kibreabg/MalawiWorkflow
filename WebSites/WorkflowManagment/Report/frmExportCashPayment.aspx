<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmExportCashPayment.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Report.Views.frmExportCashPayment"
    Title="Export Cash Payment" MasterPageFile="~/Shared/ModuleMaster.master" %>


<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Export Cash Payment</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-4">
                                <label class="label">Date From</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox></label>
                            </section>
                            <section class="col col-4">
                                <label class="label">Date To</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox></label>
                            </section>
                            <section class="col col-4">
                                <label class="label">Export Status</label>
                                <label class="select">

                                    <asp:DropDownList ID="ddlExportType" runat="server">
                                        <asp:ListItem>Not Exported</asp:ListItem>
                                        <asp:ListItem>Exported</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click"></asp:Button>
                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-primary" OnClick="btnExport_Click"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>

        <asp:GridView ID="grvCashPaymentRequestList"
            runat="server"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
            <RowStyle CssClass="rowstyle" />

            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />
            <RowStyle CssClass="rowstyle" />
        </asp:GridView>
    </div>


</asp:Content>

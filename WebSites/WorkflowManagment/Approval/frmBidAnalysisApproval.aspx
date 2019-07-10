<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="frmBidAnalysisApproval.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmBidAnalysisApproval" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
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
    </script>
    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Bid Analysis Approval</h2>
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
                                    <asp:TextBox ID="txtRequestNosearch" runat="server"></asp:TextBox>
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
                        <asp:Button ID="btnPop2" runat="server" />
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

        <asp:GridView ID="grvPurchaseRequestList"
            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
            OnRowDataBound="grvPurchaseRequestList_RowDataBound" OnRowDeleting="grvPurchaseRequestList_RowDeleting"
            OnSelectedIndexChanged="grvPurchaseRequestList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvPurchaseRequestList_PageIndexChanging"
            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" OnRowCommand="grvPurchaseRequestList_RowCommand" PageSize="30">
            <RowStyle CssClass="rowstyle" />
            <Columns>
                <asp:BoundField DataField="RequestNo" HeaderText="Request No" SortExpression="RequestNo" />
                <asp:BoundField HeaderText="Requester" />
                <asp:TemplateField HeaderText="Request Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("RequestDate", "{0:dd/MM/yyyy}")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="PurchaseRequest.SuggestedSupplier" HeaderText="Suggested Supplier" SortExpression="PurchaseRequest.SuggestedSupplier" />
                
               
                <asp:BoundField   DataField="TotalPrice"  DataFormatString="{0:C}" HeaderText="Total Price" SortExpression="TotalPrice" />
               
                <asp:ButtonField ButtonType="Button" CommandName="ViewItem" Text="View Item Detail" />
                <asp:CommandField ButtonType="Button" SelectText="Process Request" ShowSelectButton="True" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button runat="server" ID="btnStatus" Text="" BorderStyle="None" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="headerstyle" />
            <PagerStyle CssClass="PagerStyle" />

        </asp:GridView>
        <div>
            <asp:Button runat="server" ID="btnInProgress" Text="" BorderStyle="None" BackColor="#FFFF6C" />
            <b>In Progress</b><br />
            <asp:Button runat="server" ID="btnComplete" Text="" BorderStyle="None" BackColor="#FF7251" />
            <b>Completed</b><br />
            <asp:Button runat="server" ID="btnAuthorized" Text="" BorderStyle="None" BackColor="#112552" />
            <b>Authorized</b>

        </div>
        <br />
        <br />




    </div>
    <asp:Panel ID="pnlDetail" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Requested Item Detail</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">

                                    <asp:DataGrid ID="dgPurchaseRequestDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped table-bordered table-hover" DataKeyField="Id" GridLines="None" OnItemDataBound="dgPurchaseRequestDetail_ItemDataBound" PagerStyle-CssClass="paginate_button active" ShowFooter="True" OnItemCommand="dgPurchaseRequestDetail_ItemCommand" OnItemCreated="dgPurchaseRequestDetail_ItemCreated">
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Supplier">


                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Bidder.Supplier.SupplierName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                              <asp:TemplateColumn HeaderText="Account Code">


                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Item">


                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Rank">
                                                <ItemTemplate>
                                                    <asp:Label id="lblRank" runat="server" Text= '<%# DataBinder.Eval(Container.DataItem, "Bidder.Rank")%>'></asp:Label>
                                                </ItemTemplate>
                                           </asp:TemplateColumn>
                                              <asp:TemplateColumn HeaderText="Reason For Selection">
                                                <ItemTemplate>
                                           
                                            <asp:Label id="lblReason" runat="server" Text= '<%# DataBinder.Eval(Container.DataItem, "Bidder.GetSelectionReason")%>'></asp:Label>
                                                </ItemTemplate>
                                           </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Qty">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                                </ItemTemplate>


                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Price per unit">
                                                <ItemTemplate>                                                     
                                                  $  <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                                                </ItemTemplate>


                                            </asp:TemplateColumn>
                                             <asp:TemplateColumn HeaderText="Total Price">
                                                   <ItemTemplate>                                                   
                                                   $  <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                                                </ItemTemplate>


                                            </asp:TemplateColumn>
                                          
                                           


                                        </Columns>
                                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                    </asp:DataGrid>


                                    <footer>

                                        <asp:Button ID="btnCancelPopup2" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup2_Click"></asp:Button>

                                    </footer>
                                </div>
                            </div>
                        </div>

                    </div>



                </div>




            </div>

        </div>

    </asp:Panel>
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop2" PopupControlID="pnlDetail" CancelControlID="btnCancelPopup2"
        ID="pnlDetail_ModalPopupExtender">
    </asp:ModalPopupExtender>
    <asp:Panel ID="pnlApproval" runat="server">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
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
                                            <asp:Panel ID="pnlInfo" runat="server" Visible="false">
                                                <div class="alert alert-info fade in">

                                                    <i class="fa-fw fa fa-info"></i>
                                                    <strong>Info!</strong> Please perform Bid Analysis before Approving Bid.
                                                </div>
                                            </asp:Panel>
                                            <section class="col col-6">
                                                <asp:Label ID="lblApprovalStatus" runat="server" Text="Approval Status" CssClass="label"></asp:Label>

                                                <label class="select">
                                                    <asp:DropDownList ID="ddlApprovalStatus" runat="server" OnSelectedIndexChanged="ddlApprovalStatus_SelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList><i></i>
                                                    <asp:RequiredFieldValidator ID="RfvApprovalStatus" CssClass="validator" runat="server" ErrorMessage="Approval Status Required" InitialValue="0" ControlToValidate="ddlApprovalStatus" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </label>
                                            </section>
                                            <section class="col col-6">
                                                <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason" Visible="false" CssClass="label"></asp:Label>

                                                <label class="textarea">
                                                    <asp:TextBox ID="txtRejectedReason" runat="server" Visible="false" TextMode="MultiLine"></asp:TextBox>
                                                </label>
                                            </section>
                                         
                                        </div>
                                        <div class="row">
                                            <div class="row">
                                                <section class="col col-12">
                                                    <asp:Label ID="lblAttachments" runat="server" Text="Attachments" CssClass="label"></asp:Label>
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
                                                        </Columns>
                                                        <FooterStyle CssClass="FooterStyle" />
                                                        <HeaderStyle CssClass="headerstyle" />
                                                        <PagerStyle CssClass="PagerStyle" />
                                                        <RowStyle CssClass="rowstyle" />
                                                    </asp:GridView>
                                                </section>
                                            </div>
                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnApprove" runat="server" Text="Save" OnClick="btnApprove_Click" Enabled="true" CssClass="btn btn-primary" ValidationGroup="Save"></asp:Button>
                                        <asp:Button ID="btnCancelPopup" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" OnClick="btnCancelPopup_Click"></asp:Button>
                                        <asp:Button ID="btnPrint0" runat="server" Text="Print" CssClass="btn btn-primary" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False"></asp:Button>
                                        <asp:Button ID="btnPurchaseOrder" runat="server" CssClass="btn btn-primary" Enabled="false" OnClick="btnPurchaseOrder_Click" Text="Purchase Order" Visible="False" />
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
                            BID ANALYSIS REQUEST FORM</strong></td>
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
                    
                    <td style="width: 848px">
                        <strong>
                            <asp:Label ID="lblPaytype" runat="server" Text="Payment Method:"></asp:Label>
                        </strong></td>
                      <td style="width: 390px">
                        <asp:Label ID="lblpaytypeRes" runat="server" Text="" class="label"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblCommentPrint" runat="server" Text="Reason For Selection:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblCommentResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
              
                 <tr>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblRequireddateofdelivery" runat="server" Text="Special Need:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblRequireddateofdeliveryResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;"></td>
                    <td style="height: 18px">&nbsp;</td>
                </tr>
                   <tr>
                    <td style="width: 848px; height: 18px;">
                        <strong>
                            <asp:Label ID="lblTotalPrice" runat="server" Text="Total Price:"></asp:Label>
                        </strong></td>
                    <td style="width: 390px; height: 18px;">
                        <asp:Label ID="lblTotalPriceResult" runat="server"></asp:Label>
                    </td>
                    <td style="width: 389px; height: 18px;">&nbsp;</td>
                    <td style="width: 389px; height: 18px;"></td>
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
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground"
        Enabled="True" TargetControlID="btnPop" PopupControlID="pnlApproval" CancelControlID="btnCancelPopup"
        ID="pnlApproval_ModalPopupExtender">
    </asp:ModalPopupExtender>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


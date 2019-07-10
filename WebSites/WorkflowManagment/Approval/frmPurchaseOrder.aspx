<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmPurchaseOrder.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.frmPurchaseOrder" %>
<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MenuContent" runat="Server">
</asp:Content>
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
    <asp:ValidationSummary ID="VSBid" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Purchase Order</h2>
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
                            <section class="col col-4">
                                <label class="label">
                                    Purchase Order No.</label>
                                <label class="input">
                                    <asp:TextBox ID="txtPONo" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label class="label">
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                            <section class="col col-4">
                                <label id="lblDate" runat="server" class="label" visible="true">
                                    Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtDate" runat="server" Visible="true" CssClass="form-control datepicker"></asp:TextBox>
                                </label>
                            </section>

                        </div>

                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Bill to</label>
                                <label class="input">
                                    <asp:TextBox ID="txtBillto" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvBillTo" runat="server" CssClass="validator" ControlToValidate="txtBillto" ErrorMessage="Bill To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </label>
                            </section>
                            <section class="col col-6">
                                <label class="label">
                                    Ship To</label>
                                <label class="input">
                                    <asp:TextBox ID="txtShipTo" runat="server" Visible="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvShipTo" runat="server" CssClass="validator" ControlToValidate="txtShipTo" ErrorMessage="Ship To Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">
                                    Delivery Fees</label>
                                <label class="input">
                                    <asp:TextBox ID="txtDeliveeryFees" runat="server" Visible="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtDeliveeryFees" ID="txtDeliveeryFees_FilteredTextBoxExtender" FilterType="Numbers,Custom" ValidChars="."></asp:FilteredTextBoxExtender>
                                    <asp:RequiredFieldValidator ID="RfvtxtDeliveeryFees" runat="server" CssClass="validator" ControlToValidate="txtDeliveeryFees" ErrorMessage="Delivery Fees Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                     <section class="col col-6">
                                                        <label class="label">
                                                       Payment Terms</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtPaymentTerms" runat="server" Visible="true"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RfvPaymentTerms" runat="server" CssClass="validator" ControlToValidate="txtPaymentTerms" ErrorMessage="Payment Terms Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                                        </label>
                                                    </section>
                                                </div>
                                                <div class="row">
                                                 <section class="col col-4">
                                                        <label class="label">
                                                       Supplier Name</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtSupplierName" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                    <section class="col col-4">
                                                        <label class="label">
                                                       Supplier Address</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtSupplierAddress" runat="server" Enabled="true"></asp:TextBox>
                                                            
                                                        </label>
                                                    </section>
                                                    <section class="col col-4">
                                                        <label class="label">
                                                       Supplier Contact</label>
                                                        <label class="input">
                                                        <asp:TextBox ID="txtSupplierContact" runat="server" Enabled="true"></asp:TextBox>
                                                           
                                                        </label>
                                                    </section>
                                                                                                      
                                                    
                                                </div>
                                                 
                                            </fieldset>  
                                            <asp:DataGrid ID="dgPODetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0" 
        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" 
         GridLines="None" ShowFooter="True">
   
        <Columns>
            <asp:TemplateColumn HeaderText="Item">
                    
                     <ItemTemplate>
                         <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                     </ItemTemplate>
                 </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Qty">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                </ItemTemplate>
               
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Unit Cost">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                </ItemTemplate>
              
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Total Cost">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                </ItemTemplate>
               
            </asp:TemplateColumn>
            
        </Columns>
           <PagerStyle  Cssclass="paginate_button active" HorizontalAlign="Center" />
                 </asp:DataGrid>
     


                                            <br />
                                            <footer>
                                                <asp:Button ID="btnRequest" runat="server" Cssclass="btn btn-primary" onclick="btnRequest_Click" Text="Save" ValidationGroup="Save" />
                                                &nbsp;<asp:Button ID="btnCancel" runat="server" Cssclass="btn btn-primary" onclick="btnCancel_Click" Text="Back"  />
												 <asp:Button ID="btnPrintPurchaseForm" runat="server" Cssclass="btn btn-primary"  Text="Print Purchase Form" OnClientClick="javascript:Clickheretoprint('printtran')" Enabled="False" />										
                                                <asp:Button ID="btnPrintPurchaseOrder" runat="server" Cssclass="btn btn-primary"  Text="Print Purchase Order" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False" />
																						
											</footer>
                                      
                                    </div></div>
									<!-- end widget content -->
				
								</div>
        
								<!-- end widget div -->
       <div id="divprint" style="display: none;" > 
             <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align:left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            PURCHASE ORDER FORM</strong></td>
                </tr>
            </table>
				<asp:Repeater ID="Repeater1" runat="server" Visible="true">
    <HeaderTemplate>
               <table border="1">
                   <tr><td>

                       </td>
                       <td>
                           Purchase Order Form
                       </td>
                       <td>

                       </td>
                       <td>

                       </td>
                   </tr>
                    <tr >
                        <b>
                            <td>
                                PO Number
                            </td>
                             <td>
                                <asp:Label ID="lblPONumberP" runat="server" Text=""></asp:Label>
                            </td>
                             <td>                         
                            </td>
                            <td>
                            </td>
                            </tr>
                            <tr>
                              <b>
                            <td>
                                Requester
                            </td>
                            <td><asp:Label ID="lblRequesterP" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td></td>
                              <td></td>
                         </b>
                            </tr>
                   <tr>
                              <b>
                            <td>
                                Date
                            </td>
                            <td><asp:Label ID="lblDateP" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td></td>
                              <td></td>
                         </b>
                            </tr>
                      <tr>
                              <b>
                            <td>
                                Supplier Name
                            </td>
                            <td><asp:Label ID="lblSupplierName" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td></td>
                              <td></td>
                         </b>
                            </tr>      
                   <tr>
                              <b>
                            <td>
                                Supplier Address
                            </td>
                            <td><asp:Label ID="lblSupplierAddress" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td></td>
                              <td></td>
                         </b>
                            </tr>
                   <tr>
                              <b>
                            <td>
                                Supplier Contact
                            </td>
                            <td><asp:Label ID="lblSupplierContactP" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td></td>
                              <td></td>
                         </b>
                            </tr>
                   <tr>
                              <b>
                            <td>
                                Bill To:
                            </td>
                            <td><asp:Label ID="lblBillToP" runat="server" Text=""></asp:Label></td>
                                
                            
                             <td>Ship To:</td>
                              <td><asp:Label ID="lblShipTo" runat="server" Text=""></asp:Label></td>
                         </b>
                            </tr>
                   <tr>
                              <b>
                            <td>
                                Payment Terms:
                            </td>
                            <td><asp:Label ID="lblPaymentTermsP" runat="server" Text=""></asp:Label></td>
                                                      
                             <td></td>
                              <td></td>
                         </b>
                            </tr>
                   <tr style="background-color:grey">
                       <td>
                           Item Description
                       </td>
                       <td>
                           Qty
                       </td>
                       <td>
                          Unit Cost 
                       </td>
                       <td></td>
                      </tr>
            </HeaderTemplate>
            <ItemTemplate>
               <tr >
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                        
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                    </td>
                    <td>
                         <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                    </td>
                   <td>
                         <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>
                    </td>
                  </tr>
            </ItemTemplate>
            <SeparatorTemplate>
               
            </SeparatorTemplate>
            <AlternatingItemTemplate>
               
            </AlternatingItemTemplate>
            <SeparatorTemplate>
               
            </SeparatorTemplate>
            <FooterTemplate>
                <tr>
                   <td></td>
                    <td></td>
                    <td></td>
                    <td><asp:Label ID="lblItemTotalP" runat="server" Text=""></asp:Label></td>
               
                </tr>
                <tr>
                   <td></td>
                    <td></td>
                    <td>Delivery Fees:</td>
                    <td>  <asp:Label ID="lblDeliveryFeesP" runat="server" Text=""></asp:Label></td>
               
                </tr>
                <tr>
                   <td></td>
                    <td></td>
                    <td>Vat/Sales tax:</td>
                    <td>  <asp:Label ID="lblVatP" runat="server" Text=""></asp:Label></td>
               
                </tr>
                <tr>
                   <td></td>
                    <td></td>
                    <td>Total Cost (USD):</td>
                    <td>  <asp:Label ID="lblTotalP" runat="server" Text=""></asp:Label></td>
               
                </tr>
                 <tr>
                        <b>
                            <td>
                                Authorized by:
                            </td>
                            <td>
                                <asp:Label ID="lblAuthorizedByP" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                              
                            </td>
                            <td>
                               
                            </td>
                            
                    </b>
                    </tr>
                </table>
            </FooterTemplate>
</asp:Repeater>
            </div>
       <div id="printtran" style="display:none;">
        <fieldset>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align:left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            PURCHASE REQUEST FORM</strong></td>
                </tr>
            </table>

            <table style="width: 100%;">
                <tr>
                    <td style="width: 585px">Request No</td>
                    <td style="width: 617px" ><asp:Label ID="lblRequestNoResult" runat="server"></asp:Label></td>
                    <td class="modal-sm" style="width: 404px" >Purpose</td>
                    <td ><asp:Label ID="lblPurposeResult" runat="server"></asp:Label></td>
                    
                </tr>

                <tr>
                    <td style="width: 585px">                        
                           Purchase Order No
                      </td>  
                    <td style="width: 617px" >
                        <asp:Label ID="lblPurchaseOrderNo" runat="server"></asp:Label>
                    </td>
                    <td style="width: 404px" class="modal-sm">Required Date of Delivery</td>
                    <td style="width: 389px">
                        <asp:Label ID="lblDeliveryDateresult" runat="server"></asp:Label></td>
                    
                </tr>
                <tr>
                    <td style="width: 585px"  >
                           Requester
                        </td>
                    <td style="width: 617px" >
                        <asp:Label ID="lblRequesterResult" runat="server"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 404px" >Deliver To</td>
                    <td ><asp:Label ID="lblDeliverToResult" runat="server"></asp:Label></td>
                   
                </tr>
                <tr>
                    <td style="width: 585px"  >
                            Request Date
                        </td>
                    <td style="width: 617px" >
                        <asp:Label ID="lblRequestedDateResult" runat="server"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 404px" >Suggested Supplier</td>
                    <td ><asp:Label ID="lblSuggestedSupplierResult" runat="server"></asp:Label></td>
                   
                </tr>
                <tr>
                    <td style="width: 585px" >
                       Bill To
                    </td>
                    <td style="width: 617px" >
                        <asp:Label ID="lblBillToResult" runat="server"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 404px" >
                       Ship To
                    <td >
                        <asp:Label ID="lblShipToResult" runat="server"></asp:Label>
                    </td>
                    
                </tr>
                 <tr>
                    <td style="width: 585px" >
                      Reason for Selection
                    </td>
                    <td style="width: 617px" >
                        <asp:Label ID="lblReasonforSelectionResult" runat="server"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 404px" >
                       Selected by</td>
                    <td >
                        <asp:Label ID="lblSelectedbyResult" runat="server"></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <td style="width: 585px" >
                        Delivery Fees
                    </td>
                    <td style="width: 617px" >
                        <asp:Label ID="lblDeliveryFeesResult" runat="server"></asp:Label>
                    </td>
                    <td class="modal-sm" style="width: 404px" >
                       Payment Terms</td>
                    <td >
                        <asp:Label ID="lblPaymentTerms" runat="server"></asp:Label>
                    </td>
                    
                </tr>
                
                
            </table>
            <br />
            <asp:GridView ID="grvDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="Bidder.SupplierType.SupplierTypeName" HeaderText="AccountName" SortExpression="Bidder.ItemAccount.AccountName" />
                    <asp:BoundField DataField="Bidder.Supplier.SupplierName" HeaderText="Supplier" SortExpression="Bidder.Supplier.SupplierName" />
                    
                    <asp:BoundField DataField="Bidder.Rank" HeaderText="Rank" SortExpression="Bidder.Rank" />
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="ItemAccount.AccountName" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" />
                    <asp:BoundField DataField="UnitCost" HeaderText="UnitCost" />
                    <asp:BoundField DataField="TotalCost" HeaderText="TotalCost" />
                    
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvStatuses"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover" OnRowDataBound="grvStatuses_RowDataBound" >
                <RowStyle CssClass="rowstyle"  />
                <Columns>
                  <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                              <asp:Label ID="lblDate" runat="server" Text='<%# Eval("ApprovalDate", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField> 
                    <asp:BoundField HeaderText="Approver" />
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
							</div>
 	 	 	 
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    	 
 	 	 	 	 	 
 	 	 	 	 	 

</asp:Content>


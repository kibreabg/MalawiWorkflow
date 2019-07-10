<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucPrintRequestedItem.ascx.cs" Inherits="Chai.WorkflowManagment.Modules.Approval.Views.ucPrintRequestedItem" %>
<asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
               <table border="1">
                    <tr>
                        <b>
                            <td>
                                Request No
                            </td>
                             <td>
                                Requester
                            </td>
                             <td>
                                Requested Date
                            </td>
                            <td>
                                Approved By
                            </td>
                            <td>
                                Needed for
                            </td>
                            <td>
                                Special Need
                            </td>
                         </b>
                            </tr>
                            <tr>
                                 <b>
                            <td>
                                <asp:Label ID="lblrequestNo" runat="server" Text=""></asp:Label>
                            </td>
                           
                            <td>
                                <asp:Label ID="lblRequester" runat="server" Text=""></asp:Label>
                            </td>
                            
                            <td>
                                <asp:Label ID="lblRequestDate" runat="server" Text=""></asp:Label>
                            </td>
                             
                            <td>
                                <asp:Label ID="lblApprovedBy" runat="server" Text=""></asp:Label>
                            </td>
                            
                         
                            
                            <td>
                                <asp:Label ID="lblSpecialNeed" runat="server" Text=""></asp:Label>
                            </td>
                                 </b>
                       
                    </tr>
                   <tr>
                                 <b>
                            <td>
                                
                            </td>
                           
                            <td>
                                
                            </td>
                            
                            <td>
                                
                            </td>
                             
                            <td>
                                
                            </td>
                            
                            <td>
                                
                            </td>
                            
                            <td>
                                
                            </td>
                                 </b>
                       
                    </tr>
                   <tr>
                       <td>
                           Description of Item
                       </td>
                       <td></td>
                       <td>
                           Supplier 
                       </td>
                       <td></td>
                       <td>
                           Supplier 

                           </td><td>
                       <td>
                           Supplier 
                       </td>
                       <td></td>
                   </tr>
                   <tr>
                       <td>
                           Item Required
                       </td>
                       <td>
                           Qty
                       </td>
                       <td>
                          Unit Cost 
                       </td>
                       <td>Total Cost</td>
                       <td>
                           Unit Cost

                           </td>
                       <td>Total Cost</td>
                       <td>
                          Unit Cost
                       </td>
                       <td>Total Cost</td>
                   </tr>
            </HeaderTemplate>
            <ItemTemplate>
               <tr >
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Account.AccountName")%>
                        
                    </td>
                    <td>
                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                    </td>
                    <td>
                        sup1 unitcost
                    </td>
                   <td>
                        sup1 Total cost
                    </td>
                   <td>
                        sup2 unitcost
                    </td>
                   <td>
                        sup2 Total cost
                    </td>
                   <td>
                        sup3 unitcost
                    </td>
                   <td>
                        sup3 Total cost
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
                   <td>Total Amount</td>
                    <td></td><td></td><td>Sup1 Tot am</td>
                     <td></td><td>Sup2 Tot am</td>
                     <td></td><td>Sup2 Tot am</td>
                </tr>
                <tr>
                    <td>Lead time for supplier</td><td></td><td></td><td></td>
                    <td></td><td></td>
                    <td></td><td></td>
                </tr>
                
                <tr>
                     <td>Special Terms & Delivery</td><td></td><td></td><td></td>
                    <td></td><td></td>
                    <td></td><td></td>
                </tr>
                <tr>
                        <b>
                            <td>
                                Selected Supplier
                            </td>
                            <td>
                                <asp:Label ID="lblselectedSupplier" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                                Reason for Selection
                            </td>
                            <td>
                                <asp:Label ID="lblReasonforsel" runat="server" Text=""></asp:Label>
                            </td>
                             <td>
                                Selected by
                            </td>
                            <td>
                                <asp:Label ID="lblSelectedBy" runat="server" Text=""></asp:Label>
                            </td>
                    </b>
                    </tr>
                </table>
            </FooterTemplate>
</asp:Repeater>


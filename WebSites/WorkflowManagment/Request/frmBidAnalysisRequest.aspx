<%@ Page Title=" Bid Analysis Request Form" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmBidAnalysisRequest.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Request.Views.frmBidAnalysisRequest" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content" ContentPlaceHolderID="DefaultContent" runat="Server">
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
    
   <%-- <asp:ValidationSummary ID="VSBid" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:ValidationSummary ID="VSBidder" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proadd" ForeColor="" />
    <asp:ValidationSummary ID="VSBidderEdit" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="proedit" ForeColor="" />--%>

    <div id="wid-id-0" class="jarviswidget" data-widget-custombutton="false" data-widget-editbutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>BID ANALYSIS WORKSHEET</h2>
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
                                    Requester</label>
                                <label class="input">
                                    <asp:TextBox ID="txtRequester" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                             
                            <section class="col col-4">
                                <label id="lblRequestDate" runat="server" class="label" visible="true">
                                   Purchase Requested Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtRequestDate" runat="server" Visible="true" CssClass="form-control datepicker" Enabled="False"></asp:TextBox>
                                </label>
                            </section>
                           
                            <section class="col col-4">
                                <label class="label">
                                     Date</label>
                                <label class="input">
                                    <i class="icon-append fa fa-calendar"></i>
                                    <asp:TextBox ID="txtAnalyzedDate" runat="server" Visible="true" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Enabled="False"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvAnalyzedDate" runat="server" CssClass="validator" ControlToValidate="txtAnalyzedDate" ErrorMessage="Analyzed Date Required" InitialValue="" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                </label>
                            </section>
                        </div>
                          <%-- <div class="row">                            
                            <section class="col col-6">
                            
                             
                                <asp:GridView ID="GridView1" runat="server" HorizontalAlign="Left" Width="430px" DataKeyNames="Id"   CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="False">
                <RowStyle CssClass="rowstyle" />  
                                    <Columns>
                                        <asp:BoundField DataField="PurchaseRequest.ConditionsofOrder" HeaderText="Item Description" />
                                       
                                    </Columns>
               
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
               
                             </asp:GridView>
                              </section>
                           </div>--%>
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
                         
                            
                             <section class="col col-4">
                                <label class="label">
                                    Special Need</label>
                                <label class="input">
                                    <asp:TextBox ID="txtSpecialNeed" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                             <section class="col col-8">
                                <label class="label">
                                    Reason for Selecting Supplier</label>
                                <label class="input">
                                    <asp:TextBox ID="txtselectionfor" runat="server" Visible="true"></asp:TextBox>
                                </label>
                            </section>
                        </div>
                       <div class="row">
                           <section class="col col-6">
                                <label class="label">Total Price </label>
                                <label class="input">
                                    <asp:TextBox ID="txtTotal" ReadOnly="true" runat="server"></asp:TextBox>
                                </label>
                            </section>   
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
                            </section>               
                           </div>
           </fieldset>
                    <div class="tab-content">
                    <div class="tab-pane active" id="hr2">

                                    <ul class="nav nav-tabs">
                                        <li class="active">
                                            <a href="#iss1" data-toggle="tab">Bidder Detail</a>
                                        </li>
                                        <li class="">
                                            <a href="#iss2" data-toggle="tab">Attachments</a>
                                        </li>
                                    </ul>
                    <div class="tab-content padding-10">
                    <div class="tab-pane active" id="iss1">
                         <fieldset>
                            <div class="row">
                    <asp:DataGrid ID="dgBidders" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                        ShowFooter="True" GridLines="None" Height="30px" OnSelectedIndexChanged="dgBidders_SelectedIndexChanged1" OnUpdateCommand="dgBidders_UpdateCommand" OnCancelCommand="dgBidders_CancelCommand1" OnDeleteCommand="dgBidders_DeleteCommand1" OnEditCommand="dgBidders_EditCommand1" OnItemCommand="dgBidders_ItemCommand1" OnItemDataBound="dgBidders_ItemDataBound1">
                      

                        <Columns>
                            <asp:TemplateColumn HeaderText="Supplier Type">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSupplierType" runat="server" CssClass="form-control"
                                        AppendDataBoundItems="True" DataTextField="SupplierTypeName" DataValueField="Id"
                                         AutoPostBack="True" OnSelectedIndexChanged="ddlSupplierType_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Supplier Type</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfvSupplierType" runat="server" CssClass="validator"
                                        ControlToValidate="ddlSupplierType" ErrorMessage="Supplier Required"
                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlFSupplierType" runat="server" CssClass="form-control"
                                        AppendDataBoundItems="True" DataTextField="SupplierTypeName" DataValueField="Id"
                                        EnableViewState="true" AutoPostBack="True" OnSelectedIndexChanged="ddlFSupplierType_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Select Supplier</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfvFSupplierType" runat="server" CssClass="validator"
                                        ControlToValidate="ddlFSupplierType" Display="Dynamic"
                                        ErrorMessage="Supplier Type Required" InitialValue="0" SetFocusOnError="True"
                                        ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "SupplierType.SupplierTypeName")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Supplier">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-control"
                                        AppendDataBoundItems="True" DataTextField="SupplierName" DataValueField="Id"
                                        ValidationGroup="proedit">
                                        <asp:ListItem Value="0">Select Supplier</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfvSupplier" runat="server" CssClass="validator"
                                        ControlToValidate="ddlSupplier" ErrorMessage="Supplier Required"
                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddlFSupplier" runat="server" CssClass="form-control"
                                        AppendDataBoundItems="True" DataTextField="SupplierName" DataValueField="Id"
                                        EnableViewState="true" ValidationGroup="proadd">
                                        <asp:ListItem Value="0">Select Supplier</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RfvFSupplier" runat="server" CssClass="validator"
                                        ControlToValidate="ddlFSupplier" Display="Dynamic"
                                        ErrorMessage="Supplier Required" InitialValue="0" SetFocusOnError="True"
                                        ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                </FooterTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Supplier.SupplierName")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                             <asp:TemplateColumn HeaderText="Contact Details">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "ContactDetails")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtContactDetails" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ContactDetails")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvContactDetails" runat="server" CssClass="validator" ControlToValidate="txtContactDetails" ErrorMessage="Contact Details of Supplier Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFContactDetails" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvFContactDetails" runat="server" CssClass="validator" ControlToValidate="txtFContactDetails" ErrorMessage="Contact Details of Supplier Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                          
                            <asp:TemplateColumn HeaderText="Lead Time from Supplier">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "LeadTimefromSupplier")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtLeadTimefromSupplier" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "LeadTimefromSupplier")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="validator" ControlToValidate="txtLeadTimefromSupplier" ErrorMessage="Lead Time from Supplier Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFLeadTimefromSupplier" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFLeadTimefromSupplier" ErrorMessage="Lead Time from Supplier Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                           
                            <asp:TemplateColumn HeaderText="Special Terms Delivery">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "SpecialTermsDelivery")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSpecialTermsDelivery" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "SpecialTermsDelivery")%>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvSpecialTermsDelivery" runat="server" CssClass="validator" ControlToValidate="txtSpecialTermsDelivery" ErrorMessage="Special Terms Delivery Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFSpecialTermsDeliveryy" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RfvFSpecialTermsDelivery" runat="server" CssClass="validator" ControlToValidate="txtFSpecialTermsDeliveryy" ErrorMessage="Special Terms Delivery Supplier Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Rank">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Rank")%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtRank" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Rank")%>'></asp:TextBox>
                                   
                                    <asp:RequiredFieldValidator ID="RfvRank" runat="server" CssClass="validator" ControlToValidate="txtRank" ErrorMessage="Rank Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtFRank" runat="server" CssClass="form-control"></asp:TextBox>
                                  
                                    <asp:RequiredFieldValidator ID="RfvFRank" runat="server" CssClass="validator" ControlToValidate="txtFRank" ErrorMessage="Rank Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
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
                            <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="Bidder Item Detail"></asp:ButtonColumn>
                        </Columns>
                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                    </asp:DataGrid>
                            </div>
                        </fieldset>
                        </div>

                    <div class="tab-pane" id="iss2">
                                            <fieldset>
                                                <div class="row">
                                                    <section class="col col-6">
                                                        <label class="label">Attach Quotation doc.</label>
                                                        <asp:FileUpload ID="fuReciept" runat="server" />
                                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="btn btn-primary" OnClick="btnUpload_Click" />
                                                    </section>
                                                </div>
                                            </fieldset>
                                            <asp:GridView ID="grvAttachments"
                                                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                                                <RowStyle CssClass="rowstyle" />
                                                <Columns>
                                                    <asp:BoundField DataField="FilePath" HeaderText="File Name" SortExpression="FilePath" />
                                                    <asp:TemplateField>
                                                     <ItemTemplate>
                                                       <asp:LinkButton ID="lnkDownload" Text = "Download" CommandArgument = '<%# Eval("FilePath") %>' runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                                     </ItemTemplate>
                                                   </asp:TemplateField>
                                                   <asp:TemplateField>
                                                   <ItemTemplate>
                                                  <asp:LinkButton ID = "lnkDelete" Text = "Delete" CommandArgument = '<%# Eval("FilePath") %>' runat = "server" OnClick = "DeleteFile" />
                                                  </ItemTemplate>
                                                  </asp:TemplateField>
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
                    <br />
                    <footer>
                        <asp:Button ID="btnRequest" runat="server" CssClass="btn btn-primary" OnClick="btnRequest_Click" Text="Request" ValidationGroup="Save" />
                        &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary" OnClick="btnCancel_Click" Text="Back" />
                     <%--<asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search" />--%>
                            <a data-toggle="modal" runat="server" id="searchLink" href="#searchModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                        <asp:Button ID="btnPrintworksheet" runat="server" CssClass="btn btn-primary" Text="Print WorkSheet" OnClientClick="javascript:Clickheretoprint('divprint')" Enabled="False" />
                          <asp:Button ID="btnHiddenPopupp" runat="server" />
                           <asp:HiddenField ID="hfDetailId" runat="server" />
                    </footer>

                </div>
            </div>
            <!-- end widget content -->

        </div>

        <!-- end widget div -->
        <div id="divprint" style="display: none;" visible="true">
            <fieldset>
             <table style="width: 100%;">
                <tr>
                    <td style="width: 17%; text-align:left;">
                        <img src="../img/CHAI%20Logo.png" width="70" height="50" /></td>
                    <td style="font-size: large; text-align: center;">
                        <strong>CHAI ZIMBABWE
                            <br />
                            BID ANALYSIS WORKSHEET</strong></td>
                </tr>
            </table>
           
                    <table>
                      <tr> 
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                             <asp:Label ID="Label1" runat="server" Text="Total Price:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                           <asp:Label ID="lblTot" runat="server" class="label" Text=""></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                               <asp:Label ID="lblRequesterlable" runat="server" Text="Requester"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblRequester" runat="server" class="label"></asp:Label>
                    </td>
                   <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
              <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblProposedSupplier" runat="server" Text="Reason For Selection:"></asp:Label>
                        </strong>
                    </td>
                    <td style="width: 244px; height: 18px;">
                         <asp:Label ID="lblReasonForSelection" runat="server" class="label" Text=""></asp:Label>
                    </td>  
                   <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                <tr>
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblRequestDa" runat="server" Text="Request Date:"></asp:Label>
                        </strong></td>
                    <td style="width: 244px; height: 18px;">
                       <asp:Label ID="lblRequestDate0" runat="server" class="label" Text=""></asp:Label>
                    </td>
                     <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                   </tr>
                <tr>
                    
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblProposedPurchasedprice" runat="server" Text="Special Need:"></asp:Label>
                        </strong></td>
                      <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblSpecialNeed" runat="server" Text="" class="label"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                         <tr>
                    
                    <td style="width: 629px; height: 18px; padding-left: 20%;">
                        <strong>
                            <asp:Label ID="lblPaytype" runat="server" Text="Payment Method:"></asp:Label>
                        </strong></td>
                      <td style="width: 244px; height: 18px;">
                        <asp:Label ID="lblpaytypeRes" runat="server" Text="" class="label"></asp:Label>
                    </td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td style="width: 389px;">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                    </table>
                                     
                           
                      
                    
            
            <asp:GridView ID="grvprtBidders"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="SupplierType.SupplierTypeName" HeaderText="Supplier Type" SortExpression="SupplierType.SupplierTypeName" />
                    <asp:BoundField DataField="Supplier.SupplierName" HeaderText="Supplier" SortExpression="Supplier.SupplierName" />
                    <asp:BoundField DataField="LeadTimefromSupplier" HeaderText="Lead Time from Supplier" SortExpression="LeadTimefromSupplier" />
                    <asp:BoundField DataField="SpecialTermsDelivery" HeaderText="Special Terms Delivery" SortExpression="SpecialTermsDelivery" />
                    <asp:BoundField DataField="Rank" HeaderText="Rank" SortExpression="Rank" />
                   
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
            <br />
            <asp:GridView ID="grvprtBidderItemDetails"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ItemAccount.AccountName" HeaderText="Requested Items" SortExpression="ItemAccount.AccountName" />
                    <asp:BoundField DataField="ItemAccount.AccountCode" HeaderText="Account Code" SortExpression="AccountCode" />
                    <asp:BoundField DataField="ItemDescription" HeaderText="Item Description" SortExpression="ItemDescription" />
                    <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" SortExpression="UnitCost" />                    
                    <asp:BoundField DataField="TotalCost" HeaderText="Total" SortExpression="TotalCost" />
                    <asp:BoundField DataField="Qty" HeaderText="Quantity" SortExpression="Qty" />
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView>
                <br/>
               <asp:GridView ID="grvStatuses" OnRowDataBound="grvStatuses_RowDataBound"
                runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                CssClass="table table-striped table-bordered table-hover">
                <RowStyle CssClass="rowstyle" />
                <Columns>
                    <asp:BoundField DataField="ApprovalDate" HeaderText="Date" SortExpression="ApprovalDate" />
                    <asp:BoundField HeaderText="Approver" />
                      <asp:BoundField HeaderText="Approval Status" DataField="ApprovalStatus"/>
                </Columns>
                <FooterStyle CssClass="FooterStyle" />
                <HeaderStyle CssClass="headerstyle" />
                <PagerStyle CssClass="PagerStyle" />
                <RowStyle CssClass="rowstyle" />
            </asp:GridView><br /> </fieldset>
        </div>
    </div>
    <div class="modal fade" id="searchModal" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" id="myModalLabel">Search Bid Analysis Requests</h4>
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
                                      <asp:GridView ID="grvBidAnalysisRequestList"
                                        runat="server" AutoGenerateColumns="False" DataKeyNames="Id"
                                        
                                        AllowPaging="True" 
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" PageSize="5" OnSelectedIndexChanged="grvBidAnlysisRequestList_SelectedIndexChanged" OnPageIndexChanging="grvBidAnalysisRequestList_PageIndexChanging">
                                        <RowStyle CssClass="rowstyle" />
                                        <Columns>
                                            <asp:BoundField DataField="RequestNo" HeaderText="Vourcher No" SortExpression="RequestNo" />
                                            <asp:BoundField DataField="RequestDate" HeaderText="Request Date" SortExpression="RequestDate" />
                                            <asp:BoundField DataField="SpecialNeed" HeaderText="Suggested Supplier" SortExpression="SpecialNeed" />
                                            <asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" />
                                            <asp:BoundField DataField="ProgressStatus" HeaderText="Status" SortExpression="ProgressStatus" />
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
   



       <asp:Panel ID="pnlBidItem" Visible="true" runat="server">
        <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
            <header>
                <span class="widget-icon"><i class="fa fa-edit"></i></span>
                <h2>Requested Items</h2>
            </header>
            <div>
                <div class="jarviswidget-editbox"></div>
                <div class="widget-body no-padding">
                    <div class="smart-form">

                                                           <asp:DataGrid ID="dgItemDetail" runat="server" AlternatingRowStyle-CssClass="" CellPadding="0"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id" AutoGenerateColumns="false"
                            GridLines="None" OnItemDataBound="dgItemDetail_ItemDataBound" ShowFooter="True" OnCancelCommand="dgItemDetail_CancelCommand" OnDeleteCommand="dgItemDetail_DeleteCommand" OnItemCommand="dgItemDetail_ItemCommand" OnUpdateCommand="dgItemDetail_UpdateCommand" OnEditCommand="dgItemDetail_EditCommand1">

                            <Columns>
                                <asp:TemplateColumn HeaderText="Requested Items">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlItemAcc" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id"
                                            ValidationGroup="detailedit">
                                            <asp:ListItem Value="0">Select Item</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvItemAcc" runat="server" CssClass="validator"
                                            ControlToValidate="ddlItemAcc" ErrorMessage="Item Account Required"
                                            InitialValue="0" SetFocusOnError="True" ValidationGroup="detailedit">*</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlFItemAcc" runat="server" CssClass="form-control"
                                            AppendDataBoundItems="True" DataTextField="AccountName" DataValueField="Id"
                                            EnableViewState="true">
                                            <asp:ListItem Value="0">Select Item Account</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RfvFItemAcc" runat="server" CssClass="validator"
                                            ControlToValidate="ddlFItemAcc" Display="Dynamic"
                                            ErrorMessage="Item Account Required" InitialValue="0" SetFocusOnError="True"
                                            ValidationGroup="detailadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ItemAccount.AccountName")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Item Description">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ItemDescription")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvDescription" runat="server" CssClass="validator" ControlToValidate="txtDescription" ErrorMessage="Item Description Required" ValidationGroup="detailedit">*</asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtFDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvFDescription" runat="server" CssClass="validator" ControlToValidate="txtFDescription" ErrorMessage="Item Description Required" ValidationGroup="detailadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Qty">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "Qty")%>
                                        <asp:HiddenField ID="hfqty" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:HiddenField>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtQty" Enabled="true" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Qty")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtQty" runat="server" Enabled="true" CssClass="form-control"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Unit Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "UnitCost")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtUnitCost" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "UnitCost")%>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvEdtUnitCost" runat="server" ControlToValidate="txtEdtUnitCost" CssClass="validator" Display="Dynamic" ErrorMessage="Unit Cost is required" SetFocusOnError="true" ValidationGroup="detailedit"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvUnitCost" runat="server" ControlToValidate="txtUnitCost" CssClass="validator" Display="Dynamic" ErrorMessage="Unit Cost is required" SetFocusOnError="true" ValidationGroup="detailadd"></asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Total Cost">
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container.DataItem, "TotalCost")%>                                        
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtEdtTotalCost" runat="server" Enabled="false" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "TotalCost")%>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RfvTotalCost" runat="server" ControlToValidate="txtTotalCost" ErrorMessage="Total Cost Required" Enabled="false" ValidationGroup="detailadd">*</asp:RequiredFieldValidator>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Actions">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="detailedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lnkAddNewa" runat="server" CommandName="AddNew" ValidationGroup="detailadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
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
                                        <asp:Button ID="btncancelCost" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btncancelCost_Click" />
                                    </footer>


                                </div>
                            </div>
                        </div>

                    </div>
              
        <!-- /.modal-content -->
    </asp:Panel>
     <cc1:ModalPopupExtender runat="server" Enabled="True" CancelControlID="btncancelCost"
        ID="pnlBidItem_ModalPopupExtender" TargetControlID="btnHiddenPopupp" BackgroundCssClass="modalBackground"
        PopupControlID="pnlBidItem">
    </cc1:ModalPopupExtender>
    
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

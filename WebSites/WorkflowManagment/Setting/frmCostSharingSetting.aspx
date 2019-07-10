<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmCostSharingSetting.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmCostSharingSetting" %>
 <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">

    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
         <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Cost Sharing Setting</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                     </fieldset>
                    <footer>
                       <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save" ValidationGroup="Savedetail" OnClick="btnSave_Click"  />
                                                <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    
                    </footer>
                </div>
            </div>
        </div>
         <asp:DataGrid ID="dgItemDetail" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="ProjectId"
                                        GridLines="None"  ShowFooter="True" OnItemDataBound="dgItemDetail_ItemDataBound">

                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Project ID">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Project.ProjectCode")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                              <asp:TemplateColumn HeaderText="Project Description">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Project.ProjectDescription")%>
                                                </ItemTemplate>

                                            </asp:TemplateColumn>
                                              <asp:TemplateColumn HeaderText="Grant ID">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlGrant" runat="server" AppendDataBoundItems="True" CssClass="form-control" DataTextField="GrantCode" DataValueField="Id">
                                                                <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RfvGrant" runat="server"  CssClass="validator" ControlToValidate="ddlGrant" ErrorMessage="Grant is required" InitialValue="0" SetFocusOnError="True" ValidationGroup="Savedetail"></asp:RequiredFieldValidator>
                                                </ItemTemplate>

                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Percentage">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem,"Percentage")%>' Height="20px" Width="104px"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender runat="server" Enabled="True" TargetControlID="txtPercentage" ID="txtPercentage_FilteredTextBoxExtender" FilterType="Custom,Numbers" ValidChars="."></asp:FilteredTextBoxExtender>
                                                    <asp:RequiredFieldValidator ID="RfvPercentage" runat="server" CssClass="validator" ControlToValidate="txtPercentage" ErrorMessage="Percentage Required" ValidationGroup="Savedetail" InitialValue="0">*</asp:RequiredFieldValidator>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            
                                        </Columns>
                                        <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                                    </asp:DataGrid>
    
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
</asp:Content>


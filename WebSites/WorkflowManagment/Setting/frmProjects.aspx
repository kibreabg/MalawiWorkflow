<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmProjects.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmProjects" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <asp:ValidationSummary ID="NewValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="2" ForeColor="" />
    <asp:ValidationSummary ID="EditValidationSummary2" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="1" ForeColor="" />
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Search Project</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <asp:Label ID="lblProjectCode" runat="server" Text="Project ID" CssClass="label"></asp:Label>
                                <label class="input">
                                    <asp:TextBox ID="txtProjectCode" runat="server"></asp:TextBox></label>
                            </section>

                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                          <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>

        <div runat="server" id="propan" class="row" visible="true">
            <article class="col-xs-12 col-sm-12 col-md-12 col-lg-12 sortable-grid ui-sortable">
                <div class="jarviswidget jarviswidget-color-blueDark jarviswidget-sortable" id="wid-id-0" data-widget-editbutton="false" role="widget" style="">
                    <header role="heading">
                        <h2>Projects</h2>
                    </header>
                    <div role="content">
                        <div class="widget-body">
                            <asp:DataGrid ID="dgProject" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None"
                                OnCancelCommand="dgProject_CancelCommand" OnDeleteCommand="dgProject_DeleteCommand" OnEditCommand="dgProject_EditCommand"
                                OnItemCommand="dgProject_ItemCommand" OnItemDataBound="dgProject_ItemDataBound" OnUpdateCommand="dgProject_UpdateCommand"
                                ShowFooter="True" OnSelectedIndexChanged="dgProject_SelectedIndexChanged1">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Project Description">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ProjectDescription")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProjectDescription" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ProjectDescription")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtProjectDescription" ErrorMessage="Project Description Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFProjectDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="validator" ControlToValidate="txtFProjectDescription" ErrorMessage="Project Description Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Project ID">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "ProjectCode")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProjectCode" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "ProjectCode")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="validator" ControlToValidate="txtProjectCode" ErrorMessage="Project Code Required" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFProjectCode" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="validator" ControlToValidate="txtFProjectCode" ErrorMessage="Project Code Required" ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Starting Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "StartingDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtStartingDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "StartingDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvEdtFromDate" runat="server" ErrorMessage="Starting date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proedit" 
                                                SetFocusOnError="true" ControlToValidate="txtEdtStartingDate"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvEdtStartEndDates" runat="server" ErrorMessage="Starting Date must be less than End Date"
                                                ControlToCompare="txtEdtEndDate" ControlToValidate="txtEdtStartingDate"
                                                ValidationGroup="proedit" Type="Date" Operator="LessThan"></asp:CompareValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtStartingDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtFromDate" runat="server" ErrorMessage="Starting date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proadd"
                                                SetFocusOnError="true" ControlToValidate="txtStartingDate"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="cvStartEndDates" runat="server" ErrorMessage="Starting Date must be less than End Date"
                                                ControlToCompare="txtEndDate" ControlToValidate="txtStartingDate"
                                                ValidationGroup="proadd" Type="Date" Operator="LessThan"></asp:CompareValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="End Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "EndDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEdtEndDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "EndDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtEdtToDate" runat="server" ErrorMessage="End date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proedit"
                                                SetFocusOnError="true" ControlToValidate="txtEdtEndDate"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker"
                                                data-dateformat="mm/dd/yy"></asp:TextBox>
                                            <asp:RequiredFieldValidator
                                                ID="rfvtxtEndDate" runat="server" ErrorMessage="End date is required" Display="Dynamic"
                                                CssClass="validator" ValidationGroup="proadd"
                                                SetFocusOnError="true" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Total Budget">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "TotalBudget")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTotalBudget" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "TotalBudget")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFTotalBudget" runat="server" CssClass="form-control"></asp:TextBox>

                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Remaining Budget">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "RemainingBudget")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRemainingBudget" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "RemainingBudget")%>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtFRemainingBudget" runat="server" CssClass="form-control"></asp:TextBox>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Program Manager">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "AppUser.FullName")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlEdtProgramManager" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="FullName"
                                                Width="90px" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Program Manager</asp:ListItem>
                                            </asp:DropDownList><i></i><asp:RequiredFieldValidator ID="RfvProMan" runat="server"
                                                ControlToValidate="ddlEdtProgramManager" ErrorMessage="Program Manager Required"
                                                InitialValue="0" SetFocusOnError="True" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlProgramManager" runat="server" CssClass="form-control" DataValueField="Id" DataTextField="FullName"
                                                Width="90px" AppendDataBoundItems="True">
                                                <asp:ListItem Value="0">Select Program Manager</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="RfvFProMan" runat="server"
                                                ControlToValidate="ddlProgramManager" Display="Dynamic"
                                                ErrorMessage="Program Manager Required" InitialValue="0" SetFocusOnError="True"
                                                ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                    </asp:TemplateColumn>

                                    <asp:TemplateColumn HeaderText="Status">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server" Width="90px" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                ValidationGroup="3">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvStatus" runat="server"
                                                ControlToValidate="ddlStatus" ErrorMessage="Status Required"
                                                InitialValue="" SetFocusOnError="True" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFStatus" runat="server" Width="90px" CssClass="form-control"
                                                AppendDataBoundItems="True" EnableViewState="true" ValidationGroup="proadd">
                                                <asp:ListItem Value="">Select Status</asp:ListItem>
                                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                                <asp:ListItem Value="InActive">InActive</asp:ListItem>

                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvFStatus" runat="server"
                                                ControlToValidate="ddlFStatus" Display="Dynamic"
                                                ErrorMessage="Status Required" InitialValue="" SetFocusOnError="True"
                                                ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Status")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Actions">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:ButtonColumn ButtonType="PushButton" CommandName="Select" Text="Project Grant"></asp:ButtonColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>
                        </div>
                    </div>
                </div>
            </article>
        </div>
    </div>
    <asp:Panel ID="PnlProGrant" runat="server" Style="position: absolute; top: 10%; left: 20%;" Visible="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                </div>
                <div class="modal-body no-padding">
                    <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                        <header>
                            <span class="widget-icon"><i class="fa fa-edit"></i></span>
                            <h2>Project Grants</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <asp:DataGrid ID="dgProjectGrant" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                        CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                        GridLines="None"
                                        OnCancelCommand="dgProjectGrant_CancelCommand" OnDeleteCommand="dgProjectGrant_DeleteCommand" OnEditCommand="dgProjectGrant_EditCommand"
                                        OnItemCommand="dgProjectGrant_ItemCommand" OnItemDataBound="dgProjectGrant_ItemDataBound" OnUpdateCommand="dgProjectGrant_UpdateCommand"
                                        ShowFooter="True">

                                        <Columns>
                                            <asp:TemplateColumn HeaderText="Grant ID">
                                                <EditItemTemplate>
                                                    <asp:DropDownList ID="ddlGrant" runat="server" CssClass="form-control"
                                                        AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                                        ValidationGroup="proeditg">
                                                        <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RfvGrant" runat="server"
                                                        ControlToValidate="ddlGrant" ErrorMessage="Grant ID Required"
                                                        InitialValue="0" SetFocusOnError="True" ValidationGroup="proeditg">*</asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlFGrant" runat="server" CssClass="form-control"
                                                        AppendDataBoundItems="True" DataTextField="GrantCode" DataValueField="Id"
                                                        EnableViewState="true" ValidationGroup="proaddg">
                                                        <asp:ListItem Value="0">Select Grant</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RfvFGrantCode" runat="server"
                                                        ControlToValidate="ddlFGrant" Display="Dynamic"
                                                        ErrorMessage="Grant ID Required" InitialValue="0" SetFocusOnError="True"
                                                        ValidationGroup="proaddg">*</asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "Grant.GrantCode")%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Grant Date">
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container.DataItem, "GrantDate")%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtGrantDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "GrantDate")%>'></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtGrantDate" ErrorMessage="Grant Date Required" ValidationGroup="proeditg">*</asp:RequiredFieldValidator>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtFGrantDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFGrantDate" ErrorMessage="Grant Date Required" ValidationGroup="proaddg">*</asp:RequiredFieldValidator>
                                                </FooterTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Actions">
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proeditg" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proaddg" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
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

                                        <asp:Button ID="btnCancedetail" runat="server" CssClass="btn btn-primary" Text="Close" OnClick="btnCancedetail_Click" />
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


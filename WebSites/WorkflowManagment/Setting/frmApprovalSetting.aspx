<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmApprovalSetting.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmApprovalSetting" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <script src="../js/libs/jquery-2.0.2.min.js"></script>
    <script type="text/javascript">
        function showSearch() {
            $(document).ready(function () {
                $('#myModal').modal('show');
            });
        }
    </script>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server"
        CssClass="alert alert-danger fade in" DisplayMode="SingleParagraph"
        ValidationGroup="Save" ForeColor="" />
    <asp:Label ID="lblError" runat="server" Text="" Visible="false" CssClass="alert alert-danger fade in" Width="500px"></asp:Label>
    <div class="row">

        <!-- NEW COL START -->
        <article class="col-sm-12 col-md-12 col-lg-12">


            <div class="jarviswidget" id="wid-id-0" data-widget-colorbutton="false" data-widget-editbutton="false" data-widget-custombutton="false">

                <header>
                    <span class="widget-icon"><i class="fa fa-edit"></i></span>
                    <h2>Approval Setting</h2>

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
                                    <section class="col col-2">
                                        <label class="label">Request Type</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlRequestType" runat="server">
                                                <asp:ListItem Value="0">Select Request Type</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="RfvRequestType" runat="server" ControlToValidate="ddlRequestType" ErrorMessage="Request Type Required" InitialValue="0" SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                    <section class="col col-2">
                                        <label class="label">Criteria Condition</label>
                                        <label class="select">
                                            <asp:DropDownList ID="ddlCriteria" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCriteria_SelectedIndexChanged">
                                                <asp:ListItem Value=" ">Select Condition</asp:ListItem>
                                                <asp:ListItem>None</asp:ListItem>
                                                <asp:ListItem Value="Between">Between</asp:ListItem>
                                                <asp:ListItem Value="&lt;">Less Than</asp:ListItem>
                                                <asp:ListItem Value="&gt;">Greater Than</asp:ListItem>
                                                <asp:ListItem Value="Payment">Payment</asp:ListItem>
                                            </asp:DropDownList><i></i>
                                            <asp:RequiredFieldValidator ID="RfvCondition" runat="server" ControlToValidate="ddlCriteria" ErrorMessage="Condition Required" InitialValue=" " SetFocusOnError="True" ValidationGroup="Save">*</asp:RequiredFieldValidator>
                                        </label>
                                    </section>
                                    <section class="col col-2">
                                        <label class="label" runat="server" id="lblValue" visible="false">Value</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtValue" runat="server" Visible="False"></asp:TextBox>
                                        </label>
                                    </section>
                                    <section class="col col-2">
                                        <label class="label" runat="server" id="lblValue2" visible="false">Value2</label>
                                        <label class="input">
                                            <asp:TextBox ID="txtValue2" runat="server" Visible="False"></asp:TextBox>
                                        </label>
                                    </section>
                                </div>

                            </fieldset>
                            <asp:DataGrid ID="dgApprovalLevel" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyField="Id" GridLines="None" OnItemCommand="dgApprovalLevel_ItemCommand" OnItemDataBound="dgApprovalLevel_ItemDataBound" ShowFooter="True" CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" OnDeleteCommand="dgApprovalLevel_DeleteCommand1" OnEditCommand="dgApprovalLevel_EditCommand1" OnUpdateCommand="dgApprovalLevel_UpdateCommand1" OnCancelCommand="dgApprovalLevel_CancelCommand1">

                                <Columns>

                                    <asp:TemplateColumn HeaderText="Approver Position">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPosition" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" DataTextField="PositionName" DataValueField="Id"
                                                ValidationGroup="proedit">
                                                <asp:ListItem Value="-1">Select Position</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvPostion" runat="server" CssClass="validator" ControlToValidate="ddlPosition" InitialValue="-1" ValidationGroup="proedit">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFPosition" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True" DataTextField="PositionName" DataValueField="Id"
                                                EnableViewState="true" ValidationGroup="proadd">
                                                <asp:ListItem Value="-1">Select Position</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvFPostion" runat="server" CssClass="validator"
                                                ControlToValidate="ddlFPosition" Display="Dynamic"
                                                ErrorMessage="Employee Position Required" InitialValue="-1" SetFocusOnError="True"
                                                ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "EmployeePosition.PositionName")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Will">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlWill" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                ValidationGroup="proedit">
                                                <asp:ListItem Value=" ">Select Will</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvWill" runat="server" CssClass="validator" ControlToValidate="ddlWill" InitialValue=" " ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:DropDownList ID="ddlFWill" runat="server" CssClass="form-control"
                                                AppendDataBoundItems="True"
                                                EnableViewState="true" ValidationGroup="proadd">
                                                <asp:ListItem Value=" ">Select Will</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RfvFWill" runat="server" CssClass="validator"
                                                ControlToValidate="ddlFWill" Display="Dynamic"
                                                ErrorMessage="Will Status Required" InitialValue=" " SetFocusOnError="True"
                                                ValidationGroup="proadd">*</asp:RequiredFieldValidator>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Will")%>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="proedit" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default"><i class="fa fa-times"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn btn-xs btn-default" OnClientClick="javascript:return confirm('Are you sure you want to delete this entry?');"><i class="fa fa-times"></i></asp:LinkButton>

                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAddNew" runat="server" CommandName="AddNew" ValidationGroup="proadd" CssClass="btn btn-sm btn-success"><i class="fa fa-save"></i></asp:LinkButton>
                                        </FooterTemplate>

                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>

                            <br />


                            <footer>
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn btn-primary" ValidationGroup="Save"></asp:Button>
                                <a data-toggle="modal" href="#myModal" class="btn btn-primary"><i class="fa fa-circle-arrow-up fa-lg"></i>Search</a>
                                <%--<asp:Button ID="btnSearch" data-toggle="modal" runat="server" OnClientClick="#myModal" Text="Search" ></asp:Button>--%>
                        <asp:Button ID="btnClosepage" runat="server" Text="Close" data-dismiss="modal" CssClass="btn btn-primary" PostBackUrl="../Default.aspx"></asp:Button>
                            </footer>
                        </div>

                    </div>
                    <!-- end widget content -->

                </div>
                <!-- end widget div -->

            </div>
            <!-- end widget -->

        </article>
        <!-- END COL -->

    </div>


    <%-- Modal --%>

    <div class="modal fade" id="myModal" tabindex="-1" role="dialog">
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
                            <h2>Search Approval Setting</h2>
                        </header>
                        <div>
                            <div class="jarviswidget-editbox"></div>
                            <div class="widget-body no-padding">
                                <div class="smart-form">
                                    <fieldset>
                                        <div class="row">
                                            <section class="col col-6">
                                                <asp:Label ID="lblRequestType" runat="server" Text="Request Type" CssClass="label"></asp:Label>

                                                <label class="select">
                                                    <asp:DropDownList ID="ddlRequestTypesrch" runat="server">
                                                        <asp:ListItem Value=" ">Select Request Type</asp:ListItem>
                                                    </asp:DropDownList><i></i>
                                                </label>
                                            </section>

                                        </div>
                                    </fieldset>
                                    <footer>
                                        <asp:Button ID="btnFind" runat="server" Text="Find" OnClick="btnFind_Click" CssClass="btn btn-primary"></asp:Button>
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" data-dismiss="modal" CssClass="btn btn-primary"></asp:Button>
                                    </footer>
                                </div>
                            </div>
                        </div>

                        <asp:GridView ID="grvApprovalSettingList"
                            runat="server" AutoGenerateColumns="False" DataKeyNames="ID"
                            OnRowDataBound="grvApprovalSettingList_RowDataBound" OnRowDeleting="grvApprovalSettingList_RowDeleting"
                            OnSelectedIndexChanged="grvApprovalSettingList_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="grvApprovalSettingList_PageIndexChanging"
                            CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active">
                            <RowStyle CssClass="rowstyle" />
                            <Columns>
                                <asp:BoundField DataField="RequestType" HeaderText="Request Type" SortExpression="RequestType" />
                                <asp:BoundField DataField="CriteriaCondition" HeaderText="Condition" SortExpression="CriteriaCondition" />
                                <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Value" />
                                <asp:BoundField DataField="ApprovalLevel" HeaderText="Approval Level" SortExpression="ApprovalLevel" />
                                <asp:CommandField ShowSelectButton="True" />
                                <asp:CommandField ShowDeleteButton="True" />
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
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
    <!-- /.modal -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" runat="Server">
</asp:Content>


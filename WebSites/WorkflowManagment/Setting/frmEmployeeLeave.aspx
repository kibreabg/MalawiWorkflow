<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ModuleMaster.master" AutoEventWireup="true" CodeFile="frmEmployeeLeave.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Setting.Views.frmEmployeeLeave" %>
 <%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>


<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">


    <div id="content4">
        <div class="row">
            <article id="A2" class="col-sm-6 col-md-6 col-lg-3 sortable-grid ui-sortable">
                <div id="wid-id-4" class="jarviswidget jarviswidget-color-blue jarviswidget-sortable" data-widget-colorbutton="false" data-widget-editbutton="false" role="widget" style="">

                    <header role="heading">
                        <div class="jarviswidget-ctrls" role="menu">
                            <a class="button-icon jarviswidget-toggle-btn" data-original-title="Collapse" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-minus "></i></a><a class="button-icon jarviswidget-fullscreen-btn" data-original-title="Fullscreen" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-expand "></i></a><a class="button-icon jarviswidget-delete-btn" data-original-title="Delete" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-times"></i></a>
                        </div>
                        <span class="widget-icon"><i class="fa fa-check txt-color-white"></i></span>
                        <h2>Employee List</h2>
                        <!-- <div class="widget-toolbar">
									add: non-hidden - to disable auto hide

									</div>-->
                        <span class="jarviswidget-loader"><i class="fa fa-refresh fa-spin"></i></span>
                    </header>

                    <!-- widget div-->
                    <div role="content">
                        <!-- widget edit box -->
                        <div class="jarviswidget-editbox">
                            <div>
                                <label>
                                    Title:</label>
                                <input type="text" />
                            </div>
                        </div>
                        <!-- end widget edit box -->

                        <div class="widget-body no-padding smart-form">
                            <!-- content goes here -->
                            <h5 class="todo-group-title">Employee List</h5>
                            <ul id="sortable3" class="todo">
                                <asp:TreeView ID="TrvEmployeeList" runat="server" OnSelectedNodeChanged="TrvEmployeeList_SelectedNodeChanged">
                                </asp:TreeView>
                            </ul>

                            <!-- end content -->
                        </div>
                    </div>
                    <!-- end widget div -->
                </div>
            </article>
            <article id="A1" class="col-sm-12 col-md-12 col-lg-8 sortable-grid ui-sortable">
          
                <div id="wid-id-3" class="jarviswidget jarviswidget-color-blue jarviswidget-sortable" data-widget-colorbutton="false" data-widget-editbutton="false" role="widget" style="">

                    <header role="heading">
                        <div class="jarviswidget-ctrls" role="menu">
                            <a class="button-icon jarviswidget-toggle-btn" data-original-title="Collapse" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-minus "></i></a><a class="button-icon jarviswidget-fullscreen-btn" data-original-title="Fullscreen" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-expand "></i></a><a class="button-icon jarviswidget-delete-btn" data-original-title="Delete" data-placement="bottom" href="javascript:void(0);" rel="tooltip" title=""><i class="fa fa-times"></i></a>
                        </div>
                        <span class="widget-icon"><i class="fa fa-check txt-color-white"></i></span>
                        <h2>Employee Leave Setting</h2>

                        <span class="jarviswidget-loader"><i class="fa fa-refresh fa-spin"></i></span>
                    </header>

                    <!-- widget div-->
                    <div role="content">
                        <!-- widget edit box -->
                        <div class="jarviswidget-editbox">
                            <div>
                                <label>
                                    Title:</label>
                                <input type="text" />
                            </div>
                        </div>
                        <!-- end widget edit box -->

                        <div class="widget-body no-padding smart-form">
                            <!-- content goes here -->
                            <h5 class="todo-group-title">Employee Leave Setting for
                                    <asp:Label ID="lblSelectedEmp" runat="server"></asp:Label>
                            </h5>


                            <table class="nav-justified">
                                <tr>
                                   
                                    <td style="padding-left: 32em">
                                        <asp:Button ID="btnAddContract" runat="server" Text="Add Contract" CssClass="btn btn-primary" Enabled="false"></asp:Button>
                                        <asp:Button ID="btnRenewContract" runat="server" Text="Renew Contract" CssClass="btn btn-primary" Enabled="false" OnClick="btnRenewContract_Click" CausesValidation="False" CommandName="Renew"></asp:Button>
                                        <asp:Button ID="btnTerminateContract" runat="server" Text="Terminate Contract" CssClass="btn btn-primary" Enabled="false" OnClick="btnTerminateContract_Click"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                            <asp:DataGrid ID="dgEmployeeSetting" runat="server" AlternatingRowStyle-CssClass="" AutoGenerateColumns="False" CellPadding="0"
                                CssClass="table table-striped table-bordered table-hover" PagerStyle-CssClass="paginate_button active" DataKeyField="Id"
                                GridLines="None" OnCancelCommand="dgEmployeeSetting_CancelCommand" OnEditCommand="dgEmployeeSetting_EditCommand"
                                OnItemDataBound="dgEmployeeSetting_ItemDataBound" OnUpdateCommand="dgEmployeeSetting_UpdateCommand"
                                ShowFooter="True">

                                <Columns>
                                    <asp:TemplateColumn HeaderText="Start Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "StartDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "StartDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorstartdate" runat="server" CssClass="validator" ControlToValidate="txtStartDate" ErrorMessage="Start Date Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="End Date">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "EndDate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy" Text=' <%# DataBinder.Eval(Container.DataItem, "EndDate","{0:M/d/yyyy}")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorenddate" runat="server" CssClass="validator" ControlToValidate="txtEndDate" ErrorMessage="End Date Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Leave Taken">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "LeaveTaken")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtLeaveTaken" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "LeaveTaken")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorleavetaken" runat="server" CssClass="validator" ControlToValidate="txtLeaveTaken" ErrorMessage="Leave Taken Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Opening balance">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Beginingbalance")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtBeginingbalance" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Beginingbalance")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorbeginingbalance" runat="server" CssClass="validator" ControlToValidate="txtBeginingbalance" ErrorMessage="Opening Balance Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Rate">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Rate")%>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control" Text=' <%# DataBinder.Eval(Container.DataItem, "Rate")%>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorrrate" runat="server" CssClass="validator" ControlToValidate="txtRate" ErrorMessage="Rate Required" ValidationGroup="1">*</asp:RequiredFieldValidator>
                                        </EditItemTemplate>

                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Status">
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "Status")%>
                                        </ItemTemplate>


                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn HeaderText="Actions">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" ValidationGroup="1" CssClass="btn btn-xs btn-default"><i class="fa fa-save"></i></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CssClass="btn btn-xs btn-default"><i class="fa fa-pencil"></i></asp:LinkButton>

                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle CssClass="paginate_button active" HorizontalAlign="Center" />
                            </asp:DataGrid>




                            <!-- end content -->
                        </div>
                    </div>
                    <!-- end widget div -->
                </div>
            </article>
              
        </div>
    </div>
    <asp:Panel ID="pnlAddEmployeeLeave" Style="position: absolute; top: 10%; left: 20%;" runat="server">
        <div class="modal-content">
            <div class="modal-header">
            </div>
            <div class="modal-body no-padding">
                <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                    <header>
                        <span class="widget-icon"><i class="fa fa-edit"></i></span>
                        <h2>Add Employee Contract</h2>
                    </header>
                    <div>
                        <div class="jarviswidget-editbox"></div>
                        <div class="widget-body no-padding">
                            <div class="smart-form">
                                <fieldset>
                                    <asp:UpdatePanel ID="upApproval" runat="server">
                                        <ContentTemplate>
                                            <div class="row">
                                                   <section class="col col-6">
                                                    <asp:Label ID="lblStartDate" runat="server" Text="Start Date" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <i class="icon-append fa fa-calendar"></i>
                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ErrorMessage="Start Date Required" ControlToValidate="txtStartDate" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                                   <section class="col col-6">
                                                    <asp:Label ID="lblEndDate" runat="server" Text="End Date" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <i class="icon-append fa fa-calendar"></i>
                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ErrorMessage="End Date Required" ControlToValidate="txtEndDate" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                             </div>
                                            <div class="row">
                                                <section class="col col-6">
                                                    <asp:Label ID="lblRate" runat="server" Text="Rate" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox ID="txtRate" runat="server"></asp:TextBox>
                                                         <asp:FilteredTextBoxExtender ID="txtRate_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtRate" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                         <asp:RequiredFieldValidator ID="rfvrate" runat="server" ErrorMessage="Rate Required" ControlToValidate="txtRate" ValidationGroup="save"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                                <section class="col col-6">
                                                    <asp:Label ID="lblBeginingBalance" runat="server" Text="Opening Balance" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox ID="txtBeginingBalance" runat="server"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtBeginingBalance_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtBeginingBalance" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                    </label>
                                                </section>
                                            </div>
                                            <div class="row">
                                                 <section class="col col-6">
                                                    <asp:Label ID="lblLeaveTaken" runat="server" Text="Leave Taken" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox ID="txtLeaveTaken" runat="server"></asp:TextBox>
                                                     <asp:FilteredTextBoxExtender ID="txtLeaveTaken_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtLeaveTaken" ValidChars=".">
                                                     </asp:FilteredTextBoxExtender>
                                                    </label>
                                                </section>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                                <footer>
                                    <asp:Button ID="btnApprove" runat="server" ValidationGroup="save" Text="Save" CssClass="btn btn-primary" OnClick="btnApprove_Click"></asp:Button>
                                    <asp:Button ID="btnCanceladd" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCanceladd_Click" ></asp:Button>

                                </footer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-content -->
    </asp:Panel>
    
    <asp:Panel ID="pnlRenewEmployeeLeave" Style="position: absolute; top: 10%; left: 20%;"  Visible="false" runat="server">
        <div class="modal-content">
            <div class="modal-header">
            </div>
            <div class="modal-body no-padding">
                <div class="jarviswidget" data-widget-editbutton="false" data-widget-custombutton="false">
                    <header>
                        <span class="widget-icon"><i class="fa fa-edit"></i></span>
                        <h2>Process Employee Contract Renewal</h2>
                    </header>
                    <div>
                        <div class="jarviswidget-editbox"></div>
                        <div class="widget-body no-padding">
                            <div class="smart-form">
                                <fieldset>
                                    
                                            <div class="row">
                                                   <section class="col col-6">
                                                    <asp:Label ID="lblRStartDate" runat="server" Text="Start Date" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <i class="icon-append fa fa-calendar"></i>
                                                        <asp:TextBox ID="txtRStartDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvrStartDate" runat="server" ErrorMessage="Start Date Required" ControlToValidate="txtRStartDate" ValidationGroup="Renew"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                                   <section class="col col-6">
                                                    <asp:Label ID="lblREndDate" runat="server" Text="End Date" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <i class="icon-append fa fa-calendar"></i>
                                                        <asp:TextBox ID="txtREndDate" runat="server" CssClass="form-control datepicker" data-dateformat="mm/dd/yy"></asp:TextBox>
                                                         <asp:RequiredFieldValidator ID="rfvREndDate" runat="server" ErrorMessage="End Date Required" ControlToValidate="txtREndDate" ValidationGroup="Renew"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                             </div>
                                            <div class="row">
                                                <section class="col col-6">
                                                    <asp:Label ID="lblRRate" runat="server" Text="Rate" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox ID="txtRRate" runat="server"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtRRate_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtRRate" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="rfvRRate" runat="server" ErrorMessage="Rate Required" ControlToValidate="txtRRate" ValidationGroup="Renew"></asp:RequiredFieldValidator>
                                                    </label>
                                                </section>
                                                <section class="col col-6">
                                                    <asp:Label ID="lblRBeginingBalance" runat="server" Text="Opening Balance" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox runat="server" ID="txtRBeginingBalance"></asp:TextBox>
                                                       
                                                    <asp:FilteredTextBoxExtender ID="txtRBeginingBalance_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtRBeginingBalance" ValidChars=".">
                                                    </asp:FilteredTextBoxExtender>
                                                       
                                                    </label>
                                                </section>
                                            </div>
                                            <div class="row">
                                                 <section class="col col-6">
                                                    <asp:Label ID="lblRLeaveTaken" runat="server" Text="Leave Taken" Visible="false" CssClass="label"></asp:Label>
                                                    <label class="input">
                                                        <asp:TextBox ID="txtRLeaveTaken" runat="server" Visible="false"></asp:TextBox>
                                                     <asp:FilteredTextBoxExtender ID="txtRLeaveTaken_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="txtRLeaveTaken" ValidChars=".">
                                                     </asp:FilteredTextBoxExtender>
                                                    </label>
                                                </section>
                                            </div>
                                 
                                </fieldset>
                                <footer>
                                    <asp:Button ID="btnRenew" runat="server" ValidationGroup="Renew" Text="Renew" CssClass="btn btn-primary" OnClick="btnRenew_Click"></asp:Button>
                                    <asp:Button ID="btnCancelRenew" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnCancelRenew_Click"></asp:Button>

                                </footer>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
      
        <!-- /.modal-content -->
    </asp:Panel>  
    <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground" Enabled="True" PopupControlID="pnlAddEmployeeLeave"
        TargetControlID="btnAddContract" CancelControlID="btnCancelAdd" ID="pnlAddEmployeeLeave_ModalPopupExtender">
    </asp:ModalPopupExtender>
	 <%-- <asp:ModalPopupExtender runat="server" BackgroundCssClass="modalBackground" Enabled="True" PopupControlID="pnlRenewEmployeeLeave"
        TargetControlID="btnRenewContract" CancelControlID="btnCancelRenew" ID="pnlRenewEmployeeLeave_ModalPopupExtender">
    </asp:ModalPopupExtender>	--%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="menuContent" Runat="Server">
</asp:Content>


<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignJob.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.AssignJob"
    Title="Assign Job" MasterPageFile="~/Shared/ModuleMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>Assign Job</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Assign To</label>
                                <label class="select">

                                    <asp:DropDownList ID="ddlAssignTo" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="rfvAssignTo" runat="server" ControlToValidate="ddlAssignTo" CssClass="validator"
                                        Display="Dynamic" ErrorMessage="Assign To is required"
                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvAssignTo_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvAssignTo" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                        </div>
                    </fieldset>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Assign" OnClick="btnSave_Click" CssClass="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnUnassign" runat="server" Text="Unassign" CausesValidation="False"
                            CssClass="btn btn-primary" OnClick="btnUnassign_Click"></asp:Button>
                        <asp:Button ID="btnCancel" runat="server" Text="Close" CausesValidation="False"
                            OnClick="btnCancel_Click" CssClass="btn btn-primary"></asp:Button>

                    </footer>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserEdit.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Admin.Views.UserEdit"
    Title="UserEdit" MasterPageFile="~/Shared/AdminMaster.master" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="content" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="jarviswidget" id="wid-id-8" data-widget-editbutton="false" data-widget-custombutton="false">
        <header>
            <span class="widget-icon"><i class="fa fa-edit"></i></span>
            <h2>User</h2>
        </header>
        <div>
            <div class="jarviswidget-editbox"></div>
            <div class="widget-body no-padding">
                <div class="smart-form">
                    <fieldset>
                        <div class="row">
                            <section class="col col-6">

                                <label class="label">Username</label>
                                <label class="input">
                                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                                        Display="Dynamic" ErrorMessage="User name is required" CssClass="validator"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvUsername_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvUsername" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                    <asp:Label ID="lblUsername" runat="server" Visible="False"></asp:Label></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">First name</label>
                                <label class="input">

                                    <asp:TextBox ID="txtFirstname" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Last name</label>
                                <label class="input">
                                    <asp:TextBox ID="txtLastname" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Employee No.</label>
                                <label class="input">
                                    <asp:TextBox ID="txtEmployeeNo" runat="server"></asp:TextBox></label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Employee Position</label>
                                <label class="select">
                                    <asp:DropDownList ID="ddlEmployeePostion" runat="server" AppendDataBoundItems="True" DataTextField="PositionName" DataValueField="Id">
                                        <asp:ListItem Value="0">Select Employee</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="rfvEmployeePosition" runat="server" ControlToValidate="ddlEmployeePostion"
                                        Display="Dynamic" ErrorMessage="Employee Position is required" CssClass="validator"
                                        SetFocusOnError="True" InitialValue="0"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvEmployeePosition_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvEmployeePosition" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>


                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Superviser</label>
                                <label class="select">

                                    <asp:DropDownList ID="ddlSuperviser" runat="server" AppendDataBoundItems="True" DataTextField="FullName" DataValueField="Id">
                                        <asp:ListItem Value="-1">Select Superviser</asp:ListItem>
                                    </asp:DropDownList><i></i>
                                    <asp:RequiredFieldValidator ID="rfvSuperviser" runat="server" ControlToValidate="ddlSuperviser"
                                        Display="Dynamic" ErrorMessage="Superviser is required" CssClass="validator"
                                        SetFocusOnError="True" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvSuperviser_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvSuperviser" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Email</label>
                                <label class="input">


                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="validator" ControlToValidate="txtEmail"
                                        Display="Dynamic" ErrorMessage="Invalid Email Address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                        SetFocusOnError="True"></asp:RegularExpressionValidator>
                                    <cc1:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                    &nbsp;<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                        Display="Dynamic" ErrorMessage="Email is required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <cc1:ValidatorCalloutExtender ID="rfvEmail_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="rfvEmail" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                    <asp:CheckBox ID="chkActive" runat="server" Text="Is Active" Checked="True"></asp:CheckBox>
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Password</label>
                                <label class="input">


                                    <asp:TextBox ID="txtPassword1" type="password" runat="server" TextMode="Password"></asp:TextBox></label>
                                 <asp:RegularExpressionValidator ID="Regex2" runat="server" ControlToValidate="txtPassword1"
    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"
    ErrorMessage="Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character" ForeColor="Red" />
                            </section>
                        </div>
                        <div class="row">
                            <section class="col col-6">
                                <label class="label">Confirm password</label>
                                <label class="input">



                                    <asp:TextBox ID="txtPassword2" type="password" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPassword1"
                                        ControlToValidate="txtPassword2" Display="Dynamic" ErrorMessage="The password you typed donot match. Please retype the password"
                                        SetFocusOnError="True"></asp:CompareValidator>
                                    <cc1:ValidatorCalloutExtender ID="CompareValidator1_ValidatorCalloutExtender" runat="server"
                                        Enabled="True" TargetControlID="CompareValidator1" Width="300px">
                                    </cc1:ValidatorCalloutExtender>
                                </label>
                            </section>
                        </div>
                    </fieldset>
                    <h4>Roles</h4>
                    <div class="table-responsive">

                        <table class="table table-striped table-bordered table-hover">

                            <asp:Repeater ID="rptRoles" runat="server">
                                <HeaderTemplate>
                                    <tr>
                                        <th>Role
                                        </th>
                                        <th></th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:CheckBox ID="chkRole" runat="server"></asp:CheckBox>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                    <footer>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="False"
                            OnClick="btnCancel_Click" class="btn btn-primary"></asp:Button>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" Visible="False" class="btn btn-primary"></asp:Button>
                    </footer>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

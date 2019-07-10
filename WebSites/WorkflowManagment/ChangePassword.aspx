<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Shared/LogInMaster.master" AutoEventWireup="true"
    CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword" %>

<%@ MasterType TypeName="Chai.WorkflowManagment.Modules.Shell.BaseMaster" %>
<asp:Content ID="Content2" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div id="panMessage" runat="server">
        <asp:Label ID="lblErrormessage" runat="server"></asp:Label>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-7 col-lg-8 hidden-xs hidden-sm">
		<div class="row">
			<div class="col-xs-12 col-sm-12 col-md-6 col-lg-6"></div>
		</div>					
	</div>					
    <div class="col-xs-12 col-sm-12 col-md-5 col-lg-4">
			<div class="well no-padding">
                 <header>Change Password</header>
					<fieldset>
                         <section>
                                        <asp:Label ID="CurrentPasswordLabel0" runat="server" AssociatedControlID="CurrentPassword" CssClass="Label">Password:</asp:Label>
                                    <label class="input"> <i class="icon-append fa fa-lock"></i>
                                        <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password"></asp:TextBox></label>

                                        <asp:RequiredFieldValidator ID="CurrentPasswordRequired0" runat="server" ControlToValidate="CurrentPassword"
                                            ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                  </section>
                          <section>
                                        <asp:Label ID="NewPasswordLabel0" runat="server" AssociatedControlID="NewPassword" CssClass="label">New Password:</asp:Label>
                                   <label class="input"> <i class="icon-append fa fa-lock"></i>
                                        <asp:TextBox ID="NewPassword" runat="server" TextMode="Password"></asp:TextBox></label>
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired0" runat="server" ControlToValidate="NewPassword"
                                            ErrorMessage="New Password is required." ToolTip="New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                              <asp:RegularExpressionValidator ID="Regex2" runat="server" ControlToValidate="NewPassword"
    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"
    ErrorMessage="Minimum 8 characters atleast 1 Alphabet, 1 Number and 1 Special Character" ForeColor="Red" />
                                  </section>
                         <section>
                                        <asp:Label ID="ConfirmNewPasswordLabel0" runat="server" AssociatedControlID="ConfirmNewPassword" CssClass="">Confirm New Password:</asp:Label>
                                    <label class="input"> <i class="icon-append fa fa-lock"></i>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" CssClass="label"></asp:TextBox></label>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired0" runat="server" ControlToValidate="ConfirmNewPassword"
                                            ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                            ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    
                                        <asp:CompareValidator ID="CpVconfirm0" runat="server" ControlToCompare="NewPassword"
                                            ControlToValidate="ConfirmNewPassword" ErrorMessage="Your passwords do not match up!"
                                            SetFocusOnError="True" ValidationGroup="ChangePassword1"></asp:CompareValidator></section>
                                    </fieldset>
								<footer>
                              
                                        <asp:Button ID="ChangePasswordPushButton0" runat="server" class="btn btn-primary"
                                            CommandName="ChangePassword" OnClick="ChangePasswordPushButton_Click" Text="Change Password"
                                            ValidationGroup="ChangePassword1" />
                                  
                                        <asp:Button ID="CancelPushButton0" runat="server" CausesValidation="False" class="btn btn-primary"
                                            CommandName="Cancel" Text="Close" OnClick="CancelPushButton0_Click1" /></footer>
                </div></div>
                                
</asp:Content>

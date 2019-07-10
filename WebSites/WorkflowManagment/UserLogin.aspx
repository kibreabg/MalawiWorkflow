<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserLogin.aspx.cs" Inherits="Chai.WorkflowManagment.Modules.Shell.Views.UserLogin"
    Title="UserLogin" MasterPageFile="~/Shared/LogInMaster.master" %>

<asp:Content ID="content1" ContentPlaceHolderID="DefaultContent" runat="Server">
    <div class="col-xs-12 col-sm-12 col-md-7 col-lg-8 hidden-xs hidden-sm">
		<div class="row">
			<div class="col-xs-12 col-sm-12 col-md-6 col-lg-6"></div>
		</div>					
	</div>					
    	<div class="col-xs-12 col-sm-12 col-md-5 col-lg-4">
			<div class="well no-padding">
                 <header>Sign In</header>
					<fieldset>
                                        <asp:Label ID="lblLoginError" runat="server" CssClass="label" EnableViewState="False"></asp:Label>
                                        <asp:Label ID="lblForgotPassword" runat="server" CssClass="label" EnableViewState="False" ForeColor="Red"></asp:Label>
                       <section>
										<label class="label">User Name</label>
                                        <label class="input"> <i class="icon-append fa fa-user"></i>
                                        <asp:TextBox class="inputText"  ID="txtUsername" runat="server"></asp:TextBox>
                                         <b class="tooltip tooltip-top-right"><i class="fa fa-user txt-color-teal"></i> Please enter username</b></label>
    	            </section>
                     <section>
			                            <label class="label">Password</label>
                                        <label class="input"> <i class="icon-append fa fa-lock"></i>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                        <b class="tooltip tooltip-top-right"><i class="fa fa-lock txt-color-teal"></i> Enter your password</b> </label>
  	          
                     </section>
                     <section>
										
									    <asp:CheckBox ID="chkPersistLogin" runat="server" Text=""  Font-Size="Small" style="font-size: x-small" />
                        				Stay signed in
					</section>
                         <section>
										
                        <asp:LinkButton ID="lnkForgotPassword"  runat="server" OnClick="lnkForgotPassword_Click">Forgot Password</asp:LinkButton>
					</section>
                   </fieldset>
								<footer>
                                 <asp:Button ID="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Sign in" class="btn btn-primary"></asp:Button>
										
								</footer>
                  
          </div>

      </div>
</asp:Content>


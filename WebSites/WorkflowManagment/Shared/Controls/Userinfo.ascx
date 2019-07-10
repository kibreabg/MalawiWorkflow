<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Userinfo.ascx.cs" Inherits="Chai.WorkflowManagment.Modules.Shell.Views.UserInfopage" %>

 <asp:LinkButton ID="lnkChangepassword" runat="server" 
              onclick="lnkChangepassword_Click" Font-Underline="True" 
            ForeColor="White">Change Password</asp:LinkButton>&nbsp;<asp:LoginStatus runat="server" ID="LoginStatusText" CssClass ="headerLink" 
                            LogoutAction="Redirect" LogoutPageUrl="~/UserLogin.aspx" 
            Font-Underline="True" ForeColor="White" /> 
                <asp:Label ID="lblUser" runat="server" ForeColor="White"></asp:Label>


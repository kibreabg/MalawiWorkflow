using System;
using Microsoft.Practices.ObjectBuilder;
using System.Net.Mail;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Shared;

namespace Chai.WorkflowManagment.Modules.Shell.Views
{
    public partial class UserLogin : Microsoft.Practices.CompositeWeb.Web.UI.Page, IUserLoginView
    {
        private UserLoginPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                if (Context.User.Identity.IsAuthenticated)
                    Context.Response.Redirect("~/");
            }

            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                txtUsername.Focus();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public UserLoginPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)   
        {
            if (this.txtUsername.Text.Trim().Length > 0 && this.txtPassword.Text.Trim().Length > 0)
            {
                try
                {
                    if (_presenter.AuthenticateUser())
                    {
                        //_presenter.RedirectToRowUrl();
                        Context.Response.Redirect("Default.aspx");
                    }
                    else
                    {
                        this.lblLoginError.Text = "User name or password incorrect";
                        this.lblLoginError.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    this.lblLoginError.Text = ex.Message + " The user may be not active user";
                    this.lblLoginError.Visible = true;
                }
            }
            else
            {
                this.lblLoginError.Text = "Please enter both a username and password";
                this.lblLoginError.Visible = true;
            }

        }
        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text != "")
            {
                try
                {
                    AppUser user = _presenter.SearchUser(txtUsername.Text);
                    user.Password = Encryption.StringToMD5Hash("pass@123");
                    _presenter.SaveOrUpdateUser(user);
                    MailMessage Msg = new MailMessage();
                    // Sender e-mail address.
                    Msg.From = new MailAddress("chaizim.wfms@gmail.com");
                    // Recipient e-mail address.
                    Msg.To.Add(user.Email);
                    Msg.Subject = "Your Password Details";
                    Msg.Body = "Hi, <br/>Please check your Login Details <br/><br/>Your Username: " + txtUsername.Text + "<br/><br/>Your Password: " + "pass@123" + "<br/><br/>";
                    Msg.IsBodyHtml = true;
                    // your remote SMTP server IP.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("chaizim.wfms@gmail.com", "ChaiZim@12345");
                    smtp.EnableSsl = true;
                    smtp.Send(Msg);
                    //Msg = null;
                    lblForgotPassword.Text = "Your Password Details Sent to your Email, Please change your password after you login to the system";
                    // Clear the textbox valuess
                    //lblForgotPassword.Text = "";
                }
                catch (Exception ex)
                {
                    lblForgotPassword.Text = ex.Message;
                }
            }
            else
            {
                lblForgotPassword.Text = "The Username you entered does not exist.";
            }
        }

        #region IUserLoginView Members

        public string GetUserName
        {
            get { return txtUsername.Text; }
        }

        public string GetPassword
        {
            get { return txtPassword.Text; }
        }

        public bool PersistLogin
        {
            get { return chkPersistLogin.Checked; }
        }

        #endregion
    }
}


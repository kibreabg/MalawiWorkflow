using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Services;
using log4net;
using log4net.Config;

namespace Chai.WorkflowManagment.Modules.Shell
{
    /// <summary>
    /// Summary description for AuthenticationModule
    /// </summary>
    public class AuthenticationModule : IHttpModule
    {
        private const int AUTHENTICATION_TIMEOUT = 20;
        private int _failedAttempts = 0;
        private static readonly ILog LogInFailureLog = LogManager.GetLogger("FailedLoginLog");
        
        public AuthenticationModule()
        {
            XmlConfigurator.Configure();
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(Context_AuthenticateRequest);
        }

        public void Dispose()
        {
            // Nothing here 
        }

        /// <summary>
        /// Try to authenticate the user.
       /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool AuthenticateUser(string username, string password, bool persistLogin)
        {
            string hashedPassword = Encryption.StringToMD5Hash(password);

            try
            {
                using (var wr = WorkspaceFactory.Create())
                {
                    AppUser user = wr.Single<AppUser>(x => x.UserName == username && x.Password == hashedPassword, x => x.EmployeePosition); 
                    if (user != null)
                    {
                        if (!user.IsActive)
                        {
                            throw new Exception("The account is disabled.");
                        }
                        user.IsAuthenticated = true;
                        string currentIp = HttpContext.Current.Request.UserHostAddress;
                        user.LastLogin = DateTime.Now;
                        user.LastIp = currentIp;
                        wr.CommitChanges();
                        
                        HttpContext.Current.User = new ChaiPrincipal(user);
                        FormsAuthentication.SetAuthCookie(user.Name, persistLogin);

                        return true;
                    }
                    else
                    {
                        _failedAttempts++;
                        if (_failedAttempts == 3)
                        {
                            LogInFailureLog.Warn("User with username: " + username + " IP "+ HttpContext.Current.Request.UserHostAddress + " has failed a log in attempt!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unable to log in user '{0}': " + ex.Message, username), ex.InnerException);
            }
            return false;
        }
        ///
        /// <summary>
        /// Log out the current user.
        /// </summary>
        /// 
        public void Logout()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
        }

        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            if (app.Context.User != null && app.Context.User.Identity.IsAuthenticated)
            {
                int userId = Int32.Parse(app.Context.User.Identity.Name);

                using (var wr = WorkspaceFactory.CreateReadOnly())
                {
                    AppUser user = wr.Single<AppUser>(x => x.Id == userId, x => x.EmployeePosition, x => x.AppUserRoles.Select(y => y.Role));
                    user.IsAuthenticated = true;
                    app.Context.User = new ChaiPrincipal(user);
                }
            }
        }
    }
}

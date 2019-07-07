using System;
using System.Security;
using System.Security.Principal;

using Chai.WorkflowManagment.CoreDomain.Users;



namespace Chai.WorkflowManagment.CoreDomain
{
    public class ChaiPrincipal: IPrincipal
    {
        private AppUser _user;
       
		/// <summary>
		/// 
		/// </summary>
        public IIdentity Identity
		{
			get { return this._user; }
		}
        public string UserName
        {
            get { return _user.UserName; }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="role"></param>
		/// <returns></returns>
		public bool IsInRole(string role)
		{
			foreach (AppUserRole roleObject in this._user.AppUserRoles)
			{
				if (roleObject.Role.Name.Equals(role))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Default constructor. An instance of an authenticated user is required when creating this principal.
		/// </summary>
		/// <param name="user"></param>
		public ChaiPrincipal(AppUser user)
		{
			if (user != null && user.IsAuthenticated)
			{
				this._user = user;
                
			}
			else
			{
				throw new SecurityException("Cannot create a principal without a valid user");
			}
		}


        
    }
}

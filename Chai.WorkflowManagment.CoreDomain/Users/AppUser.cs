
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Setting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Chai.WorkflowManagment.CoreDomain.Users
{
    [Table("AppUsers", Schema = "dbo")]
    public partial class AppUser : IEntity, IIdentity
    {
        public AppUser()
        {
            this._appUserRole = new List<AppUserRole>();
            this.Projects = new List<Project>();
        }
        
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string EmployeeNo { get; set; }
        public Nullable<int> Superviser { get; set; }
        public virtual EmployeePosition EmployeePosition { get; set; }
       
        public Nullable<bool> IsAssignedJob { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastIp { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }

        private IList<AppUserRole> _appUserRole;
        public virtual IList<AppUserRole> AppUserRoles
        {
            get { return _appUserRole; }
            set { _appUserRole = value; }
        }
        public virtual IList<Project> Projects { get; set; }

        #region public methods

        private AccessLevel[] _permissions = new AccessLevel[0];

        [NotMapped]
        public AccessLevel[] Permissions
        {
            get
            {
                if (this._permissions.Length == 0)
                {
                    ArrayList permissions = new ArrayList();
                    foreach (AppUserRole role in AppUserRoles)
                    {
                        foreach (AccessLevel permission in role.Role.Permissions)
                        {
                            if (permissions.IndexOf(permission) == -1)
                                permissions.Add(permission);
                        }
                    }
                    this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
                }
                return this._permissions;
            }
        }

        public bool HasPermission(AccessLevel permission)
        {
            return Array.IndexOf(this.Permissions, permission) > -1;
        }

        public bool CanView(Node node)
        {
            foreach (NodeRole p in node.NodeRoles)
            {
                if (p.ViewAllowed && IsInRole(p.Role))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanEdit(Node node)
        {
            foreach (NodeRole p in node.NodeRoles)
            {
                if (p.EditAllowed && IsInRole(p.Role))
                {
                    return true;
                }
            }
            return false;
        }

        public static string HashPassword(string password)
        {
            if (ValidatePassword(password))
            {
                return Encryption.StringToMD5Hash(password);
            }
            else
            {
                throw new ArgumentException("Invalid password");
            }
        }

        public static bool ValidatePassword(string password)
        {
            return (password.Length >= 5);
        }

        public bool IsInRole(Role roleToCheck)
        {
            foreach (AppUserRole role in this.AppUserRoles)
            {
                if (role.Role.Id == roleToCheck.Id && role.Role.Name == roleToCheck.Name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInRole(string roleName)
        {
            foreach (AppUserRole role in this.AppUserRoles)
            {
                if (role.Role.Name == roleName)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "RamcsAuthentication"; }
        }

        private bool _isAuthenticated = false;
        [NotMapped]
        public bool IsAuthenticated
        {
            get { return this._isAuthenticated; }
            set { this._isAuthenticated = value; }
        }

        [NotMapped]
        public string Name
        {
            get
            {
                if (this._isAuthenticated)
                    return Id.ToString();
                else
                    return "";
            }
        }
        [NotMapped]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        #endregion

    }

}

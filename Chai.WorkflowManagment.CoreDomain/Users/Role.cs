
using System;
using System.Collections;
using System.Text;
using Chai.WorkflowManagment.Enums;

namespace Chai.WorkflowManagment.CoreDomain.Users
{
    public class Role : IEntity
    {
        public Role()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        
        private int _permissionLevel;
        public int PermissionLevel
        {
            get { return _permissionLevel; }
            set
            {
                _permissionLevel = value;
                TranslatePermissionLevelToAccessLevels();
            }
        }

        private AccessLevel[] _permissions;
        public virtual AccessLevel[] Permissions
        {
            get { return this._permissions; }
        }

        public virtual string PermissionsString
        {
            get { return GetPermissionsAsString(); }
        }

        public virtual bool HasPermission(AccessLevel permission)
        {
            return Array.IndexOf(this.Permissions, permission) > -1;
        }

        private void TranslatePermissionLevelToAccessLevels()
        {
            ArrayList permissions = new ArrayList();
            AccessLevel[] accessLevels = (AccessLevel[])Enum.GetValues(typeof(AccessLevel));

            foreach (AccessLevel accesLevel in accessLevels)
            {
                if ((this.PermissionLevel & (int)accesLevel) == (int)accesLevel)
                {
                    permissions.Add(accesLevel);
                }
            }
            this._permissions = (AccessLevel[])permissions.ToArray(typeof(AccessLevel));
        }

        private string GetPermissionsAsString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this._permissions.Length; i++)
            {
                AccessLevel accessLevel = this._permissions[i];
                sb.Append(accessLevel.ToString());
                if (i < this._permissions.Length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }


    }
}

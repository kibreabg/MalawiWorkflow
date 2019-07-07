
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class Node : IEntity
	{
        public Node()
        {
            this.NodeRoles = new List<NodeRole>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string PageId { get; set; }
        
        public virtual PocModule PocModule { get; set; }
        public virtual IList<NodeRole> NodeRoles { get; set; }

		#region Public Properties
        public virtual bool Exists(int id)
        {
            bool val = false;
            foreach (NodeRole NR in NodeRoles)
            {
                if (NR.Role.Id == Id)
                {
                    val = true;
                    break;
                }
                else
                {
                    val = false;
                }
            }
            return val;
        }
        [NotMapped]
        public string NodeUrl
        {
            get
            {
                return String.Format("~/{0}/{1}", PocModule.FolderPath, FilePath);
            }
        }

        [NotMapped]
        public bool AnonymousViewAllowed
        {
            get
            {
                foreach (NodeRole np in NodeRoles)
                {
                    if (Array.IndexOf(np.Role.Permissions, AccessLevel.Anonymous) > -1)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool ViewAllowed(IIdentity user)
        {
            AppUser u = user as AppUser;
            if (this.AnonymousViewAllowed)
            {
                return true;
            }
            else if (u != null)
            {
                return u.CanView(this);
            }
            else
            {
                return false;
            }
        }

        public bool ViewAllowed(Role role)
        {
            foreach (NodeRole np in this.NodeRoles)
            {
                if (np.Role.Id == role.Id && np.ViewAllowed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EditAllowed(Role role)
        {
            foreach (NodeRole np in this.NodeRoles)
            {
                if (np.Role.Id == role.Id && np.EditAllowed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool EditAllowed(IIdentity user)
        {
            AppUser u = user as AppUser;
            if (u != null)
            {
                return u.CanEdit(this);
            }
            else
            {
                return false;
            }
        }
        public virtual void RemoveNoderole(int Id)
        {

            foreach (NodeRole NR in NodeRoles)
            {
                if (NR.Id == Id)
                    NodeRoles.Remove(NR);
                break;
            }

        }
        #endregion
    }

}

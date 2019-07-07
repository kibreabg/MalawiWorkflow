
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Admins
{
    public class Tab : IEntity
    {
        public Tab()
        {
            this.PopupMenus = new List<PopupMenu>();
            this.TabRoles = new List<TabRole>();
            this.TaskPans = new List<TaskPan>();
        }

        public int Id { get; set; }
        public string TabName { get; set; }
        public int Position { get; set; }
        public string Description { get; set; }

        public virtual PocModule PocModule { get; set; }
        public virtual IList<PopupMenu> PopupMenus { get; set; }
        public virtual IList<TabRole> TabRoles { get; set; }
        public virtual IList<TaskPan> TaskPans { get; set; }

        public virtual bool Exists(int id)
        {
            bool val = false;
            foreach(TabRole NR in TabRoles)
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
        public string TabUrl
        {
            get
            {
                return String.Format("~/{0}", PocModule.FolderPath);
            }
        }

        [NotMapped]
        public bool AnonymousViewAllowed
        {
            get
            {
                foreach (TabRole tr in TabRoles)
                {
                    if (Array.IndexOf(tr.Role.Permissions, AccessLevel.Anonymous) > -1)
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
                foreach (AppUserRole ur in u.AppUserRoles)
                {
                    if (ViewAllowed(ur.Role))
                        return true;
                }
            }

            return false;
        }

        public bool ViewAllowed(Role role)
        {
            foreach (TabRole tr in this.TabRoles)
            {
                if (tr.Role.Id == role.Id && tr.ViewAllowed)
                {
                    return true;
                }
            }
            return false;
        }

        public bool NodeWasAddedToPopupMenu(int nodeid)
        {
            try
            {
                return PopupMenus.Select(x => x.Node.Id == nodeid).Count() > 0 ? true : false;
            }
            catch
            {
                return false;
            }
        }

        public PopupMenu GetPopupMenu(int id)
        {
            return PopupMenus.Single<PopupMenu>(x => x.Id == id);
        }

        public TaskPan GetTaskpanById(int tid)
        {
            return TaskPans.Single<TaskPan>(x => x.Id == tid);
        }
    }

}

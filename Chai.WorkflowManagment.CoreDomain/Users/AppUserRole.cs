
using System;
using System.Collections;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Chai.WorkflowManagment.CoreDomain.Users
{
    public class AppUserRole: IEntity
    {
        public int Id { get; set; }
        
        public virtual AppUser AppUser { get; set; }
        public virtual Role Role { get; set; }

        public AppUserRole()
        {
        }
    }
}


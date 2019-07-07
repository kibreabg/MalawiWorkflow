using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Project : IEntity
    {
        public Project()
        {
            this.ProGrants = new List<ProGrant>();
        }
        public int Id { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCode { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalBudget { get; set; }
        public decimal RemainingBudget { get; set; }
        public string Status { get; set; }
        public virtual IList<ProGrant> ProGrants { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual ProGrant GetProjectGrant(int Id)
        {

            foreach (ProGrant pr in ProGrants)
            {
                if (pr.Id == Id)
                    return pr;

            }
            return null;
        }
        public virtual IList<ProGrant> GetProjectGrantByProjectId(int ProjectId)
        {
            IList<ProGrant> Projects = new List<ProGrant>();
            foreach (ProGrant pr in ProGrants)
            {
                if (pr.Project.Id == ProjectId)
                    ProGrants.Add(pr);

            }
            return Projects;
        }
        public virtual void RemoveProjectGrant(int Id)
        {

            foreach (ProGrant pr in ProGrants)
            {
                if (pr.Id == Id)
                    ProGrants.Remove(pr);
                break;
            }

        }
    }
}
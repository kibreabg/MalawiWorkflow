using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ApprovalSetting : IEntity 
    {

       public ApprovalSetting()
        {
            this.ApprovalLevels = new List<ApprovalLevel>();
        }
       public int Id { get; set; }
       public string RequestType { get; set; }
       public string CriteriaCondition { get; set; }
       public int Value { get; set; }
       public Nullable<int> Value2 { get; set; }
       public string CriteriaQuery { get; set; }
       public Nullable<int> ApprovalLevel { get; set; }
       public virtual IList<ApprovalLevel> ApprovalLevels { get; set; }

       public virtual ApprovalLevel GetApprovalLevel(int Id)
       {

           foreach (ApprovalLevel pr in ApprovalLevels)
           {
               if (pr.Id == Id)
                   return pr;

           }
           return null;
       }
       public virtual IList<ApprovalLevel> GetApprovalLevelByApprovalId(int ApprovalId)
       {
           IList<ApprovalLevel> ApprovalLevels = new List<ApprovalLevel>();
           foreach (ApprovalLevel AR in ApprovalLevels)
           {
               if (AR.ApprovalSetting.Id == ApprovalId)
                   ApprovalLevels.Add(AR);

           }
           return ApprovalLevels;
       }
       public virtual void RemoveApprovalLevel(int Id)
       {

           foreach (ApprovalLevel AL in ApprovalLevels)
           {
               if (AL.Id == Id)
                   ApprovalLevels.Remove(AL);
               break;
           }

       }
       public virtual bool IsEmployeePositionExist(int PositionId)
       {
           bool result = false;
           foreach (ApprovalLevel isDetail in ApprovalLevels)
           {
               if (isDetail.EmployeePosition.Id == PositionId)
               {
                   result = true;
                   break;
               }
           }
           return result;
       }

    }
}

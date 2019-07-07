using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Request;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class Vehicle : IEntity
    {
        public int Id { get; set; }
        public string PlateNo { get; set; }
        public string Status { get; set; }
       
    }
}

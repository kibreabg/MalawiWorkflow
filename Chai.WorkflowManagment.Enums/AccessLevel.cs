using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Enums
{
    [Flags]
    public enum AccessLevel
    {
        /// <summary>
        /// 
        /// </summary>
        Anonymous = 1,
        /// <summary>
        /// 
        /// </summary>
        Authenticated = 2,
        /// <summary>
        /// 
        /// </summary>
        Editor = 4,
        /// <summary>
        /// 
        /// </summary>
        Administrator = 8
    }
}

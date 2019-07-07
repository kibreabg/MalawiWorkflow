﻿using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chai.WorkflowManagment.Modules.Setting.Views
{
    public interface IBankAccountView
    {
        IList<Account> BankAccounts { get; set; }
        string GetName { get; }
     
    }
}





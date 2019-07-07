using System;
using System.Collections.Generic;
//using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using System.Collections.Specialized;
using Chai.WorkflowManagment.Shared.Navigation;

namespace Chai.WorkflowManagment.Services
{
    public class SiteNullException : ApplicationException
    {
        public SiteNullException(string message)
            : base(message)
        {
        }
    }

    public class AccessForbiddenException : ApplicationException
    {
        public AccessForbiddenException(string message)
            : base(message)
        {
        }
    }

    public class NodeNullException : ApplicationException
    {
        public NodeNullException(string message)
            : base(message)
        {
        }
    }

    //[ConfigurationElementType(typeof(CustomHandlerData))]
    //public class RApplicationErrorHandler : IExceptionHandler
    //{
    //    public RApplicationErrorHandler(NameValueCollection ignore)
    //    {
    //    }
    //    #region IExceptionHandler Members

    //    public Exception HandleException(Exception exception, Guid handlingInstanceId)
    //    {
    //        RedirectNavigationService rd = new RedirectNavigationService();
    //        rd.Navigate("~/Errors/Error.aspx");
    //        return exception;
    //    }

    //    #endregion
    //}
}

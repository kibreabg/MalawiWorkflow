using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Shared
{
    public sealed class ExceptionUtility
    {
        private static readonly ILog ErrorLog = LogManager.GetLogger("ErrorLog");
        // All methods are static, so this can be private 
        private ExceptionUtility()
        { }

        // Log an Exception 
        public static void LogException(Exception exc, string source)
        {
            XmlConfigurator.Configure();
            // Include enterprise logic for logging exceptions 

            if (exc.InnerException != null)
            {
                ErrorLog.Error("Inner Exception Type: " + exc.InnerException.GetType().ToString());
                ErrorLog.Error("Inner Exception: " + exc.InnerException.Message);
                if (exc.InnerException.InnerException != null)
                    ErrorLog.Error("Second Level Exception: " + exc.InnerException.InnerException.Message);
                ErrorLog.Error("Inner Source: " + exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    ErrorLog.Error("Inner Stack Trace: " + exc.InnerException.StackTrace);
                }
            }
            ErrorLog.Error("Exception Type: " + exc.GetType().ToString());
            ErrorLog.Error("Exception: " + exc.Message);
            ErrorLog.Error("Source: " + source);
            if (exc.StackTrace != null)
            {
                ErrorLog.Error("Stack Trace: ");
                ErrorLog.Error(exc.StackTrace);
            }
        }

        // Notify System Operators about an exception 
        public static void NotifySystemOps(Exception exc)
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine("<b>Inner Exception</b> " + exc.InnerException + System.Environment.NewLine + "<b>Stacktrace</b> " + exc.StackTrace + System.Environment.NewLine + "<b>Source</b> " + exc.Source + System.Environment.NewLine + "  <b>Target Site</b>  " + exc.TargetSite);
            //EmailSender.SendEmails("Exception Detail", "supportwfms@clintonhealthaccess.org", "Exception Raised", exc.StackTrace);
            EmailSender.SendException("kgizatu@clintonhealthaccess.org,dhaddis@clintonhealthaccess.org", exc.Message, body.ToString());
        }
    }
}

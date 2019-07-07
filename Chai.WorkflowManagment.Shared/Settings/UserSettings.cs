using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Chai.WorkflowManagment.Shared.Settings
{
    public static class UserSettings
    {
        public static string GetReportPath
        {
            get { return UserConfig.GetConfiguration()["ReportDir"]; }
        }

        public static string GetTemplatePath
        {
            get { return UserConfig.GetConfiguration()["TemplateDir"]; }
        }

        public static string GetBackupPath
        {
            get { return UserConfig.GetConfiguration()["BackupDir"]; }
        }

        public static string GetMessageIcon
        {
            get { return UserConfig.GetConfiguration()["MessageIcon"]; }
        }
        public static string GetAdministratorRole
        {
            get { return UserConfig.GetConfiguration()["AdministratorRole"]; }
        }

        public static string GetSuperUser
        {
            get { return UserConfig.GetConfiguration()["SuperUser"]; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Oopes : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadError(Server.GetLastError()); 
    }
    protected void LoadError(Exception objError)
    {
        if (objError != null)
        {
            StringBuilder lasterror = new StringBuilder();

            if (objError.Message != null)
            {
                //lblError.Text = objError.Message;
                //lasterror.AppendLine("Message:");
                //lasterror.AppendLine(objError.Message);
                //lasterror.AppendLine();
            }

            if (objError.InnerException != null)
            {
                //lblInnerException.Text = objError.InnerException.ToString();
                //lasterror.AppendLine("InnerException:");
                //lasterror.AppendLine(objError.InnerException.ToString());
                //lasterror.AppendLine();
            }

            if (objError.Source != null)
            {
                //lblSource.Text = objError.Source;
                //lasterror.AppendLine("Source:");
                //lasterror.AppendLine(objError.Source);
                //lasterror.AppendLine();
            }

            if (objError.StackTrace != null)
            {
                //lblStackTrace.Text = objError.StackTrace;
                //lasterror.AppendLine("StackTrace:");
                //lasterror.AppendLine(objError.StackTrace);
                //lasterror.AppendLine();
            }

            ViewState.Add("LastError", lasterror.ToString());
            //lblError.Text = lasterror.ToString();
        }
    }

    //protected void btnReportError_Click(object sender, EventArgs e)
    //{
    //    SendEmail();
    //}

    //public void SendEmail()
    //{
    //    try
    //    {
    //        MailMessage msg = new MailMessage("webteam", "webteam");
    //        StringBuilder body = new StringBuilder();

    //        body.AppendLine("An unexcepted error has occurred.");
    //        body.AppendLine();

    //        body.AppendLine(ViewState["LastError"].ToString());

    //        msg.Subject = "Error";
    //        msg.Body = body.ToString();
    //        msg.IsBodyHtml = false;

    //        SmtpClient smtp = new SmtpClient("exchangeserver");
    //        smtp.Send(msg);
    //    }

    //    catch (Exception ex)
    //    {
    //        lblException.Text = ex.Message;
    //    }
    //}
}
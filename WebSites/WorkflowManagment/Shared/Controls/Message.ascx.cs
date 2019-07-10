using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Shared_Controls_Message : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public enum MessageType : int
    {
        Error = 0,
        Notice = 1,
        Information = 2,
        Success = 3
    }

    public void SetMessage(string sMessage, MessageType messageType)
    {


        if (messageType == MessageType.Notice)
        {
            pnlMessage.CssClass = "alert alert-warning fade in";
            litMessageType.Text = "Notice";
        }
        else if (messageType == MessageType.Information)
        {
            pnlMessage.CssClass = "alert alert-info fade in";
            litMessageType.Text = "Information";
        }
        else if (messageType == MessageType.Success)
        {
            pnlMessage.CssClass = "alert alert-success fade in";
            litMessageType.Text = "Success";
        }
        else if (messageType == MessageType.Error)
        {
            pnlMessage.CssClass = "alert alert-danger fade in";
            litMessageType.Text = "Error";
        }

        pnlMessage.Visible = (sMessage.Length > 0);
        litMessage.Text = sMessage;
        litMessage.Visible = true;
        litMessageType.Visible = true;
    }

    public void ClearMessage()
    {
        litMessage.Text = "";
        litMessageType.Text = "";
        pnlMessage.Visible = false;
    }
}
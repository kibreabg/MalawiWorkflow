using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Modules.Shell;
using Chai.WorkflowManagment.CoreDomain;
public partial class ChangePassword : POCBasePage
{
    private IWorkspace _workspace;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        _workspace = ZadsServices.Workspace;
        int UserId = Convert.ToInt32(HttpContext.Current.User.Identity.Name);
        AppUser user = _workspace.Single<AppUser>(x=> x.Id == UserId);// as AppUser;
             
        AdminServices userservices = new AdminServices();
        string Encryptedcurrentuser = Encryption.StringToMD5Hash(CurrentPassword.Text);

        try
        {
            if (Encryptedcurrentuser == user.Password)
            {
                user.Password = Chai.WorkflowManagment.CoreDomain.Users.AppUser.HashPassword(NewPassword.Text);
                _workspace.Update(user);
                _workspace.CommitChanges();
                Master.ShowMessage(new AppMessage("Password successfully Changed", Chai.WorkflowManagment.Enums.RMessageType.Info));
              
               
               
                
            }
            else
            {
                //this.panMessage.Attributes.Add("class", "response-msg error ui-corner-all");
                Master.ShowMessage(new AppMessage("Error: Please Enter the Correct old password", Chai.WorkflowManagment.Enums.RMessageType.Error));
                
               
            }
        }
        catch (Exception ex)
        {
            //this.panMessage.Attributes.Add("class", "response-msg error ui-corner-all");
            Master.ShowMessage(new AppMessage("Error: '"+ ex.Message + "'", Chai.WorkflowManagment.Enums.RMessageType.Error));
            
           
        }
    }
    
    protected void CancelPushButton0_Click1(object sender, EventArgs e)
    {
        Response.Redirect("~/UserLogIn.aspx");
    }
}

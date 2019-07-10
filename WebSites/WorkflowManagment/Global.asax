<%@ Application Language="C#" Inherits="Microsoft.Practices.CompositeWeb.WebClientApplication" %>
<script RunAt="server">

    private static readonly string ERROR_PAGE_LOCATION = "~/Oopes.aspx";
    
    void Application_Error(object sender, EventArgs e)
    {
        if (Context != null && Context.IsCustomErrorEnabled)
        {
            Server.Transfer(ERROR_PAGE_LOCATION, false);
        }

        // Code that runs when an unhandled error occurs

        // Get the exception object.
        Exception exc = Server.GetLastError();

        // Handle HTTP errors
        if (exc.GetType() == typeof(HttpException))
        {
            // The Complete Error Handling Example generates
            // some errors using URLs with "NoCatch" in them;
            // ignore these here to simulate what would happen
            // if a global.asax handler were not implemented.
            if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                return;

            //Redirect HTTP errors to HttpError page
            Server.Transfer(ERROR_PAGE_LOCATION, false);
        }

        // For other kinds of errors give the user some information
        // but stay on the default page
        Response.Write("<h2>Global Page Error</h2>\n");
        Response.Write(
            "<p>" + exc.Message + "</p>\n");
        Response.Write("Return to the <a href='Default.aspx'>" +
            "Default Page</a>\n");

        // Log the exception and notify system operators
        Chai.WorkflowManagment.Shared.ExceptionUtility.LogException(exc, "DefaultPage");
        Chai.WorkflowManagment.Shared.ExceptionUtility.NotifySystemOps(exc);

        // Clear the error from the server
        Server.ClearError();
    }
    
    
</script>
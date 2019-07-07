using System;
using System.Web;
using System.Collections.Generic;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.Util;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.Shared.Navigation;

namespace Chai.WorkflowManagment.Services
{
    public class ControllerBase
    {
        private IHttpContextLocatorService _httpContextLocatorService;
        private INavigationService _navigationService;

        private ControllerBase()
        {
        }

        public ControllerBase(IHttpContextLocatorService httpCLService, INavigationService navigationService)
        {
            _httpContextLocatorService = httpCLService;
            _navigationService = navigationService;
        }

        public AppUser GetCurrentUser()
        {
            return GetCurrentContext().User.Identity as AppUser;
        }

        public Node ActiveNode(string pageid)
        {
            return ZadsServices.AdminServices.ActiveNode(pageid);
        }

        public IHttpContext GetCurrentContext()
        {
            return _httpContextLocatorService.GetCurrentContext();
        }

        public bool UserIsAuthenticated
        {
            get { return _httpContextLocatorService.GetCurrentContext().User.Identity.IsAuthenticated; }
        }
        
        public void Navigate(string url)
        {
            _navigationService.Navigate(url);
        }

        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }

        public void SaveSearchReasult(object result, object criteria)
        {
            this.SearchResult = result;
            this.SearchCriteria = criteria;
        }
        /// <summary>
        /// stores search criteria
        /// </summary>
        public object SearchCriteria
        {
            get
            {
                return GetCurrentContext().Session["SEARCH_CRITERIA"];
            }
            private set
            {
                GetCurrentContext().Session["SEARCH_CRITERIA"] = value;
            }
        }

        /// <summary>
        /// It holdes search results in the session
        /// </summary>
        public object SearchResult
        {
            get
            {
                return GetCurrentContext().Session["SEARCH_RESULT"];
            }
            private set
            {
                GetCurrentContext().Session["SEARCH_RESULT"] = value;
            }
        }

        public string GetApplicationPath()
        {
            return TextUtil.EnsureTrailingSlash(GetCurrentContext().Request.ApplicationPath);
        }

        public string GetSiteUrl()
        {
            string path = GetCurrentContext().Request.ApplicationPath;

            if (path.EndsWith("/") && path.Length == 1)
            {
                return GetHostUrl();
            }
            else
            {
                return GetHostUrl() + path.ToLower();
            }
        }

        private string GetHostUrl()
        {
            string securePort = GetCurrentContext().Request.ServerVariables["SERVER_PORT_SECURE"];
            string protocol = securePort == null || securePort == "0" ? "http" : "https";
            string serverPort = GetCurrentContext().Request.ServerVariables["SERVER_PORT"];
            string port = serverPort == "80" ? string.Empty : ":" + serverPort;
            string serverName = GetCurrentContext().Request.ServerVariables["SERVER_NAME"];
            return string.Format("{0}://{1}{2}", protocol, serverName, port);
        }
    }
}

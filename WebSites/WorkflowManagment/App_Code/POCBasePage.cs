using System;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Modules.Shell;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.CoreDomain.Admins;

/// <summary>
/// Summary description for POCBasePage
/// </summary>
public class POCBasePage : Microsoft.Practices.CompositeWeb.Web.UI.Page, IPOCBasePage
{
    private ControllerBase _controller;
    private Node _node;
    private string _pageid;

	public POCBasePage()
	{

	}

    public void SetBasePageVAR(ControllerBase controller, string pageid)
    {
        _controller = controller;
        _pageid = pageid;
    }

    public virtual string PageID
    {
        get { throw new NotImplementedException(); }
    }
    
    public BaseMaster BMaster
    {
        get { return (BaseMaster)this.Master; }
    }

    public Node ActiveNode
    {
        get
        {
            if(_node == null)
            _node = _controller.ActiveNode(_pageid);

            return _node;
        }
    }
    
    public string TabId
    {
        get { return Request.QueryString[AppConstants.TABID]; }
    }

    public virtual void ApplyAutorizationRule()
    {
        throw new NotImplementedException();
    }
}
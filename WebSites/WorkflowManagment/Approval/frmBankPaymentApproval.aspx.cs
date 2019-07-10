using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Modules.Approval.Views;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.Shared.MailSender;
using log4net;
using log4net.Config;
using Microsoft.Practices.ObjectBuilder;

namespace Chai.WorkflowManagment.Modules.Approval.Views
{
    public partial class frmBankPaymentApproval : POCBasePage, IBankPaymentApprovalView
    {
        private BankPaymentApprovalPresenter _presenter;
        private static readonly ILog Log = LogManager.GetLogger("AuditTrailLog");
        private int reqID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
                XmlConfigurator.Configure();
                PopProgressStatus();
            }
            this._presenter.OnViewLoaded();
            BindSearchBankPaymentRequestGrid();
            if (_presenter.CurrentBankPaymentRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                PrintTransaction();
            }
        }
        [CreateNew]
        public BankPaymentApprovalPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }
        public override string PageID
        {
            get
            {
                return "{F5FE9AB4-0AF8-432F-92B4-DFA2EAECE42B}";
            }
        }
        #region Field Getters
        public int GetBankPaymentRequestId
        {
            get
            {
                if (grvBankPaymentRequestList.SelectedDataKey != null)
                {
                    return Convert.ToInt32(grvBankPaymentRequestList.SelectedDataKey.Value);
                }
                else if (reqID != 0)
                {
                    return reqID;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion
        private void PopApprovalStatus()
        {
            string[] s = Enum.GetNames(typeof(ApprovalStatus));

            for (int i = 0; i < s.Length; i++)
            {
                if (GetWillStatus().Substring(0, 2) == s[i].Substring(0, 2))
                {
                    ddlApprovalStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                }

            }
            ddlApprovalStatus.Items.Add(new ListItem(ApprovalStatus.Rejected.ToString().Replace('_', ' '), ApprovalStatus.Rejected.ToString().Replace('_', ' ')));

        }
        private string GetWillStatus()
        {
            ApprovalSetting AS = _presenter.GetApprovalSettingforProcess(RequestType.BankPayment_Request.ToString().Replace('_', ' ').ToString(), 0);
            string will = "";
            foreach (ApprovalLevel AL in AS.ApprovalLevels)
            {
                if (AL.EmployeePosition.PositionName == "Superviser/Line Manager" || AL.EmployeePosition.PositionName == "Program Manager" && _presenter.CurrentBankPaymentRequest.CurrentLevel == 1)
                {
                    will = "Approve";

                }
                else if (_presenter.GetUser(_presenter.CurrentBankPaymentRequest.CurrentApprover).EmployeePosition.PositionName == AL.EmployeePosition.PositionName)
                {
                    will = AL.Will;
                }

            }
            return will;
        }
        private void PopProgressStatus()
        {
            string[] s = Enum.GetNames(typeof(ProgressStatus));

            for (int i = 0; i < s.Length; i++)
            {
                ddlSrchProgressStatus.Items.Add(new ListItem(s[i].Replace('_', ' '), s[i].Replace('_', ' ')));
                ddlSrchProgressStatus.DataBind();
            }
        }
        private void BindSearchBankPaymentRequestGrid()
        {
            grvBankPaymentRequestList.DataSource = _presenter.ListBankPaymentRequests(txtSrchRequestNo.Text, txtSrchRequestDate.Text, ddlSrchProgressStatus.SelectedValue);
            grvBankPaymentRequestList.DataBind();
        }
        private void BindBankPaymentRequestStatus()
        {
            foreach (BankPaymentRequestStatus BPRS in _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses)
            {
                //if (BPRS.Approver == _presenter.CurrentUser().Id && BPRS.WorkflowLevel == _presenter.CurrentBankPaymentRequest.CurrentLevel && _presenter.CurrentBankPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                //{
                //    btnApprove.Enabled = true;
                //}
                //else
                //    btnApprove.Enabled = false;
                if (_presenter.CurrentBankPaymentRequest.CurrentLevel == _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses.Count && BPRS.ApprovalStatus != null)
                {
                    btnPrint.Enabled = true;
                    btnApprove.Enabled = false;
                }
                else
                    btnPrint.Enabled = false;
            }
        }
        private void ShowPrint()
        {
            if (_presenter.CurrentBankPaymentRequest.CurrentLevel == _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses.Count && _presenter.CurrentBankPaymentRequest.ProgressStatus == ProgressStatus.Completed.ToString())
            {
                btnPrint.Enabled = true;
            }
        }
        private void SendEmail(BankPaymentRequestStatus BPRS)
        {
            if (_presenter.GetUser(BPRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(_presenter.GetUser(BPRS.Approver).Email, "Bank Payment Approval", "'" + (_presenter.CurrentBankPaymentRequest.AppUser.FullName).ToUpper() + "' Requests for payment");
            }
            else
            {
                EmailSender.Send(_presenter.GetUser(_presenter.GetAssignedJobbycurrentuser(BPRS.Approver).AssignedTo).Email, "Bank Payment Approval", (_presenter.CurrentBankPaymentRequest.AppUser.FullName).ToUpper() + "' Requests for Payment Request No. '" + (_presenter.CurrentBankPaymentRequest.RequestNo).ToUpper() + "'");
            }

        }
        private void SendEmailRejected(BankPaymentRequestStatus BPRS)
        {
            EmailSender.Send(_presenter.GetUser(_presenter.CurrentBankPaymentRequest.AppUser.Id).Email, "Bank Payment Request Rejection", "'" + "' Your Payment Request with request no. '" + (_presenter.CurrentBankPaymentRequest.RequestNo.ToString()).ToUpper() + "' was Rejected for this reason '" + (BPRS.RejectedReason).ToUpper() + "'");

            if (BPRS.WorkflowLevel > 1)
            {
                for (int i = 0; i + 1 < BPRS.WorkflowLevel; i++)
                {
                    EmailSender.Send(_presenter.GetUser(_presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses[i].Approver).Email, "Bank Payment Request Rejection", "'" + "' Bank Payment Request with request no. '" + (_presenter.CurrentBankPaymentRequest.RequestNo.ToString()).ToUpper() + "' made by " + (_presenter.GetUser(_presenter.CurrentBankPaymentRequest.AppUser.Id).FullName).ToUpper() + " was Rejected for this reason - '" + (BPRS.RejectedReason).ToUpper() + "'");
                }
            }
        }
        private void GetNextApprover()
        {
            foreach (BankPaymentRequestStatus BPRS in _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses)
            {
                if (BPRS.ApprovalStatus == null)
                {
                    SendEmail(BPRS);
                    _presenter.CurrentBankPaymentRequest.CurrentApprover = BPRS.Approver;
                    _presenter.CurrentBankPaymentRequest.CurrentLevel = BPRS.WorkflowLevel;
                    _presenter.CurrentBankPaymentRequest.CurrentStatus = BPRS.ApprovalStatus;
                    _presenter.CurrentBankPaymentRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                    break;
                }
            }
        }
        private void SaveBankPaymentRequestStatus()
        {
            foreach (BankPaymentRequestStatus BPRS in _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses)
            {
                if ((BPRS.Approver == _presenter.CurrentUser().Id || _presenter.CurrentUser().Id == (_presenter.GetAssignedJobbycurrentuser(BPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(BPRS.Approver).AssignedTo : 0 )) && BPRS.WorkflowLevel == _presenter.CurrentBankPaymentRequest.CurrentLevel)
                {
                    BPRS.ApprovalStatus = ddlApprovalStatus.SelectedValue;
                    BPRS.RejectedReason = txtRejectedReason.Text;
                    BPRS.Date = DateTime.Now;
                    BPRS.AssignedBy = _presenter.GetAssignedJobbycurrentuser(BPRS.Approver) != null ? _presenter.GetAssignedJobbycurrentuser(BPRS.Approver).AppUser.FullName : "";
                    if (BPRS.ApprovalStatus != ApprovalStatus.Rejected.ToString())
                    {
                        if (_presenter.CurrentBankPaymentRequest.CurrentLevel == _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses.Count)
                        {
                            _presenter.CurrentBankPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        }
                        GetNextApprover();
                        BPRS.Approver = _presenter.CurrentUser().Id;
                        Log.Info(_presenter.GetUser(BPRS.Approver).FullName + " has " + BPRS.ApprovalStatus + " Bank Payment Request made by " + _presenter.CurrentBankPaymentRequest.AppUser.FullName);
                    }
                    else
                    {
                        _presenter.CurrentBankPaymentRequest.ProgressStatus = ProgressStatus.Completed.ToString();
                        BPRS.Approver = _presenter.CurrentUser().Id;
                        SendEmailRejected(BPRS);
                        Log.Info(_presenter.GetUser(BPRS.Approver).FullName + " has " + BPRS.ApprovalStatus + " Bank Payment Request made by " + _presenter.CurrentBankPaymentRequest.AppUser.FullName);
                    }
                }
                break;
            }
        }
        protected void grvBankPaymentRequestList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                reqID = (int)grvBankPaymentRequestList.DataKeys[Convert.ToInt32(e.CommandArgument)].Value;
                _presenter.CurrentBankPaymentRequest = _presenter.GetBankPaymentRequest(reqID);
                _presenter.OnViewLoaded();
                dgBankPaymentRequestDetail.DataSource = _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails;
                dgBankPaymentRequestDetail.DataBind();
                pnlDetail.Visible = true;
            }
        }
        protected void grvBankPaymentRequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void grvBankPaymentRequestList_SelectedIndexChanged(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
            PopApprovalStatus();
            //grvAttachments.DataSource = _presenter.CurrentBankPaymentRequest.CPRAttachments;
            //grvAttachments.DataBind();
            btnApprove.Enabled = true;
            BindBankPaymentRequestStatus();
            pnlApproval_ModalPopupExtender.Show();
        }
        protected void grvBankPaymentRequestList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBankPaymentRequestList.PageIndex = e.NewPageIndex;
            btnFind_Click(sender, e);
        }
        protected void grvStatuses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (_presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses != null)
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].Text = _presenter.GetUser(_presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses[e.Row.RowIndex].Approver).FullName;
                }
            }
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindSearchBankPaymentRequestGrid();
        }
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (_presenter.CurrentBankPaymentRequest.ProgressStatus != ProgressStatus.Completed.ToString())
                {
                    SaveBankPaymentRequestStatus();
                    _presenter.SaveOrUpdateBankPaymentRequest(_presenter.CurrentBankPaymentRequest);
                    ShowPrint();
                    Master.ShowMessage(new AppMessage("Bank Payment Request Successfully Approved!", Chai.WorkflowManagment.Enums.RMessageType.Info));
                    btnApprove.Enabled = false;
                    BindSearchBankPaymentRequestGrid();
                    pnlApproval_ModalPopupExtender.Show();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void PrintTransaction()
        {

            lblRequesterResult.Text = _presenter.CurrentBankPaymentRequest.AppUser.FullName;
            lblRequestedDateResult.Text = _presenter.CurrentBankPaymentRequest.ProcessDate.ToString();
            lblRequestNoResult.Text = _presenter.CurrentBankPaymentRequest.RequestNo.ToString();
            lblApprovalStatusResult.Text = _presenter.CurrentBankPaymentRequest.ProgressStatus.ToString();
            lblpaytypeRes.Text = _presenter.CurrentBankPaymentRequest.PaymentMethod;
            grvDetails.DataSource = _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails;
            grvDetails.DataBind();

            grvStatuses.DataSource = _presenter.CurrentBankPaymentRequest.BankPaymentRequestStatuses;
            grvStatuses.DataBind();
        }
        protected void btnCancelPopup2_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }
        public string GetMimeTypeByFileName(string sFileName)
        {
            string sMime = "application/octet-stream";

            string sExtension = System.IO.Path.GetExtension(sFileName);
            if (!string.IsNullOrEmpty(sExtension))
            {
                sExtension = sExtension.Replace(".", "");
                sExtension = sExtension.ToLower();

                if (sExtension == "xls" || sExtension == "xlsx")
                {
                    sMime = "application/ms-excel";
                }
                else if (sExtension == "doc" || sExtension == "docx")
                {
                    sMime = "application/msword";
                }
                else if (sExtension == "ppt" || sExtension == "pptx")
                {
                    sMime = "application/ms-powerpoint";
                }
                else if (sExtension == "rtf")
                {
                    sMime = "application/rtf";
                }
                else if (sExtension == "zip")
                {
                    sMime = "application/zip";
                }
                else if (sExtension == "mp3")
                {
                    sMime = "audio/mpeg";
                }
                else if (sExtension == "bmp")
                {
                    sMime = "image/bmp";
                }
                else if (sExtension == "gif")
                {
                    sMime = "image/gif";
                }
                else if (sExtension == "jpg" || sExtension == "jpeg")
                {
                    sMime = "image/jpeg";
                }
                else if (sExtension == "png")
                {
                    sMime = "image/png";
                }
                else if (sExtension == "tiff" || sExtension == "tif")
                {
                    sMime = "image/tiff";
                }
                else if (sExtension == "txt")
                {
                    sMime = "text/plain";
                }
            }

            return sMime;
        }
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlApprovalStatus.SelectedValue == "Rejected")
            {
                lblRejectedReason.Visible = true;
                txtRejectedReason.Visible = true;
            }
            else
            {
                lblRejectedReason.Visible = false;
                txtRejectedReason.Visible = false;
            }
            pnlApproval_ModalPopupExtender.Show();
        }
        public static void CreateCSV<T>(IList<T> list, string csvNameWithExt)
        {
            if (list == null || list.Count == 0) return;

            string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = pathDesktop + "\\" + csvNameWithExt;

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            //get type from 0th member
            Type t = list[0].GetType();
            string newLine = Environment.NewLine;

            using (var sw = new StreamWriter(filePath))
            {
                //make a new instance of the class name we figured out to get its props
                object o = Activator.CreateInstance(t);
                //gets all properties
                PropertyInfo[] props = o.GetType().GetProperties();

                //foreach of the properties in class above, write out properties
                //this is the header row
                foreach (PropertyInfo pi in props)
                {
                    if (pi.Name != "Account" || pi.Name.ToUpper() != "BANKPAYMENTREQUEST")
                        sw.Write(pi.Name.ToUpper() + ",");
                }
                sw.Write(newLine);

                //this acts as datarow
                foreach (T item in list)
                {
                    //this acts as datacolumn
                    foreach (PropertyInfo pi in props)
                    {
                        if (pi.Name != "Account" || pi.Name.ToUpper() != "BANKPAYMENTREQUEST")
                        {
                            BankPaymentRequestDetail detail = (BankPaymentRequestDetail)item.GetType()
                                                     .GetProperty(pi.Name)
                                                     .GetValue(item, null);
                            //this is the row+col intersection (the value)
                            string whatToWrite =
                                Convert.ToString(item.GetType()
                                                     .GetProperty(pi.Name)
                                                     .GetValue(item, null))
                                    .Replace(',', ' ') + ',';

                            sw.Write(whatToWrite);
                        }

                    }
                    sw.Write(newLine);
                }
            }
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            IList<BankPaymentRequestDetail> export = _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails;
            CreateCSV(export, "PaynetExport.csv");

            //string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string filePath = pathDesktop + "\\Paynet Export.csv";

            //if (!File.Exists(filePath))
            //{
            //    File.Create(filePath).Close();
            //}
            //string delimter = ",";
            //List<string[]> output = new List<string[]>();
            //IList<BankPaymentRequestDetail> export = _presenter.CurrentBankPaymentRequest.BankPaymentRequestDetails;

            ////flexible part ... add as many object as you want based on your app logic
            //output.Add(new string[] { "TEST1", "TEST2" });
            //output.Add(new string[] { "TEST3", "TEST4" });

            //int length = output.Count;

            //using (System.IO.TextWriter writer = File.CreateText(filePath))
            //{

            //    for (int index = 0; index < length; index++)
            //    {
            //        writer.WriteLine(string.Join(delimter, export[index].ToString()));
            //    }
            //}
        }
    }
}
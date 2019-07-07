using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Chai.WorkflowManagment.Shared;
using System.Configuration;
namespace Chai.WorkflowManagment.CoreDomain.DataAccess
{
    public class ReportDao
    {
        public DataSet LeaveReport(int EmployeeName, int LeaveType)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPLeaveReport";
                cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName);
                cmd.Parameters.AddWithValue("@LeaveType", LeaveType);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet PurchaseReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPPurchaseReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet VehicleReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPVehicleReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet LiquidationReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPLiquidationReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet TravelAdvanceReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPTravelAdvanceReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet CashPaymentReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPCashPaymentReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet CostSharingPaymentReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPCostSharingPaymentReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet BankPaymentPaymentReport(string datefrom, string dateto)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPBankPaymentReport";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet ExportCostSharingPayment(string datefrom, string dateto, string ExportType)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPExportCostSharingPayment";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                cmd.Parameters.AddWithValue("@ExportType", ExportType);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet ExportBankPayment(string datefrom, string dateto,string ExportType)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPExportBankPayment";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                cmd.Parameters.AddWithValue("@ExportType", ExportType);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet ExportCashPayment(string datefrom, string dateto, string ExportType)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPExportCashPayment";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                cmd.Parameters.AddWithValue("@ExportType", ExportType);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet ExportTravelAdvance(string datefrom, string dateto, string ExportType)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPExportTravelAdvance";
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                cmd.Parameters.AddWithValue("@ExportType", ExportType);
                
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
        public DataSet ExportLiquidationReport(int LiquidationId)
        {
            string connstring = ConfigurationManager.ConnectionStrings["WorkflowManagmentReportConnectionString"].ToString();
            using (SqlConnection cn = new SqlConnection(connstring))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SPExportLiquidationReport";
                cmd.Parameters.AddWithValue("@LiquidationId", LiquidationId);
                var da = new SqlDataAdapter(cmd);
                var ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }
    }
}

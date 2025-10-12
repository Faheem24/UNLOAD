using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UNLOADAPI.Models;

namespace UNLOADAPI.Controllers
{
    public class LogsController : ApiController
    {
        private SqlConnection _Conn;
        //private SqlDataAdapter _Adapter;

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return "value";
        }

        public void Post([FromBody]CreateLogs value)
        {
            string EventDetail;
            string LogonDetail;
            if (value.EventID == true)
            {
                EventDetail = "Failed Login";
            }
            else
            {
                EventDetail = "Successful Login";
            }
            if (value.LogonType == true)
            {
                LogonDetail = "Remote";
            }
            else
            {
                LogonDetail = "Interactive";
            }
            _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;user id=sa;password=sa123");
            //DataTable DT = new DataTable();
            var query = "INSERT INTO Logs (AssetIP, UserAccount, DateAndTime, EventID, SourceIP, LogonType," +
                " EventDetail, LogonDetail) VALUES (@AssetIP, @UserAccount, @DateAndTime, @EventID, @SourceIP," +
                " @LogonType, @EventDetail, @LogonDetail)";
            SqlCommand InsertCommand = new SqlCommand(query, _Conn);
            InsertCommand.Parameters.AddWithValue("@AssetIP", value.AssetIP);
            InsertCommand.Parameters.AddWithValue("@UserAccount", value.UserAccount);
            InsertCommand.Parameters.AddWithValue("@DateAndTime", value.DateAndTime);
            InsertCommand.Parameters.AddWithValue("@EventID", value.EventID);
            InsertCommand.Parameters.AddWithValue("@SourceIP", value.SourceIP);
            InsertCommand.Parameters.AddWithValue("@LogonType", value.LogonType);
            InsertCommand.Parameters.AddWithValue("@EventDetail", EventDetail);
            InsertCommand.Parameters.AddWithValue("@LogonDetail", LogonDetail);
            _Conn.Open();
            int Result = InsertCommand.ExecuteNonQuery();
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
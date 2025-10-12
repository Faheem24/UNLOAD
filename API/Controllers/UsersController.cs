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
    public class UsersController : ApiController
    {
        private SqlConnection _Conn;
        private SqlDataAdapter _Adapter;

        public IEnumerable<Users> Get()
        {
            _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;user id=sa;password=sa123");
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Users";
            _Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, _Conn)
            };
            _Adapter.Fill(DT);
            List<Users> UsersList = new List<Models.Users>(DT.Rows.Count);
            if (DT.Rows.Count > 0)
            {
                foreach (DataRow Row in DT.Rows)
                {
                    UsersList.Add(new ReadUsers(Row));
                }
            }
            return UsersList;
        }

        // GET api/<controller>/5
        public bool Get(string UserAccount)
        {
            _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;user id=sa;password=sa123");
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Users WHERE UserAccount = '" + UserAccount + "'";
            _Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, _Conn)
            };
            _Adapter.Fill(DT);
            //List<Asset> AssetList = new List<Models.Asset>(DT.Rows.Count);
            Users U = new Users();
            if (DT.Rows.Count > 0)
            {
                U = new ReadUsers(DT.Rows[0]);
                return true;
            }
            else
            {
                return false;
            }
        }

        // POST api/<controller>
        public void Post([FromBody]CreateUsers value)
        {
            if (this.Get(value.UserAccount)==false)
            {
                _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;" +
                    "user id=sa;password=sa123");
                DataTable DT = new DataTable();
                var query = "INSERT INTO Users (UserAccount) VALUES (@UserAccount)";
                SqlCommand InsertCommand = new SqlCommand(query, _Conn);
                InsertCommand.Parameters.AddWithValue("@UserAccount", value.UserAccount);
                _Conn.Open();
                int Result = InsertCommand.ExecuteNonQuery();
            }
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
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
    public class AssetController : ApiController
    {
        private SqlConnection _Conn;
        private SqlDataAdapter _Adapter;

        public IEnumerable<Asset> Get()
        {
            _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;user id=sa;password=sa123");
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Asset";
            _Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, _Conn)
            };
            _Adapter.Fill(DT);
            List<Asset> AssetList = new List<Models.Asset>(DT.Rows.Count);
            if (DT.Rows.Count > 0)
            {
                foreach (DataRow Row in DT.Rows)
                {
                    AssetList.Add(new ReadAsset(Row));
                }
            }
            return AssetList;
        }

        // GET api/<controller>/5
        public bool Get(string AssetIP)
        {
            _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;user id=sa;password=sa123");
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Asset WHERE AssetIP = '" + AssetIP + "'";
            _Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, _Conn)
            };
            _Adapter.Fill(DT);
            //List<Asset> AssetList = new List<Models.Asset>(DT.Rows.Count);
            Asset A = new Asset();
            if (DT.Rows.Count > 0)
            {
                A = new ReadAsset(DT.Rows[0]);
                return true;
                /*foreach (DataRow Row in DT.Rows)
                {
                    AssetList.Add(new ReadAsset(Row));
                }*/
            }
            else
            {
                return false;
            }
            //return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]CreateAsset value)
        {
            if (this.Get(value.AssetIP)==false)
            {
                _Conn = new SqlConnection("data source=DESKTOP-4PR0T2J;Initial catalog=UNLOADDB;" +
                    "user id=sa;password=sa123");
                DataTable DT = new DataTable();
                var query = "INSERT INTO Asset (AssetIP) VALUES (@AssetIP)";
                SqlCommand InsertCommand = new SqlCommand(query, _Conn);
                InsertCommand.Parameters.AddWithValue("@AssetIP", value.AssetIP);
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
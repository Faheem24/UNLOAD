using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UNLOADAPI.Models
{
    public class Logs
    {
        public int LogID { get; set; }
        public string AssetIP { get; set; }
        //public int AlertID { get; set; }
        public string UserAccount { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool EventID { get; set; }
        public string SourceIP { get; set; }
        public bool LogonType { get; set; }
    }

    public class CreateLogs : Logs
    {
    }

    public class ReadLogs : Logs
    {
        public ReadLogs(DataRow Row)
        {
            LogID = Convert.ToInt32(Row["LogID"]);
            AssetIP = Row["AssetIP"].ToString();
            UserAccount = Row["UserAccount"].ToString();
            DateAndTime = Convert.ToDateTime(Row["DateAndTime"]);
            EventID = Convert.ToBoolean(Row["EventID"]);
            SourceIP = Row["SourceIP"].ToString();
            LogonType = Convert.ToBoolean(Row["LogonType"]);
        }

        public int LogID { get; set; }
        public string AssetIP { get; set; }
        public string UserAccount { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool EventID { get; set; }
        public string SourceIP { get; set; }
        public bool LogonType { get; set; }
    }
}
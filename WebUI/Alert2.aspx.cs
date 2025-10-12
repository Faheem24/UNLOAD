using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace UNLOAD_Web_UI
{
    public partial class Alert2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-4PR0T2J;Initial Catalog=UNLOADDB;" +
                "Integrated Security=True");
            PasswordGuess(con);
            NonOfficeHoursLogin(con);
            RemoteLogin(con);
            UpdateAnalyzed(con);
        }

        public void PasswordGuess(SqlConnection Conn)
        {
            int Attempts;
            String CurrentUser;
            DateTime LogDateTime;
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Logs WHERE EventID = 1 AND Analyzed IS NULL ORDER BY DateAndTime ASC;";
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, Conn)
            };
            Adapter.Fill(DT);
            foreach (DataRow Row in DT.Rows)
            {
                CurrentUser = Row["UserAccount"].ToString();
                LogDateTime = (DateTime)Row["DateAndTime"];
                query = "SELECT * FROM PasswordGuess WHERE UserAccount = '" + CurrentUser + "';";
                Adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, Conn)
                };
                DataTable UserDT = new DataTable();
                Adapter.Fill(UserDT);
                Attempts = Convert.ToInt32(UserDT.Rows[0]["InvalidAttempts"]);

                query = "SELECT * FROM Logs WHERE EventID = 1 AND UserAccount = '" + CurrentUser + "' ORDER BY DateAndTime DESC;";
                Adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, Conn)
                };
                DataTable LogDT = new DataTable();
                Adapter.Fill(LogDT);
                int count = 0;
                foreach (DataRow LogRow in LogDT.Rows)
                {
                    DateTime CheckDateTime = (DateTime)LogRow["DateAndTime"];
                    if (CheckMin(LogDateTime, CheckDateTime))
                    {
                        count++;
                    }
                }
                if (count >= Attempts)
                {
                    //Generate Alert
                    query = "INSERT INTO Alert (AlertType, AlertDetail) VALUES ('a', 'Password Guess');";
                    SqlCommand cmd = new SqlCommand(query, Conn);
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                    UpdateAlertID(Conn, Row["LogID"].ToString());
                }
            }
        }

        public bool CheckMin(DateTime Log, DateTime Check)
        {
            if (Check > Log)
            {
                return false;
            }
            else
            {
                TimeSpan TS = Log - Check;
                if (TS.TotalMinutes <= 1)
                {
                    return true;
                }
            }
            return false;
        }

        public void NonOfficeHoursLogin(SqlConnection Conn)
        {
            String CurrentUser;// = "";
            TimeSpan StartTime;// = new TimeSpan();
            TimeSpan EndTime;// = new TimeSpan();
            TimeSpan LogTime;// = new TimeSpan();
            String LogDateTime;// = "";
            DataTable DT = new DataTable();
            var query = "SELECT * FROM Logs WHERE EventID = 0 AND Analyzed IS NULL ORDER BY DateAndTime ASC;";
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, Conn)
            };
            Adapter.Fill(DT);
            foreach (DataRow Row in DT.Rows)
            {
                LogDateTime = Row["DateAndTime"].ToString();
                LogTime = DateTime.Parse(Row["DateAndTime"].ToString()).TimeOfDay;
                CurrentUser = Row["UserAccount"].ToString();
                query = "SELECT * FROM OfficeHours WHERE UserAccount = '" + CurrentUser + "';";
                Adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, Conn)
                };
                DataTable UserDT = new DataTable();
                Adapter.Fill(UserDT);
                StartTime = TimeSpan.Parse(UserDT.Rows[0]["StartTime"].ToString());
                EndTime = TimeSpan.Parse(UserDT.Rows[0]["EndTime"].ToString());
                if (!((LogTime > StartTime) && (LogTime < EndTime)))
                {
                    //Generate Alert
                    query = "INSERT INTO Alert (AlertType, AlertDetail) VALUES ('c'," +
                        " 'Non Office Hours Login');";
                    SqlCommand cmd = new SqlCommand(query, Conn);
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                    UpdateAlertID(Conn, Row["LogID"].ToString());
                }
            }
        }

        public void RemoteLogin(SqlConnection Conn)
        {
            string SrcIP;
            string DstIP;
            string StartIP;
            string EndIP;
            DataTable DT = new DataTable();
            //String LogDateTime;// = "";
            var query = "SELECT * FROM Logs WHERE EventID = 0 AND LogonType = 1 AND Analyzed IS NULL" +
                " ORDER BY DateAndTime ASC;";
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, Conn)
            };
            Adapter.Fill(DT);
            foreach (DataRow Row in DT.Rows)
            {
                //LogDateTime = Row["DateAndTime"].ToString();
                DstIP = Row["AssetIP"].ToString();
                query = "SELECT * FROM InternalSubnet WHERE ASSETIP = '" + DstIP + "';";
                Adapter = new SqlDataAdapter
                {
                    SelectCommand = new SqlCommand(query, Conn)
                };
                DataTable IPDT = new DataTable();
                Adapter.Fill(IPDT);
                StartIP = IPDT.Rows[0]["StartIP"].ToString();
                EndIP = IPDT.Rows[0]["EndIP"].ToString();
                SrcIP = Row["SourceIP"].ToString();
                if (CheckSubnet(StartIP, EndIP, SrcIP) == false)
                {
                    //Generate Alert
                    query = "INSERT INTO Alert (AlertType, AlertDetail) VALUES ('b', 'Outside Remote Login');";
                    SqlCommand cmd = new SqlCommand(query, Conn);
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                    Conn.Close();
                    UpdateAlertID(Conn, Row["LogID"].ToString());
                }
            }
        }

        public bool CheckSubnet(string StartIP, string EndIP, string CheckIP)
        {
            string[] StartIPString = StartIP.Split('.');
            string[] EndIPString = EndIP.Split('.');
            string[] CheckIPString = CheckIP.Split('.');

            int[] SIP = new int[4];
            int[] EIP = new int[4];
            int[] CIP = new int[4];
            for (int i = 0; i < 4; i++)
            {
                SIP[i] = Convert.ToInt32(StartIPString[i]);
                EIP[i] = Convert.ToInt32(EndIPString[i]);
                CIP[i] = Convert.ToInt32(CheckIPString[i]);
            }
            if (CheckNumber(SIP[0], EIP[0], CIP[0]) && CheckNumber(SIP[1], EIP[1], CIP[1]) &&
                CheckNumber(SIP[2], EIP[2], CIP[2]) && CheckNumber(SIP[3], EIP[3], CIP[3]))
            {
                return true;
            }
            return false;
        }

        public bool CheckNumber(int start, int end, int check)
        {
            if ((check == start) || (check == end))
            {
                return true;
            }
            else
            {
                if ((check > start) && (check < end))
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateAnalyzed(SqlConnection Conn)
        {
            var query = "UPDATE Logs SET Analyzed = 1 WHERE Analyzed IS NULL;";
            SqlCommand cmd = new SqlCommand(query, Conn);
            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }

        public void UpdateAlertID(SqlConnection Conn, string LogID)
        {
            string AID = "";
            DataTable DT = new DataTable();
            var query = "SELECT TOP 1 * FROM Alert ORDER BY AlertID DESC;";
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, Conn)
            };
            Adapter.Fill(DT);
            AID = DT.Rows[0]["AlertID"].ToString();

            query = "UPDATE Logs SET ALERTID = " + AID + " WHERE LogID = " + LogID + ";";
            SqlCommand cmd = new SqlCommand(query, Conn);
            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }
    }
}
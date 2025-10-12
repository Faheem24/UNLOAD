using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Generic;

namespace SADAgentService
{
    public partial class Service1 : ServiceBase
    {
        public class Logs
        {
            public string AssetIP { get; set; }
            public string UserAccount { get; set; }
            public DateTime DateAndTime { get; set; }
            public bool EventID { get; set; }
            public string SourceIP { get; set; }
            public bool LogonType { get; set; }
        }

        public class Asset
        {
            public string AssetIP { get; set; }
        }

        public class Users
        {
            public string UserAccount { get; set; }
        }

        System.Timers.Timer timer = new System.Timers.Timer();
        DateTime Now;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ExecuteTasks();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 60000; //number in milisecinds  
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            ExecuteTasks();
        }

        public void ExecuteTasks()
        {
            Now = DateTime.Now;

            EventLog[] EventLogs;
            EventLogEntry[] SecurityEventLogs;

            EventLogs = EventLog.GetEventLogs();

            foreach (EventLog log in EventLogs)
            {
                if (log.LogDisplayName == "Security")
                {
                    //Get Security Event Log
                    SecurityEventLogs = new EventLogEntry[log.Entries.Count];
                    log.Entries.CopyTo(SecurityEventLogs, 0);

                    for (int i = SecurityEventLogs.Length; i > 0; i--)
                    {
                        //Check 1 minute condition
                        TimeSpan Interval = Now - SecurityEventLogs[i - 1].TimeGenerated;
                        if ((Interval.Hours == 0) && (Interval.Minutes < 1))
                        {
                            //WriteToFile(SecurityEventLogs[i - 1]);
                            //The current event happen before 60 seconds
                            if (SecurityEventLogs[i - 1].InstanceId == 4624)
                            {
                                if (CheckRelevant4624(SecurityEventLogs[i - 1]))
                                {
                                    //Successful Login
                                    //WriteToFile(SecurityEventLogs[i - 1]);
                                    SendToServer(SecurityEventLogs[i - 1]);
                                }
                            }
                            if (SecurityEventLogs[i - 1].InstanceId == 4776)
                            {
                                if (CheckRelevant4625(SecurityEventLogs[i - 1]))
                                {
                                    //Failed Login
                                    //WriteToFile(SecurityEventLogs[i - 1]);
                                    SendToServer(SecurityEventLogs[i - 1]);
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public void GetSetAsset(string IPAddress)
        {
            Asset Asst = new Asset
            {
                AssetIP = IPAddress
            };
            //List<Asset> AssetList = new List<Asset>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://192.168.50.1:44318/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, 
                    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                /*var ResponseTask = client.GetAsync("api/Asset");
                ResponseTask.Wait();
                var Result = ResponseTask.Result;
                if (Result.IsSuccessStatusCode)
                {
                    var ReadTask = Result.Content.ReadAsStringAsync();
                    ReadTask.Wait();
                    string AssetData = ReadTask.Result;
                    JavaScriptSerializer JSserializer = new JavaScriptSerializer();
                    AssetList = JSserializer.Deserialize<List<Asset>>(AssetData);
                }*/
                var PostTask = client.PostAsync("api/Asset", new StringContent(new JavaScriptSerializer()
                    .Serialize(Asst), Encoding.UTF8, "application/json"));
                PostTask.Wait();
                var result = PostTask.Result;
            };
        }

        public void GetSetUsers(string UserAcc)
        {
            Users Usr = new Users
            {
                UserAccount = UserAcc
            };
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://192.168.50.1:44318/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, 
                    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                var PostTask = client.PostAsync("api/Users", new StringContent(new JavaScriptSerializer()
                    .Serialize(Usr), Encoding.UTF8, "application/json"));
                PostTask.Wait();
                var result = PostTask.Result;
            };
        }

        public void SendToServer(EventLogEntry Log)
        {
            //Get System IP Address
            string IPAddr = "";
            string HostName = Dns.GetHostName();
            IPHostEntry IPEntry = Dns.GetHostEntry(HostName);
            IPAddress[] IPAddresses = IPEntry.AddressList;
            foreach (IPAddress IP in IPAddresses)
            {
                if ((IP.IsIPv6LinkLocal == false) && (IP.AddressFamily.ToString() == "InterNetwork"))
                {
                    IPAddr = IP.ToString();
                    break;
                }
            }

            string UAcc;
            GetSetAsset(IPAddr);
            if (Log.InstanceId == 4624)
            {
                UAcc = Log.ReplacementStrings[5];
            }
            else
            {
                UAcc = Log.ReplacementStrings[1];
            }
            GetSetUsers(UAcc);

            bool EvtID;
            bool LType = false;
            string SrcIP = IPAddr;
            if (Log.InstanceId == 4624)
            {
                EvtID = false;
                if (Log.ReplacementStrings[8] == "3")
                {
                    SrcIP = Log.ReplacementStrings[18];
                    LType = true;
                }
            }
            else
            {
                EvtID = true;
                /*if (Log.ReplacementStrings[10] == "3")
                {
                    SrcIP = Log.ReplacementStrings[19];
                    LType = true;
                }*/
            }

            Logs log = new Logs
            {
                AssetIP = IPAddr,
                UserAccount = UAcc,
                DateAndTime = Log.TimeGenerated,
                EventID = EvtID,
                SourceIP = SrcIP,
                LogonType = LType
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://192.168.50.1:44318/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, 
                    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                var PostTask = client.PostAsync("api/Logs", new StringContent(new JavaScriptSerializer()
                    .Serialize(log), Encoding.UTF8, "application/json"));
                PostTask.Wait();
                var result = PostTask.Result;
            }
        }

        public void SendToDatabase(EventLogEntry Log)
        {
            //
            //var path = @"C:\Checkpoints.txt";
            //

            //Get System IP Address
            string IPAddr = "";
            string HostName = Dns.GetHostName();
            IPHostEntry IPEntry = Dns.GetHostEntry(HostName);
            IPAddress[] IPAddresses = IPEntry.AddressList;
            foreach (IPAddress IP in IPAddresses)
            {
                if (IP.IsIPv6LinkLocal == false)
                {
                    IPAddr = IP.ToString();
                    break;
                }
            }
            //string IPAddr = "192.168.50.10";
            //
            //File.AppendAllText(path, IPAddr);
            //File.AppendAllText(path, "|");
            //

            //Make Database Connection
            //string ConnectionString = ConfigurationManager.ConnectionStrings["SADAgentService.Properties.Settings.Setting"].ConnectionString;
            string ConnectionString = ConfigurationManager.ConnectionStrings["SADAgentService.Properties.Settings.Setting2"].ConnectionString;
            SqlConnection Connection = new SqlConnection(ConnectionString);
            Connection.Open();

            //
            //File.AppendAllText(path, Connection.ServerVersion);
            //File.AppendAllText(path, "|");
            //

            //Check if LocalHost IP is present in Asset Table
            string CheckAssetQuery = "SELECT [AssetIP] FROM [dbo].[Asset] WHERE [AssetIP] = @IPAddress";
            SqlCommand CheckAssetCommand = new SqlCommand(CheckAssetQuery, Connection);
            CheckAssetCommand.Parameters.AddWithValue("@IPAddress", IPAddr);
            SqlDataReader CheckAssetReader = CheckAssetCommand.ExecuteReader();

            if (CheckAssetReader.HasRows == false)
            {
                //Asset IP does not exist. Insert into Asset
                string InsertAssetQuery = "INSERT INTO [dbo].[Asset] ([AssetIP]) VALUES (@IPAddress)";
                SqlCommand InsertAssetCommand = new SqlCommand(InsertAssetQuery, Connection);
                InsertAssetCommand.Parameters.AddWithValue("@IPAddress", IPAddr);
                int InsertedAsset = InsertAssetCommand.ExecuteNonQuery();

                //Get Asset IP Again
                CheckAssetReader = CheckAssetCommand.ExecuteReader();
            }

            //
            //File.AppendAllText(path, CheckAssetReader["AssetIP"].ToString());
            //File.AppendAllText(path, "|");
            //

            //Get UserAccount
            string CheckUserQuery = "SELECT [UserAccount] FROM [dbo].[Users] WHERE [UserAccount] = @UserAcc";
            SqlCommand CheckUserCommand = new SqlCommand(CheckUserQuery, Connection);
            CheckUserCommand.Parameters.AddWithValue("@UserAcc", Log.ReplacementStrings[5]);
            SqlDataReader CheckUserReader = CheckUserCommand.ExecuteReader();

            if (CheckUserReader.HasRows == false)
            {
                //UserAccount does not exist. Inert into Users
                string InsertUserQuery = "INSERT INTO [dbo].[Users] ([UserAccount]) VALUES (@UserAcc)";
                SqlCommand InsertUserCommand = new SqlCommand(InsertUserQuery, Connection);
                InsertUserCommand.Parameters.AddWithValue("@UserAcc", Log.ReplacementStrings[5]);
                int InsertedUser = InsertUserCommand.ExecuteNonQuery();

                //Get UserAccount Again
                CheckUserReader = CheckUserCommand.ExecuteReader();
            }

            //
            //File.AppendAllText(path, CheckUserReader["UserAccount"].ToString());
            //File.AppendAllText(path, "|");
            //

            //Insert into Log
            CheckAssetReader.Read();
            CheckUserReader.Read();
            string AssetIP = CheckAssetReader["AssetIP"].ToString();
            string UserAccount = CheckUserReader["UserAccount"].ToString();
            DateTime DateAndTime = Log.TimeGenerated;
            bool EventID;
            if (Log.InstanceId == 4624)
            {
                EventID = false;
            }
            else
            {
                EventID = true;
            }
            string SourceIP = null;
            bool LogonType = false;
            if (Log.ReplacementStrings[8] == "10")
            {
                //Get Source IP
                LogonType = true;
            }
            
            string InsertLogQuery = "INSERT INTO [dbo].[Logs] ([AssetIP], [UserAccount], [DateAndTime], [EventID], [SourceIP], [LogonType]) VALUES (@AssetIP, @UserAccount, @DateAndTime, @EventID, @SourceIP, @LogonType)";
            SqlCommand InsertLogCommand = new SqlCommand(InsertLogQuery, Connection);
            InsertLogCommand.Parameters.AddWithValue("@AssetIP", AssetIP);
            InsertLogCommand.Parameters.AddWithValue("@UserAccount", UserAccount);
            InsertLogCommand.Parameters.AddWithValue("@DateAndTime", DateAndTime);
            InsertLogCommand.Parameters.AddWithValue("@EventID", EventID);
            InsertLogCommand.Parameters.AddWithValue("@SourceIP", SourceIP);
            InsertLogCommand.Parameters.AddWithValue("@LogonType", LogonType);
            int InsertedLog = InsertLogCommand.ExecuteNonQuery();

            //
            //File.AppendAllText(path, InsertedLog.ToString());
            //File.AppendAllText(path, "|");
            //
            
            Connection.Close();
        }

        public void WriteToFile(EventLogEntry Log)
        {
            var path = @"C:\Logs.txt";
            //string text = Log.TimeWritten + "|" + Log.InstanceId + "\n";

            string datetime = "DateTime: " + Log.TimeGenerated + "\n";
            string EventID = "Event ID: " + Log.InstanceId + "\n";
            string UserAccount = "UserAccount: " + Log.ReplacementStrings[5] + "\n";
            string LogonType = "LogonType: " + Log.ReplacementStrings[8] + "\n";
            string MachineName = "MachineName: " + Log.MachineName + "\n";

            File.AppendAllText(path, datetime);
            File.AppendAllText(path, EventID);
            File.AppendAllText(path, UserAccount);
            File.AppendAllText(path, LogonType);
            File.AppendAllText(path, MachineName);

            /*for (int i=0; i<Log.ReplacementStrings.Length; i++)
            {
                string str = i.ToString() + " | " + Log.ReplacementStrings[i].ToString() + "\n";
                File.AppendAllText(path, str);
            }*/

            //File.AppendAllText(path, text);
        }

        public bool CheckRelevant4624(EventLogEntry Log)
        {
            //No SYSTEM User
            if (Log.ReplacementStrings[5] != "SYSTEM")
            {
                //Logon Type is 2 (Interactive) OR 3 (Network)
                if ((Log.ReplacementStrings[8] == "2") || (Log.ReplacementStrings[8] == "3"))
                {
                    //Remove Other Users
                    if ((Log.ReplacementStrings[5].StartsWith("DWM") == false) && (Log.ReplacementStrings[5].StartsWith("UMFD") == false))
                    {
                        if (Log.ReplacementStrings[26] == "%%1843")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool CheckRelevant4625(EventLogEntry Log)
        {
            //No SYSTEM User
            /*if (Log.ReplacementStrings[5] != "SYSTEM")
            {
                //Logon Type is 2 (Interactive) OR 10 (Terminal Services)
                if ((Log.ReplacementStrings[10] == "2") || (Log.ReplacementStrings[10] == "10"))
                {
                    return true;
                }
            }*/
            return true;
        }
    }
}

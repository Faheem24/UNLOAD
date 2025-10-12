using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UNLOAD_Web_UI
{
    public partial class Config2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Clear1_Click(object sender, EventArgs e)
        {
            UserName1.Text = "";
            Attempts1.Text = "";
        }

        protected void Clear2_Click(object sender, EventArgs e)
        {
            UserName2.Text = "";
            Start2.Text = "";
            End2.Text = "";
        }

        protected void Clear3_Click(object sender, EventArgs e)
        {
            Asset3.Text = "";
            Start3.Text = "";
            End3.Text = "";
        }

        protected void Update1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-4PR0T2J;Initial Catalog=UNLOADDB;" +
                "Integrated Security=True");
            string CheckQuery = "SELECT * FROM PasswordGuess WHERE UserAccount = '" + UserName1.Text + "';";
            DataTable DT = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(CheckQuery, con)
            };
            Adapter.Fill(DT);
            if (DT.Rows.Count > 0)
            {
                //Old User. Update User
                string UpdateQuery = "UPDATE PasswordGuess SET InvalidAttempts = " + Attempts1.Text + " WHERE UserAccount = '" + UserName1.Text + "';";
                SqlCommand cmd = new SqlCommand(UpdateQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                //New User. Add new Row
                string InsertQuery = "INSERT INTO PasswordGuess (UserAccount, InvalidAttempts) VALUES ('"+ UserName1.Text + "', " + Attempts1.Text + ");";
                SqlCommand cmd = new SqlCommand(InsertQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void Update2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-4PR0T2J;Initial Catalog=UNLOADDB;" +
                "Integrated Security=True");
            string CheckQuery = "SELECT * FROM OfficeHours WHERE UserAccount = '" + UserName2.Text + "';";
            DataTable DT = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(CheckQuery, con)
            };
            Adapter.Fill(DT);
            if (DT.Rows.Count > 0)
            {
                //Old User. Update User
                string UpdateQuery = "UPDATE OfficeHours SET StartTime = " + Start2.Text + " AND EndTime = " + End2.Text + " WHERE UserAccount = '" + UserName2.Text + "';";
                SqlCommand cmd = new SqlCommand(UpdateQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                //New User. Add new Row
                string InsertQuery = "INSERT INTO OfficeHours (UserAccount, StartTime, EndTime) VALUES ('" + UserName2.Text + "', " + Start2.Text + ", " + End2.Text + ");";
                SqlCommand cmd = new SqlCommand(InsertQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void Update3_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=DESKTOP-4PR0T2J;Initial Catalog=UNLOADDB;" +
                "Integrated Security=True");
            string CheckQuery = "SELECT * FROM InternalSubnet WHERE AssetIP = '" + Asset3.Text + "';";
            DataTable DT = new DataTable();
            SqlDataAdapter Adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(CheckQuery, con)
            };
            Adapter.Fill(DT);
            if (DT.Rows.Count > 0)
            {
                //Old User. Update User
                string UpdateQuery = "UPDATE InternalSubnet SET StartIP = '" + Start3.Text + "' AND EndIP = '" + End3.Text + "' WHERE AssetIP = '" + Asset3.Text + "';";
                SqlCommand cmd = new SqlCommand(UpdateQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                //New User. Add new Row
                string InsertQuery = "INSERT INTO InternalSubnet (AssetIP, StartIP, EndIP) VALUES ('" + Asset3.Text + "', '" + Start3.Text + "', '" + End3.Text + "');";
                SqlCommand cmd = new SqlCommand(InsertQuery, con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
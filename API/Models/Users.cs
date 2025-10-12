using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UNLOADAPI.Models
{
    public class Users
    {
        public string UserAccount { get; set; }
    }

    public class CreateUsers : Users
    {
    }

    public class ReadUsers : Users
    {
        public ReadUsers(DataRow Row)
        {
            UserAccount = Row["UserAccount"].ToString();
        }

        public string UserAccount { get; set; }
    }
}
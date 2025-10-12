using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace UNLOADAPI.Models
{
    public class Asset
    {
        public string AssetIP { get; set; }
    }

    public class CreateAsset : Asset
    {
    }

    public class ReadAsset : Asset
    {
        public ReadAsset(DataRow Row)
        {
            AssetIP = Row["AssetIP"].ToString();
        }

        public string AssetIP { get; set; }
    }
}
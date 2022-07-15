using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
//using Oracle.DataAccess.Client;
//using System.Data.OracleClient;

namespace skyinfyMVC.Models
{
    public class Connection
    {
        public OracleConnection fCon;
        public Connection(string key, String CoCd)
        {
            fCon = null;
            if (fCon == null || fCon.State != ConnectionState.Open)
            {
                fCon = new OracleConnection(Multiton.connString(CoCd));
                try
                {
                    fCon.Open();
                }
                catch (Exception err)
                {
                    fCon = null;

                }
            }
        }
       

    }
}
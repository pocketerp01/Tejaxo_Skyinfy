

using Oracle.ManagedDataAccess.Client;
using skyinfyMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

/// <summary>
/// Summary description for sqltransaction
/// </summary>
public class Satransaction
{
    sgenFun sgen;
    private OracleConnection myconn1;

    //MySqlTransaction transaction = null;
    OracleTransaction transaction = null;

    //private MySqlConnection myconn1;

    //MySqlCommand cmd;
    OracleCommand cmd;

    public Satransaction(string userCode, string Myguid)
    {
        //Multiton multiton = Multiton.GetInstance(Myguid);
        sgen = new sgenFun(Myguid);
        System.Environment.SetEnvironmentVariable("ORA_NCHAR_LITERAL_REPLACE", "TRUE");
        //myconn1 = new MySqlConnection(sgen.connStringmysql(userCode));
        myconn1 = new OracleConnection(Multiton.connString(userCode));
        myconn1.Open();
        //myconn1 = multiton.OConn;

        transaction = myconn1.BeginTransaction(IsolationLevel.Serializable);
        //cmd = new MySqlCommand();
        cmd = new OracleCommand();
        cmd.Connection = myconn1;
        cmd.Transaction = transaction;
    }

    public bool Execute_cmd(string command)
    {
        bool data = true;
        try
        {

            cmd.CommandText = command;
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            //((WebViewPage)WebPageContext.Current.Page).ViewBag.scripCall += ex.Message;
            data = false;
        }
        return data;
    }
    public string Execute_cmd(string command, string hh)
    {
        string data = "1";
        try
        {
            cmd.CommandText = command;
            data = cmd.ExecuteNonQuery().ToString();
        }
        catch (Exception ex) { data = ex.Message; }
        return data;
    }

    public void Commit()
    {
        transaction.Commit();
        transaction.Dispose();
        myconn1.Close();
    }

    public void Rollback()
    {
        transaction.Rollback();
        myconn1.Close();
    }
}
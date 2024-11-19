using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DataConectionHandler : MonoBehaviour
{
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    private MD5 _md5Hash;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = host;
        builder.Database = database;
        builder.UserID = user;
        builder.Password = password;
        //if (pooling)
        //{
        //    connectionString += "True";
        //}
        //else
        //{
        //    connectionString += "False";
        //}
        try
        {
            using (con = new MySqlConnection(builder.ToString()))
            {

                con.Open();
                Debug.Log("Mysql state: " + con.State);


                string sql = "SELECT customerid FROM customers";
                cmd = new MySqlCommand(sql, con);
                //			string sql = "SELECT * FROM clothes";
                //			cmd = new MySqlCommand(sql, con);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Debug.Log("???");
                    Debug.Log(rdr[0] );
                }
                rdr.Close();
            }

        }
        catch (MySqlException e)
        {
            Debug.Log(e.Message);
        }
    }
    void onApplicationQuit()
    {
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
            {
                con.Close();
                Debug.Log("Mysql connection closed");
            }
            con.Dispose();
        }
    }

    public string getFirstShops()
    {
        using (rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                return rdr[0] + " -- " + rdr[1];
            }
        }
        return "empty";
    }
    public string GetConnectionState()
    {
        return con.State.ToString();
    }
}

using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using System.IO;

class DataBaseSc:MonoBehaviour
{
    string DBname = "DATABASE";
    public string LastName, FirstName, Phone, Password;
    public void WriteUser()//string LastName,string FirstName,string Phone,string Password
    {
        string conn = SetDataBaseClass.SetDataBase(DBname + ".db");
        IDbConnection dbconn;
        IDbCommand dbcmd;
        IDataReader reader;

        dbconn=new SqliteConnection(conn);
        dbconn.Open();
        dbcmd = dbconn.CreateCommand();
        string sqlQuery = "Insert into Users(LastName,FirstName,Phone,Password) "+
                            "Values('"+LastName+"','" + FirstName + "','" + Phone + "','" + Password + "')";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()){       }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null; 
        dbconn.Close();
        dbconn = null;
    }

}

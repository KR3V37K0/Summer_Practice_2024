using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using System.IO;

public class DataBaseSc : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    string conn = SetDataBaseClass.SetDataBase("DATABASE.db");
    IDbConnection dbconn;
    IDbCommand dbcmd;
    IDataReader reader;


    void OpenConnection()
    {
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        dbcmd = dbconn.CreateCommand();
    }
    void CloseConnection()
    {
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    public void WriteUser(string Name, string Phone, string Password)//string LastName,string FirstName,string Phone,string Password
    {
        OpenConnection();

        string sqlQuery = "Insert into Users(Name,Phone,Password) "+
                            "Values('" + Name + "','" + Phone + "','" + Password + "')";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()){       }

        CloseConnection();
    }
    public void FindUser(string phone)
    {     
        OpenConnection();
        string sqlQuery="Select id,Name,Password,Card FROM Users Where Phone='"+phone+"'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) 
        {
            if (reader.GetString(2) != null)
            {
                ScManager.User.id = reader.GetInt32(0);
                ScManager.User.Name = reader.GetString(1);
                ScManager.User.Phone = phone;
                ScManager.User.Pass = reader.GetString(2);
                ScManager.User.Card = reader.GetString(3);
            }
        }
        CloseConnection();
    }

}

using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.EnterpriseServices;
using static Unity.Burst.Intrinsics.X86;
using System.Xml.Linq;

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
    public void WriteUser(string Name, string Phone, string Password,string Card,int Ava,int A)
    {
        OpenConnection();

        string sqlQuery = "Insert into Users(Name,Phone,Password,Card,idAvatar,A) "+
                            "Values('" + Name + "','" + Phone + "','" + Password + "','"+Card+ "','" + Ava + "','" + A+"')";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()){       }

        CloseConnection();
    }
    public void FindUser(string phone)
    {     
        OpenConnection();
        string sqlQuery="Select id,Name,Password,Card,idAvatar,A FROM Users Where Phone='"+phone+"'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) 
        {
            if (reader.GetInt32(0)>0)
            {
                dbUser u = new dbUser();

                ScManager.User.id = reader.GetInt32(0);
                ScManager.User.Name = reader.GetString(1);
                ScManager.User.Phone = phone;
                ScManager.User.Pass = reader.GetString(2);
                ScManager.User.Card = reader.GetString(3);
                ScManager.User.Ava = reader.GetInt32(4);
                ScManager.User.A = reader.GetInt32(5);

            }
        }
        CloseConnection();
    }
    public void ChangeUserPass(int id,string newPass)
    {
        OpenConnection();

        string sqlQuery = "UPDATE Users SET Password = '"+newPass+"' WHERE id = '"+id+"'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }

        CloseConnection();
    }
    public void ChangeUserData(int id, string Name, int Ava, string Phone)
    {
        OpenConnection();

        string sqlQuery = "UPDATE Users SET Name = '" + Name + "', idAvatar = '" + Ava + "', Phone = '" + Phone + "' WHERE id = '" + id + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }

        CloseConnection();
    }
    public void DeleteUser(int id)
    {
        OpenConnection();

        string sqlQuery = "DELETE FROM Users WHERE id='"+id+"'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }

        CloseConnection();
    }

    public class dbUser
    {
        public int id;
        public string Name;
        public string Phone;
        public string Pass;
        public string Card;
        public int Ava;
        public int A;
    }
}


using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.EnterpriseServices;
using static Unity.Burst.Intrinsics.X86;
using System.Xml.Linq;
using System.Collections.Generic;

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

    //USER
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
   
    //BOOKS
    public dbBook[] bookBest(int top)
    {
        OpenConnection();
        string sqlQuery = "SELECT id,Date,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books ORDER BY Rating DESC";
        //SELECT name, product_count FROM products ORDER BY product_count ASC
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        dbBook[] b = new dbBook[top];
        int i = 0;
        while (reader.Read()) 
        {
            
            if (i == b.Length) break;
            b[i] = new dbBook();
            //Debug.Log(reader.GetInt32(0));
            //Debug.Log(reader.GetString(20));
            
            b[i].id=reader.GetInt32(0);
            b[i].Date = reader.GetString(1);
            b[i].Cover = reader.GetInt32(2);
            b[i].Cost = reader.GetInt32(3);
            b[i].Age = reader.GetInt32(4);
            b[i].Tom = reader.GetInt32(5);
            b[i].Name = reader.GetString(6);
            b[i].Author = reader.GetString(7);
            b[i].Provider = reader.GetString(8);
            b[i].Description = reader.GetString(9);
            b[i].Images = reader.GetString(10);
            b[i].Genre = reader.GetString(11);
            b[i].Rating = reader.GetFloat(12);
            b[i].Series = reader.GetString(13);

            i++;
        }        
        CloseConnection();
        return b;
    }
    public dbBook FindBook(int id)
    {
        OpenConnection();
        string sqlQuery = "SELECT id,Date,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books Where id='" + id + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        dbBook b = new dbBook();
        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                b.id = reader.GetInt32(0);
                b.Date = reader.GetString(1);
                b.Cover = reader.GetInt32(2);
                b.Cost = reader.GetInt32(3);
                b.Age = reader.GetInt32(4);
                b.Tom = reader.GetInt32(5);
                b.Name = reader.GetString(6);
                b.Author = reader.GetString(7);
                b.Provider = reader.GetString(8);
                b.Description = reader.GetString(9);
                b.Images = reader.GetString(10);
                b.Genre = reader.GetString(11);
                b.Rating = reader.GetFloat(12);
                b.Series = reader.GetString(13);

            }
        }
        CloseConnection();
        return b;
    }



    //CHECK ALL
    public List<int> GetAllID(int idUser,string table)
    {
        List<int> books = new List<int>();
        OpenConnection();
        string sqlQuery = "Select idBook FROM "+table+" Where idUser='" + idUser + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {

                books.Add(reader.GetInt32(0));
            }
        }
        CloseConnection();
        return books;
    }
    public bool BookIs(int idBook, int idUser, string table)
    {
        OpenConnection();
        string sqlQuery = "Select idBook,id User FROM "+table+" Where idUser='" + idUser + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                if (reader.GetInt32(0) == idBook) { CloseConnection(); return true; }
            }
        }
        CloseConnection();
        return false;
    }
    public void AddBookTo(int idBook, int idUser, string table)
    {
        OpenConnection();
        string sqlQuery = "INSERT INTO "+table+"(idBook, idUser) VALUES(" + idBook + "," + idUser + ")";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }
        CloseConnection();
    }
    public void DeleteBookFrom(int idBook, int idUser,string table)
    {
        OpenConnection();

        string sqlQuery = "DELETE FROM "+table+" WHERE idBook=" + idBook + " AND idUser=" + idUser;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }
        CloseConnection();
    }

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
public class dbBook
{
    public int id, Cover, Cost, Age, Tom;
    public string Name, Author, Provider, Description, Images, Genre, Date,Series;
    public float Rating;

}


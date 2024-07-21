using UnityEngine;
using System.Data;
using Mono.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.EnterpriseServices;
using static Unity.Burst.Intrinsics.X86;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Threading;
using UnityEngine.UIElements;
using Unity.VisualScripting;

//Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
//CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

public class DataBaseSc : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    string conn = SetDataBaseClass.SetDataBase("DATABASE.db");
    IDbConnection dbconn;
    IDbCommand dbcmd;
    IDataReader reader;
    
    private void Start()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }
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
    public void SetCard(int id, string card)
    {
        OpenConnection();
        string sqlQuery = "UPDATE Users SET Card = '" + card + "' WHERE id = '" + id + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }
        CloseConnection();
    }
   
    //BOOKS
    public dbBook[] bookBest(int top)
    {
        OpenConnection();
        string sqlQuery = "SELECT id,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books ORDER BY Rating DESC";
        //SELECT name, product_count FROM products ORDER BY product_count ASC
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        dbBook[] b = new dbBook[top];
        int i = 0;
        while (reader.Read()) 
        {
            
            if (i == b.Length) break;
            b[i] = new dbBook();
            b[i].id=reader.GetInt32(0);
            b[i].day = reader.GetInt32(1);
            b[i].month = reader.GetInt32(2);
            b[i].year = reader.GetInt32(3);
            b[i].Cover = reader.GetInt32(4);
            b[i].Cost = reader.GetInt32(5);
            b[i].Age = reader.GetInt32(6);
            b[i].Tom = reader.GetInt32(7);
            b[i].Name = reader.GetString(8);
            b[i].Author = reader.GetString(9);
            b[i].Provider = reader.GetString(10);
            b[i].Description = reader.GetString(11);
            b[i].Images = reader.GetString(12);
            b[i].Genre = reader.GetString(13);
            b[i].Rating = reader.GetFloat(14);
            b[i].Series = reader.GetString(15);

            i++;
        }        
        CloseConnection();
        return b;
    }
    public dbBook[] bookNew(int top)
    {
        //Debug.Log("start");
        OpenConnection();
        string sqlQuery = "SELECT ID,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books ORDER BY year DESC,month DESC,day DESC";
        //SELECT name, product_count FROM products ORDER BY product_count ASC
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        dbBook[] b = new dbBook[top];
        int i = 0;
        while (reader.Read())
        {
            //Debug.Log("work "+i);
            //Debug.Log("length " + b.Length);
            //Debug.Log(reader.GetInt32(0));
            if (i == b.Length) {  break; }
            b[i] = new dbBook();
            b[i].id = reader.GetInt32(0);         
            b[i].day = reader.GetInt32(1);
            b[i].month = reader.GetInt32(2);
            b[i].year = reader.GetInt32(3);
            b[i].Cover = reader.GetInt32(4);
            b[i].Cost = reader.GetInt32(5);
            b[i].Age = reader.GetInt32(6);
            b[i].Tom = reader.GetInt32(7);
            b[i].Name = reader.GetString(8);
            b[i].Author = reader.GetString(9);
            b[i].Provider = reader.GetString(10);
            b[i].Description = reader.GetString(11);
            b[i].Images = reader.GetString(12);
            b[i].Genre = reader.GetString(13);
            b[i].Rating = reader.GetFloat(14);
            b[i].Series = reader.GetString(15);
            //Debug.Log("complete " + i);
            i++;
        }
        CloseConnection();
        //Debug.Log("close " + i);
        return b;
    }
    public dbBook FindBook(int id)
    {
        OpenConnection();
        string sqlQuery = "SELECT id,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books Where id='" + id + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        dbBook b = new dbBook();
        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                b.id = reader.GetInt32(0);
                b.day = reader.GetInt32(1);
                b.month = reader.GetInt32(2);
                b.year = reader.GetInt32(3);
                b.Cover = reader.GetInt32(4);
                b.Cost = reader.GetInt32(5);
                b.Age = reader.GetInt32(6);
                b.Tom = reader.GetInt32(7);
                b.Name = reader.GetString(8);
                b.Author = reader.GetString(9);
                b.Provider = reader.GetString(10);
                b.Description = reader.GetString(11);
                b.Images = reader.GetString(12);
                b.Genre = reader.GetString(13);
                b.Rating = reader.GetFloat(14);
                b.Series = reader.GetString(15);

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

    //ADRESS
    public List<dbAdress> GetAllAdress()
    {
        List<dbAdress> adresses = new List<dbAdress>();
        OpenConnection();
        string sqlQuery = "Select id,Adress,Day FROM DeliveryPoints ORDER BY Adress";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                dbAdress adress = new dbAdress();
                adress.id = reader.GetInt32(0);
                adress.Adress = reader.GetString(1);
                adress.Time = reader.GetInt32(2);
                adresses.Add(adress);
            }
        }
        CloseConnection();
        return adresses;
    }
    public int AdressTime(int idAdress)
    {
        OpenConnection();
        string sqlQuery = "Select Day FROM DeliveryPoints Where id="+idAdress;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        int Time = 0;
        while (reader.Read())
        {
            Time = reader.GetInt32(0);
        }
        CloseConnection();
        return Time;
    }
    public string GetAdress(int idAdress)
    {
        OpenConnection();
        string sqlQuery = "Select Adress FROM DeliveryPoints Where id=" + idAdress;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        string adress = "";
        while (reader.Read())
        {
            adress = reader.GetString(0);
        }
        CloseConnection();
        return adress;
    }

    //BUYING
    public void Buy(List<dbBook> books,int day, int month, int year, string Adress,int idUser)
    {
        foreach(dbBook book in books)
        {
            OpenConnection();           
            string sqlQuery = "INSERT INTO Buyed(idBook, idUser,day,month,year,idAdress) VALUES(" + book.id + "," + idUser + "," + day + "," + month + "," + year + "," + Adress+ ")";
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read()) { }
            CloseConnection();
        }
    }
    public List<dbBuyed> GetAllBuyedID(int idUser)
    {
        List<dbBuyed> books = new List<dbBuyed>();
        OpenConnection();
        string sqlQuery = "Select id,idUser,idBook,day,month,year,idAdress FROM Buyed Where idUser='" + idUser + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                dbBuyed book = new dbBuyed();
                book.id = reader.GetInt32(0);
                book.idUser =reader.GetInt32(1);
                book.idBook = reader.GetInt32(2);
                book.day = reader.GetInt32(3);
                book.month = reader.GetInt32(4);
                book.year = reader.GetInt32(5);
                book.idAdress = reader.GetInt32(6);
                books.Add(book);
            }
        }
        CloseConnection();
        return books;
    }

    //RATYING
    public void addRating(int idBook,int idUser,int Rating)
    {
        bool rate=isRate(idBook,idUser);
        OpenConnection();
        if (rate)
        {
            string sqlQuery = "UPDATE Rating SET Rating = '" + Rating + "' WHERE idUser = '" + idUser + "'" + " AND idBook=" + idBook;
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read()) { }
        }
        else
        {
            string sqlQuery = "INSERT INTO Rating(idBook, idUser,Rating) VALUES(" + idBook + "," + idUser + "," + Rating + ")";
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read()) { }
        }
        CloseConnection();
        recalculateRating(idBook);
    }
    public bool isRate(int idBook, int idUser)
    {
        OpenConnection();
        string sqlQuery = "Select idUser,idBook FROM Rating Where idUser='" + idUser + "'" + " AND idBook=" +idBook;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                CloseConnection();
                return true;
            }
        }
        CloseConnection();
        return false;
    }
    public void recalculateRating(int idBook)
    {
        OpenConnection();
        string sqlQuery = "Select Rating FROM Rating Where idBook='" + idBook + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        double count = 0;
        double summRate = 0;

        while (reader.Read())
        {
            count++;
            summRate += reader.GetInt32(0);
        }
        CloseConnection();
        double f = summRate / count;
        OpenConnection();

        sqlQuery = "UPDATE Books SET Rating = '" + f + "' WHERE id = '" + idBook + "'";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read()) { }

        CloseConnection();
    }
    public int GetRating(int idBook, int idUser)
    {
        int score = 0;
        OpenConnection();
        string sqlQuery = "Select Rating FROM Rating Where idUser='" + idUser + "'" + " AND idBook=" + idBook;
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            score= reader.GetInt32(0);
        }
        CloseConnection();
        return score;
    }

    //SEARCH
    public List<string> easySearch(string txt)
    {
        OpenConnection();
        string sqlQuery = "SELECT id,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books Where Name LIKE '%" + txt + "%'";
        //SELECT* FROM bus_station WHERE  bus_key_query LIKE '%2%';
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        List<dbBook> books = new List<dbBook>();
        while (reader.Read())
        {
            if (reader.GetInt32(0) > 0)
            {
                dbBook b = new dbBook();
                b.id = reader.GetInt32(0);
                b.day = reader.GetInt32(1);
                b.month = reader.GetInt32(2);
                b.year = reader.GetInt32(3);
                b.Cover = reader.GetInt32(4);
                b.Cost = reader.GetInt32(5);
                b.Age = reader.GetInt32(6);
                b.Tom = reader.GetInt32(7);
                b.Name = reader.GetString(8);
                b.Author = reader.GetString(9);
                b.Provider = reader.GetString(10);
                b.Description = reader.GetString(11);
                b.Images = reader.GetString(12);
                b.Genre = reader.GetString(13);
                b.Rating = reader.GetFloat(14);
                b.Series = reader.GetString(15);
                books.Add(b);
            }
        }
        CloseConnection();
        List<string> list = unicue(books);
        return list;
    }
    public List<dbBook> Search(string txt)
    {
        List<dbBook> books = new List<dbBook>();
        OpenConnection();
        if (txt.Contains("::"))
        {
            string command = txt.Substring(0,txt.IndexOf("::"));
            string contain = txt.Substring(txt.IndexOf("::")+2);
            switch (command)
            {
                case "Æàíð":
                    command = "Genre";
                    break;
                case "Àâòîð":
                    command = "Author";
                    break;
            }

            string sqlQuery = "SELECT id,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books Where " + command+" LIKE '%" + contain + "%'";
            //SELECT* FROM bus_station WHERE  bus_key_query LIKE '%2%';
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    dbBook b = new dbBook();
                    b.id = reader.GetInt32(0);
                    b.day = reader.GetInt32(1);
                    b.month = reader.GetInt32(2);
                    b.year = reader.GetInt32(3);
                    b.Cover = reader.GetInt32(4);
                    b.Cost = reader.GetInt32(5);
                    b.Age = reader.GetInt32(6);
                    b.Tom = reader.GetInt32(7);
                    b.Name = reader.GetString(8);
                    b.Author = reader.GetString(9);
                    b.Provider = reader.GetString(10);
                    b.Description = reader.GetString(11);
                    b.Images = reader.GetString(12);
                    b.Genre = reader.GetString(13);
                    b.Rating = reader.GetFloat(14);
                    b.Series = reader.GetString(15);
                    books.Add(b);
                }
            }
        }
        else
        {
            string sqlQuery = "SELECT id,day,month,year,Cover,Cost,Age,Tom,Name,Author,Provider,Description,Images,Genre,Rating,Series FROM Books Where Name LIKE '%" + txt + "%'";
            //SELECT* FROM bus_station WHERE  bus_key_query LIKE '%2%';
            dbcmd.CommandText = sqlQuery;
            reader = dbcmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(0) > 0)
                {
                    dbBook b = new dbBook();
                    b.id = reader.GetInt32(0);
                    b.day = reader.GetInt32(1);
                    b.month = reader.GetInt32(2);
                    b.year = reader.GetInt32(3);
                    b.Cover = reader.GetInt32(4);
                    b.Cost = reader.GetInt32(5);
                    b.Age = reader.GetInt32(6);
                    b.Tom = reader.GetInt32(7);
                    b.Name = reader.GetString(8);
                    b.Author = reader.GetString(9);
                    b.Provider = reader.GetString(10);
                    b.Description = reader.GetString(11);
                    b.Images = reader.GetString(12);
                    b.Genre = reader.GetString(13);
                    b.Rating = reader.GetFloat(14);
                    b.Series = reader.GetString(15);
                    books.Add(b);
                }
            }
        }
        CloseConnection();

        return books;
    }
    public List<string> Get_Genres()
    {
        List<string> genres = new List<string>();
        //SELECT DISTINCT company FROM products;
        OpenConnection();
        string sqlQuery = "SELECT DISTINCT Genre FROM Books";
        dbcmd.CommandText = sqlQuery;
        reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            foreach(string s in reader.GetString(0).Split("|"))
            {
                genres.Add(s);
            }       
        }
        CloseConnection();
        return genres;
    }
    List<string> unicue(List<dbBook> orig)
    {
        List<string> list = new List<string>();
        for (int x = 0; x < orig.Count; x++)
        {
            if (!(list.Contains(orig[x].Name))) list.Add(orig[x].Name);
        }
        list.Sort();
        return list;
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
    public int id, Cover, Cost, Age, Tom,day,month,year;
    public string Name, Author, Provider, Description, Images, Genre,Series;
    public float Rating;

}
public class dbAdress
{
    public int id;
    public string Adress;
    public int Time;
}
public class dbBuyed
{
    public int id;
    public int idUser,idBook;
    public int day, month, year;
    public int idAdress;
}



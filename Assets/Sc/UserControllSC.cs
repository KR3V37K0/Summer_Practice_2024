using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class UserControllSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    public int id;
    public string Name;
    public string Phone;
    public string Pass;
    public string Card;
    string UserFile = Application.dataPath + "/user/UserFile.txt";

    private void Start()
    {
        if (!(File.Exists(UserFile))) { var f =File.Create(UserFile); f.Close(); }

        if (CheckByPhone()) { CompleteEnter(); }
        
    }
    public IEnumerator Check()
    {
        for(int x = 0; x < 100; x++) 
        { 
            yield return new WaitForSeconds(1f);
            LoadUser();
            if(Phone!=null)break;
            Debug.Log(id + Name + Phone + Pass + Card);
        }
        ScManager.Buttons.btn_menuUSER();
;
    }
    public void CompleteEnter()
    {
        Debug.Log("COMPLETE");
        Debug.Log(id + Name + Phone + Pass + Card);
        StartCoroutine(Check());

    }
    public void SaveUser()
    {

        StreamWriter sw = new StreamWriter(UserFile);
        sw.WriteLine(id);
        sw.WriteLine(Name);
        sw.WriteLine(Phone);
        sw.WriteLine(Pass);
        sw.WriteLine(Card);
        sw.Close();
    }
    public void LoadUser()
    {
        StreamReader sr = new StreamReader(UserFile);
        id = int.Parse(sr.ReadLine());
        Name= sr.ReadLine();
        Phone = sr.ReadLine();
        Pass = sr.ReadLine();
        Card = sr.ReadLine();
        sr.Close();

    }
    public bool CheckByPhone()
    {
        StreamReader sr = new StreamReader(UserFile);
        string s = sr.ReadLine();          
        s = sr.ReadLine();
        s = sr.ReadLine();
        sr.Close();
        ScManager.DataBase.FindUser(s);
        if (id != 0) return true;

        return false;
    }
}

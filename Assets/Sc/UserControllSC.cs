using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;


public class UserControllSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    public int id;
    public string Name;
    public string Phone;
    public string Pass;
    public string Card;
    public int Ava;
    public int A;
    string UserFile;


    private void Start()
    {
#if UNITY_ANDROID

        UserFile = Application.persistentDataPath + "/UserFile.txt";

#endif
#if UNITY_EDITOR
        UserFile = Application.dataPath + "/user/UserFile.txt";
#endif


        if (!(File.Exists(UserFile))) { var f =File.Create(UserFile); f.Close(); }

        if (CheckByPhone()) { CompleteEnter(); }
        
    }
    public IEnumerator Check()
    {
        for(int x = 0; x < 100; x++) 
        {
            yield return new WaitForSeconds(1f);
            if (Phone != null) { ScManager.DataBase.FindUser(Phone); break; }
            
        }
        ScManager.Buttons.btn_menuUSER();
        ScManager.Buttons.btn_menuHOME();
;
    }
    public void CompleteEnter()
    {
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
        sw.WriteLine(Ava);
        sw.WriteLine(A);
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
        Ava = int.Parse(sr.ReadLine());
        A = int.Parse(sr.ReadLine());
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

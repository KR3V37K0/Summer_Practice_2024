using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonsSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject reg_log, navigation,loadScreen;

    private void Start()
    {
       StartCoroutine(StartScreen());
    }
    public IEnumerator StartScreen()
    {
        loadScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (ScManager.User.Phone == null) { 
            
            btn_OpenOnlyOne(1);
            panels[1].SetActive(false);
            reg_log.SetActive(true);
            reg_log.transform.Find("__LOGIN").gameObject.SetActive(true);
            reg_log.transform.Find("__REGISTRATION").gameObject.SetActive(false);    
        }
        loadScreen.SetActive(false);
    }
    public void btn_UserExit()
    {
        ClearUser();
        StartCoroutine(StartScreen());
    }
    public void btn_OpenOnlyOne(int n)
    {
        foreach(GameObject p in panels)
        {
            p.SetActive(false);
        }
        panels[n].SetActive(true);
    }
    public void ClearUser()
    {
        ScManager.User.id = 0;
        ScManager.User.Name = null;
        ScManager.User.Pass = null;
        ScManager.User.Phone = null;
        ScManager.User.Card = null;
    }
    public void btn_UserEnter()
    {
        string phone = reg_log.transform.Find("__LOGIN/input_LOGIN").gameObject.GetComponent<TMP_InputField>().text;
        string pass = reg_log.transform.Find("__LOGIN/input_PASS").gameObject.GetComponent<TMP_InputField>().text;

        ScManager.DataBase.FindUser(phone);
        //if()reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Неверный логин";

        if (ScManager.User.id == 0) 
        {
            reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Неверный логин";
            ClearUser();
        }
        else
        {
            if (ScManager.User.Pass == pass)
            {
                reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Успех";
                Debug.Log("вошел как " + ScManager.User.Name);
                ScManager.User.SaveUser();
                ScManager.User.CompleteEnter();

                //----------------------------------------------------ЗДЕСЬ ПРОДОЛЖИТЬ ВХОД
            }
            else reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Неверный пароль";
            ClearUser();
        }
    }
    public void btn_UserReg()
    {
        string _name = reg_log.transform.Find("__REGISTRATION/input_NAME").gameObject.GetComponent<TMP_InputField>().text;
        string phone = reg_log.transform.Find("__REGISTRATION/input_LOGIN").gameObject.GetComponent<TMP_InputField>().text;
        string pass = reg_log.transform.Find("__REGISTRATION/input_PASS").gameObject.GetComponent<TMP_InputField>().text;


        ScManager.DataBase.FindUser(phone);
        if (ScManager.User.id != 0) reg_log.transform.Find("__REGISTRATION/text_Message").gameObject.GetComponent<TMP_Text>().text = "Аккаунт с этим телефоном уже существует";
        else
        {
            reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Успех";
            ScManager.DataBase.WriteUser(_name, phone, pass);
            ScManager.User.CompleteEnter();
            //----------------------------------------------------ЗДЕСЬ ПРОДОЛЖИТЬ ВХОД
        }
    }
    public void btn_menuUSER()
    {
        reg_log.SetActive(false);
        btn_OpenOnlyOne(4);
        navigation.SetActive(true);
    }
}

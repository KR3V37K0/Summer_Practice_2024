using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonsSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    [SerializeField] GameObject[] panels;
    [SerializeField] GameObject reg_log;

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

        if(ScManager.User.id == 0) 
        {
            reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "�������� �����";
            ClearUser();
        }
        else
        {
            if (ScManager.User.Pass == pass)
            {
                reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "";
                Debug.Log("����� ��� " + ScManager.User.Name);

                //----------------------------------------------------����� ���������� ����
            }
            else reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "�������� ������";
            ClearUser();
        }
    }
    public void btn_UserReg()
    {
        string _name = reg_log.transform.Find("__REGISTRATION/input_NAME").gameObject.GetComponent<TMP_InputField>().text;
        string phone = reg_log.transform.Find("__REGISTRATION/input_LOGIN").gameObject.GetComponent<TMP_InputField>().text;
        string pass = reg_log.transform.Find("__REGISTRATION/input_PASS").gameObject.GetComponent<TMP_InputField>().text;


        ScManager.DataBase.FindUser(phone);
        if (ScManager.User.id != 0) reg_log.transform.Find("__REGISTRATION/text_Message").gameObject.GetComponent<TMP_Text>().text = "������� � ���� ��������� ��� ����������";
        else
        {
            reg_log.transform.Find("__REGISTRATION/text_Message").gameObject.GetComponent<TMP_Text>().text = "";
            ScManager.DataBase.WriteUser(_name, phone, pass);
            //----------------------------------------------------����� ���������� ����
        }
    }
}

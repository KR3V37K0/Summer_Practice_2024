using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    public GameObject[] panels;
    [SerializeField] GameObject reg_log, navigation,loadScreen;
    public Color colorLike, colorUnlike;


    private void Start()
    {
        
        StartCoroutine(StartScreen());
    }
    //FOR ALL
    public IEnumerator StartScreen()
    {
        loadScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        if (ScManager.User.Phone.Length < 3)
        {

            btn_OpenOnlyOne(1);
            panels[1].SetActive(false);
            reg_log.SetActive(true);
            reg_log.transform.Find("__LOGIN").gameObject.SetActive(true);
            reg_log.transform.Find("__REGISTRATION").gameObject.SetActive(false);
        }
        loadScreen.SetActive(false);
    }
    public void btn_OpenOnlyOne(int n)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(false);
        }
        panels[n].SetActive(true);
    }



    //USER
    public void btn_UserExit()
    {
        ClearUser();
        StartCoroutine(StartScreen());
    }
    public void ClearUser()
    {
        ScManager.User.id = 0;
        ScManager.User.Name = null;
        ScManager.User.Pass = null;
        ScManager.User.Phone = " ";
        ScManager.User.Card = null;
        ScManager.User.Ava = 0;
        ScManager.User.A = 0;
        ScManager.User.SaveUser();
    }
    //LOG|REG
    public void btn_UserEnter()
    {
        string phone = reg_log.transform.Find("__LOGIN/input_LOGIN").gameObject.GetComponent<TMP_InputField>().text;
        string pass = reg_log.transform.Find("__LOGIN/input_PASS").gameObject.GetComponent<TMP_InputField>().text;

        ScManager.DataBase.FindUser(phone);

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
                ScManager.User.SaveUser();
                ScManager.User.CompleteEnter();

                //----------------------------------------------------ЗДЕСЬ ПРОДОЛЖИТЬ ВХОД
            }
            else
            {
                reg_log.transform.Find("__LOGIN/text_Message").gameObject.GetComponent<TMP_Text>().text = "Неверный пароль";
                ClearUser();
            }
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
            ScManager.DataBase.WriteUser(_name, phone, pass,"",0,0);
            
            reg_log.transform.Find("__LOGIN").gameObject.SetActive(true);
            reg_log.transform.Find("__REGISTRATION").gameObject.SetActive(false);
        }
    }
    public void btn_menuUSER()
    {
        reg_log.SetActive(false);
        btn_OpenOnlyOne(4);
        navigation.SetActive(true);
        ScManager.Menu.menuUser(panels[4].GetComponent<Transform>());
    }


    //USER SETTINGS
    [SerializeField] Transform content;
    public void btn_Settings()
    {
        content = panels[6].transform.Find("Scroll View/Viewport/Content");
        ScManager.Menu.menuSettings(content);
    }
    public void btn_SaveSettings()
    {
        ScManager.User.Name = content.Find("input_Name").GetComponent<TMP_InputField>().text;
        ScManager.User.Phone = content.Find("input_Phone").GetComponent<TMP_InputField>().text;
        ScManager.User.Ava = int.Parse(content.Find("Avatar/Image").GetComponent<Image>().sprite.name);
        ScManager.User.SaveUser();
        ScManager.DataBase.ChangeUserData(ScManager.User.id, ScManager.User.Name, ScManager.User.Ava, ScManager.User.Phone);
        btn_menuUSER();

        navigation.SetActive(true);
        panels[6].SetActive(false);
    }
    public void btn_ChangePass()
    {
        string oldPass = content.Find("txt_Pass/input_oldPass").GetComponent<TMP_InputField>().text;
        string newPass = content.Find("txt_Pass/input_newPass").GetComponent<TMP_InputField>().text;
        string retryPass = content.Find("txt_Pass/input_retryPass").GetComponent<TMP_InputField>().text;

        if (oldPass == ScManager.User.Pass)
        {
            if (newPass == retryPass)
            {
                content.Find("txt_Pass/txt_Message").GetComponent<TMP_Text>().text = " ";
                ScManager.User.Pass = newPass;
                ScManager.User.SaveUser();
                ScManager.DataBase.ChangeUserPass(ScManager.User.id,newPass);
            }
            else content.Find("txt_Pass/txt_Message").GetComponent<TMP_Text>().text = "Пароль повторен неверно";
        }
        else content.Find("txt_Pass/txt_Message").GetComponent<TMP_Text>().text = "Старый пароль неверный";
    }
    public void btn_ChangeAva() //empty
    {

    }
    public void btn_DeleteAva()//empty
    {

    }
    public void btn_DeleteAcc()
    {
        ScManager.DataBase.DeleteUser(ScManager.User.id);   
        ClearUser();
        StartCoroutine(StartScreen());
    }


    //HOME
    public void btn_menuHOME()
    {
        if (panels[0].activeSelf==true) panels[0].transform.Find("scroll_Book").gameObject.SetActive(false);
        ScManager.Menu.menuHome();
        

    }
    public void btn_BookOpen(int id)
    {
        btn_OpenOnlyOne(0);
        panels[0].transform.Find("scroll_Book").gameObject.SetActive(true);
        ScManager.Menu.OpenBook(id);
    }
    //BOOK VIEW
    public void btn_Like(GameObject but)
    {
        if (but.transform.GetComponent<Image>().color == colorLike)
        {
            but.transform.GetComponent<Image>().color = colorUnlike;
            ScManager.DataBase.DeleteBookFrom(ScManager.Menu.book.id, ScManager.User.id,"Liked");
        }
        else
        {
            but.transform.GetComponent<Image>().color = colorLike;
            ScManager.DataBase.AddBookTo(ScManager.Menu.book.id, ScManager.User.id,"Liked");          
        }
    }


    //LIKE
    public void btn_menuLIKE()
    {
        ScManager.Menu.menuLike(panels[1].transform.Find("Scroll View/Viewport/Content").transform);
    }
    public void btn_Like(GameObject but,int idBook)
    {
        if (but.transform.GetComponent<Image>().color == colorLike)
        {
            but.transform.GetComponent<Image>().color = colorUnlike;
            ScManager.DataBase.DeleteBookFrom(idBook, ScManager.User.id, "Liked");
        }
        else
        {
            but.transform.GetComponent<Image>().color = colorLike;
            ScManager.DataBase.AddBookTo(idBook, ScManager.User.id,"Liked");
        }
    }

    //BUSKET
    public void btn_menuBUSKET()
    {      
        ScManager.Menu.menuBusket(panels[2].transform.Find("Scroll View/Viewport/Content").transform);
    }
    public void btn_toBusket(int idBook)
    {
        ScManager.DataBase.AddBookTo(idBook, ScManager.User.id, "Busketed");
    }
    public void btn_DeleteFromBusket(int idBook,GameObject panel)
    {
        Destroy(panel); 
        ScManager.DataBase.DeleteBookFrom(idBook, ScManager.User.id, "Busketed");
        ScManager.Menu.recalculate_Summ();
    }
    public void btn_Buy()
    {
        Debug.Log("куплено");
    }
}

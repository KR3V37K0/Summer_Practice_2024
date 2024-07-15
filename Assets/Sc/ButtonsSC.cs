using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsSC : MonoBehaviour
{
    [SerializeField] ScriptManager ScManager;
    public GameObject[] panels,buttons;
    public GameObject panel_Rating;
    [SerializeField] GameObject reg_log, navigation,loadScreen,idBook_for_Rating, prefubAva;
    public Color colorLike, colorUnlike;


    private void Start()
    {
        prefubAva = Resources.Load<GameObject>("Prefubs/Avatar_Changer 1");
        prefubSearchEasyResult =Resources.Load<GameObject>("Prefubs/btn_varSearch");
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
        panel_Rating.SetActive(false);
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
    public void btn_clearInputs()
    {
        reg_log.transform.Find("__REGISTRATION/input_NAME").GetComponent<TMP_InputField>().text = null;
        reg_log.transform.Find("__REGISTRATION/input_LOGIN").GetComponent<TMP_InputField>().text = null;
        reg_log.transform.Find("__REGISTRATION/input_PASS").GetComponent<TMP_InputField>().text = null;

        reg_log.transform.Find("__LOGIN/input_LOGIN").GetComponent<TMP_InputField>().text = null;
        reg_log.transform.Find("__LOGIN/input_PASS").GetComponent<TMP_InputField>().text = null;
    }


    //USER SETTINGS
    [SerializeField] Transform content,scroll_ava;
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
        scroll_ava= panels[4].transform.Find("panel_Settings/scroll_Ava").transform;
        scroll_ava.gameObject.SetActive(true);
        foreach (Transform child in scroll_ava.transform.Find("Viewport/Content").GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        for (int x = 1; x < 14; x++)
        {
            prefubAva.transform.Find("Image").GetComponent<Image>().sprite = ScManager.Menu.FindAva(x);
            prefubAva.GetComponent<Button>().onClick.RemoveAllListeners();
            GameObject g= Instantiate(prefubAva, scroll_ava.transform.Find("Viewport/Content"));         
            g.GetComponent<Button>().onClick.AddListener(() => { btn_AvaVar(g.transform.Find("Image").GetComponent<Image>().sprite.name); });
        }
    }
    public void btn_AvaVar(string ava)
    {
        int id = int.Parse(ava);
        
        content.Find("Avatar/Image").GetComponent<Image>().sprite = ScManager.Menu.FindAva(id);
        ScManager.User.Ava = id;
        scroll_ava.gameObject.SetActive(false);

    }
    public void btn_DeleteAva()//empty
    {
        content.Find("Avatar/Image").GetComponent<Image>().sprite=ScManager.Menu.FindAva(0);
        ScManager.User.Ava = 0;
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
        btn_OpenOnlyOne(0);
        if (panels[0].activeSelf == true) 
        { 
            panels[0].transform.Find("scroll_Book").gameObject.SetActive(false);
            panels[0].transform.Find("Scroll Search").gameObject.SetActive(false);
            panels[0].transform.Find("Scroll View").gameObject.SetActive(true);
        }
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
    public void btn_FindByGenre(string genre)
    {
        OpenSearch("Жанр::"+genre);
    }
    public void btn_FindByAuthor(TMP_Text txt)
    {
        OpenSearch("Автор::"+txt.text);
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
    public void btn_Buy(TMP_Text text)
    {
        if (text.text.ToString() == "Купить") { ScManager.Menu.Buy_Panel(); }
        else { ScManager.Menu.Buying(); }
    }
    public void dropChange_adress(TMP_Dropdown drop)
    {
        int id = int.Parse( drop.options[drop.value].text.Split(".")[0]);
        int wait =ScManager.DataBase.AdressTime(id);
        string s = "";
        switch (wait)
        {
            case 0:
            s = "сегодня";
            break;
            case 1: 
            s = "завтра";
            break;
            default:
            s = System.DateTime.Now.AddDays(wait).ToString("dd MMMM yyyy)");
            break;
        }
        panels[2].transform.Find("panel_Buy/txt_when").GetComponent<TMP_Text>().text = "Доставка будет " + s;
    }

    //DELIVERY
    public void btn_menuDELIVERY()
    {
        btn_OpenOnlyOne(3);
        ScManager.Menu.menuDELIVERY(panels[3].transform.Find("Scroll View/Viewport/Content").transform);
    }
    public void btn_OpenRank(int idBook)
    {
        panel_Rating.transform.GetChild(0).name = idBook.ToString();
        panel_Rating.SetActive(true);

    }
    public void btn_Rank(int score)
    {
        ScManager.DataBase.addRating(int.Parse(idBook_for_Rating.name), ScManager.User.id, score);

        btn_menuDELIVERY();
    }


    //SEARCH
    GameObject prefubSearchEasyResult;
    [SerializeField] GameObject easySearchContent;
    [SerializeField] TMP_Dropdown drop_genre;
    [SerializeField] TMP_InputField input_author;
    public void inp_change_SEARCH(TMP_InputField input)
    {
        foreach (Transform child in easySearchContent.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        if (input.text != "") 
        { 
            List<dbBook> books=(ScManager.DataBase.easySearch(input.text));
            foreach (dbBook book in books)
            {
                prefubSearchEasyResult.transform.Find("txt").GetComponent<TMP_Text>().text = book.Name.Split("|")[0];
                GameObject g= Instantiate(prefubSearchEasyResult, easySearchContent.transform);
                g.GetComponent<Button>().onClick.AddListener(() => { OpenSearch(book.Name.Split("|")[0]); });
            }
        }
    }
    public void btn_EnterSearch(int mode)
    {
        if (mode==0) OpenSearch(navigation.transform.Find("panel_Up/input_Search").GetComponent<TMP_InputField>().text);
        else
        {
            
            string genre = drop_genre.options[drop_genre.value].text;
            string author = input_author.text;
            if (genre != " ") {  OpenSearch("Жанр::" + genre); }
            else if (author != null) OpenSearch("Автор::" + author);
        }
    }
    public void OpenSearch(string txt)
    {
        SearchDeselect();
        navigation.transform.Find("panel_Up/input_Search").GetComponent<TMP_InputField>().text = txt;
        List<dbBook> books = ScManager.DataBase.Search(txt);
        btn_OpenOnlyOne(0);
        panels[0].transform.Find("Scroll Search").gameObject.SetActive(true);
        panels[0].transform.Find("Scroll View").gameObject.SetActive(false);
        panels[0].transform.Find("scroll_Book").gameObject.SetActive(false);
        ScManager.Menu.SearchResult(panels[0].transform.Find("Scroll Search/Viewport/Content"),books);     
    }
    public void SearchSelect()
    {
        easySearchContent.SetActive(true);
    }
    public void SearchDeselect()
    {
        easySearchContent.SetActive(false);
    }
    public void btn_BigSearch()
    {
        ScManager.Menu.panelBigSearch(panels[5].transform);
    }
}

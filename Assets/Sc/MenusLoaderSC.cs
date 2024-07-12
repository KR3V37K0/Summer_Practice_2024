using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using static UnityEditor.Progress;
using System;
using UnityEngine.Analytics;

public class MenusLoaderSC : MonoBehaviour
{
    [SerializeField] ScriptManager scmanager;

    [SerializeField]
    [Header("USER")]
    TMP_Text txt_Name;

    [Header("BOOKS")]
    public dbBook book;
    //[SerializeField] GameObject btn_Book, scroll_Group;
    [SerializeField] bool booksLoaded = false;
    [SerializeField] int booksTop;
    [SerializeField] GameObject prefubGroup,prefubBook,prefubLiked,prefubBusketed,prefubGenres,prefubDelivery, prefubDelivered;
    [SerializeField] GameObject contentGroups, contentBooks;
    [SerializeField] Transform BookView;
    List<Sprite> images = new List<Sprite>();
    int i = 0;
    private void Start()
    {
        prefubGroup = Resources.Load<GameObject>("Prefubs/scroll_Group");
        prefubBook = Resources.Load<GameObject>("Prefubs/btn_Book");
        prefubLiked = Resources.Load<GameObject>("Prefubs/panel_LikedBook");
        prefubBusketed = Resources.Load<GameObject>("Prefubs/panel_BookInBusket");
        prefubGenres = Resources.Load<GameObject>("Prefubs/btn_Genre");
        prefubDelivery = Resources.Load<GameObject>("Prefubs/panel_DeliveryBook");
        prefubDelivered = Resources.Load<GameObject>("Prefubs/panel_Delivered");

        panel_Cost = scmanager.Buttons.panels[2].transform.Find("panel_Cost").gameObject;
        panel_Buy = scmanager.Buttons.panels[2].transform.Find("panel_Buy").gameObject;
    }

    //START
    public void setup()
    {

    }

    //USER
    private Sprite FindAva(int id)
    {
        return Resources.Load<Sprite>("Avatars/" + id);
    }
    public void menuUser(Transform canvas)
    {
        //txt_Name.text = scmanager.User.Name;
        canvas.Find("text_Name").GetComponent<TMP_Text>().text= scmanager.User.Name;
        canvas.Find("btn_Avatar/Image").GetComponent<Image>().sprite = FindAva(scmanager.User.Ava);
    }
    public void menuSettings(Transform content)
    {
        content.Find("Avatar/Image").GetComponent<Image>().sprite=FindAva(scmanager.User.Ava);
        content.Find("input_Name").GetComponent<TMP_InputField>().text = scmanager.User.Name;
        content.Find("input_Phone").GetComponent<TMP_InputField>().text = scmanager.User.Phone;
    }

    //BOOKS
    public void menuHome()
    {
        if (booksLoaded == false) 
        { 
            contentGroups = scmanager.Buttons.panels[0].transform.Find("Scroll View/Viewport/Content").gameObject;
            dbBook[] booksBest = scmanager.DataBase.bookBest(booksTop);
            CreateBookGroup(booksBest,"Лучшее");
            booksLoaded = true;
        }
    }
    private void CreateBookGroup(dbBook[]b,string groupName)
    {
        GameObject G =Instantiate(prefubGroup,contentGroups.transform);
        G.transform.Find("txt_GroupName").GetComponent<TMP_Text>().text = groupName;
        contentBooks = G.transform.Find("Viewport/Content").gameObject;

        foreach(dbBook book in b)
        {
            prefubBook.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0] + " ТОМ " + book.Tom;
            prefubBook.transform.Find("txt_Cost").GetComponent<TMP_Text>().text = book.Cost + " руб";
            prefubBook.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/"+book.Cover.ToString());
            prefubBook.GetComponent<Button>().onClick.RemoveAllListeners();
            
            GameObject g= Instantiate(prefubBook, contentBooks.transform);
            g.GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
        }
    }
    public void OpenBook(int id)
    {
        book = scmanager.DataBase.FindBook(id);
        string[] im_name = book.Images.Split('|');
        images.Clear();
        images.Add(Resources.Load<Sprite>("Book Image/" + book.Cover));
        foreach (string im in im_name)
        {
            images.Add(Resources.Load<Sprite>("Book Image/" + im));
        }
        BookView.Find("__TOP/Images").GetComponent<Image>().sprite= images[0];
        BookView.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0];
        int l = 0;
        l = book.Name.IndexOf('|');
        if (l == -1) BookView.Find("txt_OtherNames").GetComponent<TMP_Text>().text = " ";
        else BookView.Find("txt_OtherNames").GetComponent<TMP_Text>().text = book.Name.Substring(book.Name.IndexOf('|') + 1);
        if (book.Tom>0) BookView.Find("txt_Name").GetComponent<TMP_Text>().text+=" ТОМ "+book.Tom;
        BookView.Find("txt_Author").GetComponent<TMP_Text>().text = "Автор: "+book.Author;
        BookView.Find("txt_ProviderDate").GetComponent<TMP_Text>().text = book.Provider + " " + book.Date;
        BookView.Find("txt_Rating/Img_Rating").GetComponent<Image>().fillAmount = (float)book.Rating / 5f;
        BookView.Find("btn_toBusket/txt_Cost").GetComponent<TMP_Text>().text = "В корзину    " + book.Cost+" руб";
        BookView.Find("btn_toBusket").GetComponent<Button>().onClick.RemoveAllListeners();
        BookView.Find("btn_toBusket").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_toBusket(book.id); });
        BookView.Find("txt_Description").GetComponent<TMP_Text>().text = "Описание" + "\n" + book.Description;
        if (scmanager.DataBase.BookIs(book.id, scmanager.User.id,"Liked"))
        {
            BookView.Find("__TOP/btn_Like").GetComponent<Image>().color = scmanager.Buttons.colorLike;
        }
        else
        {
            BookView.Find("__TOP/btn_Like").GetComponent<Image>().color = scmanager.Buttons.colorUnlike; 
        }
        GameObject panel_Genres = BookView.Find("panel_Genres/Viewport/Content").gameObject;
        foreach (Transform child in panel_Genres.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        string[] Genres = book.Genre.Split('|');
        foreach (string genre in Genres)
        {
            prefubGenres.transform.Find("txt").GetComponent<TMP_Text>().text = genre;
            GameObject g = Instantiate(prefubGenres,panel_Genres.transform);
            g.GetComponent<Button>().onClick.RemoveAllListeners();
            g.GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_FindByGenre(genre); });
        }
    }
    public void btn_NextImage()
    {
        i++;
        if (i == images.Count) i = 0;
        BookView.Find("__TOP/Images").GetComponent<Image>().sprite = images[i];
    }
    public void btn_BackImage()
    {
        i--;
        if (i <0)i=images.Count-1;
        BookView.Find("__TOP/Images").GetComponent<Image>().sprite = images[i];
    }

    //LIKE
    public void menuLike(Transform content)
    {
        foreach (Transform child in content.GetComponentInChildren<Transform>())
        {
            if(child.gameObject.name!="Content") Destroy(child.gameObject);
        }
        List<int>idBook= scmanager.DataBase.GetAllID(scmanager.User.id,"Liked");
        List<dbBook> liked=new List<dbBook>();
        foreach (int i in idBook)
        {
            liked.Add(scmanager.DataBase.FindBook(i));
        }
        if (liked.Count > 0) scmanager.Buttons.panels[1].transform.Find("txt_None").gameObject.SetActive(false);
        else scmanager.Buttons.panels[1].transform.Find("txt_None").gameObject.SetActive(true);
        foreach (dbBook book in liked) 
        {

            prefubLiked.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0];
            if(book.Tom>0) prefubLiked.transform.Find("txt_Name").GetComponent<TMP_Text>().text+= " ТОМ " + book.Tom;
            prefubLiked.transform.Find("txt_Cost").GetComponent<TMP_Text>().text = book.Cost + " руб";
            prefubLiked.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/" + book.Cover.ToString());
            prefubBook.GetComponent<Button>().onClick.RemoveAllListeners();

            GameObject g = Instantiate(prefubLiked, content.transform);
            g.transform.Find("btn_Book").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
            g.transform.Find("btn_toBusket").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_toBusket(book.id); });
            g.transform.Find("btn_Like").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_Like(g.transform.Find("btn_Like").gameObject, book.id); });
            g.transform.Find("btn_Like").GetComponent<Image>().color = scmanager.Buttons.colorLike;
        }
    }

    //BUSKET
    public TMP_Text txt_Summ;
    public void menuBusket(Transform content)
    {
        foreach (Transform child in content.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        List<int> idBook = scmanager.DataBase.GetAllID(scmanager.User.id, "Busketed");
        List<dbBook> liked = new List<dbBook>();
        foreach (int i in idBook)
        {
            liked.Add(scmanager.DataBase.FindBook(i));
        }
        panel_Cost.transform.Find("btn_Buy/txt").GetComponent<TMP_Text>().text = "Купить";
        if (liked.Count > 0) 
        { 
            scmanager.Buttons.panels[2].transform.Find("txt_None").gameObject.SetActive(false);
            panel_Cost.transform.Find("btn_Buy").GetComponent<Button>().interactable = true;
        }
        else
        {
            scmanager.Buttons.panels[2].transform.Find("txt_None").gameObject.SetActive(true);
            panel_Cost.transform.Find("btn_Buy").GetComponent<Button>().interactable = false;
        }
        foreach (dbBook book in liked)
        {

            prefubBusketed.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0];
            if (book.Tom > 0) prefubBusketed.transform.Find("txt_Name").GetComponent<TMP_Text>().text += " ТОМ " + book.Tom;
            prefubBusketed.transform.Find("txt_Cost").GetComponent<TMP_Text>().text = book.Cost + " руб";
            prefubBusketed.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/" + book.Cover.ToString());

            GameObject g = Instantiate(prefubBusketed, content.transform);
            g.transform.Find("btn_Book").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
            g.transform.Find("btn_Delete").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_DeleteFromBusket(book.id,g); });
        }
        recalculate_Summ();
    }
    public void recalculate_Summ()
    {
        List<dbBook> busketed = busketedBook();

        float summ = 0;
        foreach(dbBook book in busketed)
        {
            summ += book.Cost;
        }
        txt_Summ.text = "Стоимость "+summ.ToString()+" руб";
        if (summ > 0) scmanager.Buttons.panels[2].transform.Find("txt_None").gameObject.SetActive(false);
        else scmanager.Buttons.panels[2].transform.Find("txt_None").gameObject.SetActive(true);
        final_summ = summ;
    }
    public List<dbBook> busketedBook()
    {
        List<int> idBook = scmanager.DataBase.GetAllID(scmanager.User.id, "Busketed");
        List<dbBook> busketed = new List<dbBook>();
        foreach (int i in idBook)
        {
            busketed.Add(scmanager.DataBase.FindBook(i));
        }
        return busketed;
    }
    public GameObject panel_Buy;
    public GameObject panel_Cost;
    float final_summ;
    TMP_Dropdown drop_Adress;
    public void Buy_Panel()
    {
        panel_Cost.transform.Find("btn_Buy/txt").GetComponent<TMP_Text>().text = "Оплатить";
        panel_Buy.gameObject.SetActive(true);
        if (scmanager.User.Card.Length > 2) panel_Buy.transform.Find("input_Card").GetComponent<TMP_InputField>().text = scmanager.User.Card;
        else panel_Buy.transform.Find("input_Card").GetComponent<TMP_InputField>().text = null;
        drop_Adress = panel_Buy.transform.Find("drop_Adress").GetComponent<TMP_Dropdown>();
        drop_Adress.ClearOptions();
        List<dbAdress> adresses = scmanager.DataBase.GetAllAdress();
        foreach(dbAdress adress in adresses)
        {
            drop_Adress.options.Add(new TMP_Dropdown.OptionData() { text = adress.id+".  "+adress.Adress });
        }
    }
        //BUY
    public void Buying()
    { 
        List<dbBook> busketed = busketedBook();
        scmanager.DataBase.Buy(busketed, System.DateTime.Now.ToShortDateString(), drop_Adress.options[drop_Adress.value].text.Split(".")[0], scmanager.User.id);
        scmanager.DataBase.SetCard(scmanager.User.id, panel_Buy.transform.Find("input_Card").GetComponent<TMP_InputField>().text.ToString());
        foreach (dbBook book in busketed)
        {
            scmanager.DataBase.DeleteBookFrom(book.id, scmanager.User.id, "Busketed");
        }
        scmanager.Buttons.btn_menuDELIVERY();
    }

    //DELIVERY
    public void menuDELIVERY(Transform content)
    {
        
        
        foreach (Transform child in content.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        
        List<dbBuyed> Buyed = scmanager.DataBase.GetAllBuyedID(scmanager.User.id);
        List<dbBook> Books = new List<dbBook>();
        foreach (dbBuyed i in Buyed)
        {
            Books.Add(scmanager.DataBase.FindBook(i.idBook));
        }
        for (int i =0;i<Buyed.Count;i++)
        {
            DateTime date = System.DateTime.Parse(Buyed[i].DateBuy);          
            date=date.AddDays(scmanager.DataBase.AdressTime(Buyed[i].idAdress));       
            if (date <= DateTime.Today) Delivered(content, Books[i], Buyed[i],date);
            else inProgress(content, Books[i], Buyed[i],date);

        }
    }
    void Delivered(Transform content,dbBook book, dbBuyed buy, DateTime date)
    {
        prefubDelivered.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0];
        if (book.Tom > 0) prefubDelivery.transform.Find("txt_Name").GetComponent<TMP_Text>().text += " ТОМ " + book.Tom;
        prefubDelivered.transform.Find("txt_Date").GetComponent<TMP_Text>().text = "доставлено " + date.ToShortDateString();       
        prefubDelivered.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/" + book.Cover.ToString());

        GameObject g = Instantiate(prefubDelivered, content.transform);
        g.transform.Find("btn_Book").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
        g.transform.Find("btn_Rank").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_OpenRank(book.id); });

        g.transform.Find("btn_Rank/img_stars").GetComponent<Image>().fillAmount = scmanager.DataBase.GetRating(book.id, scmanager.User.id) / 5f;

    }
    void inProgress(Transform content, dbBook book, dbBuyed buy,DateTime date)
    {
        prefubDelivery.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0];
        if (book.Tom > 0) prefubDelivery.transform.Find("txt_Name").GetComponent<TMP_Text>().text += " ТОМ " + book.Tom;
        prefubDelivery.transform.Find("txt_Date").GetComponent<TMP_Text>().text = "доставка " + date.ToShortDateString();
        prefubDelivery.transform.Find("txt_Adress").GetComponent<TMP_Text>().text = scmanager.DataBase.GetAdress(buy.idAdress);
        prefubDelivery.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/" + book.Cover.ToString());

        GameObject g = Instantiate(prefubDelivery, content.transform);
        g.transform.Find("btn_Book").GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
    }

    //SEARCH
    public void panelBigSearch(Transform content)
    {
        TMP_Dropdown drop=content.transform.Find("drop_genre").GetComponent<TMP_Dropdown>();
        drop.ClearOptions();
        List<string> genres = scmanager.DataBase.Get_Genres();
        genres=unicue(genres);
        drop.options.Add(new TMP_Dropdown.OptionData() { text = " " });
        foreach (string genre in genres)
        {
            drop.options.Add(new TMP_Dropdown.OptionData() { text = genre });
        }

    }
    List<string> unicue(List<string> orig)
    {
        List<string> list = new List<string>();
        for(int x = 0; x < orig.Count; x++)
        {
            if(!(list.Contains(orig[x])))list.Add(orig[x]); 
        }
        list.Sort();
        return list;
    }
    public void SearchResult(Transform content, List<dbBook> books)
    {
        foreach (Transform child in content.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name != "Content") Destroy(child.gameObject);
        }
        foreach (dbBook book in books)
        {
            prefubBook.transform.Find("txt_Name").GetComponent<TMP_Text>().text = book.Name.Split('|')[0] + " ТОМ " + book.Tom;
            prefubBook.transform.Find("txt_Cost").GetComponent<TMP_Text>().text = book.Cost + " руб";
            prefubBook.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Book Image/" + book.Cover.ToString());
            prefubBook.GetComponent<Button>().onClick.RemoveAllListeners();

            GameObject g = Instantiate(prefubBook, content.transform);
            g.GetComponent<Button>().onClick.AddListener(() => { scmanager.Buttons.btn_BookOpen(book.id); });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Security.Cryptography.X509Certificates;

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
    [SerializeField] GameObject prefubGroup,prefubBook,prefubLiked;
    [SerializeField] GameObject contentGroups, contentBooks;
    [SerializeField] Transform BookView;
    List<Sprite> images = new List<Sprite>();
    int i = 0;
    private void Start()
    {
        prefubGroup = Resources.Load<GameObject>("Prefubs/scroll_Group");
        prefubBook = Resources.Load<GameObject>("Prefubs/btn_Book");
        prefubLiked = Resources.Load<GameObject>("Prefubs/panel_LikedBook");
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
        BookView.Find("txt_Rating/Img_Rating").GetComponent<Image>().fillAmount = book.Rating / 5f;
        BookView.Find("btn_toBusket/txt_Cost").GetComponent<TMP_Text>().text = "В корзину    " + book.Cost+" руб";
        BookView.Find("txt_Description").GetComponent<TMP_Text>().text = "Описание" + "\n" + book.Description;
        if (scmanager.DataBase.BookIsLike(book.id, scmanager.User.id))
        {
            BookView.Find("__TOP/btn_Like").GetComponent<Image>().color = scmanager.Buttons.colorLike;
        }
        else
        {
            BookView.Find("__TOP/btn_Like").GetComponent<Image>().color = scmanager.Buttons.colorUnlike; 
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
        List<int>idBook= scmanager.DataBase.GetAllLikedID(scmanager.User.id);
        List<dbBook> liked=new List<dbBook>();
        foreach (int i in idBook)
        {
            liked.Add(scmanager.DataBase.FindBook(i));
        }
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
        }
    }

}

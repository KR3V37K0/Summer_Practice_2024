using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class MenusLoaderSC : MonoBehaviour
{
    [SerializeField] ScriptManager scmanager;

    [SerializeField]
    [Header("USER")]
    TMP_Text txt_Name;
    

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

}

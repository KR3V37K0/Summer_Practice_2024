using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsSC : MonoBehaviour
{
    [SerializeField] GameObject[] panels;


    public void btn_OpenOnlyOne(int n)
    {
        foreach(GameObject p in panels)
        {
            p.SetActive(false);
        }
        panels[n].SetActive(true);
    }
}

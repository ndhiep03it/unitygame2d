using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemProties : MonoBehaviour
{
    public static ItemProties Singleton;
    public Text txtNameItem;
    public Text txtMotaItem;
    public Image IconItem;
    public GameObject PANEL_TT;
    public GameObject TextMota;


    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;

        }
        else { }

    }

    private void OnDisable()
    {
        PANEL_TT.SetActive(true);
        TextMota.SetActive(true);
        

    }
}

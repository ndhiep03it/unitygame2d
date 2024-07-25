using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thongbao : MonoBehaviour
{
    public static Thongbao Singleton;
    public GameObject ThongbaoUI;
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;

        }
        else { }

    }


    public void Message(string mes)
    {
        GameObject listing = Instantiate(ThongbaoUI, transform, false);
        Text txt = listing.GetComponentInChildren<Text>();
        txt.text = mes;
    }
}

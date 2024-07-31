using PlayFab;
using PlayFab.ClientModels;
//using PlayFab.ServerModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public enum ItemType
{
    TRANGBI,
    VATPHAM,
    THUCAN,
    NHOTIM,
    QUAVANG,
    XUSI,
    TUIQUAVANG,
    TUIVANG,
    HP1,
    MP1,
    TOCDOGAME,
    DANANGCAP,
    CAPSULEKB,
    NAMKN,
    MANHKHUNGLONGME1,
    MANHKHUNGLONGME2,

}
public class Item : MonoBehaviour
{
    public static Item Singleton;
    public string Iditem = null;
    public SpriteRenderer sprite;
    



    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;

        }
        else { }

    }
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

}

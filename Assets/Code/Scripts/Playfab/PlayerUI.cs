using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Singleton;
    public Transform CanvasUI;
    public Text txtNameUI;
    public Slider SliderHPCount;
    public Text txtHPUI;
    public Text txtLevelUI;
    public Slider SliderLevelCount;
    public Text txtLevelCountUI;
    [Header("ĐIỂM LIKE")]
    public Text txtDiemlike;

    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
            
        }
        else { }

    }
    private void Update()
    {
        DataUI();
    }

    private void DataUI()
    {
        txtNameUI.text = PlayfabManager.Singleton.txtName.text;
        SliderHPCount.value = PlayerData.Hp;
        SliderHPCount.maxValue = PlayerData.HpMax;
        txtHPUI.text = PlayerData.Hp + "/" + PlayerData.HpMax;
        txtLevelUI.text = PlayerData.level.ToString();
        txtLevelCountUI.text = PlayerData.LevelCount + "%";
        txtDiemlike.text = "Điểm Like:" + PlayerData.Diemlike;
        //SliderLevelCount.maxValue = PlayerData.LevelCount;
    }
    private void Start()
    {
        SliderLevelCount.value = PlayerData.LevelCount;
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Singleton;
    public Text txtNameUI;
    public Slider SliderHPCount;
    public Text txtHPUI;
    public Text txtLevelUI;
    public Slider SliderLevelCount;
    public Text txtLevelCountUI;

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
        txtLevelUI.text = PlayerData.Level.ToString();
        txtLevelCountUI.text = PlayerData.LevelCount + "%";
        SliderLevelCount.value = PlayerData.Level;
        //SliderLevelCount.maxValue = PlayerData.LevelCount;
    }
}

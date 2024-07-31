using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject PANEL_USER;
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
        SliderHPCount.value = PlayerData.Singleton.Hp;
        SliderHPCount.maxValue = PlayerData.Singleton.HpMax;
        txtHPUI.text = PlayerData.Singleton.Hp + "/" + PlayerData.Singleton.HpMax;
        txtLevelUI.text = PlayerData.Singleton.level.ToString();
        txtLevelCountUI.text = PlayerData.Singleton.LevelCount + "%";
        txtDiemlike.text = "Điểm Like:" + PlayerData.Singleton.Diemlike;
        //SliderLevelCount.maxValue = PlayerData.LevelCount;
    }
    private void Start()
    {
        SliderLevelCount.value = PlayerData.Singleton.LevelCount;
    }
    public void USER()
    {
        PANEL_USER.SetActive(true);
        UserData userData = PANEL_USER.GetComponent<UserData>();
        userData.txtTitle.text = "Thông tin:" + txtNameUI.text;
        userData.NameMapDAUTHAN = txtNameUI.text;
        userData.namePlayer = PlayfabManager.Singleton.txtName.text;
        userData.playfabid =PlayfabManager.Singleton.txtPlayfabId.text;
        //userData.Avtatar.sprite = playerAvatarImage.sprite;
        userData.sohatdauthan = PlayerData.Singleton.sohatdauthan;
        userData.leveldauthan = PlayerData.Singleton.leveldauthan;
    }
    public void GO_HOME()
    {
        SceneManager.LoadSceneAsync(5);
    }
    public void GO_LOBBY()
    {
        SceneManager.LoadSceneAsync(2);
    }
}

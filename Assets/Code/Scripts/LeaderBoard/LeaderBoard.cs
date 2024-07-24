using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LeaderBoard : MonoBehaviour
{
    public static LeaderBoard Singleton;
    public GameObject[] PrefabsItem;
    public Transform listingContainerLevel;
    //public Sprite[] RankIcon;
    //public Animator animatorPanel;
    //public GameObject Panel_BARUS;
    //public GameObject BUTTON_XEMTT;
    //public GameObject PANEL_TT;
    public string idplayfab;
    public Text txtPlayerName;
    public Text txtLevelName;
    public static int Level;
    private GameObject currentTarget;

    private void Awake()
    {
        Singleton = this;
    }
    private void Start()
    {
        //GetLeaderboarderLevel();
    }
    private void OnEnable()
    {
        GetLeaderboarderLevel();
       
    }
    void DestroyBXH()
    {
        foreach(Transform remove in listingContainerLevel)
        {
            Destroy(remove.gameObject);
        }
    }
    private void OnDisable()
    {
        //PlayfabManager.Singleton.SetStats();
        DestroyBXH();

    }
    
    public void StartClose(int count)
    {
        switch (count)
        {
            case 1:
                //animatorPanel.Play("LeaderClose");
                StartCoroutine(time2shide());
                break;

        }
    }

    public void BUTTON_XEMTTCLICK()
    {
        //BUTTON_XEMTT.SetActive(true);
    }
    public void ShowPanelTT()
    {
       // PANEL_TT.SetActive(true);
    }

    public void Checktt()
    {
           
        GetPlayerGameData(idplayfab);
        //BUTTON_XEMTT.SetActive(false);

    }
    public void GetPlayerGameData(string playFabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId,
            Keys = null // Chỉ lấy dữ liệu với key "PlayerGame" // null để lấy tất cả các dữ liệu, hoặc chỉ định các key bạn muốn lấy 

        };

        PlayFabClientAPI.GetUserData(request, result => OnGetStatusSuccess(result, idplayfab), OnGetUserDataFailure);
    }

    private void OnGetStatusSuccess(GetUserDataResult result, string text)
    {
        //GameCanvas.Singleton.SHOWTB("Đang xem thông tin");
        ShowPanelTT();


        if (result.Data != null && result.Data.ContainsKey("Player"))
        {
            string json = result.Data["Player"].Value;
            var playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            foreach (var kvp in playerData)
            {
                switch (kvp.Key)
                {
                    case "level":

                        if (int.TryParse(kvp.Value.ToString(), out Level))
                        {
                            txtLevelName.text = "Cấp:" + Level; 
                        }
                        break;


                }
            }
        }
    }
    void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Lỗi lấy dữ liệu người chơi: " + error.GenerateErrorReport());

    }
    IEnumerator time2shide()
    {
        yield return new WaitForSeconds(1f);
        
        //Panel_BARUS.SetActive(false);
        //GameCanvas.Singleton.ActiveUI();
    }
    public void GetLeaderboarderLevel()
    {
        var requestLeaderboard = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = "PlayerLevel",
            MaxResultsCount = 50,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true,
                ShowLastLogin = true,
                ShowBannedUntil = true,
                ShowCreated = true,
                ShowLocations = true,
                ShowTags = true,
                ShowStatistics = true,



            }



        };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeadboardLevel, DisplayPlayFabError);
    }



    private void DisplayPlayFabError(PlayFabError result)
    {

    }

    void OnGetLeadboardLevel(GetLeaderboardResult result)
    {

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {



            //PlayfabManager.Singleton.SetStats();
            PlayfabManager.Singleton.SaveDataPlayerGame();
            GameObject tempListing = Instantiate(PrefabsItem[0], listingContainerLevel);
            ContentLeaderBoard LL = tempListing.GetComponent<ContentLeaderBoard>();
            LL.playerNameText.text = player.DisplayName;
            LL.playerScoreText.text = "Cấp:" + player.StatValue.ToString();
            LL.playerTop.text = (player.Position + 1).ToString();
            LL.idplayfab = player.PlayFabId;
            if (player.Profile.BannedUntil.HasValue)
            {
                DateTime bannedUntil = player.Profile.BannedUntil.Value;
                if (bannedUntil > DateTime.UtcNow)
                {
                    // PlayerGame is banned
                    TimeSpan banRemaining = bannedUntil - DateTime.UtcNow;
                    //CheckUserLeadBoard.Instance.txtstatusplayer.text = "Tài khoản này đã bị khóa "; ;

                    //CheckUserLeadBoard.Instance.incountstatus = 1;
                }
                else
                {
                    // Ban has expired
                    //CheckUserLeadBoard.Instance.txtstatusplayer.text = "Tài khoản này đã bị khóa nhưng đã hết hạn.";
                    //CheckUserLeadBoard.Instance.txtstatusplayer.color = Color.red;
                    //CheckUserLeadBoard.Instance.incountstatus = 1;

                }
            }
            else
            {
                // PlayerGame is not banned
                //CheckUserLeadBoard.Instance.txtstatusplayer.text = "Đang Hoạt Động.";
                //CheckUserLeadBoard.Instance.incountstatus = 0;

            }        

            // Các đoạn code khác không thay đổi
            if (player.Profile.DisplayName == PlayfabManager.Singleton.txtName.text)
            {
                LL.playerNameText.color = Color.red;
                //NamePlayerGame = LL.playerNameText.text;
                //LL.name2.text = Data.Instance.NamePlayerGame;
                //LL.rank2.text = LL.playerScoreText.text;
                //LL.top2.text = LL.playerTop.text;
                //SLL.playerAvatarImage.sprite = LL.image.sprite;
                //LL.image.sprite = LL.playerAvatarImage.sprite;
                //CheckUserLeadBoard.Instance.txtNameCheck1.text = LL.name2.text;
                //CheckUserLeadBoard.Instance.txtTopCheck1.text = LL.top2.text;
                //CheckUserLeadBoard.Instance.txtBXHCheck1.text = LL.rank2.text;
                //CheckUserLeadBoard.Instance.imgBXHCheck1.sprite = AvatarGame.instance.avatarImage.sprite;
                //CheckUserLeadBoard.Instance.imgBXHCheck1.sprite = AvatarGame.instance.avatarImage.sprite;
                //CheckUserLeadBoard.Instance.enabled = false;
                //CheckUserLeadBoard.Instance.id = 1;

            }
            else
            {
                //LL.playerNameText.color = Color.black;
                //LL.playerNameText.fontStyle = FontStyle.Bold;
                //CheckUserLeadBoard.Instance.enabled = true;

                //CheckUserLeadBoard.Instance.id = 0;

            }
            if (player.Position == 0)
            {
                LL.playerTop.color = Color.green;
                //LL.playerTop.fontStyle = FontStyle.Bold;
                //LL.image.sprite = RankIcon[0];
            }
            if (player.Position == 1)
            {
                LL.playerTop.color = Color.cyan;
                //LL.playerTop.fontStyle = FontStyle.Bold;
               // LL.image.sprite = RankIcon[1];

            }
            if (player.Position == 2)
            {
                LL.playerTop.color = Color.gray;
                //LL.playerTop.fontStyle = FontStyle.Bold;
               // LL.image.sprite = RankIcon[2];

            }
            if (player.Position >= 3)
            {
                LL.playerTop.color = Color.white;
                //LL.playerTop.fontStyle = FontStyle.Bold;
                //LL.image.enabled = false;

            }
            
            
            //PlayfabManager.Instance.SetStats();
            //Image avatarImage = LL.playerAvatarImage; // Assuming LL.playerAvatarImage is a reference to the Image component
            //StartCoroutine(LoadAvatar(player.Profile.AvatarUrl, LL.playerAvatarImage));

        }
    }
    public void SetTarget(GameObject target)
    {
        //currentTarget = target; // Đặt đối tượng quái vật hiện tại
        if (currentTarget != null)
        {
            ContentLeaderBoard previousEnemy = currentTarget.GetComponent<ContentLeaderBoard>();
            if (previousEnemy != null)
            {
                previousEnemy.HideArrow();
            }
        }

        currentTarget = target; // Đặt đối tượng quái vật hiện tại
        ContentLeaderBoard enemyMovement = target.GetComponent<ContentLeaderBoard>();
        if (enemyMovement != null)
        {
            enemyMovement.ShowArrow();
          
        }
    }
    IEnumerator LoadAvatar(string url, Image image)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("Avatar URL is null or empty.");
            yield break;
        }

        var www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("Failed to load avatar: " + www.error);
        }
    }
}

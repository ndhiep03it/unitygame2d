using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendManager : MonoBehaviour
{
  
    public static FriendManager instance;
    //public GameObject PANEL_ADDFRIENDS;
    public string friendSearch;
    public string idfriend;
    public GameObject listingPrefab;
    public GameObject listingPrefabAdd;
    public GameObject listingPrefabError;
    //public Transform listingContainer;
    public InputField inputField_Friend;
    #region Friends
    [SerializeField]
    Transform friendScrollView;
    [SerializeField]
    Transform friendScrollViewShare;
    [SerializeField]
    Transform ContentShareError;
    List<FriendInfo> myFriends;
    List<TagModel> tagModels;
    private FriendInfo friendInfoForRemoving;
    //public Text txtNotifionChat;
    public Transform transformUI;
    public Text txtSoluongbanbe;
    [Header("TỪ CHỐI KẾT BẠN")]
    public Toggle toggleTuchoi;
    private void OnEnable()
    {

        GetFriends();
       // LoadPlayerGameStatus();

    }
    private void OnDisable()
    {      
        foreach (Transform remove in friendScrollView)
        {
                Destroy(remove.gameObject);
        }
        if (PlayerData.Singleton.Tuchoiketban == 0)
        {
            //bật
            toggleTuchoi.isOn = true;
        }
        else if (PlayerData.Singleton.Tuchoiketban == 1)
        {
            //Tắt
            toggleTuchoi.isOn = false;

        }
    }
    
    private void Update()
    {
        txtSoluongbanbe.text = "Số lượng bạn bè:" + friendScrollView.childCount + "/200";
        if (PlayerData.Singleton.Tuchoiketban == 0)
        {
            //bật
            toggleTuchoi.isOn = true;
        }
        else if (PlayerData.Singleton.Tuchoiketban == 1)
        {
            //Tắt
            toggleTuchoi.isOn = false;

        }
    }
    private void Start()
    {
        
    }
    private void LoadPlayerGameStatus()
    {
        // Make a request to PlayFab to retrieve player data, including status
        PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), OnGetPlayerGameStatisticsSuccess, OnSharedError);
    }

    private void OnSharedError(PlayFabError obj)
    {

    }
    public void RemoveDestroy()
    {
        foreach(Transform transformrm in transformUI)
        {
            Destroy(transformrm);
            
        }
        GetFriends();
    }
    private void OnGetPlayerGameStatisticsSuccess(GetPlayerStatisticsResult result)
    {
        // Example: Retrieving player's level from player statistics
        int playerLevel = GetPlayerGameLevel(result);

        // Example: Assume you have a UI element to display player status
        UpdatePlayerGameStatusUI(playerLevel);
    }
    private int GetPlayerGameLevel(GetPlayerStatisticsResult result)
    {
        // Example: Assuming player level is stored as a statistic with the name "Level"
        foreach (var stat in result.Statistics)
        {
            if (stat.StatisticName == "PlayerLevel")
            {
                return stat.Value;
            }
        }
        return 0; // Default to level 0 if not found
    }
    private void UpdatePlayerGameStatusUI(int playerLevel)
    {
        // Example: Update UI to display player status
        Debug.Log("Cấp: " + playerLevel);
        // Update UI elements with the player's status information
    }

    public void SettFriends()
    {
        if (toggleTuchoi.isOn == true)
        {
            //bật
            PlayerData.Singleton.Tuchoiketban = 0;
        }
        else if (toggleTuchoi.isOn == false)
        {
            //Tắt
            PlayerData.Singleton.Tuchoiketban = 1;

        }
        PlayfabManager.Singleton.SaveDataPlayerGame();
    }
   
    private void Awake()
    {
        instance = this;
    }
    void DisplayPlayFabError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    
    public void RemoveFriends()
    {
        //friendInfoForRemoving.FriendPlayFabId = idfriend;
        if (friendInfoForRemoving != null)
        {
            var request = new RemoveFriendRequest
            {
                FriendPlayFabId = idfriend,

            };

            PlayFabClientAPI.RemoveFriend(request, RemoveSucces, RemoveError);
        }
        else
        {
            Debug.LogError("Friend info is not available for removing.");
        }




    }

    private void RemoveError(PlayFabError obj)
    {
        Debug.Log(obj.ErrorMessage);

    }
   
    private void RemoveSucces(RemoveFriendResult obj)
    {

        foreach (var item in _friends)
        {
            if (item.FriendPlayFabId == idfriend)// your condition
            {
                friendInfoForRemoving = item;
            }
            Debug.Log(obj.Request);
        }
        GetFriends();
    }

    

        void DisplayFriends(List<FriendInfo> friendsCache)
        {
        foreach (FriendInfo f in friendsCache)
        {
            //bool isFound = false;
            if (myFriends != null)
            {
                foreach (FriendInfo g in myFriends)
                {
                    if (f.FriendPlayFabId == g.FriendPlayFabId)
                    {

                    }
                        //isFound = true;
                    
                }
            }

            
                GameObject listing = Instantiate(listingPrefab, friendScrollView, false);
                ListingPrefab tempListing = listing.GetComponent<ListingPrefab>();

                tempListing.playerNameText.text = f.TitleDisplayName;
                tempListing.txtplayfab.text = f.FriendPlayFabId;
                StartCoroutine(LoadAvatar(f.Profile.AvatarUrl, tempListing.playerAvatarImage));
            
        }
        myFriends = friendsCache;
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
    IEnumerator WaitForFriend()
    {
        yield return new WaitForSeconds(2);
        GetFriends();
    }
    public void RunWaitFunction()
    {
        StartCoroutine(WaitForFriend());
        // Khởi tạo và gắn tags cho một người chơi
    //    var updateUserDataRequest = new UpdateUserDataRequest
    //    {
    //        Data = new Dictionary<string, string>
    //{
    //    { "PlayerGameType", "VIP" },
    //    { "PlayerGameLevel", "15" }
    //}
    //    };
    //    PlayFabClientAPI.UpdateUserData(updateUserDataRequest, OnUserDataUpdated, OnPlayFabError);

    }
    List<FriendInfo> _friends = null;
    public GameObject ThongbaoOBJ;

    public void GetFriends()
    {
       
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
           // IncludeSteamFriends = false,
            //IncludeFacebookFriends = false
           
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowLastLogin = true,


            }

        }, result => {
            _friends = result.Friends;
           
            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }

    enum FriendIdType { PlayFabId, Username, Email, DisplayName };
    void AddFriend(FriendIdType idType, string friendId)
    {
        var request = new AddFriendRequest();
        switch (idType)
        {
            case FriendIdType.PlayFabId:
                request.FriendPlayFabId = friendId;
                break;
            case FriendIdType.Username:
                request.FriendUsername = friendId;
                break;
            case FriendIdType.Email:
                request.FriendEmail = friendId;
                break;
            case FriendIdType.DisplayName:
                request.FriendTitleDisplayName = friendId;

                break;
        }
        // Execute request and update friends when we are done
        PlayFabClientAPI.AddFriend(request, result => {
            Debug.Log("Friend added successfully!");
            //GameObject groupDeleteObject = Instantiate(ThongbaoOBJ, transformUI, false);
            //Notification thongbao = groupDeleteObject.GetComponent<Notification>();
            //thongbao.txtthongbao.text = "Kết Bạn Thành Công Người Chơi: " + friendSearch;
            Thongbao.Singleton.Message("Kết bạn thành công " + friendSearch);
        }, DisplayPlayFabError);
    }
    public void InputFriendID(string id)
    {
        
        friendSearch = id;
        
    }
    public void SubmitFriendRequest()
    {
        friendSearch = inputField_Friend.text;
        AddFriend(FriendIdType.DisplayName, friendSearch);
        //GetFriends();
  
    }
    public void ShareFriendRequest()
    {
        friendSearch = inputField_Friend.text;
        if (string.IsNullOrEmpty(friendSearch))
        {
           
        }
        else
        {
            
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
                TitleDisplayName = friendSearch
            }, result =>
            {
                var friendPlayFabId = result.AccountInfo.PlayFabId;
                if (!string.IsNullOrEmpty(friendPlayFabId))
                {
                    //AddFriend(FriendIdType.PlayFabId, friendSearch);
                    // Instantiate a new friend object if friend is found
                    InstantiateFriendObject(friendPlayFabId);
                }
                else
                {
                    // Show message if no friend found
                    ShowErrorMessage("Friend not found.");

                }
            }, error =>
            {
                Debug.LogError("Error searching for friend: " + error.GenerateErrorReport());
                ShowErrorMessage("Error searching for friend.");
                Thongbao.Singleton.Message("Không tìm thấy " + friendSearch);
                foreach (Transform remove in friendScrollViewShare)
                {
                    Destroy(remove.gameObject);
                }
                Instantiate(listingPrefabError, ContentShareError, false);
            });
        }
       
        
    }
    private void InstantiateFriendObject(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true
            }
        }, result =>
        {
            foreach (Transform remove in friendScrollViewShare)
            {
                Destroy(remove.gameObject);
            }
            var profile = result.PlayerProfile;
            GameObject listing = Instantiate(listingPrefabAdd, friendScrollViewShare, false);
            ListingPrefab tempListing = listing.GetComponent<ListingPrefab>();
            tempListing.txtplayfab.text = profile.PlayerId;
            tempListing.playerNameText.text = profile.DisplayName;
            
        }, error =>
        {
            Debug.LogError("Error getting player profile: " + error.GenerateErrorReport());
            
            ShowErrorMessage("Lỗi không thấy người chơi.");
        });
    }
    private void ShowErrorMessage(string message)
    {
        // Assuming you have a UI Text element or similar for displaying error messages
        Debug.Log(message); // Replace this with your UI message display code
                            // Example: errorText.text = message;
    }
    public void OpenCloseFriends()
    {
        //friendPanel.SetActive(!friendPanel.activeInHierarchy);
    }

    internal void SetPanelFriends(string notifi)
    {
       // PANEL_ADDFRIENDS.SetActive(true);
        //txtNotifionChat.text = "Bạn có chắc chắn muốn kết bạn với:" + notifi + ".Lưu ý chức năng này vẫn đang phát triển.";

    }
    internal void SetPanelFriendsHide()
    {
        //PANEL_ADDFRIENDS.SetActive(false);
    }
    #endregion Friends
}

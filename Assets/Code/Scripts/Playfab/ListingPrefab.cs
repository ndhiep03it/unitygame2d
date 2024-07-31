using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum data
{
    player,
    noplayer
    
}

public class ListingPrefab : MonoBehaviour
{
    public GameObject userTT;
    public Text playerNameText;
    public Text playerLevelText;
    public Text txtplayfab;
    public Text txtStatus;
    public Text txtLike;
    public Image playerAvatarImage;
    public static int status;
    public  int Level;
    public  int Likes;
    public  int leveldauthan;
    public  int Tuchoiketban;
    public  int sohatdauthan;
    List<PlayFab.ClientModels.FriendInfo> _friends = null;
    List<PlayFab.ClientModels.FriendInfo> myFriends;
    public GameObject Player_BUTTON;
    public GameObject Player_ADD;
    public data  data = new data();

    private void Start()
    {
       
        
        //StartCoroutine(timeCheckData());
        LoadFriendsData(txtplayfab.text);
        //GetFriends();
        StartCoroutine(LoadFriendAvatar(txtplayfab.text, playerAvatarImage));
        if (data == data.player)
        {
            if (playerNameText.text == PlayfabManager.Singleton.txtName.text)
            {
                Player_BUTTON.SetActive(true);
                Player_ADD.SetActive(false);

            }
            else
            {
                Player_BUTTON.SetActive(false);
                Player_ADD.SetActive(true);
            }
        }
    }
    IEnumerator timeCheckData()
    {
        while (true)
        {
            LoadFriendsData(txtplayfab.text);
            yield return new WaitForSeconds(5f);
        }
    }
    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new PlayFab.ClientModels.GetFriendsListRequest
        {
            // IncludeSteamFriends = false,
            //IncludeFacebookFriends = false
             
            ProfileConstraints = new PlayFab.ClientModels.PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowLastLogin = true,


            }

        }, result => {
            _friends = result.Friends;

            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }
    void DisplayFriends(List<PlayFab.ClientModels.FriendInfo> friendsCache)
    {
        foreach (PlayFab.ClientModels.FriendInfo f in friendsCache)
        {
            if (f.FriendPlayFabId == txtplayfab.text)
            {
                //StartCoroutine(LoadFriendAvatar(f.FriendPlayFabId, playerAvatarImage));
            }
            

        }
        myFriends = friendsCache;
    }
    IEnumerator LoadFriendAvatar(string playFabId, Image image)
    {
        var request = new PlayFab.ClientModels.GetPlayerProfileRequest
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayFab.ClientModels.PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
            }
        };

        PlayFabClientAPI.GetPlayerProfile(request, result =>
        {
            var avatarUrl = result.PlayerProfile.AvatarUrl;
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                StartCoroutine(LoadAvatar(avatarUrl, image));
            }
            else
            {
                Debug.LogError("Avatar URL is null or empty for PlayFab ID: " + playFabId);
            }
        }, error =>
        {
            Debug.LogError("Error retrieving player profile for PlayFab ID: " + playFabId + " - " + error.GenerateErrorReport());
        });

        yield return null;
    }
    IEnumerator LoadAvatar(string url, Image image)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("Avatar URL is null or empty.");
            yield break;
        }

        using (WWW www = new WWW(url))
        {
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
    private void DisplayPlayFabError(PlayFabError obj)
    {
       
    }
    public void LoadFriendsData(string playfabid)
    {
        var request = new PlayFab.ClientModels.GetUserDataRequest
        {
            PlayFabId = playfabid,
            Keys = null // null to get all data, or specify keys you want to get
        };

        PlayFabClientAPI.GetUserData(request, result => OnGetStatusSuccess(result, playfabid), OnGetUserDataFailure);
    }
    void OnGetUserDataFailure(PlayFabError error)
    {
        Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
    }
    private void OnGetStatusSuccess(PlayFab.ClientModels.GetUserDataResult result, string playfabid)
    {
        if (result.Data != null && result.Data.ContainsKey("Player") && result.Data.ContainsKey("Likes") && result.Data.ContainsKey("Dauthan"))
        {
            string json = result.Data["Player"].Value;
            string jsonLikes = null;
            string jsonDau = null;
            jsonLikes = result.Data["Likes"].Value;
            jsonDau = result.Data["Dauthan"].Value;
            if (int.TryParse(jsonLikes, out Likes))
            {
                txtLike.text = "Like:" + Likes;
            }
            if (int.TryParse(jsonDau, out sohatdauthan))
            {
                //txtLike.text = "Dauthan:" + sohatdauthan;
            }
            var playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            foreach (var kvp in playerData)
            {
                if (kvp.Key == "status")
                {
                    if (int.TryParse(kvp.Value.ToString(), out status))
                    {
                        UpdateStatus(status);
                    }
                }
                if (kvp.Key == "level")
                {
                    if (int.TryParse(kvp.Value.ToString(), out Level))
                    {
                        playerLevelText.text = "Cấp " + Level;
                    }
                }
                if (kvp.Key == "Tuchoiketban")
                {
                    if (int.TryParse(kvp.Value.ToString(), out Tuchoiketban))
                    {
                        
                    }
                }
                if (kvp.Key == "leveldauthan")
                {
                    if (int.TryParse(kvp.Value.ToString(), out leveldauthan))
                    {

                    }
                }

            }
            
                 
            
        }
    }
    private void UpdateStatus(int status)
    {
        if (status == 0)
        {
            txtStatus.text = "Offline";
            //MoveToBottom();
        }
        else if (status == 1)
        {
            txtStatus.text = "Online";
            txtStatus.color = Color.blue;

        }
    }
    public void AddFriends()
    {
        if (Tuchoiketban == 0)
        {
            Thongbao.Singleton.Message("Người chơi này từ chối kết bạn.");
        }
        else if(Tuchoiketban == 1)
        {
            FriendManager.instance.SubmitFriendRequest();
        }
        
    }
    public void SendLikeToFriend()
    {
        if (PlayerData.Singleton.Diemlike >= 1)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "SendLike", // Tên của hàm CloudScript trên PlayFab
                FunctionParameter = new { friendPlayFabId = txtplayfab.text },
                GeneratePlayStreamEvent = true
            }, async result =>
            {
                //Debug.Log("Successfully sent like to friend.");
                Thongbao.Singleton.Message("Like thành công cho " + playerNameText.text);
                PlayerData.Singleton.Diemlike -= 1;
                Likes++;
                txtLike.text = "Like:" + Likes;
                await Task.Delay(5000);
                // Optionally, you can update UI or other data here
            }, error =>
            {
                Debug.LogError("Failed to send like: " + error.GenerateErrorReport());
            });
        }
        else if (PlayerData.Singleton.Diemlike <= 0)
        {
            Thongbao.Singleton.Message("Không đủ điểm like.Tối thiểu 1 ");
        }

    }
    public void XemTT()
    {
        
        if (DestroyButton.Singleton != null)
        {
            DestroyButton.Singleton.DestroyData();
        }
        GameObject OBJ = Instantiate(userTT, PlayerUI.Singleton.CanvasUI, false);
        UserDataEveryone userData = OBJ.GetComponent<UserDataEveryone>();
        userData.txtTitle.text = "Thông tin:" + playerNameText.text;
        userData.NameMapDAUTHAN = playerNameText.text;
        userData.playfabid = txtplayfab.text;
        userData.Avtatar.sprite = playerAvatarImage.sprite;
        userData.sohatdauthan = sohatdauthan ;
        userData.leveldauthan = leveldauthan;
        userData.namePlayer = playerNameText.text;

    }
    public void GO_HOME()
    {
        SceneManager.LoadSceneAsync(3);
        UserData userData = userTT.GetComponent<UserData>();
        userData.txtTitle.text = "Thông tin:" + playerNameText.text;
        userData.NameMapDAUTHAN = playerNameText.text;
        userData.playfabid = txtplayfab.text;
        userData.Avtatar.sprite = playerAvatarImage.sprite;
        userData.sohatdauthan = sohatdauthan;
        userData.leveldauthan = leveldauthan;
        userData.namePlayer = playerNameText.text;
    }
    public void GO_LOBBY()
    {
        SceneManager.LoadSceneAsync(2);
    }
}

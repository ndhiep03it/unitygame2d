using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.ServerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListingPrefab : MonoBehaviour
{
    public Text playerNameText;
    public Text playerLevelText;
    public Text txtplayfab;
    public Text txtStatus;
    public Text txtLike;
    public static int status;
    public static int Level;
    public static int Likes;


    private void Start()
    {
        
        StartCoroutine(timeCheckData());
    }

    IEnumerator timeCheckData()
    {
        while (true)
        {
            LoadFriendsData(txtplayfab.text);
            yield return new WaitForSeconds(5f);
        }
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
        if (result.Data != null && result.Data.ContainsKey("Player") && result.Data.ContainsKey("Likes"))
        {
            string json = result.Data["Player"].Value;
            string jsonLikes = null;
            jsonLikes = result.Data["Likes"].Value;
            if (int.TryParse(jsonLikes, out Likes))
            {
                txtLike.text = "Like:" + Likes;
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
        FriendManager.instance.SubmitFriendRequest();
    }
    public void SendLikeToFriend()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "SendLike", // Tên của hàm CloudScript trên PlayFab
            FunctionParameter = new { friendPlayFabId = txtplayfab.text },
            GeneratePlayStreamEvent = true
        }, result =>
        {
            Debug.Log("Successfully sent like to friend.");
            Likes++;
            txtLike.text = "Like:" + Likes;
            
            // Optionally, you can update UI or other data here
        }, error =>
        {
            Debug.LogError("Failed to send like: " + error.GenerateErrorReport());
        });
    }
}

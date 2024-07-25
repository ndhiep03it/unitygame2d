using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsProlLeft : MonoBehaviour
{
    public string playfabid;
    public Text txtName;
    public Text txtLevel;
    public Image AvatarFriends;
    //public Image BoderFriends;
    public GameObject StatusOnline;
    public GameObject StatusOffline;
    public GameObject PANEL_OFFLINE;
    public int Status;
    public int Level;
    public int Like;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        StartCoroutine(timeCheckData());
    }

    IEnumerator timeCheckData()
    {
        while (true)
        {
            LoadFriendsData(playfabid);
            yield return new WaitForSeconds(5f);
        }
    }

   

    public void LoadFriendsData(string playfabid)
    {
        var request = new GetUserDataRequest
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

    private void OnGetStatusSuccess(GetUserDataResult result, string playfabid)
    {
        if (result.Data != null && result.Data.ContainsKey("Player"))
        {
            string json = result.Data["Player"].Value;
            var playerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            foreach (var kvp in playerData)
            {
                if (kvp.Key == "status")
                {
                    if (int.TryParse(kvp.Value.ToString(), out Status))
                    {
                        UpdateStatus(Status);
                    }
                }
                if (kvp.Key == "level")
                {
                    if (int.TryParse(kvp.Value.ToString(), out Level))
                    {
                        txtLevel.text = "Cấp " + Level;
                    }
                }
                if (kvp.Key == "Like")
                {
                    if (int.TryParse(kvp.Value.ToString(), out Like))
                    {
                        txtLevel.text = "Lượt like:" + Like;
                    }
                }
            }
        }
    }

    private void UpdateStatus(int status)
    {
        if (status == 0)
        {
            StatusOffline.SetActive(true);
            StatusOnline.SetActive(false);
            PANEL_OFFLINE.SetActive(true);
            //MoveToBottom();
        }
        else if (status == 1)
        {
            StatusOnline.SetActive(true);
            StatusOffline.SetActive(false);
            PANEL_OFFLINE.SetActive(false);
            MoveToTop();
        }
    }

    private void MoveToTop()
    {
        rectTransform.SetAsFirstSibling();
       
    }

    private void MoveToBottom()
    {
        rectTransform.SetAsLastSibling();
       
    }
    public void SendLikeToFriend(string friendPlayFabId)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "SendLike", // Tên của hàm CloudScript trên PlayFab
            FunctionParameter = new { friendPlayFabId = friendPlayFabId },
            GeneratePlayStreamEvent = true
        }, result =>
        {
            Debug.Log("Successfully sent like to friend.");
            // Optionally, you can update UI or other data here
        }, error =>
        {
            Debug.LogError("Failed to send like: " + error.GenerateErrorReport());
        });
    }
}

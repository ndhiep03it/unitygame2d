using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.AdminModels;
using System.Collections.Generic;

public class PlayerListManager : MonoBehaviour
{
    public static PlayerListManager Singleton;
    private GameObject currentTarget; // Biến để lưu trữ đối tượng quái vật hiện tại
    public GameObject content; // The content GameObject in the ScrollView
    public GameObject textPrefab; // The Text prefab for displaying player info

    // Start is called before the first frame update
    void Start()
    {
        PlayFabSettings.TitleId = "5A23B";
       // FetchPlayerAccounts();
    }
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
            
        }
        else { }

    }
    // Fetch the list of player accounts from PlayFab
    public void FetchPlayerAccounts()
    {
        var request = new GetPlayersInSegmentRequest
        {
            SegmentId = "FD928501E3A7EF87" // Replace with your actual segment ID if using segments
        };

        PlayFabAdminAPI.GetPlayersInSegment(request, OnGetPlayersInSegmentSuccess, OnGetPlayersInSegmentFailure);
    }

    // Success callback for fetching player accounts
    private void OnGetPlayersInSegmentSuccess(GetPlayersInSegmentResult result)
    {
        foreach (var player in result.PlayerProfiles)
        {
            GameObject playerText = Instantiate(textPrefab, content.transform);
            //playerText.GetComponent<Text>().text = player.DisplayName ?? player.PlayerId;
            playerText.GetComponent<Text>().text = player.DisplayName + " ID:" + player.PlayerId;
        }
    }

    // Failure callback for fetching player accounts
    private void OnGetPlayersInSegmentFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get player accounts: " + error.GenerateErrorReport());
    }
    public void SetTarget(GameObject target)
    {
        //currentTarget = target; // Đặt đối tượng quái vật hiện tại
        if (currentTarget != null)
        {
            PlayerListDataContent playerListDataContent = currentTarget.GetComponent<PlayerListDataContent>();
            if (playerListDataContent != null)
            {
                playerListDataContent.HideArrow();
            }
        }

        currentTarget = target; // Đặt đối tượng quái vật hiện tại
        PlayerListDataContent playerListDataContent1 = target.GetComponent<PlayerListDataContent>();
        if (playerListDataContent1 != null)
        {
            playerListDataContent1.ShowArrow();
            
        }
    }
}

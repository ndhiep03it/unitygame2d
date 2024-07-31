using UnityEngine;
using UnityEngine.UI; // Nếu sử dụng UI Text
using TMPro; // Nếu sử dụng TextMeshPro
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
[System.Serializable]
public class PlayerIdList
{
    public List<string> ids;
}


public class PlayerDataFetcher : MonoBehaviour
{
    public GameObject playerIdItemPrefab; // Prefab hiển thị ID người chơi
    public Transform contentPanel; // Panel chứa các item

    public string playerPlayFabId; // PlayFab ID của người chơi bạn muốn lấy dữ liệu

    void Start()
    {
       
    }
    private void OnEnable()
    {
        GetPlayerData(PlayfabManager.Singleton.txtPlayfabId.text);
        if (contentPanel.childCount >= 10)
        {
            ClearPlayerName();
        }
    }

    public void ClearPlayerName()
    {
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
        {
            {"PlayerNames", null}, // Đặt giá trị thành chuỗi rỗng
        },
            Permission = PlayFab.ClientModels.UserDataPermission.Public,
        }, SetDataSuccess, DisplayPlayFabError);
    }

    private void DisplayPlayFabError(PlayFabError obj)
    {
        Debug.LogError("Error clearing player names: " + obj.GenerateErrorReport());
    }

    private void SetDataSuccess(UpdateUserDataResult obj)
    {
        Debug.Log("Player names have been cleared successfully.");
    }


    void GetPlayerData(string playFabId)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId,
            Keys = new List<string> { "PlayerNames" } // Khóa chứa dữ liệu JSON
        };

        PlayFabClientAPI.GetUserData(request, OnGetUserDataSuccess, OnGetUserDataError);
    }

    void OnGetUserDataSuccess(GetUserDataResult result)
    {
        if (result.Data != null && result.Data.ContainsKey("PlayerNames"))
        {
            var playerNamesJson = result.Data["PlayerNames"].Value;

            // Nếu dữ liệu là mảng JSON, chúng ta cần bao bọc nó thành đối tượng trước
            var wrappedJson = "{\"ids\":" + playerNamesJson + "}";

            PlayerIdList playerIdList = JsonUtility.FromJson<PlayerIdList>(wrappedJson);
            DisplayPlayerIds(playerIdList.ids);
        }
        else
        {
            Debug.Log("No data found for 'PlayerNames'.");
        }
    }

    void OnGetUserDataError(PlayFabError error)
    {
        Debug.LogError("Error getting user data: " + error.GenerateErrorReport());
    }

    void DisplayPlayerIds(List<string> playFabIds)
    {
        // Xóa các item hiện có
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        // Tạo các item mới
        foreach (var id in playFabIds)
        {
            var item = Instantiate(playerIdItemPrefab, contentPanel);
            var textComponent = item.GetComponentInChildren<Text>(); // Nếu sử dụng UI Text
            if (textComponent != null)
            {
                textComponent.text = "Người chơi này đã tấn công bạn:" + id;
            }

            var tmpTextComponent = item.GetComponentInChildren<Text>(); // Nếu sử dụng TextMeshPro
            if (tmpTextComponent != null)
            {
                tmpTextComponent.text = "Ai đó đã trộm đậu của bạn:" + id;
            }
        }
    }
}

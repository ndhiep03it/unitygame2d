using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Quest
{
    public int dangnhapmoingay;
    public int nhatvangmoingay;
    public int nhat100kvang;
    public int thamgiavongquay;
    public int tangtymchobanbe;
    public int online30phut;
    public int thamgiaphobancauca;

    public Quest(int dangnhapmoingay, int nhatvangmoingay, int nhat100kvang, int thamgiavongquay, int tangtymchobanbe, int online30phut, int thamgiaphobancauca)
    {
        this.dangnhapmoingay = dangnhapmoingay;
        this.nhatvangmoingay = nhatvangmoingay;
        this.nhat100kvang = nhat100kvang;
        this.thamgiavongquay = thamgiavongquay;
        this.tangtymchobanbe = tangtymchobanbe;
        this.online30phut = online30phut;
        this.thamgiaphobancauca = thamgiaphobancauca;
    }
}

public class QuestTaskManager : MonoBehaviour
{
    public static QuestTaskManager Singleton;
    public int dangnhapmoingay;
    public int nhatvangmoingay;
    public int nhat100kvang;
    public int thamgiavongquay;
    public int tangtymchobanbe;
    public int online30phut;
    public int thamgiaphobancauca;

    public Button rewardButton;
    private DateTime serverTime;
    public int streakCount = 1;
    public Text txttb;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        GetServerTime();
    }

    void Update()
    {
        // Optional: Add any update logic if needed
    }

    public Quest ReturnClass()
    {
        return new Quest(dangnhapmoingay, nhatvangmoingay, nhat100kvang, thamgiavongquay, tangtymchobanbe, online30phut, thamgiaphobancauca);
    }

    public void SetDataUI(Quest quest)
    {
        dangnhapmoingay = quest.dangnhapmoingay;
        nhatvangmoingay = quest.nhatvangmoingay;
        nhat100kvang = quest.nhat100kvang;
        thamgiavongquay = quest.thamgiavongquay;
        tangtymchobanbe = quest.tangtymchobanbe;
        online30phut = quest.online30phut;
        thamgiaphobancauca = quest.thamgiaphobancauca;
    }

    public void ResetQuestData()
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "resetQuestData", // The name of the function in the Cloud Script
            GeneratePlayStreamEvent = true, // Optional
        };

        PlayFabClientAPI.ExecuteCloudScript(request, OnCloudScriptSuccess, OnCloudScriptFailure);
    }

    private void OnCloudScriptSuccess(ExecuteCloudScriptResult result)
    {
        Debug.Log("Cloud Script executed successfully!");
        if (result.FunctionResult != null)
        {
            Debug.Log("Function result: " + result.FunctionResult.ToString());
            // Optionally, update local data or UI if needed
            FetchQuestData();
        }
    }

    private void OnCloudScriptFailure(PlayFabError error)
    {
        Debug.LogError("Error executing Cloud Script: " + error.GenerateErrorReport());
    }

    public void FetchQuestData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = new List<string> { "Quest" }
        }, result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Quest"))
            {
                Quest questData = JsonUtility.FromJson<Quest>(result.Data["Quest"].Value);
                SetDataUI(questData);
            }
        }, error =>
        {
            Debug.LogError("Error fetching user data: " + error.GenerateErrorReport());
        });
    }

    public void ClickNhan()
    {
        //rewardButton.interactable = false;
        OnRewardButtonClicked();
    }

    void GetServerTime()
    {
        PlayFabClientAPI.GetTime(new GetTimeRequest(), OnGetTimeSuccess, OnGetTimeFailure);
    }

    void OnGetTimeSuccess(GetTimeResult result)
    {
        serverTime = result.Time;
        CheckDailyLogin();
    }

    void OnGetTimeFailure(PlayFabError error)
    {
        Debug.LogError("Error getting server time: " + error.GenerateErrorReport());
    }

    void CheckDailyLogin()
    {
        // Gọi PlayFab API để lấy dữ liệu người dùng
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = PlayfabManager.Singleton.txtPlayfabId.text,
            Keys = null
        }, result =>
        {
            // Nếu dữ liệu người dùng có chứa "LastLoginDate"
            if (result.Data != null && result.Data.ContainsKey("LastLoginDate"))
            {
                // Chuyển đổi giá trị "LastLoginDate" thành kiểu DateTime
                DateTime lastLoginDate = JsonConvert.DeserializeObject<DateTime>(result.Data["LastLoginDate"].Value);

                // Kiểm tra nếu ngày đăng nhập cuối không phải là ngày hôm nay
                if (lastLoginDate.Day != serverTime.Day)
                {
                    streakCount = 1; // Đặt chuỗi đăng nhập là 1

                    // Nếu dữ liệu chứa "LoginStreak", tăng giá trị chuỗi đăng nhập lên 1
                    if (result.Data.ContainsKey("LoginStreak"))
                    {
                        streakCount = int.Parse(result.Data["LoginStreak"].Value) + 1;
                        ResetQuestData(); // Đặt lại dữ liệu nhiệm vụ
                        txttb.text = "Nhiệm Vụ Mới Đã Được Reset.";

                    }

                    UpdateDailyLoginData(streakCount); // Cập nhật dữ liệu đăng nhập hàng ngày
                }
                else
                {
                    // Đã đăng nhập hôm nay
                    // Có thể cập nhật giao diện người dùng để thông báo đã nhận phần thưởng
                    txttb.text = "Nhiệm Vụ Hôm Nay Đã Cập Nhật";
                }
            }
            else
            {
                // Nếu không có "LastLoginDate", thiết lập dữ liệu đăng nhập với chuỗi đăng nhập là 1
                UpdateDailyLoginData(1);
            }
        }, error =>
        {
            // Xử lý lỗi nếu có
            Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
        });
    }


    void UpdateDailyLoginData(int streakCount)
    {

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "LastLoginDate", JsonConvert.SerializeObject(serverTime.ToString("yyyy-MM-dd")) },
                { "LoginStreak", streakCount.ToString() }
            }
        };

        PlayFabClientAPI.UpdateUserData(request, result =>
        {
            rewardButton.interactable = true;  // Enable reward button for new login
        }, error =>
        {
            Debug.LogError("Error updating user data: " + error.GenerateErrorReport());
        });
    }

    void OnRewardButtonClicked()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = PlayfabManager.Singleton.txtPlayfabId.text,
            Keys = null
        }, result =>
        {
            int streakCount = 1;
            if (result.Data != null && result.Data.ContainsKey("LoginStreak"))
            {
                streakCount = int.Parse(result.Data["LoginStreak"].Value);
            }

            //GiveDailyReward(streakCount);
        }, error =>
        {
            Debug.LogError("Error retrieving user data: " + error.GenerateErrorReport());
        });
    }

    void GiveDailyReward(int streakCount)
    {
        int goldReward = streakCount * 10; // Example: reward gold based on login streak
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "RB", // Your currency code
            Amount = goldReward
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, result =>
        {
            //Thongbao.Singleton.Message("Phần thưởng hàng ngày được trao: " + goldReward + " Vàng");
            //txtTb.text = "Phần thưởng hàng ngày được trao: " + goldReward + " Vàng";
            Debug.Log("Phần thưởng hàng ngày được trao: " + goldReward + " Vàng");
            //rewardButton.interactable = false; // Disable reward button after claiming
            //Give("danangcap", 10);
        }, error =>
        {
            Debug.LogError("Error giving daily reward: " + error.GenerateErrorReport());
        });
    }

    public async void Give(string ItemId, int Quantity)
    {
        for (int i = 0; i < Quantity; i++)
        {
            var request = new PurchaseItemRequest
            {
                CatalogVersion = "Item",
                ItemId = ItemId,
                VirtualCurrency = "RB",
                Price = 0
            };

            PlayFabClientAPI.PurchaseItem(request, result =>
            {
                // Handle success
            }, error =>
            {
                // Handle error
            });

            // Delay to avoid overloading the server
            await Task.Delay(1000); // Delay for 1 second between requests
        }
    }
}

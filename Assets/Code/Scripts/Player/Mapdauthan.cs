using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Mapdauthan : MonoBehaviour
{
    public Text txtName;
    public Text txtSohatdauthan;
    public Text txtDataName;
    public Text txtLeveldauthan;
    public string txtplayfab;
    public int sohatdauthan;
    public int leveldauthan;
    public string nameData;
    void Update()
    {

        
        //if (txtName.text == PlayfabManager.Singleton.txtName.text)
        //{
        //    txtName.text = PlayfabManager.Singleton.txtName.text;
        //    txtDataName.text = "Bạn đang ở trong vườn:" + UserData.Singleton.NameMapDAUTHAN;           
        //    txtplayfab = PlayfabManager.Singleton.txtPlayfabId.text;
        //    nameData = UserData.Singleton.namePlayer;
        //}
        //else 
        //{
            txtName.text = UserDataEveryone.Singleton.namePlayer;
            txtSohatdauthan.text = "Số hạt:" + sohatdauthan + "/100";
            txtLeveldauthan.text = "Cấp:" + leveldauthan;
            txtDataName.text = "Bạn đang ở trong nhà:" + UserDataEveryone.Singleton.namePlayer;
            txtplayfab = UserDataEveryone.Singleton.playfabid;
            nameData = UserDataEveryone.Singleton.namePlayer;
            sohatdauthan = UserDataEveryone.Singleton.sohatdauthan;
            leveldauthan = UserDataEveryone.Singleton.leveldauthan;
        //}


    }

    private void Start()
    {
        //if (txtName.text == PlayfabManager.Singleton.txtName.text)
        //{
        //    txtName.text = PlayfabManager.Singleton.txtName.text;
        //    txtDataName.text = "Bạn đang ở trong nhà:" + PlayfabManager.Singleton.txtName.text;
        //    txtplayfab = PlayfabManager.Singleton.txtPlayfabId.text;
        //    sohatdauthan = PlayerData.Singleton.sohatdauthan;
        //    leveldauthan = PlayerData.Singleton.leveldauthan;
        //}
        //else
        //{
        //    txtName.text = UserDataEveryone.Singleton.NameMapDAUTHAN;
        //    txtSohatdauthan.text = "Số hạt:" + sohatdauthan + "/100";
        //    txtLeveldauthan.text = "Cấp:" + leveldauthan;
        //    txtDataName.text = "Bạn đang ở trong vườn:" + UserDataEveryone.Singleton.NameMapDAUTHAN;
        //    txtplayfab = UserDataEveryone.Singleton.playfabid;
        //    sohatdauthan = UserDataEveryone.Singleton.sohatdauthan;
        //    leveldauthan = UserDataEveryone.Singleton.leveldauthan;
        //}
        
    }

    public void SendLikeToFriend()
    {
        if (sohatdauthan >= 1)
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "Senddauthan", // Tên của hàm CloudScript trên PlayFab
                FunctionParameter = new { friendPlayFabId = txtplayfab, PlayerId = PlayfabManager.Singleton.txtPlayfabId.text },
                GeneratePlayStreamEvent = true
            }, async result =>
            {
                //Debug.Log("Successfully sent like to friend.");
                Thongbao.Singleton.Message("Trộm thành công 1 hạt ");
                sohatdauthan--;
                PlayerData.Singleton.sohatdauthan++;
                UserDataEveryone.Singleton.sohatdauthan--;
                txtSohatdauthan.text = "Số hạt:" + sohatdauthan + "/100";

               
                await Task.Delay(5000);
                // Optionally, you can update UI or other data here
            }, error =>
            {
                Debug.LogError("Failed to send like: " + error.GenerateErrorReport());
            });
        }
        else
        {
            Thongbao.Singleton.Message("Không đủ lượt cướp ");

        }
        var requestMessData = new ExecuteCloudScriptRequest
        {
            FunctionName = "getPlayerName",
             FunctionParameter = new { PlayerId = PlayfabManager.Singleton.txtPlayfabId.text },
        };
        PlayFabClientAPI.ExecuteCloudScript(requestMessData, OnServerSucces, OnDataError);   // exp server



    }

    private void OnDataError(PlayFabError obj)
    {
        
    }

    private void OnServerSucces(ExecuteCloudScriptResult result)
    {
        Debug.Log("Player Name: " + result.FunctionResult);
    }
}

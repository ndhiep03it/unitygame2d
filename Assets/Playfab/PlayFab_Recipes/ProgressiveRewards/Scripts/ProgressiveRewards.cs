﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class ProgressiveRewards : MonoBehaviour {
	// UI 
	public Button checkInBtn;
	public Text directions;
	
	// INSPECTOR TWEAKABLES
	public string playFabTitleId = string.Empty;
	
	// Use this for initialization
	void Start () {
		LockUI();
		PlayFab.PlayFabSettings.TitleId = this.playFabTitleId;
		if(string.IsNullOrEmpty(PlayFab.PlayFabSettings.TitleId))
		{
			Debug.LogWarning("Id tiêu đề chưa được đặt. Để tiếp tục, hãy nhập id tiêu đề của bạn vào trình kiểm tra.");
		}
		AuthenticateWithPlayFab();
	}
	
	void UnlockUI()
	{
		this.checkInBtn.interactable = true;
		this.directions.gameObject.SetActive(true);
	}
	
	void LockUI()
	{
		this.checkInBtn.interactable = false;
		this.directions.gameObject.SetActive(false);
	}

	void AuthenticateWithPlayFab()
	{
		Debug.Log("Đang đăng nhập vào PlayFab...");
		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest() { TitleId = this.playFabTitleId, CustomId = SystemInfo.deviceUniqueIdentifier, CreateAccount = true };
		PlayFabClientAPI.LoginWithCustomID(request, OnLoginCallback, OnApiCallError, null);
	}
	
	void OnLoginCallback(LoginResult result)
	{
		Debug.Log(string.Format("Đăng nhập thành công. Chào mừng người chơi:{0}!", result.PlayFabId));
		Debug.Log(string.Format("Vé phiên của bạn là: {0}", result.SessionTicket));
		UnlockUI();
	}
	
	public void CheckIn()
	{
		Debug.Log("Đang đăng nhập với máy chủ...");
		ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest() { 
			FunctionName = "CheckIn", 
		};
		
		PlayFabClientAPI.ExecuteCloudScript(request, OnCheckInCallback, OnApiCallError);
	}
	
	void OnCheckInCallback(ExecuteCloudScriptResult result)
	{
		// output any errors that happend within cloud script
		if(result.Error != null)
		{
			Debug.LogError(string.Format("{0} -- {1}", result.Error, result.Error.Message));
			return;
		}	

		Debug.Log("Kết quả đăng ký:");
;
		var serializer = PluginManager.GetPlugin<ISerializerPlugin>(PluginContract.PlayFab_Serializer);
		List<ItemInstance> grantedItems = serializer.DeserializeObject<List<ItemInstance>>(result.FunctionResult.ToString());


		if (grantedItems != null && grantedItems.Count > 0)
		{
			Debug.Log(string.Format("Bạn đã được cấp {0} vật phẩm:", grantedItems.Count));
			
			string output = string.Empty;
			foreach(var item in grantedItems)
			{
				output += string.Format("\t {0}: {1}\n", item.ItemId, item.Annotation);
			}
			Debug.Log(output);
		}
		else if(result.Logs.Count > 0)
		{
			foreach(var statement in result.Logs)
			{
				Debug.Log(statement.Message);
			}
		}
		else
		{
			Debug.Log("Đăng ký thành công! Không có mặt hàng nào được cấp.");
		}
	}
	
	void OnApiCallError(PlayFabError err)
	{
		string http = string.Format("HTTP:{0}", err.HttpCode);
		string message = string.Format("ERROR:{0} -- {1}", err.Error, err.ErrorMessage);
		string details = string.Empty;
		
		if(err.ErrorDetails != null)
		{
			foreach(var detail in err.ErrorDetails)
			{
				details += string.Format("{0} \n", detail.ToString());
			}
		}
		
		Debug.LogError(string.Format("{0}\n {1}\n {2}\n", http, message, details));
	}
}

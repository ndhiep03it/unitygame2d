using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab.ClientModels;

using PlayFab;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlayFab.AdminModels;
using UpgradeSystem;
public class DataID
{
    public string PlayfabName;
    public string PlayfabID;
}

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Singleton;
    [Header("Login_Input")]
    [SerializeField] public InputField Gmail_InputField;
    [SerializeField] public InputField PassWord_InputField;
    [SerializeField] public GameObject PanelLogin;
    [SerializeField] public GameObject PanelCharacter;
    [SerializeField] public GameObject PanelCanvas;
    [SerializeField] public GameObject PanelCanvasUIDaTa;
    [Header("Register")]
    [SerializeField] private InputField Emaill_Input_Register;
    [SerializeField] private InputField PassWord_Input_Register;
    [SerializeField] private InputField Name_Input_Register;
    [Header("Forget PassWord")]
    [SerializeField] private InputField Emaill_Input_Forget;
    //public LoginWithPlayFabRequest LoginWithPlayFabRequest;
    public LoginWithEmailAddressRequest LoginWithPlayFabRequest;
    public GetPlayerCombinedInfoRequestParams InfoRequest;
    [Header("Toggle Điều khoản")]
    public Toggle toggleDieuKhoan;
    public int CounrDK;
    public GameObject PanelDieuKhoan;

    [Header("thông báo")]
    public GameObject NotificationGame;
    DataID dataID = new DataID();

    [Header("Show Menu")]
    public GameObject Button_Profile;
    public Text txtGmail;
    public Text txtPlayfabId;
    public Text txtName;
    public Text txtCreate;
    public GameObject ButtonQuyen;
    public GameObject ButtonQuyenAdmin;
    public PlayerListManager playerListManager;
    public PlayerData[] CharacterBox;
    private void Start()
    {
        Gmail_InputField.text = PlayerPrefs.GetString("user");
        CounrDK = PlayerPrefs.GetInt("count");
        PassWord_InputField.text = PlayerPrefs.GetString("pass");
        if (CounrDK == 0)
        {
            toggleDieuKhoan.isOn = false;
        }
        else if (CounrDK == 1)
        {
            toggleDieuKhoan.isOn = true;

        }
    }
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject); // sẽ tạo cái mới,không tự hủy khi chuyển giữa các scene
        }
        else { }

    }
    private void OnApplicationQuit()
    {
        SaveDataPlayerGame();
    }
    public virtual void SetCharacter(int id)
    {
        switch (id)
        {
            case 1:
                PlayerData.CharacterID = id;
                break;
            case 2:
                PlayerData.CharacterID = id;
                break;
        }
    }
    public virtual void GoGame()
    {
        NewUpdatesPopupUI.Singleton.PanelUI.SetActive(false);
        NewUpdatesPopupUI.Singleton.PanelLoad.SetActive(true);
        PanelCharacter.SetActive(false);
        PanelDieuKhoan.SetActive(false);
        PanelLogin.SetActive(true);
        StartCoroutine(timeNextScene());
        
    }

    IEnumerator timeNextScene()
    {
        yield return new WaitForSeconds(3f);
        if (PlayerData.Intro == 0)
        {
            SceneManager.LoadScene(1);//cảnh giới thiệu
            PanelCanvasUIDaTa.SetActive(false);

        }
        else if (PlayerData.Intro == 1)
        {
            SceneManager.LoadScene(2);//cảnh game lobby
            PanelCanvasUIDaTa.SetActive(true);


        }
        PanelCanvas.SetActive(false);
    }
    public void LogOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        SceneManager.LoadScene(0);
        NewUpdatesPopupUI.isAlreadyCheckedForUpdates = false;
        NewUpdatesPopupUI.Singleton.LoadData();
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject); // sẽ tạo cái mới,không tự hủy khi chuyển giữa các scene
        }
        else { Destroy(gameObject); }
    }
    public void SaveDataPlayerGame()
    {
        List<Character> characters = new List<Character>();
        foreach (var item in CharacterBox)
        {
            characters.Add(item.ReturnClass());
        }
        var request = new PlayFab.ClientModels.UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Player", JsonConvert.SerializeObject(CharacterBox[0].ReturnClass()) }
            },
            Permission = PlayFab.ClientModels.UserDataPermission.Public,
            

        };

        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        SetLike();
    }
    public void SetLike()
    {
        PlayFabClientAPI.UpdateUserData(new PlayFab.ClientModels.UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {             
                {"Likes", PlayerData.Likes.ToString()},
         

            },
            Permission = PlayFab.ClientModels.UserDataPermission.Public,

        }, SetDataSuccess, DisplayPlayFabError);
    }

    private void DisplayPlayFabError(PlayFabError obj)
    {
        
    }

    private void SetDataSuccess(PlayFab.ClientModels.UpdateUserDataResult result)
    {
        Debug.Log("Set Like");
    }

    private void OnDataSend(PlayFab.ClientModels.UpdateUserDataResult result)
    {

    }
    public void GetDataPlayerGame()
    {
        PlayFabClientAPI.GetUserData(new PlayFab.ClientModels.GetUserDataRequest(), OnPlayerGameData, OnError);
    }

    private void OnPlayerGameData(PlayFab.ClientModels.GetUserDataResult result)
    {

        if (result.Data != null && result.Data.ContainsKey("Player"))
        {
            //List<Character> characters = JsonConvert.DeserializeObject<List<Character>>(result.Data["PlayerGame"].Value);
            Character characters = JsonConvert.DeserializeObject<Character>(result.Data["Player"].Value);

            for (int i = 0; i < CharacterBox.Length; i++)
            {
                CharacterBox[i].SetDataUI(characters);

            }

        }
        if(PlayerData.Quyen == 0)
        {
            //ButtonQuyen.SetActive(false);
            ButtonQuyenAdmin.SetActive(false);
        }
        else if(PlayerData.Quyen == 1)
        {
            //ButtonQuyen.SetActive(true);
            ButtonQuyenAdmin.SetActive(true);

        }
    }
    public virtual void Login()
    {
        PlayerPrefs.SetInt("count", CounrDK);
        PlayerPrefs.SetString("user", Gmail_InputField.text);
        PlayerPrefs.SetString("pass", PassWord_InputField.text);
        if (toggleDieuKhoan.isOn == false)
        {
            PanelDieuKhoan.SetActive(true);
            CounrDK = 0;
        }
        else
        {
            CounrDK = 1;
            PanelDieuKhoan.SetActive(false);
            PanelLogin.SetActive(true);
            LoginWithPlayFabRequest.Email = Gmail_InputField.text;
            LoginWithPlayFabRequest.Password = PassWord_InputField.text;
            LoginWithPlayFabRequest.TitleId = "5A23B";
            LoginWithPlayFabRequest.InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserAccountInfo = true,
                
                

                ProfileConstraints = new PlayFab.ClientModels.PlayerProfileViewConstraints()
                {
                    ShowAvatarUrl = true,

                }

            };
            PlayFabClientAPI.LoginWithEmailAddress(LoginWithPlayFabRequest, OnLoginSuccses, OnError);
            LoginWithPlayFabRequest.InfoRequestParameters = InfoRequest;
        }
        

        
    }
    public virtual void ForgetPassWordLogin()
    {
        
        if (Emaill_Input_Forget.text.Length>=1)
        {
            NotificationGame.SetActive(true);
            Text txttb = NotificationGame.GetComponentInChildren<Text>();
            txttb.text = "Đang thực hiện.";
            var request = new PlayFab.ClientModels.SendAccountRecoveryEmailRequest
            {
                Email = Emaill_Input_Forget.text,
                TitleId = "5A23B",

            };
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnForgetSucces, OnErroForget);
        }
        else if (Emaill_Input_Forget.text.Length <= 0)
        {
            NotificationGame.SetActive(true);
            Text txttb = NotificationGame.GetComponentInChildren<Text>();
            txttb.text = "Không để trống Email.";
            StartCoroutine(hide());
        }



    }

    private void OnErroForget(PlayFabError obj)
    {
        NotificationGame.SetActive(true);
        Text txttb = NotificationGame.GetComponentInChildren<Text>();
        txttb.text = "Lỗi gửi:" + obj.ErrorMessage;
        StartCoroutine(hide());
    }

    private void OnForgetSucces(PlayFab.ClientModels.SendAccountRecoveryEmailResult obj)
    {
        NotificationGame.SetActive(true);
        Text txttb = NotificationGame.GetComponentInChildren<Text>();
        txttb.text = "Đã gửi mật khẩu về Email.";
        StartCoroutine(hide());
    }

    private void OnError(PlayFabError error)
    {
        // Check if the error is due to invalid username or password
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
           NotificationGame.SetActive(true);
           Text txttb = NotificationGame.GetComponentInChildren<Text>();
           txttb.text = "Sai Gmail hoặc Mật khẩu. Vui lòng thử lại";
           StartCoroutine(hide());

        }
        else
        {
            NotificationGame.SetActive(true);
            Text txttb = NotificationGame.GetComponentInChildren<Text>();
            txttb.text = "Đã xảy ra lỗi. Vui lòng thử lại.";
            StartCoroutine(hide());
            PanelLogin.SetActive(false);


        }
    }
    private IEnumerator hide()
    {
        yield return new WaitForSeconds(1.5f);
        NotificationGame.SetActive(false);
    }
    private void OnLoginSuccses(LoginResult result)
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, OnGetNameCheckSuccess, OnError);
    }

    private void OnGetNameCheckSuccess(GetAccountInfoResult result)
    {
        PanelLogin.SetActive(false);
        PanelCharacter.SetActive(true);
        dataID.PlayfabName = result.AccountInfo.TitleInfo.DisplayName;
        txtGmail.text= result.AccountInfo.PrivateInfo.Email;
        txtPlayfabId.text= result.AccountInfo.PlayFabId;
        dataID.PlayfabID= result.AccountInfo.PlayFabId;
        txtName.text= dataID.PlayfabName;
        txtCreate.text= "Ngày tạo:" + result.AccountInfo.TitleInfo.Created;
        GetDataPlayerGame();
        Button_Profile.SetActive(true);
        Debug.Log(dataID.PlayfabName = result.AccountInfo.TitleInfo.DisplayName);
        playerListManager.FetchPlayerAccounts();

    }
    public virtual void Register()
    {
        if (Name_Input_Register.text.Length >= 1)
        {
            NotificationGame.SetActive(true);
            Text txttb = NotificationGame.GetComponentInChildren<Text>();
            txttb.text = "Đang thực hiện.";
            StartCoroutine(hide());
            
            var request = new RegisterPlayFabUserRequest
            {

                
                Email = Emaill_Input_Register.text,
                Username = Name_Input_Register.text,
                Password = PassWord_Input_Register.text,
                RequireBothUsernameAndEmail = false,

            };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegister, OnErrorRegister);
           

        }
        else if(Name_Input_Register.text.Length <= 0)
        {
            NotificationGame.SetActive(true);
            Text txttb = NotificationGame.GetComponentInChildren<Text>();
            txttb.text = "Tên không để trống.";
            StartCoroutine(hide());
        }
        
    }
    public void CheckUsernameAvailability()
    {
        string username = Name_Input_Register.text;

        var request = new GetAccountInfoRequest
        {
            Username = username
        };

        PlayFabClientAPI.GetAccountInfo(request, OnUsernameCheckSuccess, OnUsernameCheckFailure);
    }
    private void OnUsernameCheckFailure(PlayFabError error)
    {
        // Lỗi xảy ra hoặc tên người chơi chưa tồn tại
        // Bạn có thể tiếp tục quá trình đăng ký ở đây
        // Ví dụ: RegisterAccount();

    }
    private void OnUsernameCheckSuccess(GetAccountInfoResult result)
    {
        // Tên người chơi đã tồn tại
        NotificationGame.SetActive(true);
        Text txttb = NotificationGame.GetComponentInChildren<Text>();
        txttb.text = "Tên này đã tồn tại.";
        StartCoroutine(hide());
    }
    private void OnErrorRegister(PlayFabError error)
    {
        // Handle registration failure
        NotificationGame.SetActive(true);
        Text txttb = NotificationGame.GetComponentInChildren<Text>();
        txttb.text = "Đăng ký thất bại: " + error.ErrorMessage;
        StartCoroutine(hide());
        //if (string.IsNullOrEmpty(Gmail_InputField.text) || string.IsNullOrEmpty(Emaill_Input_Register.text) || string.IsNullOrEmpty(PassWord_Input_Register.text))
        //{
        //    // Hiển thị thông báo cảnh báo 
        //    NotificationGame.SetActive(true);
        //    Text txttb = NotificationGame.GetComponentInChildren<Text>();
        //    txttb.text = "Vui lòng nhập đầy đủ thông tin.";
        //    StartCoroutine(hide());
            

        //}
        //else
        //{
        //    // Xử lý đăng nhập
        //    // ...
            
        //}
    }

    private void OnRegister(RegisterPlayFabUserResult result)
    {
        
        NotificationGame.SetActive(true);
        Text txttb = NotificationGame.GetComponentInChildren<Text>();    
        txttb.text = "Đăng ký thành công.";
        StartCoroutine(hide());
        PlayFabClientAPI.UpdateUserTitleDisplayName(new PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest { DisplayName = Name_Input_Register.text }, OnDisplayName, OnError);
        //UpdateAvatar();
        //SaveDataPlayerGame();
        //SetStats();
        

    }
    private void OnDisplayName(PlayFab.ClientModels.UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("Tên của bạn đã tạo:" + obj.DisplayName);
    }
    public void DeleteAccount()
    {
        
        // Create the delete request
        var request = new DeleteMasterPlayerAccountRequest
        {
            PlayFabId = dataID.PlayfabID
        };

        // Call the Admin API to delete the account
        PlayFabAdminAPI.DeleteMasterPlayerAccount(request, OnDeleteAccountSuccess, OnDeleteAccountFailure);
    }

    private void OnDeleteAccountSuccess(DeleteMasterPlayerAccountResult result)
    {
        Debug.Log("Account successfully deleted.");
        SceneManager.LoadScene(0);

    }

    private void OnDeleteAccountFailure(PlayFabError error)
    {
        Debug.LogError("Failed to delete account: " + error.GenerateErrorReport());
    }
}

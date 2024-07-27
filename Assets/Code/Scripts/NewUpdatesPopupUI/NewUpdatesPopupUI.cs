using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

namespace UpgradeSystem
{
    [System.Serializable]
    struct GameData
    {
        public string Name;
        public string Description;
        public string Version;
        public string Url;
        public string UrlImage;
    }
    public struct Data
    {
        public string Name;
        public string ImageURL;
        public int Baotri;
    }
    public class NewUpdatesPopupUI : MonoBehaviour
    {
        public static NewUpdatesPopupUI Singleton;
        [Header("## UI References :")]
        [SerializeField] GameObject uiCanvas;
        [SerializeField] Button uiNotNowButton;
        [SerializeField] Button uiUpdateButton;
        [SerializeField] Text uiDescriptionText; 
        [SerializeField] Text uiTitleText;
        [SerializeField] Text uiVersionText;

        [Space(20f)]
        [Header("## Settings :")]
        [SerializeField] [TextArea(1, 5)] string jsonDataURL;
        [SerializeField] [TextArea(1, 5)] string jsonDataImageURL;
        [SerializeField] [TextArea(1, 5)] string Name;
        [SerializeField] GameObject PanCheck;
        [SerializeField] public GameObject PanelLoad;
        [SerializeField] GameObject PanelThongbaobaotri;
        [SerializeField] public GameObject PanelUI;
        [SerializeField] Text txtCheck;
        [SerializeField] RawImage ImageLoad;
        [Header("## Thông Báo :")]
        [SerializeField] GameObject ThongBaoGame;
        [SerializeField] Transform ContentThongbao;

        public static bool isAlreadyCheckedForUpdates = false;
        GameData latestGameData;
        public string webUrl;
        public int Baotri;
        void Start()
        {
            if (!isAlreadyCheckedForUpdates)
            {
                StopAllCoroutines();
                StartCoroutine(CheckForUpdates());
                StartCoroutine(GetData(jsonDataImageURL));
                uiVersionText.text = "Phiên bản " + Application.version;
            }
        }
        private void Awake()
        {
            if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
            {
                Singleton = this;
                //DontDestroyOnLoad(gameObject); // sẽ tạo cái mới,không tự hủy khi chuyển giữa các scene
            }
            else { }

        }
        public void LoadData()
        {
            if (!isAlreadyCheckedForUpdates)
            {
                StopAllCoroutines();
                StartCoroutine(CheckForUpdates());
                StartCoroutine(GetData(jsonDataImageURL));
                uiVersionText.text = "Phiên bản " + Application.version;
            }
        }
        IEnumerator GetData(string url)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                // error ...
                PanelLoad.SetActive(true);
            }
            else
            {
                // success...
                Data data = JsonUtility.FromJson<Data>(request.downloadHandler.text);
                if(int.TryParse(data.Baotri.ToString() ,out Baotri))
                {
                    if (Baotri == 0)
                    {
                        PanelLoad.SetActive(true);
                        PanelThongbaobaotri.SetActive(true);
                        Text txttb = PanelThongbaobaotri.GetComponentInChildren<Text>();
                        txttb.text = "Trò chơi đang lỗi hoặc tạm dừng.Liên hệ admin.";
                    }
                    else if (Baotri == 1)
                    {
                        GameObject obj = Instantiate(ThongBaoGame, ContentThongbao, false);
                        Text text = obj.GetComponent<Text>();
                        text.text = data.Name;
                        Debug.Log("JSON Response: " + data.Name); // Log the JSON response

                        // Load image:
                        StartCoroutine(GetImage(data.ImageURL));
                        PanelLoad.SetActive(false);
                    }
                }
                
                

            }

            // Clean up any resources it is using.
            request.Dispose();
        }
        IEnumerator GetImage(string url)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                // error ...
                Debug.Log("Không thể kết nối");
            }
            else
            {
                //success...
                ImageLoad.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            }

            // Clean up any resources it is using.
            request.Dispose();
        }
        IEnumerator CheckForUpdates()
        {
            UnityWebRequest request = UnityWebRequest.Get(jsonDataURL);            
            request.chunkedTransfer = false;
            request.disposeDownloadHandlerOnDispose = true;
            request.timeout = 60;

            yield return request.SendWebRequest(); // Changed to SendWebRequest() for newer Unity versions

            if (request.isDone)
            {
                isAlreadyCheckedForUpdates = true;

                if (!request.isNetworkError && !request.isHttpError) // Added check for HTTP error
                {
                    string jsonResponse = request.downloadHandler.text;
                    Debug.Log("JSON Response: " + jsonResponse); // Log the JSON response

                    try
                    {
                        latestGameData = JsonUtility.FromJson<GameData>(jsonResponse);


                        if (!string.IsNullOrEmpty(latestGameData.Version) && !Application.version.Equals(latestGameData.Version))
                        {
                            // New update is available
                            uiTitleText.text = "Đã có phiên bản mới " + latestGameData.Version;
                            uiDescriptionText.text = latestGameData.Description;
                            txtCheck.text = "Vui lòng cập nhật phiên bản mới.";
                            Name = latestGameData.Name;
                            PanCheck.SetActive(true);
                           
                            ShowPopup();
                        }
                        else
                        {
                            //PanelUI.SetActive(true);
                            uiCanvas.SetActive(false);

                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Failed to parse JSON: " + e.Message);
                    }
                }
                else
                {
                    Debug.LogError("Network Error: " + request.error);
                }
            }

            request.Dispose();
        }
        void ShowPopup()
        {
            // Add buttons click listeners :
            uiNotNowButton.onClick.AddListener(HidePopup);
            uiUpdateButton.onClick.AddListener(() =>
            {
                Application.OpenURL(latestGameData.Url);
                HidePopup();
            });

            uiCanvas.SetActive(true);
        }
        void HidePopup()
        {
            uiCanvas.SetActive(false);

            // Remove buttons click listeners :
            uiNotNowButton.onClick.RemoveAllListeners();
            uiUpdateButton.onClick.RemoveAllListeners();
        }
        void OnDestroy()
        {
            StopAllCoroutines();
        }
        public void WebUrl()
        {
            Application.OpenURL(webUrl);
        }
    }
    
}

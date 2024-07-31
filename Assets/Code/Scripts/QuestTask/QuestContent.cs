using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public enum QuestSelect
{
    DANGNHAPMOINGAY,
    NHATVANGMOINGAY
}
public class QuestContent : MonoBehaviour
{
    public QuestSelect questSelect;
    public float timeRequest;
    public Button buttonResult;
    public Text txtQuest;
    public Text txtResult;
    public RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Load();
    }
    private void OnEnable()
    {
        Load();
    }

    
    private void Update()
    {
       
    }
    private void OnDisable()
    {
     
    }
    public void Load()
    {
        if (questSelect == QuestSelect.DANGNHAPMOINGAY)
        {
            txtQuest.text = "<Đăng Nhập Mỗi Ngày>";
            if (QuestTaskManager.Singleton.dangnhapmoingay == 0)
            {
                txtResult.text = "Nhận";
                buttonResult.interactable = true;
            }
            else if (QuestTaskManager.Singleton.dangnhapmoingay == 1)
            {
                txtResult.text = "Đã nhận";
                buttonResult.interactable = false;
                SetMoveBottom();
            }
        }
        if (questSelect == QuestSelect.NHATVANGMOINGAY)
        {
            txtQuest.text = "<Nhặt Vàng Mỗi Ngày> " + "Tiến Trình:" + QuestTaskManager.Singleton.nhat100kvang + "/100000";
            if (QuestTaskManager.Singleton.nhat100kvang < 100000)
            {
                txtResult.text = "Chưa Xong";
                buttonResult.interactable = false;
            }
            else
            {
                if (QuestTaskManager.Singleton.nhatvangmoingay == 0)
                {
                    txtResult.text = "Nhận";
                    buttonResult.interactable = true;
                }
                else if (QuestTaskManager.Singleton.nhatvangmoingay == 1)
                {

                    txtResult.text = "Đã nhận";
                    buttonResult.interactable = false;
                    SetMoveBottom();
                }
            }
            
        }
        
        

    }

    public void GiveQuest()
    {
        TimeQuest(timeRequest);
    }
    public async void TimeQuest(float timeQuest)
    {
        if (questSelect == QuestSelect.DANGNHAPMOINGAY)
        {
            var end = Time.time + timeQuest;
            while (Time.time < end)
            {               
                await Task.Yield();
            }
            txtResult.text = "Đã nhận";
            buttonResult.interactable = false;
            QuestTaskManager.Singleton.dangnhapmoingay = 1;
            Debug.Log("Quest completed: DANGNHAPMOINGAY");
            Thongbao.Singleton.Message("Quest completed: DANGNHAPMOINGAY");
            SetMoveBottom();
        }
        if (questSelect == QuestSelect.NHATVANGMOINGAY)
        {
            var end = Time.time + timeQuest;
            while (Time.time < end)
            {
                await Task.Yield();
            }
            txtResult.text = "Đã nhận";
            buttonResult.interactable = false;
            QuestTaskManager.Singleton.nhatvangmoingay = 1;
            Debug.Log("Quest completed: NHATVANGMOINGAY");
            Thongbao.Singleton.Message("Quest completed: NHATVANGMOINGAY");
            SetMoveBottom();
        }
        PlayfabManager.Singleton.SaveDataPlayerGame();
    }

    public void SetMoveBottom()
    {
        // Start a coroutine to delay the operation
        StartCoroutine(SetAsLastSiblingNextFrame());
    }

    private IEnumerator SetAsLastSiblingNextFrame()
    {
        // Wait for the end of the frame
        yield return new WaitForEndOfFrame();

        // Set as last sibling
        rectTransform.SetAsLastSibling();
    }
}

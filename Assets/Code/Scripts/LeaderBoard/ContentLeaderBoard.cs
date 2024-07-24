using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContentLeaderBoard : MonoBehaviour, IPointerDownHandler, IPointerClickHandler, IDeselectHandler
{
    public Image BoderUI;
    public Sprite[] spritesBoder;
    public Sprite BoderImageDefault;
    public Sprite BoderImageSelect;
    public Text playerNameText;
    public Text playerTop;
    public Text playerScoreText;
    public string idplayfab;
    public int status;



    public void Checktt()
    {
        //CanvasMain = GameObject.FindGameObjectWithTag("CanvasMain").transform;
        if (playerNameText.text == PlayfabManager.Singleton.txtName.text)
        {
            LeaderBoard.Singleton.SetTarget(gameObject); // Thiết lập đối tượng quái vật hiện tại

            //GameCanvas.Singleton.SHOWTB("Không thể xem bản thân");
            //LeaderBoard.Singleton.PANEL_TT.SetActive(false);
            //LeaderBoard.Singleton.BUTTON_XEMTT.SetActive(false);

        }
        else
        {
            LeaderBoard.Singleton.SetTarget(gameObject); // Thiết lập đối tượng quái vật hiện tại
            LeaderBoard.Singleton.BUTTON_XEMTTCLICK();
            LeaderBoard.Singleton.idplayfab = idplayfab;
            //LeaderBoard.Singleton.PANEL_TT.SetActive(false);
            LeaderBoard.Singleton.txtPlayerName.text = playerNameText.text;

        }

    }
    public void ShowArrow()
    {
        Image imageBoder = gameObject.GetComponent<Image>();
        imageBoder.sprite = BoderImageSelect;

    }

    public void HideArrow()
    {
        Image imageBoder = gameObject.GetComponent<Image>();
        imageBoder.sprite = BoderImageDefault;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //BoderUI.sprite = spritesBoder[0];

    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    BoderUI.sprite = spritesBoder[1];
    //}
    public void OnPointerClick(PointerEventData eventData)
    {
        //BoderUI.sprite = spritesBoder[0];
    }
    public void OnDeselect(BaseEventData eventData)
    {
        
        //BoderUI.sprite = spritesBoder[1]; // Change to a different sprite when the UI element loses focus
        
    }
}

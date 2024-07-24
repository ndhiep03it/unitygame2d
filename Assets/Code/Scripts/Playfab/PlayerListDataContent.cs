using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerListDataContent : MonoBehaviour/*, IDragHandler, IBeginDragHandler, IEndDragHandler , IPointerDownHandler, *//*IPointerClickHandler, IDeselectHandler*/
{
    public GameObject PanelSelectList;
    public GameObject UIBODER;

    public void OnBeginDrag(PointerEventData eventData)
    {
        //HideArrow();


    }
    public void Click()
    {
        PlayerListManager.Singleton.SetTarget(gameObject);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        //HideArrow();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //HideArrow();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //PlayerListManager.Singleton.SetTarget(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //HideArrow();

    }

    internal void HideArrow()
    {
        //PanelSelectList.SetActive(false);
        UIBODER.SetActive(false);
    }

    internal void ShowArrow()
    {
        //PanelSelectList.SetActive(true);
        UIBODER.SetActive(true);

    }


}

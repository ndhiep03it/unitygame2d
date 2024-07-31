using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemContent : MonoBehaviour/*, IPointerClickHandler*/
{
    public Sprite BoderImageDefault;
    public Sprite BoderImageSelect;
    public static Item instance;
    public string IdItem;
    public string NameItem;
    public string ItemInstanceId;
    public string Expiration;
    public string DescriptionItem;
    public Text txtcount;
    public int RevekeAmount = 0;
    public int count = 0;
    public void OnPointerClick(PointerEventData eventData)
    {
        InventoryManager.Singleton.SetTarget(gameObject); // Thiết lập đối tượng quái vật hiện tại
        CheckProperties();
    }
    public Image IconItem;

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
    public virtual void CheckProperties()
    {
        InventoryManager.Singleton.SetTarget(gameObject); 
        ItemProties.Singleton.TextMota.SetActive(false);
        ItemProties.Singleton.PANEL_TT.SetActive(false);
        ItemProties.Singleton.txtNameItem.text = NameItem;
        ItemProties.Singleton.txtMotaItem.text = DescriptionItem;
        ItemProties.Singleton.IconItem.sprite = IconItem.sprite;
    }
}

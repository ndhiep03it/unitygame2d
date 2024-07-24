using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick , IPointerExitHandler
{
    private bool isActive = true; // Flag to track if joystick is active 
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchoredPosition = Vector2.zero;
        handleRect.pivot = new Vector2(0.5f, 0.5f);
        base.OnPointerDown(eventData);
    }
   
    public override void OnPointerUp(PointerEventData eventData)
    {
        
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
    public override void OnDeselect(PointerEventData eventData)
    {

        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnDeselect(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       

        // Handle the pointer exit event
        //background.gameObject.SetActive(false); // Hide the joystick background when the pointer exits the joystick's area
        //HandlePointerUp(); // Call a method to handle the pointer up logic

       
    }

    private void HandlePointerUp()
    {
        // Reset handle position and any other necessary cleanup
        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchoredPosition = Vector2.zero;
    }

    public void ActivateJoystick()
    {
        // Method to activate the joystick
        isActive = true;
    }

    public void DeactivateJoystick()
    {
        // Method to deactivate the joystick
        isActive = false;
        //background.gameObject.SetActive(false); // Hide the joystick background
        HandlePointerUp(); // Reset handle position
    }
}
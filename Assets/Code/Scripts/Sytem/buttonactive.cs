using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonactive : MonoBehaviour
{
    public GameObject Buutonclick;

    public void Onbutton()
    {
        if (Buutonclick.activeSelf != true)
        {
            Buutonclick.SetActive(true);
        }
        else
        {
            Buutonclick.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour
{
    public static DestroyButton Singleton;
    private void Awake()
    {

        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;
           
        }
        else { }
    }
    public void DestroyData()
    {
        Destroy(gameObject);
    }
}

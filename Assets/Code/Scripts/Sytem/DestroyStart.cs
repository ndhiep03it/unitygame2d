using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyStart : MonoBehaviour
{
    public float timer = 2f;
    void Update()
    {

    }

    private void Start()
    {
        Destroy(gameObject, timer); // Destroys the bullet after 2 seconds 
    }
}

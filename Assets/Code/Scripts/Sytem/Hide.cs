using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hide : MonoBehaviour
{
    public float TimeHide = 2f;

    protected void Start()
    {
        StartCoroutine(hide());
    }
    private void Update()
    {
        StartCoroutine(hide());

    }
    private IEnumerator hide()
    {
        yield return new WaitForSeconds(TimeHide);
        gameObject.SetActive(false);
    }
    
}

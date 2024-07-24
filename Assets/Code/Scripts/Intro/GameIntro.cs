using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntro : MonoBehaviour
{
    public GameObject CameraIntro;
    public GameObject Canvas;
    public GameObject PANEL_PLAYER;
    void Start()
    {
        CameraIntro.SetActive(true);
        StartCoroutine(TimeDT());
    }

    IEnumerator TimeDT()
    { 
        yield return new WaitForSeconds(2f);
        CameraIntro.SetActive(false);
        Canvas.SetActive(true);
        PANEL_PLAYER.SetActive(true);


    }

    public void SetAttack()
    {
        EnemyIntro.Singleton.countIntro = 1;
        EnemyIntro.Singleton.Walk = true;
    }
}

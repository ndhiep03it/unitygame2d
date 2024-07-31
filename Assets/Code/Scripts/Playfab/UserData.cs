using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    public static UserData Singleton;
    public Image Avtatar;
    public Text txtTitle;
    public string NameMapDAUTHAN;
    public string playfabid;
    public int sohatdauthan;
    public int leveldauthan;
    public string namePlayer;
    private void Awake()
    {
        if (Singleton == null) // kiểm tra xem đã tồn tại chưa,nếu chưa
        {
            Singleton = this;

        }
        else { }
    }
    
}

using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsLeft : MonoBehaviour
{
    [Header("CHECK FRIENDS")]
    public Transform ContentFriendsLeft;
    public GameObject AddFriends;
    public static FriendsLeft Instance;
    public GameObject listingPrefab;
    [SerializeField]
    Transform friendScrollView;
    List<FriendInfo> myFriends;
    List<TagModel> tagModels;
    private FriendInfo friendInfoForRemoving;
    List<FriendInfo> _friends = null;
    void Start()
    {
        GetFriends();
    }
    private void Update()
    {
        CheckFriends();
    }
     void CheckFriends()
     {
        if(ContentFriendsLeft.childCount ==1)
        {
           // AddFriends.SetActive(true);
        }
        else if(ContentFriendsLeft.childCount >= 2)
        {
            //AddFriends.SetActive(false);
        }
     }
    private void Awake()
    {
        Instance = this;
    }
   
    public void GetFriends()
    {
        PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest
        {
            // IncludeSteamFriends = false,
            //IncludeFacebookFriends = false

            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,


            }

        }, result => {
            _friends = result.Friends;

            DisplayFriends(_friends); // triggers your UI
        }, DisplayPlayFabError);
    }

    private void DisplayPlayFabError(PlayFabError obj)
    {

    }

    void DisplayFriends(List<FriendInfo> friendsCache)
    {
        foreach (Transform child in friendScrollView)
        {
            Destroy(child.gameObject);
        }
        foreach (FriendInfo f in friendsCache)
        {
            if (myFriends != null)
            {
                foreach (FriendInfo g in myFriends)
                {
                    if (f.FriendPlayFabId == g.FriendPlayFabId)
                    {

                    }


                }
            }
            GameObject listing = Instantiate(listingPrefab, friendScrollView, false);
            FriendsProlLeft tempListing = listing.GetComponent<FriendsProlLeft>();
            tempListing.txtName.text = f.TitleDisplayName;
            tempListing.playfabid = f.FriendPlayFabId;
            //StartCoroutine(LoadAvatar(f.Profile.AvatarUrl, tempListing.AvatarFriends));

        }
        myFriends = friendsCache;
    }
    IEnumerator LoadAvatar(string url, Image image)
    {
        if (string.IsNullOrEmpty(url))
        {
            //Debug.LogError("Avatar URL is null or empty.");
            yield break;
        }

        var www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            Texture2D texture = www.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
        }
        else
        {
            /// Debug.LogError("Failed to load avatar: " + www.error);
        }
    }

  
}

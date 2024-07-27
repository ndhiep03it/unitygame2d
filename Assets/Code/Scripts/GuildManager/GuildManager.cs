using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuildManager : MonoBehaviour
{
    public InputField guildNameInputField; // Input field for entering the guild name
    public InputField memberIdInputField; // Input field for entering the member's PlayFab ID
    public Text feedbackText; // Text component for displaying feedback messages

    private EntityKey myEntityKey; // The player's entity key
    private string guildId; // The ID of the created guild

    void Start()
    {
        GetEntityToken();
    }

    void GetEntityToken()
    {
        //PlayFabAdminAPI.(new PlayFab.AuthenticationModels.GetEntityTokenRequest(),
        //result => {
        //    myEntityKey = result.Entity;
        //    feedbackText.text = "Entity token obtained.";
        //},
        //error => {
        //    feedbackText.text = "Error obtaining entity token: " + error.GenerateErrorReport();
        //});
    }

    public void CreateGuild()
    {
        string guildName = guildNameInputField.text;
        if (string.IsNullOrEmpty(guildName))
        {
            feedbackText.text = "Guild name is null or empty.";
            return;
        }

        CreateGroupRequest request = new CreateGroupRequest
        {
            GroupName = guildName,
            Entity = myEntityKey
        };

        PlayFabGroupsAPI.CreateGroup(request, OnGuildCreated, DisplayPlayFabError);
    }

    void OnGuildCreated(CreateGroupResponse response)
    {
        guildId = response.Group.Id;
        feedbackText.text = "Guild created successfully. Guild ID: " + guildId;
    }

    public void AddMemberToGuild()
    {
        string memberPlayFabId = memberIdInputField.text;
        if (string.IsNullOrEmpty(memberPlayFabId))
        {
            feedbackText.text = "Member PlayFab ID is null or empty.";
            return;
        }

        EntityKey memberEntityKey = new EntityKey
        {
            Id = memberPlayFabId,
            Type = "title_player_account"
        };

        AddMembersRequest request = new AddMembersRequest
        {
            Group = new EntityKey { Id = guildId, Type = "group" },
            Members = new List<EntityKey> { memberEntityKey }
        };

        PlayFabGroupsAPI.AddMembers(request, OnMemberAdded, DisplayPlayFabError);
    }

    void OnMemberAdded(EmptyResponse response)
    {
        feedbackText.text = "Member added to guild successfully.";
    }

    void DisplayPlayFabError(PlayFabError error)
    {
        feedbackText.text = "Error: " + error.GenerateErrorReport();
    }
}

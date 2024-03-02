using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyNameText;

    private LobbiesList lobbiesList;
    private Lobby lobby;

    public void InitializePrefab(LobbiesList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;
        lobbyNameText.text = this.lobby.Name;
    }

    public void Join()
    {
        lobbiesList.JoinAsync(lobby);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostGameManager : IDisposable
{
    private NetworkServer networkServer;
    private Allocation allocation;

    private string joinCode;
    private string lobbyId;

    private const int MAX_CONNECTIONS = 2;
    private const string GAME_SCENE_NAME = "LevelOne";

    public async Task StartHostAsync()
    {
        try
        {
            allocation = await Relay.Instance.CreateAllocationAsync(MAX_CONNECTIONS);
        } 
        catch (Exception exception)
        {
            Debug.LogError(exception);
            return;
        }

        try
        {
            joinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new(allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        try
        {
            CreateLobbyOptions lobbyOptions = new();

            lobbyOptions.IsPrivate = false;
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: joinCode
                        )
                }
            };

            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(PlayerPrefs.GetString(NameSelector.PLAYER_NAME), MAX_CONNECTIONS, lobbyOptions);
            lobbyId = lobby.Id;

            HostSingleton.Instance.StartCoroutine(HeartbeatLobby(20));
        }
        catch (LobbyServiceException lobbyException)
        {
            Debug.LogError(lobbyException);
        }

        networkServer = new(NetworkManager.Singleton);

        UserData userData = new()
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME, "Missing name"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };

        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartHost();

        // NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Single);
    }

    private IEnumerator HeartbeatLobby(float waitInSeconds)
    {
        WaitForSecondsRealtime secondsToWait = new WaitForSecondsRealtime(waitInSeconds);

        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return secondsToWait;
        }
    }

    public async void Dispose()
    {
        HostSingleton.Instance.StopCoroutine(nameof(HeartbeatLobby));

        if (!string.IsNullOrEmpty(lobbyId))
        {
            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(lobbyId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

            lobbyId = string.Empty;
        }

        networkServer?.Dispose();
    }

    public string GetJoinCode() => joinCode;
}

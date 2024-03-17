using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager networkManager;
    public NetworkServer(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        networkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private Dictionary<ulong, string> clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> authIdToUserData = new Dictionary<string, UserData>();

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);

        clientIdToAuth.Add(request.ClientNetworkId, userData.userAuthId);
        authIdToUserData.Add(userData.userAuthId, userData);

        response.Approved = true;
    }

    private void NetworkManager_OnServerStarted()
    {
        networkManager.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientIdToAuth.TryGetValue(clientId, out string userAuthId))
        {
            clientIdToAuth.Remove(clientId);
            authIdToUserData.Remove(userAuthId);
        }
    }

    public void Dispose()
    {
        if (networkManager != null) return;

        networkManager.ConnectionApprovalCallback -= NetworkManager_ConnectionApprovalCallback;
        networkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        networkManager.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;

        if (networkManager.IsListening)
        {
            networkManager.Shutdown();
        }
    }
}

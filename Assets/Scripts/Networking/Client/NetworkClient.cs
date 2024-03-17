using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager networkManager;

    private const string MAIN_MENU = "MainMenu";

    public NetworkClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId != 0 && clientId != networkManager.LocalClientId) return;

        if (SceneManager.GetActiveScene().name != MAIN_MENU)
        {
            SceneManager.LoadScene(MAIN_MENU);
        }

        if (networkManager.IsConnectedClient)
        {
            networkManager.Shutdown();
        }
    }

    public void Dispose()
    {
        if (networkManager != null)
        {
            networkManager.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }
}

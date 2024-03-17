using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkTesting : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    private void Start()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += Singleton_OnClientConnectedCallback;

        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(playerPrefab, client.Key, true, true);
        }
    }

    public override void OnDestroy()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= Singleton_OnClientConnectedCallback;
    }

    private void Singleton_OnClientConnectedCallback(ulong obj)
    {
        NetworkManager.Singleton.SpawnManager.InstantiateAndSpawn(playerPrefab, obj, true);
    }
}

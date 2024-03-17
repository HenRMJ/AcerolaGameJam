using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private LobbyCode lobbyCode;

    public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();

        lobbyCode.gameObject.SetActive(true);
        lobbyCode.SetLobbyCode(HostSingleton.Instance.GameManager.GetJoinCode());
    }

    public async void StartClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeInput.text);
    }
}

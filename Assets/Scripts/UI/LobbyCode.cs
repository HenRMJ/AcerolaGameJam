using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyCode : MonoBehaviour
{
    [SerializeField] private GameObject hideBar;
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    private string lobbyCode;

    public void SetLobbyCode(string lobbyCode)
    {
        this.lobbyCode = lobbyCode;
        lobbyCodeText.text = lobbyCode;

    }

    public void ToggleCode()
    {
        if (hideBar.activeInHierarchy)
        {
            hideBar.SetActive(false);
        }
        else
        {
            hideBar.SetActive(true);
        }
    }

    public void CopyLobbyCode()
    {
        GUIUtility.systemCopyBuffer = lobbyCode;
    }
}

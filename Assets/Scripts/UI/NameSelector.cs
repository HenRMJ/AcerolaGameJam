using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button connectButton;
    [SerializeField] private int minNameLength, maxNameLength;

    public const string PLAYER_NAME = "playerName";

    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            Connect();
        }

        nameField.text = PlayerPrefs.GetString(PLAYER_NAME, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        connectButton.interactable = nameField.text.Length >= minNameLength && nameField.text.Length <= maxNameLength;
    }

    public void Connect()
    {
        PlayerPrefs.SetString(PLAYER_NAME, nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

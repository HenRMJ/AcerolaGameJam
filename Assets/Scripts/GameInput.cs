using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private MultiplayerPlayer playerActions;

    private void Awake()
    {
        Instance = this;

        playerActions = new MultiplayerPlayer();
        playerActions.Player.Enable();
    }

    public Vector2 GetNormalizedInput()
    {
        Vector2 inputVector = playerActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }
}

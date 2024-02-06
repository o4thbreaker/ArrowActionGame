using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    public static PlayerInputActions playerInputActions;
    public static event Action<InputActionMap> actionMapChange; 

    public bool analogMovement;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
    }

    private void Start()
    {
        /*ToggleActionMap(playerInputActions.Player);
        DisableArrowActionMap();*/
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;
        
        playerInputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerInputActions.Player.Move.ReadValue<Vector2>();
    }

    public Vector2 GetPlayerMouseDelta()
    {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }

    public bool IsPlayerRunning()
    {
        return playerInputActions.Player.Sprint.triggered;
    }

    public bool IsPlayerJumping()
    {
        return playerInputActions.Player.Jump.triggered;
    }

    public bool IsPlayerControlTransfered()
    {
        return playerInputActions.Player.TransferToArrow.triggered;
    }

    public void DisableArrowActionMap()
    {
        playerInputActions.Arrow.Disable();
    }
}

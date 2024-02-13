using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static PlayerInputActions playerInput;
    public static event Action<InputActionMap> actionMapChange; 

    public bool analogMovement;

    private InputAction playerMove;
    private InputAction playerLook;
    private InputAction playerSprint;
    private InputAction playerJump;

    private InputAction arrowMove;
    private InputAction arrowLook;
    private InputAction arrowAccelerate;

    public Vector2 GetPlayerMovement()
    {
        return playerMove.ReadValue<Vector2>();
    }

    public Vector2 GetArrowMovement()
    {
        return arrowMove.ReadValue<Vector2>();
    }

    public InputAction GetArrowAccelerate()
    {
        return arrowAccelerate;
    }

    private void Awake()
    { 
        Instance = this;

        playerInput = new PlayerInputActions();

        playerMove = playerInput.Player.Move;
        playerLook = playerInput.Player.Look;
        playerSprint = playerInput.Player.Sprint;
        playerJump = playerInput.Player.Jump;

        arrowMove = playerInput.Arrow.Move;
        arrowLook = playerInput.Arrow.Look;
        arrowAccelerate = playerInput.Arrow.Accelerate;
    }

    private void RegisterInputActions()
    {
        if (playerInput.Arrow.enabled)
        {
            arrowAccelerate.performed += ArrowController.Instance.OnAcceleratePressed;
            arrowAccelerate.canceled += ArrowController.Instance.OnAccelerateReleased;
        }
        else if (playerInput.Player.enabled)
        {
            playerSprint.performed += ThirdPersonController.Instance.OnSprintPressed;
            playerSprint.canceled += ThirdPersonController.Instance.OnSprintReleased;

            playerJump.performed += ThirdPersonController.Instance.OnJump;
        }
    }

    private void Start()
    {
        RegisterInputActions();
    }

    public static void EnableActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;
        
        playerInput.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }

    private void OnEnable()
    {
        EnableActionMap(playerInput.Arrow);
    }

    private void OnDisable()
    {
        // smth like DisableActionMap()
    }

    public bool IsPlayerMoving()
    {
        return (playerMove.ReadValue<Vector2>().x != 0) || (playerMove.ReadValue<Vector2>().y != 0);
    }

    public bool IsArrowMoving()
    {
        return (arrowMove.ReadValue<Vector2>().x != 0) || (arrowMove.ReadValue<Vector2>().y != 0);
    }

    public bool IsPlayerControlTransfered()
    {
        return playerInput.Player.TransferToArrow.triggered;
    }

    public void DisableArrowActionMap()
    {
        playerInput.Arrow.Disable();
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static PlayerInputActions playerInput;
    public static event Action<InputActionMap> actionMapChange;

    private InputAction playerMove;
    private InputAction playerSprint;
    private InputAction playerJump;
    private InputAction playerTransferControl;

    private InputAction arrowMove;
    private InputAction arrowLook;
    private InputAction arrowAccelerate;
    private InputAction arrowTransferControl;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        playerInput = new PlayerInputActions();

        playerMove = playerInput.Player.Move;
        playerSprint = playerInput.Player.Sprint;
        playerJump = playerInput.Player.Jump;
        playerTransferControl = playerInput.Player.TransferControl;

        arrowMove = playerInput.Arrow.Move;
        arrowLook = playerInput.Arrow.Look;
        arrowAccelerate = playerInput.Arrow.Accelerate;
        arrowTransferControl = playerInput.Arrow.TransferControl;
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
        EnableActionMap(playerInput.Player);
    }

    public bool IsPlayerMoving()
    {
        return (playerMove.ReadValue<Vector2>().x != 0) || (playerMove.ReadValue<Vector2>().y != 0);
    }

    public bool IsArrowMoving()
    {
        return (arrowMove.ReadValue<Vector2>().x != 0) || (arrowMove.ReadValue<Vector2>().y != 0);
    }

    public void OnPlayerMapEnable()
    {
        Debug.Log("OnPlayerMapEnable()");

        playerSprint.performed += ThirdPersonController.Instance.OnSprintPressed;
        playerSprint.canceled += ThirdPersonController.Instance.OnSprintReleased;

        playerJump.performed += ThirdPersonController.Instance.OnJump;

        playerTransferControl.performed += ThirdPersonController.Instance.OnTransferControl;
    }
    public void OnPlayerMapDisable()
    {
        Debug.Log("OnPlayerMapDisable()");

        playerSprint.performed -= ThirdPersonController.Instance.OnSprintPressed;
        playerSprint.canceled -= ThirdPersonController.Instance.OnSprintReleased;

        playerJump.performed -= ThirdPersonController.Instance.OnJump;

        playerTransferControl.performed -= ThirdPersonController.Instance.OnTransferControl;
    }

    public void OnArrowMapEnable()
    {
        Debug.Log("OnArrowMapEnable()");

        arrowAccelerate.performed += ArrowController.Instance.OnAcceleratePressed;
        arrowAccelerate.canceled += ArrowController.Instance.OnAccelerateReleased;

        arrowTransferControl.performed += ArrowController.Instance.OnTransferControl;
    }

    public void OnArrowMapDisable()
    {
        Debug.Log("OnArrowMapDisable()");

        arrowAccelerate.performed -= ArrowController.Instance.OnAcceleratePressed;
        arrowAccelerate.canceled -= ArrowController.Instance.OnAccelerateReleased;

        arrowTransferControl.performed -= ArrowController.Instance.OnTransferControl;
    }
}

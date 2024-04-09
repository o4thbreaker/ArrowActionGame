using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static PlayerInputActions playerInput;
    public static event Action<InputActionMap> actionMapChange;

    private InputAction playerMove;
    private InputAction playerAim;
    private InputAction playerShoot;
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
        return arrowLook.ReadValue<Vector2>();
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
        playerAim = playerInput.Player.Aim;
        playerShoot = playerInput.Player.Shoot;
        playerSprint = playerInput.Player.Sprint;
        playerJump = playerInput.Player.Jump;
        playerTransferControl = playerInput.Player.TransferControl;

        arrowMove = playerInput.Arrow.Move;
        arrowLook = playerInput.Arrow.Look;
        arrowAccelerate = playerInput.Arrow.Accelerate;
        arrowTransferControl = playerInput.Arrow.TransferControl;
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnGameStart += EnableCharacter;
        PlayerStateManager.Instance.OnCharacterActivated += EnableCharacter;
        PlayerStateManager.Instance.OnArrowActivated += EnableArrow;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnGameStart -= EnableCharacter;
        PlayerStateManager.Instance.OnCharacterActivated -= EnableCharacter;
        PlayerStateManager.Instance.OnArrowActivated -= EnableArrow;
    }

    public static void EnableActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;

        playerInput.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }

    public bool IsPlayerMoving()
    {
        return (playerMove.ReadValue<Vector2>().x != 0) || (playerMove.ReadValue<Vector2>().y != 0);
    }

    public bool IsArrowMoving()
    {
        return (arrowMove.ReadValue<Vector2>().x != 0) || (arrowMove.ReadValue<Vector2>().y != 0);
    }

    private void EnableCharacter()
    {
        Debug.Log("EnableCharacter");

        playerSprint.performed += ThirdPersonController.Instance.OnSprintPressed;
        playerSprint.canceled += ThirdPersonController.Instance.OnSprintReleased;

        playerAim.performed += ThirdPersonController.Instance.OnAimPressed;
        playerAim.canceled += ThirdPersonController.Instance.OnAimReleased;

        playerShoot.performed += ThirdPersonController.Instance.OnShoot;

        playerJump.performed += ThirdPersonController.Instance.OnJump;

        playerTransferControl.performed += ThirdPersonController.Instance.OnTransferControl;

        EnableActionMap(playerInput.Player);

        //================DISABLE ARROW================

        Debug.Log("DisableArrow");

        arrowAccelerate.performed -= ArrowController.Instance.OnAcceleratePressed;
        arrowAccelerate.canceled -= ArrowController.Instance.OnAccelerateReleased;

        arrowTransferControl.performed -= ArrowController.Instance.OnTransferControl;
    }

    private void EnableArrow()
    {
        Debug.Log("EnableArrow");

        arrowAccelerate.performed += ArrowController.Instance.OnAcceleratePressed;
        arrowAccelerate.canceled += ArrowController.Instance.OnAccelerateReleased;

        arrowTransferControl.performed += ArrowController.Instance.OnTransferControl;

        EnableActionMap(playerInput.Arrow);

        // ================DISABLE PLAYER================

        Debug.Log("DisableCharacter");

        playerSprint.performed -= ThirdPersonController.Instance.OnSprintPressed;
        playerSprint.canceled -= ThirdPersonController.Instance.OnSprintReleased;

        playerAim.performed -= ThirdPersonController.Instance.OnAimPressed;
        playerAim.canceled -= ThirdPersonController.Instance.OnAimReleased;

        playerJump.performed -= ThirdPersonController.Instance.OnJump;

        playerTransferControl.performed -= ThirdPersonController.Instance.OnTransferControl;
    }
}

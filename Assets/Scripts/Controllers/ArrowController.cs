using UnityEngine;
using System;

public class ArrowController : MonoBehaviour
{
    public static ArrowController Instance { get; private set; }

    [SerializeField] private float flySpeed = 4;
    [SerializeField] private Transform arrowCamera;
    [SerializeField] private float turnSmoothing = 0.15f;
    [SerializeField] private float sprintFactor = 2f;
    [SerializeField] private TimeController timeController;

    private Rigidbody rb;
    private InputManager inputManager;

    private Vector3 lastDirection;
    private float previousUnscaledTimeFactor;
    public bool isControlTransferedToPlayer = false;
    public bool isArrowActive = false;
    private bool isAccelerating = false;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;

        Debug.Log("inputManager: " + inputManager);
    }

    private void Start()
    {
        previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
    }

    private void OnEnable()
    {
        /*InputManager.Instance.GetArrowAccelerate().performed += OnAcceleratePressed;
        InputManager.Instance.GetArrowAccelerate().canceled += OnAccelerateReleased;*/

        //playerInput.Arrow.Enable();
    }

    private void OnDisable()
    {
        /*InputManager.Instance.GetArrowAccelerate().performed -= OnAcceleratePressed;
        InputManager.Instance.GetArrowAccelerate().canceled -= OnAccelerateReleased;*/

        //playerInput.Arrow.Disable();
    }

    private void HandleFlight()
    {
        Vector3 direction = CalculateDirection(inputManager.GetArrowMovement().x, inputManager.GetArrowMovement().y);

        if (previousUnscaledTimeFactor != timeController.UnscaledTimeFactor)
        {
            rb.velocity = rb.velocity * timeController.UnscaledTimeFactor / previousUnscaledTimeFactor;
            previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
        }

        float speed = flySpeed * 10 * (isAccelerating ? sprintFactor : 1);
        float speedMultiplier = timeController.UnscaledTimeFactor;
        rb.velocity = speed * speedMultiplier * direction;

        HandleRotation();
    }

    private Vector3 CalculateDirection(float horizontal, float vertical)
    {
        Vector3 forward = arrowCamera.TransformDirection(Vector3.forward);
        // Camera forward Y component is relevant when flying.
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        // Calculate target direction based on camera forward and direction key.
        Vector3 targetDirection = forward * vertical + right * horizontal;

        // Return the current fly direction.
        return targetDirection;
    }

    private void HandleRotation()
    {
        Vector3 direction = rb.velocity;

        // Rotate the arrow to the correct fly position.
        if (inputManager.IsArrowMoving() && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);

            rb.MoveRotation(newRotation);
            SetLastDirection(direction);
        }
    }

    private void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    public void OnAcceleratePressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isAccelerating = true;

        ArrowCameraManager.Instance.SetFOV(45);
    }

    public void OnAccelerateReleased(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isAccelerating = false;

        ArrowCameraManager.Instance.ResetFOV();
    }

    private void TransferControl()
    {
        ArrowCameraManager.Instance.SetCameraInactive();
        CameraSwitcher.Instance.SwitchCameraPriority();
        InputManager.Instance.DisableArrowActionMap();
        InputManager.EnableActionMap(InputManager.playerInput.Player); 

        isControlTransferedToPlayer = true;
    }

    private void Update()
    {
        if (InputManager.playerInput.Arrow.enabled)
        {
            HandleFlight();

            /*if (Input.GetKey(KeyCode.N))
                TransferControl();*/
        }    
    }
}

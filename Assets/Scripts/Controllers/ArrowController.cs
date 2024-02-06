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

    private Vector3 lastDirection;
    private float previousUnscaledTimeFactor;
    public bool isControlTransferedToPlayer = false;
    public bool isArrowActive = false;

    private void Awake()
    {
        Instance = this;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
    }

    private void FlyManagement(float horizontal, float vertical)
    {
        Vector3 direction = Rotating(horizontal, vertical);

        if (previousUnscaledTimeFactor != timeController.UnscaledTimeFactor)
        {
            rb.velocity = rb.velocity * timeController.UnscaledTimeFactor / previousUnscaledTimeFactor;
            previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
        }

        float speed = flySpeed * 10 * (IsSprinting() ? sprintFactor : 1);
        float speedMultiplier = timeController.UnscaledTimeFactor;
        rb.velocity = speed * speedMultiplier * direction;
        HandleSprint();
    }

    private Vector3 Rotating(float horizontal, float vertical)
    {
        Vector3 forward = arrowCamera.TransformDirection(Vector3.forward);
        // Camera forward Y component is relevant when flying.
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        // Calculate target direction based on camera forward and direction key.
        Vector3 targetDirection = forward * vertical + right * horizontal;

        // Rotate the player to the correct fly position.
        if ((IsMoving() && targetDirection != Vector3.zero))
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);

            rb.MoveRotation(newRotation);
            SetLastDirection(targetDirection);
        }

        // Return the current fly direction.
        return targetDirection;
    }

    private void HandleSprint()
    {
        if (IsSprinting())
        {
            ArrowCameraManager.Instance.SetFOV(45);
        }
        else
        {
            ArrowCameraManager.Instance.ResetFOV();
        }
    }

    private bool IsMoving()
    {
        return (Input.GetAxis("Yaw") != 0) || (Input.GetAxis("Pitch") != 0);
    }

    private bool IsSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) && IsMoving();
    }

    private void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }
    private void TransferControl()
    {
        ArrowCameraManager.Instance.SetCameraInactive();
        CameraSwitcher.Instance.SwitchCameraPriority();
        InputManager.Instance.DisableArrowActionMap();
        InputManager.ToggleActionMap(InputManager.playerInputActions.Player); 

        isControlTransferedToPlayer = true;
    }

    private void Update()
    {
        if (InputManager.playerInputActions.Arrow.enabled)
        {
            FlyManagement(Input.GetAxisRaw("Yaw"), Input.GetAxisRaw("Pitch"));

            /*if (Input.GetKey(KeyCode.N))
                TransferControl();*/
        }    
    }
}

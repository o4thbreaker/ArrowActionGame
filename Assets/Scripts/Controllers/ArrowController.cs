using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    public static ArrowController Instance { get; private set; }

    [SerializeField] private float flySpeed = 4;
    [SerializeField] private float turnSmoothing = 0.15f;
    [SerializeField] private float sprintFactor = 2f;
    [SerializeField] private TimeController timeController;
    [SerializeField] private Transform arrowCamera;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform targetReturnTo;
    [SerializeField] private Transform curvePoint;


    private Rigidbody rb;
    private InputManager inputManager;

    private Vector3 lastDirection;
    private float previousUnscaledTimeFactor;
    public bool isControlTransferedToPlayer = false;
    public bool isArrowActive = false;
    private bool isAccelerating = false;
    private bool isReturning = false;
    private Vector3 oldPosition;
    private float returningTime = 0f;

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

        inputManager = InputManager.Instance;

        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
    }

    private void OnEnable()
    {
        inputManager.OnArrowMapEnable();
    }

    private void OnDisable()
    {
        inputManager.OnArrowMapDisable();
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
        float timeFactor = timeController.UnscaledTimeFactor;
        rb.velocity = speed * timeFactor * direction;

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

    public void ReturnArrow()
    {
        oldPosition = rb.position;
        isReturning = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private Vector3 GetBQCPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return p;
    }

    private void HandleReturning()
    {
        if (isReturning)
        {
            if (returningTime < 1.0f)
            {
                rb.position = GetBQCPoint(returningTime, oldPosition, curvePoint.position, targetReturnTo.position);
                returningTime += Time.unscaledDeltaTime;
            }
            else
            {
                isReturning = false;
                rb.position = targetReturnTo.position;
                rb.rotation = targetReturnTo.rotation;
                returningTime = 0f;
                rb.isKinematic = false;

                //gameObject.SetActive(false);
                transform.parent = parent;
            }
        }
    }

    private void SetLastDirection(Vector3 direction)
    {
        lastDirection = direction;
    }

    public void OnAcceleratePressed(InputAction.CallbackContext context)
    {
        isAccelerating = true;

        //ArrowCameraManager.Instance.SetFOV(45);
    }

    public void OnAccelerateReleased(InputAction.CallbackContext context)
    {
        isAccelerating = false;

        //ArrowCameraManager.Instance.ResetFOV();
    }

    public void OnTransferControl(InputAction.CallbackContext context)
    { 
        GameManager.Instance.UpdateState(GameManager.State.RepeatingArrowPath);
    }

    private void Update()
    {
        if (!RewindManager.Instance.IsBeingRewinded)
        {
            if (InputManager.playerInput.Arrow.enabled)
            {
                HandleFlight();
            }

            HandleReturning();
        }
    }
}

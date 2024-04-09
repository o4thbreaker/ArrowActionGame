using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowController : MonoBehaviour
{
    public static ArrowController Instance { get; private set; }

    [SerializeField] private float flySpeed = 4;
    [SerializeField] private float sprintFactor = 2f;
    [SerializeField] private TimeController timeController;
    [SerializeField] private Transform parent;
    [SerializeField] private Transform targetReturnTo;
    [SerializeField] private Transform curvePoint;

    private Rigidbody rb;
    private InputManager inputManager;

    private float previousUnscaledTimeFactor;
    public bool isControlTransferedToPlayer = false;
    public bool isArrowActive = false;
    private bool isAccelerating = false;

    private bool isReturning = false;
    private Vector3 oldPosition;
    private float returningTime = 0f;

    private Vector2 turn;

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
        PlayerStateManager.Instance.OnArrowActivated += EnableArrow;
        PlayerStateManager.Instance.OnCharacterActivated += DisableArrow;

        previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnArrowActivated -= EnableArrow;
        PlayerStateManager.Instance.OnCharacterActivated -= DisableArrow;
    }

    private void EnableArrow()
    {
        rb.isKinematic = false;
        GetComponentInChildren<CapsuleCollider>().enabled = true;
    }

    private void DisableArrow()
    {
        // NOTE: collider disables when in rewind mode. may cause problems in future

        rb.isKinematic = true;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
    }

    private void HandleFlight()
    {
        if (previousUnscaledTimeFactor != timeController.UnscaledTimeFactor)
        {
            rb.velocity = rb.velocity * timeController.UnscaledTimeFactor / previousUnscaledTimeFactor;
            previousUnscaledTimeFactor = timeController.UnscaledTimeFactor;
        }

        
        float speed = flySpeed * 10 * (isAccelerating ? sprintFactor : 1);
        float timeFactor = timeController.UnscaledTimeFactor;
        rb.velocity = speed * timeFactor * transform.forward;

        HandleRotation();
    }

    private void HandleRotation()
    {
        // TODO: fix the initial rotation bug (kinda fixed)
        // TODO: add button to normalise arrow rotation around z axis

        // NOTE: may be needed later
        /*turn += inputManager.GetArrowMovement();
        transform.rotation = Quaternion.Euler(-turn.y, turn.x, 0);*/

        turn = inputManager.GetArrowMovement();
        transform.Rotate(-turn.y, turn.x, 0, Space.Self);
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

                transform.parent = parent;
            }
        }
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
        PlayerStateManager.Instance.UpdateState(PlayerStateManager.playerState.RepeatingArrowPath);
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

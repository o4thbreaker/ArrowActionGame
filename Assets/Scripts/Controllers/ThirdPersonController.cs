using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController Instance { get; private set; }

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
    private InputManager inputManager;
    private Animator animator;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float sprintFactor = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSlopeAngle = 50f;
    [SerializeField] private float turnSmoothing = 0.15f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private LayerMask groundLayerMask = new LayerMask();
    [SerializeField] private Transform debugHitTransform;

    private Vector3 forceDirection = Vector3.zero;

    private int isWalkingHash;
    private int isRunningHash;
    private int throwHash;

    private bool isSprinting = false;
    private bool isAiming = false;

    private float groundedDrag;
    private float playerHeight;
    private float movementMultiplier = 100f;
    private RaycastHit slopeHit;
    

    private Vector3 mouseWorldPosition = Vector3.zero;

    public Action OnAimStart;
    public Action OnAimEnd;

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
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        throwHash = Animator.StringToHash("Throw");

        groundedDrag = rb.drag;
        playerHeight = capsuleCollider.height;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleAiming();
    }

    private void HandleMovement()
    {
        if (inputManager.IsPlayerMoving())
        {
            animator.SetBool(isWalkingHash, true);

            if (IsOnSlope())
                forceDirection = GetSlopeMoveDirection();
            else
                forceDirection = CalculateDirection(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y).normalized;

            rb.AddForce(forceDirection * playerSpeed * (isSprinting ? sprintFactor : 1) * Time.deltaTime * movementMultiplier); 

        }
        else
        {
            forceDirection = Vector3.zero;
            animator.SetBool(isWalkingHash, false);
        }

        rb.drag = IsGrounded() ? groundedDrag : 0;
        rb.useGravity = !IsOnSlope();

        //Debug.Log("grounded: " + IsGrounded());
        Debug.DrawLine(transform.position, (transform.position + Vector3.down), Color.red);
    }

    private bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        Vector3 moveDirection = CalculateDirection(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y);
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void HandleAiming()
    {
        // TODO: fix the tweaking between cameras bug
        // TODO: make crosshair appear only during aiming state

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, aimColliderMask))
        {
            mouseWorldPosition = hit.point;
        }
    }

    private void HandleRotation()
    {
        Vector3 direction = rb.velocity;

        if (inputManager.IsPlayerMoving() && direction.sqrMagnitude > 0.1f && !isAiming)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);
            rb.MoveRotation(newRotation);
        }
        else if (isAiming)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 40f);
        }
        else 
        {
            rb.angularVelocity = Vector3.zero; 
        }
    }

    private Vector3 CalculateDirection(float playerForward, float playerRight)
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 targetDirection = cameraForward * playerRight + cameraRight * playerForward;

        return targetDirection;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, playerHeight * 0.5f + 0.2f, groundLayerMask))
            return true;
        else
            return false;
    }

    public void OnSprintPressed(InputAction.CallbackContext context)
    {
        if (inputManager.IsPlayerMoving())
        {
            animator.SetBool(isRunningHash, true);
            isSprinting = true;
        }
    }

    public void OnSprintReleased(InputAction.CallbackContext context)
    { 
        animator.SetBool(isRunningHash, false);
        isSprinting = false;
    }

    public void OnAimPressed(InputAction.CallbackContext context)
    {
        isAiming = true;
        OnAimStart?.Invoke();
    }

    public void OnAimReleased(InputAction.CallbackContext context)
    {
        isAiming = false;
        OnAimEnd?.Invoke();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, aimColliderMask) && isAiming)
        {
            debugHitTransform.position = hit.point;
            GetComponent<ActivateArrow>().TriggerArrow();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnTransferControl(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.GetCooldownActive())
        {
            animator.Play(throwHash); //plays an animation with trigger
        }
    }
}

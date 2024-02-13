using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController Instance { get; private set; }

    private Rigidbody rb;
    private InputManager inputManager;
    private Animator animator;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnSmoothing = 0.15f;

    [SerializeField] private Camera playerCamera;

    private Vector3 forceDirection = Vector3.zero;
    private int isWalkingHash;
    private int isRunningHash;
    private bool isSprinting = false;
    private float groundedDrag;

    private void Awake()
    {
        Instance = this; 

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        inputManager = InputManager.Instance;
    }

    private void Start()
    {
        Cursor.visible = false;
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        groundedDrag = rb.drag;
    }

    private void OnEnable()
    {
       /* playerInput.Player.Sprint.performed += OnSprintPressed;
        playerInput.Player.Sprint.canceled += OnSprintReleased;

        playerInput.Player.Jump.performed += OnJump;
        
        playerInput.Player.Enable();*/
    }

    private void OnDisable()
    {
       /* playerInput.Player.Sprint.performed -= OnSprintPressed;
        playerInput.Player.Sprint.canceled -= OnSprintReleased;

        playerInput.Player.Jump.performed -= OnJump;

        playerInput.Player.Disable();*/
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        //bool isWalking = animator.GetBool(isWalkingHash); maybe do an if statement to check but not neccesary now
        if (inputManager.IsPlayerMoving())
        {
            animator.SetBool(isWalkingHash, true);

            forceDirection = CalculateDirection(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y);

            rb.AddForce(forceDirection * playerSpeed * (isSprinting ? sprintMultiplier : 1), ForceMode.Force); 

            // to remove movement after releasing the button
            forceDirection = Vector3.zero;
        }
        else
        {
            animator.SetBool(isWalkingHash, false);
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

        // probably did smth wrong here but it works so fine for now
        Vector3 targetDirection = cameraForward * playerRight + cameraRight * playerForward;

        return targetDirection;
    }

    private void HandleRotation()
    {
        Vector3 direction = rb.velocity;

        if (inputManager.IsPlayerMoving() && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);
            rb.MoveRotation(newRotation);
        }
        else 
            rb.angularVelocity = Vector3.zero;
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
        {
            rb.drag = groundedDrag;
            return true;
        }
        else
        {
            rb.drag = 0;
            return false;
        }
    }

    public void OnSprintPressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (inputManager.IsPlayerMoving())
        {
            animator.SetBool(isRunningHash, true);
            isSprinting = true;
        }
    }

    public void OnSprintReleased(UnityEngine.InputSystem.InputAction.CallbackContext context)
    { 
        animator.SetBool(isRunningHash, false);
        isSprinting = false;
    }

    public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.velocity += jumpForce * Vector3.up;
        }
    }
}

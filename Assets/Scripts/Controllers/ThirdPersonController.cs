using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    private PlayerInputActions playerInput;
    private InputAction move;
    private Animator animator;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnSmoothing = 0.15f;

    [SerializeField] private Camera playerCamera;

    private Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;
    private int isWalkingHash;
    private int isRunningHash;
    private bool isSprinting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInputActions();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.visible = false;
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    private void OnEnable()
    {
        move = playerInput.Player.Move;

        playerInput.Player.Sprint.performed += OnSprintPressed;
        playerInput.Player.Sprint.canceled += OnSprintReleased;

        playerInput.Player.Jump.performed += OnJump;
        
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Sprint.performed -= OnSprintPressed;
        playerInput.Player.Sprint.canceled -= OnSprintReleased;

        playerInput.Player.Jump.performed -= OnJump;

        playerInput.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        if (IsGrounded())
        {
            Debug.Log("isGrounded");
        }
        else
        {
            Debug.Log("-");
        }
    }

    private void HandleMovement()
    {
        //bool isWalking = animator.GetBool(isWalkingHash); maybe do an if statement to check but not neccesary now
        if (IsMoving())
        {
            animator.SetBool(isWalkingHash, true);

            forceDirection = CalculateDirection(move.ReadValue<Vector2>().x, move.ReadValue<Vector2>().y);

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

        if (IsMoving() && direction.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);
            rb.MoveRotation(newRotation);
        }
        else 
            rb.angularVelocity = Vector3.zero;
    }

    private bool IsMoving()
    {
        return (move.ReadValue<Vector2>().x != 0) || (move.ReadValue<Vector2>().y != 0);
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else
            return false;
    }

    private void OnSprintPressed(InputAction.CallbackContext context)
    {
        if (IsMoving())
        {
            animator.SetBool(isRunningHash, true);
            isSprinting = true;
        }
    }

    private void OnSprintReleased(InputAction.CallbackContext context)
    { 
        animator.SetBool(isRunningHash, false);
        isSprinting = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            rb.velocity += jumpForce * Vector3.up;
        }
    }
}

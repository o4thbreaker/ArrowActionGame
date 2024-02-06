using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    private PlayerInputActions playerInput;
    private InputAction move;

    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnSmoothing = 0.15f;

    [SerializeField] private Camera playerCamera;

    private Rigidbody rb;
    private Vector3 forceDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Player.Jump.started += OnJump;
        move = playerInput.Player.Move;
        playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        playerInput.Player.Jump.started -= OnJump;
        playerInput.Player.Disable();
    }

    private void FixedUpdate()
    {
        Debug.Log("ReadValue: " + move.ReadValue<Vector2>());
        HandleMovement(move.ReadValue<Vector2>().x, move.ReadValue<Vector2>().y);

        // to remove movement after releasing the button
        forceDirection = Vector3.zero;

        HandleJump();
    }

    private void HandleMovement(float forward, float right)
    {
        Vector3 direction = Rotating(forward, right);

        rb.AddForce(direction * movementForce, ForceMode.Impulse);
        Debug.Log(direction);
    }

    private Vector3 Rotating(float playerForward, float playerRight)
    {
        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // probably did smth wrong here but it works so fine for now
        Vector3 targetDirection = cameraForward * playerRight + cameraRight * playerForward;

        if (IsMoving() && targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, turnSmoothing);

            rb.MoveRotation(newRotation);
        }
        
        return targetDirection;
    }

    private bool IsMoving()
    {
        return (move.ReadValue<Vector2>().x != 0) || (move.ReadValue<Vector2>().y != 0);
    }

    private void HandleJump()
    {
        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;

        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
       if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else 
            return false;
    }
}

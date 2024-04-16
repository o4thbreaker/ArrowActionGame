using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    public static ThirdPersonController Instance { get; private set; }

    private CharacterController cc;
    private InputManager inputManager;
    private Animator animator;

    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float sprintFactor = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float turnSmoothing = 0.15f;
    [SerializeField] private float aimTurnSmoothing = 5;

    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = -.05f;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugHitTransform;

    private Vector3 moveDirection = Vector3.zero;
    private float currentGravity = 0f;

    private int isWalkingHash;
    private int isRunningHash;
    private int throwHash;

    private bool isSprinting = false;
    private bool isAiming = false;

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

        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }

    private void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        throwHash = Animator.StringToHash("Throw");
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

            moveDirection = CalculateDirection(inputManager.GetPlayerMovement().x, inputManager.GetPlayerMovement().y).normalized;
            moveDirection.y = GetGravityValue();

            cc.Move(moveDirection * playerSpeed * (isSprinting ? sprintFactor : 1) * Time.deltaTime); 
        }
        else
        {
            moveDirection.y = GetGravityValue();
            cc.Move(new Vector3(0f, moveDirection.y, 0f) * Time.deltaTime);

            animator.SetBool(isWalkingHash, false);
        }
    }

    private void HandleAiming()
    {
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
        Vector3 direction = moveDirection;
        direction.y = 0f;
        Quaternion currentRotation = transform.rotation;

        if (inputManager.IsPlayerMoving() && !isAiming)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, turnSmoothing * Time.deltaTime);
        }
        else if (isAiming)
        {
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, aimTurnSmoothing * Time.deltaTime);
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

    private float GetGravityValue()
    {
        if (cc.isGrounded)
        {
            currentGravity = 0f;
            return groundedGravity;
        }
        else
        {
            float previousGravity = currentGravity;
            float newGravity = currentGravity + (gravity * Time.deltaTime);
            float nextGravity = Mathf.Max((previousGravity + newGravity) * .5f, -20.0f);
            currentGravity = nextGravity;
            return currentGravity;
        }
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
   
    }

    public void OnTransferControl(InputAction.CallbackContext context)
    {
        if (!UIManager.Instance.GetCooldownActive())
        {
            animator.Play(throwHash); //plays an animation with trigger
        }
    }
}

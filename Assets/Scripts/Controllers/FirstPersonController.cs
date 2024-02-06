using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		[SerializeField] private float moveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
        [SerializeField] private float sprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
        [SerializeField] private float rotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		[SerializeField] private float speedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
        [SerializeField] private float jumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float jumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		[SerializeField] private float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] private bool grounded = true;
		[Tooltip("Useful for rough ground")]
        [SerializeField] private float groundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] private float groundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask groundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        [SerializeField] private GameObject cinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		[SerializeField] private float topClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		[SerializeField] private float bottomClamp = -90.0f;

		// cinemachine
		private float cinemachineTargetPitch;

		// player
		private float speed;
		private float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;

		// timeout deltatime
		private float jumpTimeoutDelta;
		private float fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput playerInput;
#endif
		private CharacterController controller;
		private InputManager inputManager;
		private GameObject mainCamera;

		private const float threshold = 0.01f;

		private bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return playerInput.currentControlScheme == "KeyboardMouse";
				#else
				return false;
				#endif
			}
		}

		private void Awake()
		{
			// get a reference to our main camera
			if (mainCamera == null)
			{
				mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			controller = GetComponent<CharacterController>();
			inputManager = GetComponent<InputManager>();
#if ENABLE_INPUT_SYSTEM
			playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			jumpTimeoutDelta = jumpTimeout;
			fallTimeoutDelta = fallTimeout;
		}

		private void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
			//TransferControl();
        }

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (inputManager.GetPlayerMouseDelta().sqrMagnitude >= threshold)
			{
				//Don't multiply mouse input by Time.deltaTime
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
				cinemachineTargetPitch += inputManager.GetPlayerMouseDelta().y * rotationSpeed * deltaTimeMultiplier;
				rotationVelocity = inputManager.GetPlayerMouseDelta().x * rotationSpeed * deltaTimeMultiplier;

				// clamp our pitch rotation
				cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

				// Update Cinemachine camera target pitch
				cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

				// rotate the player left and right
				transform.Rotate(Vector3.up * rotationVelocity);
			}
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = inputManager.IsPlayerRunning() ? sprintSpeed : moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (inputManager.GetPlayerMovement() == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = inputManager.analogMovement ? inputManager.GetPlayerMovement().magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(inputManager.GetPlayerMovement().x, 0.0f, inputManager.GetPlayerMovement().y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (inputManager.GetPlayerMovement() != Vector2.zero)
			{
				// move
				inputDirection = transform.right * inputManager.GetPlayerMovement().x + transform.forward * inputManager.GetPlayerMovement().y;
			}

			// move the player
			controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				fallTimeoutDelta = fallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (inputManager.IsPlayerJumping() && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				//inputManager.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private void TransferControl()
		{
				ArrowCameraManager.Instance.SetCameraActive();
				CameraSwitcher.Instance.SwitchCameraPriority();
				InputManager.ToggleActionMap(InputManager.playerInputActions.Arrow);
				
				ArrowController.Instance.isArrowActive = true;
				ArrowController.Instance.isControlTransferedToPlayer = false;
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z), groundedRadius);
		}
	}
}
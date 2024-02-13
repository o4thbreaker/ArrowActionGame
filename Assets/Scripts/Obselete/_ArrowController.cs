using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class _ArrowController : MonoBehaviour
{
    [Tooltip("Как сильно наращивает скорость вверх/вниз")]
    [SerializeField] private float throttleIncrement = 0.1f;

    [Tooltip("Максимальная тяга при 100% throttle")]
    [SerializeField] private float maxThrust = 200f;

    [Tooltip("Отзывчивость при повороте бочкой (rolling), наклонение носа (pitching), повороте носа влево/вправо (yawing)")]
    [SerializeField] private float responsiveness = 10f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;

    private Rigidbody rb;

    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        if (InputManager.playerInput.Arrow.enabled)
        {
            roll = Input.GetAxis("Roll");
            pitch = Input.GetAxis("Pitch");
            yaw = Input.GetAxis("Yaw");

            if (Input.GetKey(KeyCode.W))
            {
                throttle += throttleIncrement;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                throttle -= throttleIncrement;
            }

            throttle = Mathf.Clamp(throttle, 0f, 100f);
        }
    }

    private void Update()
    {
        HandleInputs();
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * maxThrust * throttle);
        rb.AddTorque(transform.up * yaw * responseModifier);
        rb.AddTorque(-transform.right * pitch * responseModifier);
        //rb.AddTorque(-transform.forward * roll * responseModifier);
    }
}

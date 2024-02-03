using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private List<Vector3> positions;
    private List<Quaternion> rotations;
    private int positionIndex;
    private int rotationIndex;
    private bool isMovementStarted = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();  
    }

    private void StartMove()
    {
        if (!isMovementStarted)
        {
            positions = PositionRecorder.Instance.GetPositionsList();
            rotations = PositionRecorder.Instance.GetRotationsList();
            //Debug.Log("positions[positionIndex]: " + positions[positionIndex]);
            transform.position = positions[positionIndex]; // but not in start
            transform.rotation = rotations[rotationIndex];
            isMovementStarted = true;
        }
    }

    private void Move()
    {
        if (ArrowController.Instance.isControlTransferedToPlayer)
        {
            StartMove();

            if (positionIndex <= positions.Count - 1)
            {

                Debug.Log("cur position list" + positions[positionIndex] + "i: " + positionIndex);
                Debug.Log("next pos: " + positions[positionIndex + 1]);
                Debug.Log("cur position" + transform.position);

                transform.position = Vector3.MoveTowards(transform.position, positions[positionIndex], moveSpeed * Time.deltaTime);
                transform.rotation = rotations[rotationIndex];
            }

            if (transform.position == positions[positionIndex])
            {
                //Debug.Log("entered index increment");
                positionIndex++;
                rotationIndex++;
            }
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
}

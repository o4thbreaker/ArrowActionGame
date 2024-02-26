using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    public static TransformRecorder Instance { get; private set; }

    private List<Vector3> recordedPositions;
    private List<Quaternion> recordedRotations;
    public bool recordingEnabled = false; //Set to true to start recording;

    private void Awake()
    {
        Instance = this;

        recordedPositions = new List<Vector3>();
        recordedRotations = new List<Quaternion>();
    }

    private void Update()
    {
        Record();
        Debug.Log(recordedPositions.Count);

        /*if (ArrowController.Instance.isControlTransferedToPlayer)
        {
            recordingEnabled = false;
            return;
        }
            
        if (CameraSwitcher.Instance.GetSwitchToArrow()) recordingEnabled = true;

        if (recordingEnabled)
        {
            positions.Add(transform.position);
            rotations.Add(transform.rotation);
        }*/
    }

    private void Record()
    {
        recordedPositions.Add(transform.position);
        recordedRotations.Add(transform.rotation);
    }

    public void RestoreFrame(int frame)
    {
        Debug.Assert(recordedPositions.Count > frame);
        transform.position = recordedPositions[frame];
        transform.rotation = recordedRotations[frame];
    }

    public List<Vector3> GetPositionsList()
    {
        return recordedPositions;
    }

    public List<Quaternion> GetRotationsList()
    {
        return recordedRotations;
    }
}

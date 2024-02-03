using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRecorder : MonoBehaviour
{
    public static PositionRecorder Instance { get; private set; }

    private List<Vector3> positions;
    private List<Quaternion> rotations;
    public bool recordingEnabled = false; //Set to true to start recording;

    private void Awake()
    {
        Instance = this;

        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
    }

    private void Update()
    {
        if (ArrowController.Instance.isControlTransferedToPlayer)
        {
            recordingEnabled = false;
            return;
        }
            

        if (CameraSwitcher.Instance.GetSwitchToArrow()) recordingEnabled = true;
        if (recordingEnabled)
        {
            positions.Add(transform.position);
            rotations.Add(transform.rotation);
        }
    }

    public List<Vector3> GetPositionsList()
    {
        return positions;
    }

    public List<Quaternion> GetRotationsList()
    {
        return rotations;
    }
}

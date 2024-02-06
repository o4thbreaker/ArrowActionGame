using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera arrowCamera; 

    private bool isPlayerCameraOn = true;
    private bool isArrowCameraOn = false;

    private bool isCameraSwitchedToArrow = false;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchCameraPriority()
    {
        if (isPlayerCameraOn)
        {
            playerCamera.Priority = 0;
            arrowCamera.Priority = 1;

            isCameraSwitchedToArrow = !isCameraSwitchedToArrow;
            isPlayerCameraOn = !isPlayerCameraOn;
        }
        else if (isArrowCameraOn)
        {
            arrowCamera.Priority = 0;
            playerCamera.Priority = 1;

            isArrowCameraOn = !isArrowCameraOn;
        }
    }

    public bool GetSwitchToArrow()
    {
        return isCameraSwitchedToArrow;
    }
}

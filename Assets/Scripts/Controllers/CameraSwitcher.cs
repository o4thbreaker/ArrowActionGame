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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchCameraPriority()
    {
        if (isPlayerCameraOn)
        {
            playerCamera.Priority = 0;
            arrowCamera.Priority = 1;

            isCameraSwitchedToArrow = !isCameraSwitchedToArrow;
            isPlayerCameraOn = !isPlayerCameraOn;
            isArrowCameraOn = true;
        }
        else if (isArrowCameraOn)
        {   
            arrowCamera.Priority = 0;
            playerCamera.Priority = 1;

            isArrowCameraOn = !isArrowCameraOn;
            isPlayerCameraOn = true;
        }
    }

    public bool GetSwitchToArrow()
    {
        return isCameraSwitchedToArrow;
    }
}

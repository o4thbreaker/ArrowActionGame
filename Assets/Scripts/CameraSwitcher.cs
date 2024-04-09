using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera playerAimCamera;
    [SerializeField] private CinemachineVirtualCamera arrowCamera;

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

    private void Start()
    {
        PlayerStateManager.Instance.OnArrowActivated += TurnOnArrowCamera;
        PlayerStateManager.Instance.OnArrowPathRepeated += TurnOnPlayerCamera;
        PlayerStateManager.Instance.OnCharacterActivated += TurnOnPlayerCamera;

        ThirdPersonController.Instance.OnAimStart += TurnOnAimCamera;
        ThirdPersonController.Instance.OnAimEnd += TurnOffAimCamera;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnArrowActivated -= TurnOnArrowCamera;
        PlayerStateManager.Instance.OnArrowPathRepeated -= TurnOnPlayerCamera;
        PlayerStateManager.Instance.OnCharacterActivated -= TurnOnPlayerCamera;

        ThirdPersonController.Instance.OnAimStart -= TurnOnAimCamera;
        ThirdPersonController.Instance.OnAimEnd -= TurnOffAimCamera;
    }

    private void TurnOnArrowCamera()
    {
        playerCamera.Priority = 0;
        arrowCamera.Priority = 1;
    }

    private void TurnOnPlayerCamera()
    {
        arrowCamera.Priority = 0;
        playerCamera.Priority = 1;
    }

    private void TurnOnAimCamera()
    {
        Debug.Log("TurnOnAimCamera");
        playerAimCamera.Priority = 2;
    }

    private void TurnOffAimCamera()
    {
        Debug.Log("TurnOffAimCamera");
        playerAimCamera.Priority = 0;
    }
}

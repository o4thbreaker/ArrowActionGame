using UnityEngine;
using Cinemachine;

public class ArrowCameraManager : MonoBehaviour 
{
	public static ArrowCameraManager Instance { get; private set; } 

    [SerializeField] private float defaultFOV = 60;                     // Default Camera's FOV

    private CinemachineVirtualCamera arrowVirtualCamera;               // Camera's reference

    private float targetFOV;                                           // Target camera Field of View

	void Awake()
	{
		Instance = this;
		Debug.Log(Instance);
		arrowVirtualCamera = GetComponent<CinemachineVirtualCamera>();

		ResetFOV();
	}

	void Update()
	{
        // Set FOV.
        arrowVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(arrowVirtualCamera.m_Lens.FieldOfView, targetFOV,  Time.unscaledDeltaTime);
	}

	// Set custom Field of View.
	public void SetFOV(float customFOV)
	{
		this.targetFOV = customFOV;
	}

	// Reset Field of View to default value.
	public void ResetFOV()
	{
		this.targetFOV = defaultFOV;
	}

	public void SetCameraActive()
	{
		arrowVirtualCamera.gameObject.SetActive(true);
	}

	public void SetCameraInactive()
	{
        arrowVirtualCamera.gameObject.SetActive(false);
    }
}

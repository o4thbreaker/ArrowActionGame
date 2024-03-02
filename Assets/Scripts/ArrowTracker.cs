using UnityEngine;

public class ArrowTracker : MonoBehaviour
{
    public static ArrowTracker Instance { get; private set; }

    private bool isTrackingAvailable = false;
    private float seconds = 0f;

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
        GameManager.Instance.OnArrowActivated += StartTracking;
        GameManager.Instance.OnArrowPathRepeated += StopTracking;
    }

    private void StartTracking()
    {
        isTrackingAvailable = true;
    }

    private void StopTracking()
    {
        isTrackingAvailable = false;
    }

    public float GetSeconds()
    {
        return seconds;
    }

    private void Update()
    {
        if (isTrackingAvailable)
        {
            seconds += Time.deltaTime;
        }
    }
}

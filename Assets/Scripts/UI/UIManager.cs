using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private ArrowTimer clock;

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
        GameManager.Instance.OnArrowActivated += ActivateArrowTimer;
        GameManager.Instance.OnArrowPathRepeated += ResetArrowTimer;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnArrowActivated -= ActivateArrowTimer;
        GameManager.Instance.OnArrowPathRepeated -= ResetArrowTimer;
    }

    private void ActivateArrowTimer()
    {
        clock.gameObject.SetActive(true);
    }

    private void ResetArrowTimer()
    {
        clock.ResetTimer();
        clock.gameObject.SetActive(false);
    }
}

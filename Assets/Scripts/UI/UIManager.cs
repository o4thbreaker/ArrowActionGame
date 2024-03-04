using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private ArrowModeTimer arrowModeClock;
    [SerializeField] private ArrowCooldownTimer arrowCooldownClock;

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

        GameManager.Instance.OnArrowActivated += ResetCooldownTimer;
        GameManager.Instance.OnCharacterActivated += ActivateCooldownTimer;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnArrowActivated -= ActivateArrowTimer;
        GameManager.Instance.OnArrowPathRepeated -= ResetArrowTimer;
    }

    private void ActivateArrowTimer()
    {
        arrowModeClock.gameObject.SetActive(true);
    }

    private void ResetArrowTimer()
    {
        arrowModeClock.ResetTimer();
        arrowModeClock.gameObject.SetActive(false);
    }

    private void ActivateCooldownTimer()
    {
        arrowCooldownClock.ActivateCooldown();
    }

    private void ResetCooldownTimer()
    {
        arrowCooldownClock.ResetCooldown();
    }

    public bool GetCooldownActive()
    {
        return arrowCooldownClock.IsCooldown;
    }
}

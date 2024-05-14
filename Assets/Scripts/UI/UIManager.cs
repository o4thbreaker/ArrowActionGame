using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private ArrowModeTimer arrowModeClock;
    [SerializeField] private ArrowCooldownTimer arrowCooldownClock;
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private MissionWaypoint missionWaypoint;

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
        PlayerStateManager.Instance.OnArrowActivated += ActivateArrowTimer;
        PlayerStateManager.Instance.OnArrowPathRepeated += ResetArrowTimer;

        PlayerStateManager.Instance.OnArrowActivated += ResetCooldownTimer;
        PlayerStateManager.Instance.OnCharacterActivated += ActivateCooldownTimer;

        GameManager.Instance.OnLevelEntered += SetUpLevelUI;
        GameManager.Instance.OnGameStart += SetUpGameUI;
        GameManager.Instance.OnLevelCompleted += SetWinUI;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnArrowActivated -= ActivateArrowTimer;
        PlayerStateManager.Instance.OnArrowPathRepeated -= ResetArrowTimer;
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

    private void SetUpGameUI()
    {
        missionText.text = "Go to enterance";
        missionWaypoint.gameObject.SetActive(true);
    }

    private void SetUpLevelUI()
    {
        missionWaypoint.gameObject.SetActive(false);
        missionText.text = "Eliminate all enemies";
    }

    private void SetWinUI()
    {
        missionText.text = $"<color=green>MISSION COMPLETE</color>";
    }
}

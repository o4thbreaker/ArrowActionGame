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
    [SerializeField] private TutorialUI tutorialUI;
    [SerializeField] private GameOverUI gameOverUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnArrowActivated += ActivateArrowTimer;
        PlayerStateManager.Instance.OnArrowActivated += ResetCooldownTimer;
        PlayerStateManager.Instance.OnArrowActivated += HideShootTutorialUI;

        PlayerStateManager.Instance.OnArrowPathRepeated += ResetArrowTimer;

        PlayerStateManager.Instance.OnCharacterActivated += ActivateCooldownTimer;

        GameManager.Instance.OnLevelEntered += SetUpLevelUI;
        GameManager.Instance.OnLevelEntered += HideArrowModeTutorialUI;
        GameManager.Instance.OnLevelEntered += ShowShootTutorialUI;

        GameManager.Instance.OnTutorial += ShowArrowModeTutorialUI;

        GameManager.Instance.OnGameStart += SetUpGameUI;
        GameManager.Instance.OnLevelCompleted += SetVictoryUI;
        GameManager.Instance.OnGameOver += SetUpGameOverUI;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnArrowActivated -= ActivateArrowTimer;
        PlayerStateManager.Instance.OnArrowActivated -= ResetCooldownTimer;
        PlayerStateManager.Instance.OnArrowActivated -= HideShootTutorialUI;

        PlayerStateManager.Instance.OnArrowPathRepeated -= ResetArrowTimer;

        PlayerStateManager.Instance.OnCharacterActivated -= ActivateCooldownTimer;

        GameManager.Instance.OnLevelEntered -= SetUpLevelUI;
        GameManager.Instance.OnLevelEntered -= HideArrowModeTutorialUI;
        GameManager.Instance.OnLevelEntered -= ShowShootTutorialUI;

        GameManager.Instance.OnTutorial -= ShowArrowModeTutorialUI;

        GameManager.Instance.OnGameStart -= SetUpGameUI;
        GameManager.Instance.OnLevelCompleted -= SetVictoryUI;
        GameManager.Instance.OnGameOver -= SetUpGameOverUI;
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

    private void SetVictoryUI()
    {
        missionText.text = $"<color=green>MISSION COMPLETE</color>";
    }

    private void ShowArrowModeTutorialUI()
    {
        tutorialUI.arrowModeTutorial.SetActive(true);
    }

    private void HideArrowModeTutorialUI()
    {
        tutorialUI.arrowModeTutorial.SetActive(false);
    }

    private void ShowShootTutorialUI()
    {
        tutorialUI.shootArrowTutorial.SetActive(true);
    }

    private void HideShootTutorialUI()
    {
        tutorialUI.shootArrowTutorial.SetActive(false);
    }

    private void SetUpGameOverUI()
    {
        gameOverUI.gameOverScreen.SetActive(true);
    }
}

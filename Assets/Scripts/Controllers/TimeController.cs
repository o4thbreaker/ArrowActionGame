using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float hintTimeScale = 0.01f;
    [SerializeField] private float arrowModeTimeScale = 0.2f;
    [SerializeField] private float shootTutorialTimeScale = 0.15f;
    [SerializeField] private float gameOverTimeScale = 0.001f;

    private float fixedTime = 0f;
    private float maxfixedTime = 0f;
    private int lastFrameChanged;
    float unscaledTimeFactor; // Set this to 1/Time.timeScale in Start
    private float previousUnscaledTimeFactor;
    private const float minUnscaledTimeFactor = 0.000001f;

    public float UnscaledTimeFactor => GetUnscaledTimeFactor();

    private void Start()
    {
        GameManager.Instance.OnTutorial += SetHintTimeScale;
        GameManager.Instance.OnLevelEntered += SetShootTutorialTimeScale;
        GameManager.Instance.OnGameOver += SetGameOverTimeScale;
        GameManager.Instance.OnGameStart += SetNormalTimeScale;

        PlayerStateManager.Instance.OnArrowActivated += SetArrowModeTimeScale;
        PlayerStateManager.Instance.OnCharacterActivated += SetNormalTimeScale;

        fixedTime = Time.fixedDeltaTime;
        maxfixedTime = Time.fixedDeltaTime;
        unscaledTimeFactor = 1 / Time.timeScale;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnTutorial -= SetHintTimeScale;
        GameManager.Instance.OnLevelEntered -= SetShootTutorialTimeScale;
        GameManager.Instance.OnGameOver -= SetGameOverTimeScale;
        GameManager.Instance.OnGameStart -= SetNormalTimeScale;

        PlayerStateManager.Instance.OnArrowActivated -= SetArrowModeTimeScale;
        PlayerStateManager.Instance.OnCharacterActivated -= SetNormalTimeScale;
    }

    private void SetHintTimeScale()
    {
        Time.timeScale = hintTimeScale;

        ChangeTimeScale();
    }

    private void SetArrowModeTimeScale()
    {
        Time.timeScale = arrowModeTimeScale;

        ChangeTimeScale();
    }

    private void SetShootTutorialTimeScale()
    {
        Time.timeScale = shootTutorialTimeScale;

        ChangeTimeScale();
    }

    private void SetNormalTimeScale()
    {
        Debug.Log("Set normal");
        Time.timeScale = 1f;

        ChangeTimeScale();
    }

    private void SetGameOverTimeScale()
    {
        Time.timeScale = gameOverTimeScale;

        ChangeTimeScale();
    }

    public void ChangeTimeScale()
    {
        Time.fixedDeltaTime = Mathf.Clamp(fixedTime * Time.timeScale, 0f, maxfixedTime);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        lastFrameChanged = Time.frameCount;

        previousUnscaledTimeFactor = unscaledTimeFactor;
        unscaledTimeFactor = 1 / Time.timeScale;
    }

    private float GetUnscaledTimeFactor()
    {
        float factor = Time.frameCount <= lastFrameChanged ? previousUnscaledTimeFactor : unscaledTimeFactor;
        return factor <= minUnscaledTimeFactor ? minUnscaledTimeFactor : factor;
    }

    private void Update()
    {
        //Debug.Log(Time.timeScale);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI speedText;

    private float fixedTime = 0f;
    private float maxfixedTime = 0f;
    private int lastFrameChanged;
    float unscaledTimeFactor; // Set this to 1/Time.timeScale in Start
    private float previousUnscaledTimeFactor;
    private const float minUnscaledTimeFactor = 0.000001f;

    private void Start()
    {
        fixedTime = Time.fixedDeltaTime;
        maxfixedTime = Time.fixedDeltaTime;
        unscaledTimeFactor = 1 / Time.timeScale;

        SetSlider();
    }

    private void Update()
    {
        ChangeTimeScale();
    }

    private void SetSlider()
    {
        slider.maxValue = 1.0f;
        slider.minValue = 0.01f;
        slider.value = 1f;
    }

    public void ChangeTimeScale()
    {
        /*if (ArrowController.Instance.isArrowActive)
        {
            Time.timeScale = 0.2f;
        }*/
        speedText.text = slider.value.ToString("N2");
        Time.timeScale = slider.value;

        Time.fixedDeltaTime = Mathf.Clamp(fixedTime * Time.timeScale, 0f, maxfixedTime);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        lastFrameChanged = Time.frameCount;

        previousUnscaledTimeFactor = unscaledTimeFactor;
        unscaledTimeFactor = 1 / Time.timeScale;
    }

    public float UnscaledTimeFactor => GetUnscaledTimeFactor();

    private float GetUnscaledTimeFactor()
    {
        float factor = Time.frameCount <= lastFrameChanged ? previousUnscaledTimeFactor : unscaledTimeFactor;
        return factor <= minUnscaledTimeFactor ? minUnscaledTimeFactor : factor;
    }

}

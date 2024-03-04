using UnityEngine;
using UnityEngine.UI;

public class ArrowModeTimer : MonoBehaviour
{
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;

    [SerializeField] private float maxArrowModeTime = 10f;
    private float timeLeft;
    private bool isTimeOut = false;

    private void Start()
    {
        timeLeft = maxArrowModeTime;
    }

    private void TimerCountdown()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.unscaledDeltaTime;
            redWheel.fillAmount = (timeLeft / maxArrowModeTime + 0.05f);
            greenWheel.fillAmount = (timeLeft / maxArrowModeTime);
        }
        else if (timeLeft < 0f)
        {
            timeLeft = 0f;
            GameManager.Instance.UpdateState(GameManager.State.RepeatingArrowPath);
            //ArrowController.Instance.ReturnArrow();

            isTimeOut = true;
        }
    }

    public void ResetTimer()
    {
        timeLeft = maxArrowModeTime;
        isTimeOut = false;
    }

    public bool GetTimeOut()
    {
        return isTimeOut;
    }

    private void Update()
    {
        TimerCountdown();
    }
}

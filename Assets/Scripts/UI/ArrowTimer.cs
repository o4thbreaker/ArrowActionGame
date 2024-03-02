using UnityEngine;
using UnityEngine.UI;

public class ArrowTimer : MonoBehaviour
{
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;

    [SerializeField] private float maxTime = 10f;
    private float timeLeft;
    private bool isTimeOut = false;

    private void Start()
    {
        timeLeft = maxTime;
    }

    private void TimerCountdown()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.unscaledDeltaTime;
            redWheel.fillAmount = (timeLeft / maxTime + 0.05f);
            greenWheel.fillAmount = (timeLeft / maxTime);
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
        timeLeft = maxTime;
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

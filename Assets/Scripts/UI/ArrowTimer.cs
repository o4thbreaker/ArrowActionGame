using UnityEngine;
using UnityEngine.UI;

public class ArrowTimer : MonoBehaviour
{
    [SerializeField] private Image greenWheel;
    [SerializeField] private Image redWheel;

    [SerializeField] private float maxTime = 10f;
    private float timeLeft;

    private void Start()
    {
        timeLeft = maxTime;
    }

    private void Countdown()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.unscaledDeltaTime;
            redWheel.fillAmount = (timeLeft / maxTime + 0.07f);
            greenWheel.fillAmount = (timeLeft / maxTime);
        }
        else if (timeLeft < 0f)
        {
            timeLeft = 0f;
            GameManager.Instance.UpdateState(GameManager.State.ControllingCharacter);
            ArrowController.Instance.ReturnArrow();
        }
    }

    public void ResetTimer()
    {
        timeLeft = maxTime;
    }

    private void Update()
    {
        Countdown();
    }
}

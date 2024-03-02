using UnityEngine;

public class RewindByKeyPress : MonoBehaviour
{
    [SerializeField] private float rewindIntensity = 0.02f;          //Variable to change rewind speed
    private bool isRewinding = false;
    private float rewindValue = 0;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            rewindValue += rewindIntensity;  //While holding the button, we will gradually rewind more and more time into the past

            if (!isRewinding)
            {
                RewindManager.Instance.StartRewindTimeBySeconds(rewindValue);
            }

            else
            {
                if (RewindManager.Instance.HowManySecondsAvailableForRewind > rewindValue) //Safety check so it is not grabbing values out of the bounds
                    RewindManager.Instance.SetTimeSecondsInRewind(rewindValue);
            }

            isRewinding = true;
        }

        else
        {
            if (isRewinding)
            {
                RewindManager.Instance.StopRewindTimeBySeconds();
                rewindValue = 0;
                isRewinding = false;
            }
        }
    }
}

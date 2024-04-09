using UnityEngine;

public class RewindByTimer : MonoBehaviour
{
    private bool isRewindEnabled = false;

    private void Start()
    {
        PlayerStateManager.Instance.OnArrowPathRepeated += DisableRewind;
        PlayerStateManager.Instance.OnCharacterActivated += EnableRewind;
    }

    private void OnDestroy()
    {
        PlayerStateManager.Instance.OnArrowPathRepeated -= DisableRewind;
        PlayerStateManager.Instance.OnCharacterActivated -= EnableRewind;
    }

    private void EnableRewind()
    {
        isRewindEnabled = true;
    }

    private void DisableRewind()
    {
        isRewindEnabled = false;
    }

    private void FixedUpdate()
    {
        if (isRewindEnabled)
        {
            float seconds = RewindManager.Instance.HowManySecondsAvailableForRewind - 0.01f;
            RewindManager.Instance.InstantRewindTimeBySeconds(seconds);

            isRewindEnabled = false;
        } 
    }
}

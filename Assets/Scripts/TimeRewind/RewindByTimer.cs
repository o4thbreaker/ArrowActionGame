using UnityEngine;

public class RewindByTimer : MonoBehaviour
{
    private bool isRewindEnabled = false;
    private float rewindSeconds = 0f;

    private void Start()
    {
        GameManager.Instance.OnArrowPathRepeated += DisableRewind;
        GameManager.Instance.OnCharacterActivated += EnableRewind;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnArrowPathRepeated -= DisableRewind;
        GameManager.Instance.OnCharacterActivated -= EnableRewind;
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
            RewindManager.Instance.InstantRewindTimeBySeconds(ArrowTracker.Instance.GetSeconds());
        } 
    }
}

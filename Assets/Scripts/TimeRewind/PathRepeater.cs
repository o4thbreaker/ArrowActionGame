using UnityEngine;

public class PathRepeater : MonoBehaviour
{
    public bool CanReturnArrow { get; private set; }

    private RewindManager rewindManager;
    private bool graduallyMoveForward = false;
    private float lastRewindTime;

    private void Awake()
    {
        rewindManager = GetComponent<RewindManager>();
    }

    private void Start()
    {
        PlayerStateManager.Instance.OnArrowActivated += StartTracking;
        PlayerStateManager.Instance.OnArrowPathRepeated += StopTracking;
    }

    private void FixedUpdate()
    {
        if (graduallyMoveForward)
        {
            lastRewindTime -= Time.fixedDeltaTime;

            if (lastRewindTime > 0)
                rewindManager.SetTimeSecondsInRewind(lastRewindTime);
            else
            {
                graduallyMoveForward = false;
                rewindManager.StopRewindTimeBySeconds();
                Debug.Log("Object is now in present");
                // call a return arrow
                ArrowController.Instance.ReturnArrow();
            }
        }
    }

    public void StartTracking()
    {
        if (rewindManager.IsBeingRewinded)
            rewindManager.StopRewindTimeBySeconds();

        rewindManager.RestartTracking();
        Debug.Log("Tracking started");
    }


    public void StopTracking()
    {
        lastRewindTime = rewindManager.HowManySecondsAvailableForRewind;
        rewindManager.StartRewindTimeBySeconds(lastRewindTime);
        graduallyMoveForward = true;
        Debug.Log("Moving back to past");

        PlayerStateManager.Instance.UpdateState(PlayerStateManager.playerState.ControllingCharacter);
    }
}

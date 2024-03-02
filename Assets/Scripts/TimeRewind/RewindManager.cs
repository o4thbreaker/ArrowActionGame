using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance { get; private set; }

    [field:SerializeField] public float HowManySecondsToTrack { get; private set; } = 12;

    public float HowManySecondsAvailableForRewind { get; private set; }

    public bool IsBeingRewinded { get; private set; } = false;
    
    public bool TrackingEnabled { get; set; } = false;

    private float rewindSeconds = 0;
    private List<RewindAbstract> rewindedObjects;

    private void OnEnable()
    {
        HowManySecondsAvailableForRewind = 0;
    }

    private void Awake()
    {
        // warning!!! may cause unpredictable behaviour cuz (true) tracks inactive objects aswell
        rewindedObjects = FindObjectsOfType<RewindAbstract>(true).ToList();

        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;

        rewindedObjects.ForEach(x => x.MainInit());
    }

    private void Start()
    {
        GameManager.Instance.OnArrowActivated += EnableTracking;
        GameManager.Instance.OnArrowPathRepeated += DisableTracking;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnArrowActivated -= EnableTracking;
        GameManager.Instance.OnArrowPathRepeated -= DisableTracking;
    }

    private void FixedUpdate()
    {
        if (IsBeingRewinded)
        {
            rewindedObjects.ForEach(x => x.Rewind(rewindSeconds));
        }
        else
        {
            rewindedObjects.ForEach(x => x.Track());

            if (TrackingEnabled)
                HowManySecondsAvailableForRewind = Mathf.Min(HowManySecondsAvailableForRewind + Time.fixedDeltaTime, HowManySecondsToTrack);
        }
    }

    public void InstantRewindTimeBySeconds(float seconds)
    {
        if (seconds > HowManySecondsAvailableForRewind)
        {
            Debug.LogError("Not enough stored tracked value!!! Reaching on wrong index. " +
                "Called rewind should be less than HowManySecondsAvailableForRewind property");
            return;
        }
        if (seconds < 0)
        {
            Debug.LogError("Parameter in RewindTimeBySeconds() must have positive value!!!");
            return;
        }
        rewindedObjects.ForEach(x => x.Rewind(seconds));
        HowManySecondsAvailableForRewind -= seconds;
        BuffersRestore?.Invoke(seconds);
    }

    // Call this method if you want to start rewinding time with ability to preview snapshots. 
    // After done rewinding, StopRewindTimeBySeconds() must be called!!! To update snapshot preview between, call method SetTimeSecondsInRewind().
    public void StartRewindTimeBySeconds(float seconds)
    {
        if (IsBeingRewinded)
            Debug.LogError("The previous rewind must be stopped by calling StopRewindTimeBySeconds() before you start another rewind");

        CheckReachingOutOfBounds(seconds);

        rewindSeconds = seconds;
        IsBeingRewinded = true;
    }

    // Call this method to update rewind preview while rewind is active (StartRewindTimeBySeconds() method was called before)
    public void SetTimeSecondsInRewind(float seconds)
    {
        CheckReachingOutOfBounds(seconds);
        rewindSeconds = seconds;
    }

    // Call this method to stop previewing rewind state and resume normal game flow
    public void StopRewindTimeBySeconds()
    {
        if (!IsBeingRewinded)
            Debug.LogError("Rewind must be started before you try to stop it. StartRewindTimeBySeconds() must be called first");

        HowManySecondsAvailableForRewind -= rewindSeconds;
        BuffersRestore?.Invoke(rewindSeconds);
        IsBeingRewinded = false;
    }

    public void RestartTracking()
    {
        if (IsBeingRewinded)
            StopRewindTimeBySeconds();

        HowManySecondsAvailableForRewind = 0;
        TrackingEnabled = true;
    }

    private void CheckReachingOutOfBounds(float seconds)
    {
        if (Mathf.Round(seconds * 100) > Mathf.Round(HowManySecondsAvailableForRewind * 100))
        {
            Debug.LogError("Not enough stored tracked value!!! Reaching on wrong index. " +
                "Called rewind should be less than HowManySecondsAvailableForRewind property");
            return;
        }
        if (seconds < 0)
        {
            Debug.LogError("Parameter in StartRewindTimeBySeconds() must have positive value!!!");
            return;
        }
    }

    private void EnableTracking()
    {
        TrackingEnabled = true;
    }

    private void DisableTracking()
    {
        TrackingEnabled = false;
    }

    /// <summary>
    /// This action is not meant to be used by users. CircularBuffers listens to it
    /// </summary>
    public static Action<float> BuffersRestore { get; set; }
}


using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaybackController : MonoBehaviour
{
    public bool isPlayback = false;
    private int playbackFrame = 0;
    private int playbackSpeed = 0;

    public int Frame => playbackFrame;
    public bool IsPlayback => isPlayback;

    public void Pause()
    {

    }

    public void Resume()
    {

    }

    private void RestoreFrame()
    {
        /*foreach (var recorder in recorders)
        {
            recorder.RestoreFrame(playbackFrame);
        }*/

        TransformRecorder.Instance.RestoreFrame(playbackFrame);
    }

    private void Update()
    {
        Debug.Log(playbackFrame);
        if (isPlayback)
        {
            playbackFrame = Mathf.Max(0, playbackFrame + playbackSpeed);
            RestoreFrame();
        }
        else
        {
            playbackFrame++;
        }
    }
}

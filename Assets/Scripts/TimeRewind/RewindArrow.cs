using UnityEngine;

public class RewindArrow : RewindAbstract
{
    [SerializeField] bool trackObjectActiveState;
    [SerializeField] bool trackTransform;
    [SerializeField] bool trackVelocity;

    
    public override void Rewind(float seconds)
    {
        if (trackObjectActiveState)
            RestoreObjectActiveState(seconds);
        if (trackTransform)
            RestoreTransform(seconds);
        if (trackVelocity)
            RestoreVelocity(seconds);
    }

    public override void Track()
    {
        if (trackObjectActiveState)
            TrackObjectActiveState();
        if (trackTransform)
            TrackTransform();
        if (trackVelocity)
            TrackVelocity();
    }
}

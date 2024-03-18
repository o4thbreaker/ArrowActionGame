using UnityEngine;

public class WeaponIk : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform bone;

    public int iterations = 10;
    public float angleLimit = 90f;
    public float distanceLimit = 1.5f;

    private void LateUpdate()
    {
        Vector3 targetPosition = GetTargetPosition();

        for (int i = 0; i < iterations; i++)
            AimAtTarget(bone, targetPosition);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Vector3 aimDirection = aimTransform.forward;
        float blendOut = 0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > angleLimit)
        {
            blendOut += (targetAngle - angleLimit) / 50f;
        }

        float targetDistance = targetDirection.magnitude;
        if (targetDistance < distanceLimit)
        {
            blendOut += distanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return aimTransform.position + direction;
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);

        bone.rotation = aimTowards * bone.rotation;
    }
}

using System;
using UnityEngine;

[Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
}

public class ShootingControl : MonoBehaviour
{
    [SerializeField] private Transform bone;

    [Range(0,1)] [SerializeField] private float boneWeight = 1.0f;

    [SerializeField] private float angleLimit = 90f;
    [SerializeField] private float distanceLimit = 1.5f;
    [SerializeField] private Vector3 aimingOffset;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private LayerMask shootingLayerMask;

    private Transform targetTransform;
    private Transform aimTransform;


    // animation trigger
    private void Shoot()
    {
        if (GetRaycastHit())
        {
            Debug.DrawRay(gunMuzzle.position, gunMuzzle.forward * 50, Color.red, 1f);

            GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.parent.rotation);

            muzzleFlash.Emit(1);
        }
    }
    public bool GetRaycastHit()
    {
        return Physics.Raycast(gunMuzzle.position, gunMuzzle.position + gunMuzzle.forward * 50, 
            out RaycastHit hit, float.MaxValue, shootingLayerMask);
    }

    // NOTE: may be used later
    private Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = (targetTransform.position + aimingOffset) - aimTransform.position;
       
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

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float boneWeight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);

        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, boneWeight);
        bone.rotation = blendedRotation * bone.rotation;
    }

    public void SetTargetTransform(Transform target)
    {
        targetTransform = target;
    }

    public void SetAimTransform(Transform target)
    {
        aimTransform = target;
    }
}

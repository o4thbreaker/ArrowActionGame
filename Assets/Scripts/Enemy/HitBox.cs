using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public event EventHandler OnCollideWithArrow;
    public CapsuleCollider arrow;

    private void OnTriggerExit(Collider other)
    {
        if (other == arrow)
        {
            OnCollideWithArrow?.Invoke(this, EventArgs.Empty);
            arrow.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<CapsuleCollider>() == arrow)
            arrow.isTrigger = true;
    }
}

using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    public event EventHandler OnCollideWithArrow;
    public CapsuleCollider arrow;
    public static event Action OnKill;
    private bool isKilled;


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
        {
            arrow.isTrigger = true;
            if (!isKilled)
            {
                OnKill?.Invoke();
            }
        }
        else
        {
            arrow.isTrigger = false;
        }
    }
}

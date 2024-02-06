using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Ragdoll ragdoll;
    [SerializeField] private CapsuleCollider arrow;

    private bool isDead = false;

    private void Start()
    {
        ragdoll = GetComponent<Ragdoll>();

        var rigidBodies = GetComponentsInChildren<Rigidbody>();

        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.arrow = arrow;
            hitBox.OnCollideWithArrow += HitBox_OnCollideWithArrow;
        }
    }

    private void HitBox_OnCollideWithArrow(object sender, EventArgs e)
    {
        Debug.Log("HitBox_OnCollideWithArrow");
        Die();
    }

    private void Die()
    {
        isDead = true;
        ragdoll.ActivateRagdoll();
    }

    public bool IsDead()
    {
        return isDead;
    }
}

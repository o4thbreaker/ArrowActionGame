using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    private Rigidbody[] rigidBodies;
    private Animator animator;

    private void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidBody in rigidBodies)
        {
            rigidBody.isKinematic = false;
        }
        animator.enabled = false;
    }
}

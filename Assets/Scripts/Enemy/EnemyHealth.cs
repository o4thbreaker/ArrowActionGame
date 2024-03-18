using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private AIAgent agent;
    [SerializeField] private CapsuleCollider arrow;

    private void Start()
    {
        agent = GetComponent<AIAgent>();

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
        agent.stateMachine.ChangeState(AIStateId.Death);
    }

}

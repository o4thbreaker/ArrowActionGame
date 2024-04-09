using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private AIAgent agent;
    private CapsuleCollider arrow;

    private void Start()
    {
        agent = GetComponent<AIAgent>();
        arrow = ArrowController.Instance.GetComponentInChildren<CapsuleCollider>();

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
        if (!agent.stateMachine.CheckCurrentState(AIStateId.Death))
        {
            
            agent.stateMachine.ChangeState(AIStateId.Death);
           
        }
            
    }

}

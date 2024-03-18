using UnityEngine;

public class IdleState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Idle;
    }

    public void EnterState(AIAgent agent)
    {
        
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        Vector3 playerDirection = agent.target.position - agent.transform.position;

        if (playerDirection.magnitude > agent.config.maxDetectionDistance)
            return;

        Vector3 agentDirection = agent.transform.forward;
        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        if (dotProduct > 0) // target is in front (or in sight to be more precise)
        {
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
        }
    }
}

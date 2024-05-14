using UnityEngine;
public class RunToCoverState : AIState
{
    private string speed = "speed";
    public AIStateId GetId()
    {
        return AIStateId.RunToCover;
    }

    public void EnterState(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0;

        Cover nextCover = agent.coverArea.GetClosestCover(agent.transform.position);

        agent.navMeshAgent.SetDestination(nextCover.transform.position);
    }

    public void Update(AIAgent agent)
    {
        agent.animator.SetFloat(speed, agent.navMeshAgent.desiredVelocity.sqrMagnitude);

        if (HasArrivedToCover(agent))
        {
            agent.stateMachine.ChangeState(AIStateId.Delay);
        }
    }

    public void ExitState(AIAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = agent.stoppingDistance;
        agent.animator.SetFloat(speed, 0);
    }

    public bool HasArrivedToCover(AIAgent agent)
    {
        return agent.navMeshAgent.remainingDistance < 0.12f;
    }
}

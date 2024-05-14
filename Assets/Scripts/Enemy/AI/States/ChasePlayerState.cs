using UnityEngine;

public class ChasePlayerState : AIState
{
    private float shootingDistance;
    private bool targetInRange = false;
    private float pathUpdateDeadline;
    private string speed = "speed";
    private string isCombat = "isCombat";

    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void EnterState(AIAgent agent)
    {
        shootingDistance = agent.navMeshAgent.stoppingDistance;
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        if (agent.target != null)
        {
            targetInRange = Vector3.Distance(agent.transform.position, agent.target.position) <= shootingDistance;

            agent.animator.SetBool(isCombat, targetInRange);

            if (targetInRange)
                agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
            else
                UpdatePath(agent);
        }
        agent.animator.SetFloat(speed, agent.navMeshAgent.desiredVelocity.sqrMagnitude);
    }

    private void UpdatePath(AIAgent agent)
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + agent.config.pathUpdateDelay;
            agent.navMeshAgent.SetDestination(agent.target.position);
        }
    }
}

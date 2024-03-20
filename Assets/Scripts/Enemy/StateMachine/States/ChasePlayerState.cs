using UnityEngine;

public class ChasePlayerState : AIState
{
    private float shootingDistance;
    private bool targetInRange = false;
    private float pathUpdateDeadline;
    private string speed = "speed";
    private string isAiming = "isAiming";

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

            agent.animator.SetBool(isAiming, targetInRange);

            if (targetInRange)
                agent.stateMachine.ChangeState(AIStateId.AttackPlayer);
            else
                UpdatePath(agent);
        }
        agent.animator.SetFloat(speed, agent.navMeshAgent.desiredVelocity.sqrMagnitude);
    }

    private void LookAtTarget(AIAgent agent)
    {
        Vector3 lookPosition = agent.target.position - agent.gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, agent.config.rotateSpeed * Time.deltaTime);
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

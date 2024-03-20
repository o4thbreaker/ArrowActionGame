using UnityEngine;

public class AttackPlayerState : AIState
{
    private float shootingDistance;
    private string isShooting = "isShooting";

    public AIStateId GetId()
    {
        return AIStateId.AttackPlayer;
    }

    public void EnterState(AIAgent agent)
    {
        agent.weapon.SetAimTransform(agent.gunMuzzle);
        agent.weapon.SetTargetTransform(agent.target);

        agent.animator.SetBool(isShooting, true);

        shootingDistance = agent.navMeshAgent.stoppingDistance;
    }

    public void ExitState(AIAgent agent)
    {
        agent.weapon.SetAimTransform(null);
        agent.weapon.SetShooting(false);

        agent.animator.SetBool(isShooting, false);
    }

    public void Update(AIAgent agent)
    {
        bool targetInRange = Vector3.Distance(agent.transform.position, agent.target.position) <= shootingDistance;

        if (targetInRange)
            agent.weapon.SetShooting(true);
        else
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
    }
}

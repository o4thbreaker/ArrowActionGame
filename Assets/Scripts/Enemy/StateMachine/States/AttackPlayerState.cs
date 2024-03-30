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
        
        if (agent.weapon.GetRaycastHit())
        {
            LookAtTarget(agent);
            agent.weapon.SetShooting(true);
        }
        else
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
    }

    private void LookAtTarget(AIAgent agent)
    {
        Vector3 lookPosition = agent.target.position - agent.gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, agent.config.rotateSpeed * Time.deltaTime);
    }
}

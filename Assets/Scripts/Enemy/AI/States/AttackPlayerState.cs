using UnityEngine;

public class AttackPlayerState : AIState
{
    private string isShooting = "isShooting";

    public AIStateId GetId()
    {
        return AIStateId.AttackPlayer;
    }

    public void EnterState(AIAgent agent)
    {
        Debug.Log("entering");

        agent.weapon.SetAimTransform(agent.gunMuzzle);
        agent.weapon.SetTargetTransform(agent.target);

        agent.animator.SetBool(isShooting, true);
    }

    public void Update(AIAgent agent) 
    {
        if (agent.weapon.GetRaycastHit())
        {
            LookAtTarget(agent);
        }
        else
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
    }

    public void ExitState(AIAgent agent)
    {
        Debug.Log("exiting");

        agent.weapon.SetAimTransform(null);
        agent.weapon.SetTargetTransform(null);

        agent.animator.SetBool(isShooting, false);
    }

    private void LookAtTarget(AIAgent agent)
    {
        Vector3 lookPosition = agent.target.position - agent.gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, agent.config.rotateSpeed * Time.deltaTime);
    }
}

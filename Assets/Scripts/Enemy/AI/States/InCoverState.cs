using UnityEngine;

public class InCoverState : AIState
{
    [HideInInspector] public AIStateMachine stateMachine;

    private string isCombat = "isCombat";

    public AIStateId GetId()
    {
       return AIStateId.InCover;
    }

    public InCoverState(AIAgent agent)
    {
        stateMachine = new AIStateMachine(agent);

        stateMachine.RegisterState(new AttackPlayerState());
        stateMachine.RegisterState(new DelayState(stateMachine, AIStateId.AttackPlayer, 1f));
    }

    public void EnterState(AIAgent agent)
    {
        agent.animator.SetBool(isCombat, true);

        stateMachine.ChangeState(AIStateId.AttackPlayer);
    }

    public void Update(AIAgent agent)
    {
        stateMachine.UpdateState();
    }

    public void ExitState(AIAgent agent)
    {
        agent.animator.SetBool(isCombat, false);
    }
}

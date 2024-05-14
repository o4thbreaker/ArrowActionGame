
using UnityEngine;

public class DelayState : AIState
{
    private AIStateMachine stateMachine;
    private AIStateId id;
    private float waitForSeconds;

    private float deadline;

    public DelayState(AIStateMachine stateMachine, AIStateId id, float waitForSeconds)
    {
        this.stateMachine = stateMachine;
        this.id = id;
        this.waitForSeconds = waitForSeconds;
    }

    public AIStateId GetId()
    {
        return AIStateId.Delay;
    }

    public void EnterState(AIAgent agent)
    {
        deadline = Time.time + waitForSeconds;
    }

    public void Update(AIAgent agent)
    {
        if (IsDone())
        {
            stateMachine.ChangeState(id);
        }
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public bool IsDone()
    {
        return Time.time >= deadline;
    }
}

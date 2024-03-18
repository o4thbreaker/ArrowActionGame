using UnityEngine;

public class DeathState : AIState
{
    public AIStateId GetId()
    {
        return AIStateId.Death;
    }

    public void EnterState(AIAgent agent)
    {
        agent.ragdoll.ActivateRagdoll();
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
       
    }
}

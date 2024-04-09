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
        AIAgent.totalEnemiesAmount--;
        Debug.Log(AIAgent.totalEnemiesAmount);

        if (AIAgent.totalEnemiesAmount == 0)
        {
            GameManager.Instance.UpdateState(GameManager.gameState.LevelCompleted);
        }
    }

    public void ExitState(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
       
    }
}


public enum AIStateId
{
    ChasePlayer,
    Death,
    Idle,
    AttackPlayer
}

public interface AIState
{
    AIStateId GetId();
    void EnterState(AIAgent agent);
    void Update(AIAgent agent);
    void ExitState(AIAgent agent);
}

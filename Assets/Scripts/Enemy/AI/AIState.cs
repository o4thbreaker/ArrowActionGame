public enum AIStateId
{
    Idle,
    RunToCover,
    InCover,
    ChasePlayer,
    AttackPlayer,
    Death,
    Delay
}

public interface AIState
{
    AIStateId GetId();
    void EnterState(AIAgent agent);
    void Update(AIAgent agent);
    void ExitState(AIAgent agent);
}

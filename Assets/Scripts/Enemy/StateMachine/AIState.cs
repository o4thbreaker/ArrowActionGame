
public enum AIStateId
{
    ChasePlayer,
    Death,
    Idle
}

public interface AIState
{
    AIStateId GetId();
    void EnterState(AIAgent agent);
    void Update(AIAgent agent);
    void ExitState(AIAgent agent);
}

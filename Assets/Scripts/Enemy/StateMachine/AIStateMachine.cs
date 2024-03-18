
public class AIStateMachine
{
    public AIState[] states;
    public AIAgent agent;
    public AIStateId currentState;

    public AIStateMachine(AIAgent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(AIStateId)).Length;
        states = new AIState[numStates];
    }

    public AIState GetState(AIStateId stateId)
    {
        int index = (int) stateId;
        return states[index];
    }

    public void RegisterState(AIState state)
    {
        int index = (int) state.GetId();
        states[index] = state;
    }

    public void UpdateState()
    {
        GetState(currentState)?.Update(agent);
    }

    public void ChangeState(AIStateId newState)
    {
        GetState(currentState)?.ExitState(agent);
        currentState = newState;
        GetState(currentState)?.EnterState(agent);
    }
}

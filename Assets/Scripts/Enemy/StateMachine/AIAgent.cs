using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Ragdoll ragdoll;

    public AIStateId initialState;
    public EnemyConfigurationSO config;
    public Transform gunMuzzle;
    public Transform target;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new ChasePlayerState());
        stateMachine.RegisterState(new DeathState());
        stateMachine.RegisterState(new IdleState());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }
}

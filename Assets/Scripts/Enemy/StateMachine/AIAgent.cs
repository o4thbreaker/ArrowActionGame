using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public ShootingControl weapon;

    public LayerMask shootingLayerMask;
    public AIStateId initialState;
    public EnemyConfigurationSO config;
    public Transform gunMuzzle;
    public Transform target;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        weapon = GetComponent<ShootingControl>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new ChasePlayerState());
        stateMachine.RegisterState(new DeathState());
        stateMachine.RegisterState(new IdleState());
        stateMachine.RegisterState(new AttackPlayerState());
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }
}

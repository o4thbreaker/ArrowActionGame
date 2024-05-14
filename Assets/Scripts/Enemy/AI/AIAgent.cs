using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector] public AIStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public ShootingControl weapon;
    [HideInInspector] public CoversArea coverArea;

    public LayerMask shootingLayerMask;
    public AIStateId initialState;
    public EnemyConfigurationSO config;
    public Transform gunMuzzle;
    public Transform target;
    public float stoppingDistance = 8;

    public static int totalEnemiesAmount;

    private void Start()
    {
        target = ThirdPersonController.Instance.transform;
        totalEnemiesAmount++;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ragdoll = GetComponent<Ragdoll>();
        weapon = GetComponent<ShootingControl>();
        coverArea = FindObjectOfType<CoversArea>();

        stateMachine = new AIStateMachine(this);
        stateMachine.RegisterState(new ChasePlayerState());
        stateMachine.RegisterState(new DeathState());
        stateMachine.RegisterState(new IdleState());
        stateMachine.RegisterState(new AttackPlayerState());
        stateMachine.RegisterState(new RunToCoverState());
        stateMachine.RegisterState(new InCoverState(this));
        stateMachine.RegisterState(new DelayState(stateMachine, AIStateId.InCover, 1f));
        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }
}

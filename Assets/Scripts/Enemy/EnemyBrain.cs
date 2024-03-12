using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;

    private EnemyManager enemyManager;
    private EnemyShoot shootManager;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public Transform target;
    private float shootingDistance;
    private bool targetInRange = false;
    private string isRunning = "isRunning";
    private string isAiming = "isAiming";

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        shootManager = GetComponent<EnemyShoot>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        shootingDistance = navMeshAgent.stoppingDistance;
    }

    private void Update()
    {
        if (target != null)
        {
            targetInRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (targetInRange)
            {
                LookAtTarget();
                StartCoroutine(shootManager.Aim());
            }
            else
            {
                animator.SetBool(isAiming, false);
            }
        }
    }

    private void LookAtTarget()
    {
        Vector3 lookPosition = target.position - gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}

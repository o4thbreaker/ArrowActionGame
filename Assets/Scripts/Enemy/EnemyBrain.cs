using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float pathUpdateDelay = 0.2f;

    private EnemyShoot shootManager;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    
    private float shootingDistance;
    private bool targetInRange = false;
    private float pathUpdateDeadline;
    private string speed = "speed";
    private string isShooting = "isShooting";
    private string isRunning = "isRunning";
    private string isAiming = "isAiming";

    private void Awake()
    {
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
                LookAtTarget();
            else
                UpdatePath();

            animator.SetBool(isShooting, targetInRange);
        }
        animator.SetFloat(speed, navMeshAgent.desiredVelocity.sqrMagnitude);
    }

    private void LookAtTarget()
    {
        Vector3 lookPosition = target.position - gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    private void UpdatePath()
    {
        if (Time.time >= pathUpdateDeadline)
        {
            pathUpdateDeadline = Time.time + pathUpdateDelay;
            navMeshAgent.SetDestination(target.position);
        }
    }
}

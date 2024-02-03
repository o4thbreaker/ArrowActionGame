using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private EnemyShoot shootManager;
    [SerializeField] private Transform gunMuzzle;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float distanceThreshold = .1f;

    public Transform target;
    private EnemyReferences enemyReferences;
    private float shootingDistance;
    private bool inRange = false;
    private string isRunning = "isRunning";
    private string isAiming = "isAiming";

    private Transform currentWaypoint;
    private Quaternion rotationGoal;
    private Vector3 directionToWaypoint;

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    private void Start()
    {
        // ----------navmesh part----------
        shootingDistance = enemyReferences.navMeshAgent.stoppingDistance;
        // --------------------------------

        //set the initial waypoint
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        transform.LookAt(currentWaypoint);

    }

    private void Update()
    {
        if (!waypoints.isLastWaypointReached & !inRange & !enemyReferences.enemyManager.IsDead())
        {
            enemyReferences.animator.SetBool(isRunning, true);
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            enemyReferences.animator.SetBool(isRunning, false);
            enemyReferences.animator.SetBool(isAiming, true);
        }
       
            

        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        }
        if (!inRange)
        {
            RotateTowardsWaypoint();
        }
        
        // ----------navmesh part----------
        if (target != null)
        {
            inRange = Vector3.Distance(transform.position, target.position) <= shootingDistance;

            if (inRange & !enemyReferences.enemyManager.IsDead())
            {
                LookAtTarget();
                StartCoroutine(shootManager.Aim());
            }
        }
        // --------------------------------
    }

    private void RotateTowardsWaypoint()
    {
        directionToWaypoint = (currentWaypoint.position - transform.position).normalized;
        rotationGoal = Quaternion.LookRotation(directionToWaypoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationGoal, rotateSpeed * Time.deltaTime);   
    }

    private void LookAtTarget()
    {
        Vector3 lookPosition = target.position - gunMuzzle.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }
}

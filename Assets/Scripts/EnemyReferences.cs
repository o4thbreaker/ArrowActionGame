using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyReferences : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    public EnemyManager enemyManager;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }

}

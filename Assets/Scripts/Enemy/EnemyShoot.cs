using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private Transform shootFrom;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float aimingTime = 2f;
    [SerializeField] private float shootCooldownTime = 0.5f;

    private Animator animator;
    private EnemyHealth enemyManager;

    private bool canShoot = true;

    private int isAimingHash;
    private int shootHash;
    
    // TODO: SCRIPT IS A PIECE OF TRASH REMAKE EVERYTHING

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        isAimingHash = Animator.StringToHash("isAiming");
        shootHash = Animator.StringToHash("Shoot");
    }

    // animation trigger
    private void Shoot()
    {
        Vector3 direction = GetDirection();
        if (Physics.Raycast(shootFrom.position, direction, out RaycastHit hit, float.MaxValue, layerMask))
            Debug.DrawLine(shootFrom.position, shootFrom.position + direction * 10f, Color.red, 1f);

        muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        direction.Normalize();

        return direction;
    }

    public IEnumerator Aim()
    {
        animator.SetBool(isAimingHash, true);
        yield return new WaitForSeconds(aimingTime);
        animator.Play(shootHash);
    }

    private IEnumerator shootCooldown()
    {
        yield return new WaitForSeconds(shootCooldownTime);
        canShoot = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private float aimingTime = 2f;
    [SerializeField] private float shootCooldownTime = 0.5f;

    private Animator animator;
    private EnemyManager enemyManager;

    private bool canShoot = true;

    private int isAimingHash;
    private int shootHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }

    private void Start()
    {
        isAimingHash = Animator.StringToHash("isAiming");
        shootHash = Animator.StringToHash("Shoot");
    }

    private void _Shoot()
    {
        if (enemyManager.IsDead()) return;

        if (canShoot)
        {
            animator.SetBool(isAimingHash, false);

            GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
            muzzleFlash.Play();
            canShoot = false;            
            StartCoroutine(shootCooldown());
        }
    }

    // animation trigger
    private void Shoot()
    {
        muzzleFlash.Play();
        GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private Transform gunMuzzle;
    [SerializeField] private EnemyReferences enemyReferences;    
    [SerializeField] private float aimingTime = 2f;
    [SerializeField] private float shootCooldownTime = 0.5f;

    private bool canShoot = true;
    private string isFiring = "isFiring";
    private string isAiming = "isAiming";

    private void Awake()
    {
        enemyReferences = GetComponent<EnemyReferences>();
    }

    private void Shoot()
    {
        if (enemyReferences.enemyManager.IsDead()) return;

        if (canShoot)
        {
            enemyReferences.animator.SetBool(isAiming, false);
            enemyReferences.animator.SetBool(isFiring, true);
            GameObject bullet = Instantiate(bulletPrefab, gunMuzzle.position, gunMuzzle.rotation) as GameObject;
            muzzleFlash.Play();
            canShoot = false;            
            StartCoroutine(shootCooldown());
        }
    }

    public IEnumerator Aim()
    {
        enemyReferences.animator.SetBool(isFiring, false);
        enemyReferences.animator.SetBool(isAiming, true);
        yield return new WaitForSeconds(aimingTime);
        Shoot();
    }

    private IEnumerator shootCooldown()
    {
        yield return new WaitForSeconds(shootCooldownTime);
        canShoot = true;
    }
}

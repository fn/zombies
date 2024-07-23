using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] int faceTargetSpeed;
    [SerializeField] Transform shootPos;
    [SerializeField] int viewAngle;
    [SerializeField] Transform headPos;
    [SerializeField] float detectionRange;
    [SerializeField] float fireRate;
    [SerializeField] WeaponComponent weaponComponent;

    private Transform target;
    private float fireCountdown = 0f;

    void Update()
    {
        UpdateTarget();

        if (target == null)
            return;

        AimAtTarget();

        if (fireCountdown <= 0f && weaponComponent.currentAmmo > 0)
        {
            weaponComponent.Shoot(shootPos.position, (target.position - shootPos.position).normalized);
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

        if (weaponComponent.currentAmmo <= 0 && !weaponComponent.infAmmo)
        {
            Destroy(gameObject);
        }
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= detectionRange)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= detectionRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void AimAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * faceTargetSpeed).eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
}
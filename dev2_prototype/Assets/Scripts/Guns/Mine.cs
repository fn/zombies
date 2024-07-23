using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private AttackArea aArea;
    public int damageAmount;
    public float range;
    public int destroyTime;
    public GameObject explosionEffect; // Explosion effect prefab

    void Start()
    {
        // Optional: Initialize mine-specific settings
        Destroy(gameObject, destroyTime);
    }

    public void Detonate()
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        if (aArea.affected.Count == 0)
        {
            Destroy(gameObject);
        }

        foreach (var o in aArea.affected)
        {
            IDamageable dmg = o.GetComponent<IDamageable>();
            if (dmg == null) continue;

            var damagePercent = Vector3.Distance(o.transform.position, transform.position) / range;
            int calculatedDamage = (int)(damageAmount * (1 - damagePercent));
            dmg.TakeDamage(calculatedDamage);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision mineColl)
    {
        if (mineColl.collider.isTrigger)
        {
            return;
        }

        // Check for bullet
        var bulletDamage = mineColl.collider.GetComponent<DamageSource>();
        if (bulletDamage != null && bulletDamage.DamageSourceType == DamageSource.DamageType.Bullet)
        {
            Detonate();
            return;
        }

        // Check for player or enemy
        if (mineColl.collider.CompareTag("Player") || mineColl.collider.CompareTag("Enemy"))
        {
            Detonate();
            return;
        }
    }
}
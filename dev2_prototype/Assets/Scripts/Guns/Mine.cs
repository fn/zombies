using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour, IDamageable
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

    public void TakeDamage(int amount)
    {
        Detonate(null);
    }

    private void OnTriggerEnter(Collider other)
    {        
        IDamageable dmg = other.GetComponent<IDamageable>();
 
        if(dmg != null)
        {
            Detonate(dmg);
        }
    }


    public void Detonate(IDamageable d)
    {
        // Instantiate explosion effect
        if (explosionEffect != null)
        {
             Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        RaycastHit hit;
        if(Physics.SphereCast(transform.position, range, Vector3.up, out hit))
        {
            float damagePercent = Vector3.Distance(hit.transform.position, transform.position) / range;
            int calculatedDamage = (int)(damageAmount * (1 - damagePercent));
            if(d != null)
                d.TakeDamage(-calculatedDamage);
        }

        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider mineColl)
    //{
    //    if (mineColl.isTrigger)
    //    {
    //        return;
    //    }

    //    // Check for bullet
    //    var bulletDamage = mineColl.GetComponent<DamageSource>();
    //    if (bulletDamage != null && bulletDamage.Type == DamageSourceType.Bullet)
    //    {
    //        Detonate();
    //        return;
    //    }

    //    // Check for player or enemy
    //    if (mineColl.CompareTag("Player") || mineColl.CompareTag("Enemy"))
    //    {
    //        Detonate();
    //        return;
    //    }
    //}
}
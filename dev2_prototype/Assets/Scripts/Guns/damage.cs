using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public int damageAmount;
    public enum damageType { bullet, stationary };
    public damageType type;
    public Rigidbody rb;
    public int speed;
    public int destroyTime;

    private float lastHitTime;

    bool hasDamaged;

    // Start is called before the first frame update
    void Start()
    {
        if (type == damageType.bullet)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    public void SetDamage(float weaponDamage)
    {
        damageAmount = Mathf.RoundToInt(weaponDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && !hasDamaged)
        {
            dmg.takeDamage(damageAmount);
            hasDamaged = true;
        }

        if (type == damageType.bullet)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (type == damageType.stationary)
        {
            if (dmg != null)
            {
                if( Time.time - lastHitTime > 0.3f)
                {
                    dmg.takeDamage(damageAmount);
                    lastHitTime = Time.time;    
                }
            }
        }
    }
}
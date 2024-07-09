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
                dmg.takeDamage(damageAmount);
                StartCoroutine(waitTime());
            }
        }
    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(0.3f);
    }

}

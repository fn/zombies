using UnityEngine;

public enum DamageSourceType
{
    Bullet,
    Stationary
};

public class DamageSource : MonoBehaviour
{
    public int DamageAmount;
    public DamageSourceType Type;
    public Rigidbody rb;
    public int speed;
    public float destroyTime;

    private float lastHitTime;
    bool DealtDamage;

    // Start is called before the first frame update
    void Start()
    {
        if (Type == DamageSourceType.Bullet)
        {
            rb.velocity = transform.forward * speed;
            Destroy(gameObject, destroyTime);
        }
    }

    public void SetDamage(float weaponDamage)
    {
        DamageAmount = Mathf.RoundToInt(weaponDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.TryGetComponent(out IDamageable dmg) && !DealtDamage)
        {
            dmg.TakeDamage(DamageAmount);
            DealtDamage = true;
        }

        if (Type == DamageSourceType.Bullet)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        if (Type == DamageSourceType.Stationary)
        {
            if (other.TryGetComponent(out IDamageable dmg))
            {
                // Maybe make the 0.3f a serialized field.
                if (Time.time - lastHitTime > 0.3f)
                {
                    dmg.TakeDamage(DamageAmount);
                    lastHitTime = Time.time;
                }
            }
        }
    }
}
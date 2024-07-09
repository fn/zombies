using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public float damage;
    public float rateOfFire;
    public int ammoCapacity;
    public int currentAmmo;
    public bool specialGun;
    public bool infAmmo;
    [SerializeField] GameObject Bullet_Standard;

    private float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ammo count
        currentAmmo = ammoCapacity;
        lastShotTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle shooting input
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot(transform.position, transform.forward);
        }

        // Handle reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        float shotCooldown = 1f / rateOfFire;

        if (Time.time - lastShotTime < shotCooldown || currentAmmo <= 0)
        {
            return;
        }

        // Update the last shot time and decrease ammo count
        lastShotTime = Time.time;
        if (!infAmmo)
        {
            currentAmmo--;
        }
        // Instantiate the bullet
        GameObject bullet = Instantiate(Bullet_Standard, origin, Quaternion.LookRotation(direction));

        // Transfer the damage value to the bullet
        damage bulletDamage = bullet.GetComponent<damage>();
        if (bulletDamage != null)
        {
            bulletDamage.SetDamage(damage);

            // Apply velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletDamage.speed;
            }

            // Destroy the bullet after a certain time
            Destroy(bullet, bulletDamage.destroyTime);
        }
    }

    public void Reload()
    {
        // Reload logic here
        currentAmmo = ammoCapacity;
        Debug.Log("Reloading!");
    }
}

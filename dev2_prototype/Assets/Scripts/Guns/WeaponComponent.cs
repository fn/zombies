using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public float damage;
    public float rateOfFire;
    public int ammoCapacity;
    public int currentAmmo;
    public int magSize;
    public bool specialGun;
    public bool infAmmo;
    public string layer;
    [SerializeField] GameObject Bullet_Standard;
    int usedAmmo;
    public int remainingAmmo;

    public bool HasAmmo { get => currentAmmo > 0; }
    public bool CanReload { get => currentAmmo != magSize; }

    private float lastShotTime;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize ammo count
        currentAmmo = magSize;
        remainingAmmo = ammoCapacity;
        lastShotTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        float shotCooldown = 1f / rateOfFire;
        if (currentAmmo <= 0 && !infAmmo)
            return;

        if (Time.time - lastShotTime >= shotCooldown)
        {
            // Update the last shot time and decrease ammo count
            if (!infAmmo)
            {
                currentAmmo--;
                usedAmmo++;
            }
            // Instantiate the bullet
            GameObject bullet = Instantiate(Bullet_Standard, origin, Quaternion.LookRotation(direction));
            if (bullet == null)
            {
                return;
            }

            bullet.layer = LayerMask.NameToLayer(layer);

            // Transfer the damage value to the bullet
            DamageSource bulletDamage = bullet.GetComponent<DamageSource>();
            bulletDamage.SetDamage(damage);

            // Apply velocity to the bullet
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * bulletDamage.speed;
            }

            // Destroy the bullet after a certain time
            Destroy(bullet, bulletDamage.destroyTime);

            lastShotTime = Time.time;
        }
    }

    public void Reload()
    {
        remainingAmmo -= usedAmmo;
        usedAmmo = 0;

        currentAmmo = magSize <= remainingAmmo ? magSize : remainingAmmo;
    }
}
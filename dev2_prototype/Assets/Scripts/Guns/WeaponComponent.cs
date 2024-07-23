using System.Collections;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public WeaponStats Info;

    public string layer;
    public GameObject Model;

    [SerializeField] GameObject Bullet_Standard;

    public int CurrentAmmo;
    public int RemainingAmmo;

    public bool HasAmmo { get => CurrentAmmo > 0; }
    public bool CanReload { get => CurrentAmmo != Info.MagSize && RemainingAmmo > 0 && !IsReloading; }

    public bool IsReloading;
    private float lastShotTime;
    private float reloadStartTime;
    // Start is called before the first frame update
    void Start()
    {
        ResetWeapon();
    }

    public void ResetWeapon()
    {
        // Initialize ammo count
        CurrentAmmo = Info.MagSize;
        RemainingAmmo = Info.AmmoCapacity;
        lastShotTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        if (IsReloading)
            return;

        float shotCooldown = 1f / Info.FireRate;
        if (CurrentAmmo <= 0 && !Info.InfiniteAmmo)
            return;

        if (Time.time - lastShotTime >= shotCooldown)
        {
            // Update the last shot time and decrease ammo count
            if (!Info.InfiniteAmmo)
            {
                CurrentAmmo--;
            }

            for (int i = 0; i < Info.ProjectilesPerShot; i++)
            {
                // Instantiate the bullet
                Vector3 spreadDir = direction + (Random.insideUnitSphere * Info.SpreadFactor);

                GameObject bullet = Instantiate(Bullet_Standard, origin, Quaternion.LookRotation(spreadDir));
                if (bullet == null)
                {
                    return;
                }

                bullet.layer = LayerMask.NameToLayer(layer);

                // Transfer the damage value to the bullet
                DamageSource bulletDamage = bullet.GetComponent<DamageSource>();
                bulletDamage.SetDamage(Info.Damage);

                // Apply velocity to the bullet
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = spreadDir * bulletDamage.speed;
                }

                // Destroy the bullet after a certain time
                Destroy(bullet, bulletDamage.destroyTime);
            }

            lastShotTime = Time.time;
        }
    }

    public void Reload()
    {
        // Reload logic here
        // No more ammos :C
        if (RemainingAmmo < 0)
            return;

        // How much ammo we need to reload.
        int neededAmmo = Info.MagSize - CurrentAmmo;
        int oldAmmo = CurrentAmmo;

        // Reload that ammo.
        CurrentAmmo += neededAmmo > RemainingAmmo ? RemainingAmmo : neededAmmo;

        // Remove it from our stockpile.
        RemainingAmmo -= (CurrentAmmo - oldAmmo);

        IsReloading = false;
    }
}
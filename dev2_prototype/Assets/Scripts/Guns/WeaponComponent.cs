using System.Collections;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public float Damage;

    // Prevent division by zero.
    [Range(1f, 10000f)]
    public float FireRate;
    public int AmmoCapacity;
    public int MagSize;
    public bool IsSpecial;
    public bool InfiniteAmmo;
    public int ProjectilesPerShot;
    public float SpreadFactor;
}

public class WeaponComponent : MonoBehaviour
{
    public WeaponStats Info;

    public string layer;
    public bool isShotgun; // Add this variable
    [SerializeField] GameObject Bullet_Standard;
    [SerializeField] GameObject Bullet_Shotgun;
    [SerializeField] GameObject Mine;
    int usedAmmo;
    public int remainingAmmo;
    public int shotgunPellets = 9; // Number of pellets for shotgun
    public float spreadAngle = 5f; // Spread angle for shotgun
    public float reloadTime = 2f; // Reload time in seconds


    public bool HasAmmo { get => currentAmmo > 0; }
    public bool CanReload { get => currentAmmo != magSize && remainingAmmo > 0; }

    private float lastShotTime;
    private bool isReloading; // Add this variable

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
        isReloading = false; // Initialize isReloading
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(Vector3 origin, Vector3 direction)
    {
        if (isReloading)
            return; // Prevent shooting while reloading

        float shotCooldown = 1f / rateOfFire;
        if (currentAmmo <= 0 && !infAmmo)
            return;

        if (Time.time - lastShotTime >= shotCooldown)
        {
            // Update the last shot time and decrease ammo count
            if (!Info.InfiniteAmmo)
            {
                CurrentAmmo--;
            }

            if (isShotgun)
            {
                // Instantiate multiple bullets for shotgun
                for (int i = 0; i < shotgunPellets; i++)
                {
                    Vector3 spreadDirection = Quaternion.Euler(
                        Random.Range(-spreadAngle, spreadAngle),
                        Random.Range(-spreadAngle, spreadAngle),
                        0) * direction;

                    SpawnBullet(origin, spreadDirection);
                }
            }
            else
            {
                // Instantiate a single bullet for regular gun
                SpawnBullet(origin, direction);
            }

            lastShotTime = Time.time;
        }
    }

    private void SpawnBullet(Vector3 origin, Vector3 direction)
    {
        if (!isShotgun)
        {
            GameObject bullet = Instantiate(Bullet_Standard, origin, Quaternion.LookRotation(direction));
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
        else
        {
            GameObject bullet = Instantiate(Bullet_Shotgun, origin, Quaternion.LookRotation(direction));
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
            }x

            // Destroy the bullet after a certain time
            Destroy(bullet, bulletDamage.destroyTime);
        }
    }

    public void Reload()
    {
        if (isReloading)
            return; // Prevent multiple reload calls

        StartCoroutine(ReloadCoroutine());
    }
    
    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;

        // Optionally, you could add code here to show reloading animation or UI
        yield return new WaitForSeconds(reloadTime);

        // Reload logic here
        // No more ammos :C
        if (remainingAmmo < 0)
            yield break;

        // How much ammo we need to reload.
        int neededAmmo = Info.MagSize - CurrentAmmo;
        int oldAmmo = CurrentAmmo;

        // Reload that ammo.
        CurrentAmmo += neededAmmo > RemainingAmmo ? RemainingAmmo : neededAmmo;

        // Remove it from our stockpile.
        remainingAmmo -= (currentAmmo - oldAmmo);
        isReloading = false;
    }
}
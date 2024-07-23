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
        // Initialize ammo count
        currentAmmo = magSize;
        remainingAmmo = ammoCapacity;
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
            if (!infAmmo)
            {
                currentAmmo--;
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
        else
        {
            GameObject bullet = Instantiate(Bullet_Shotgun, origin, Quaternion.LookRotation(direction));
            if (bullet == null)
            {
                return;
            }

            bullet.layer = LayerMask.NameToLayer(layer);

            // Transfer the damage value to the bullet
            Damage bulletDamage = bullet.GetComponent<Damage>();
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
            return;

        // How much ammo we need to reload.
        int neededAmmo = magSize - currentAmmo;
        int oldAmmo = currentAmmo;

        // Reload that ammo.
        currentAmmo += neededAmmo > remainingAmmo ? remainingAmmo : neededAmmo;

        // Remove it from our stockpile.
        remainingAmmo -= (currentAmmo - oldAmmo);
        isReloading = false;
    }
}
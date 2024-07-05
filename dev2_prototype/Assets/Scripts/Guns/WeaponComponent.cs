using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public float damage;
    public float rateOfFire;
    public int ammoCapacity;
    public int currentAmmo;

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
        currentAmmo--;

        // Implement actual shooting mechanics (raycasting, instantiating bullets, etc.) here

        Debug.DrawRay(origin, direction);
    }

    public void Reload()
    {
        // Reload logic here
        currentAmmo = ammoCapacity;
        Debug.Log("Reloading!");
    }
}
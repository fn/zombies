using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class autoItemPickup : MonoBehaviour
{
    [SerializeField] enum pickupType { Health, Ammo }
    [SerializeField] pickupType type;

    public int restoreAmount;
    public WeaponComponent weapon;

    private void OnTriggerEnter(Collider other)
    {
        if (type == pickupType.Health)
        {
            Destroy(gameObject);
            //Player.HP = Player.HP + restoreAmount;

        }
        if (type == pickupType.Ammo)
        {
            Destroy(gameObject);
            weapon.currentAmmo = weapon.currentAmmo + restoreAmount;
        }
    }
}

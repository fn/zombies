using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class autoItemPickup : MonoBehaviour
{
    [SerializeField] enum pickupType { Health, Ammo }
    [SerializeField] pickupType type;

    [SerializeField] int restoreAmount;
    //public WeaponComponent weapon;
    //public Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<Player>();

            if (type == pickupType.Health)
            {
                Destroy(gameObject);
                player.Health += restoreAmount;

            }
            if (type == pickupType.Ammo)
            {
                Destroy(gameObject);
                player.HeldWeapon.remainingAmmo += restoreAmount;
            }
        }
    }
}

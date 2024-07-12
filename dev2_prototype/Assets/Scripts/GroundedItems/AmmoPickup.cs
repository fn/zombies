using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class AmmoPickup : autoItemPickup
{
    [SerializeField] int restoreAmount;

    public override void ApplyAmount(Player player)
    {
        player.HeldWeapon.remainingAmmo += restoreAmount;
        Destroy(gameObject);
    }

}

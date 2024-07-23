using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class AmmoPickup : autoItemPickup
{
    [SerializeField] int restoreAmount;
    [SerializeField] float timer;

    private void Awake()
    {
        destroyTimer = timer;
    }

    public override void ApplyAmount(Player player)
    {
        player.HeldWeapon.RemainingAmmo += restoreAmount;
        Destroy(gameObject);
    }

}

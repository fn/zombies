using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class HealthPickup : autoItemPickup
{
    [SerializeField] int restoreAmount;
    [SerializeField] float timer;

    private void Awake()
    {
        destroyTimer = timer;
    }

    public override void ApplyAmount(Player player)
    {
        player.Health += restoreAmount;
        player.Health = Mathf.Clamp(player.Health, 0, player.MaxHealth);
        Destroy(gameObject);
    }
}

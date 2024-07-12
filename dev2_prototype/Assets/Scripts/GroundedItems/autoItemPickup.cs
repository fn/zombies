using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

// abstract base class for pickup items to derive from, they dont need IPickup as long as they derive from here
public abstract class autoItemPickup : MonoBehaviour, IPickup
{
    // no need to check ontriggerenter for every item
    private void OnTriggerEnter(Collider other)
    {
        // checks and returns the player if true
        if (other.TryGetComponent(out Player player))
        {
            ApplyAmount(player);
        }
    }

    public abstract void ApplyAmount(Player player);

    private void Start()
    {
        // destory the gameobject after a set time
        Object.Destroy(gameObject, 10f);
    }

}

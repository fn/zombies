using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

// abstract base class for pickup items to derive from, they dont need IPickup as long as they derive from here
public abstract class autoItemPickup : MonoBehaviour, IPickup
{
    // when the object should be destroyed, defined in awake of derived classes
    protected float destroyTimer = -1f; // default value of -1

    // no need to check ontriggerenter for every item
    private void OnTriggerEnter(Collider other)
    {
        // checks and returns the player if true
        if (other.TryGetComponent(out Player player))
        {
            ApplyAmount(player);
        }
    }

    protected void DestroyTimer()
    {
        // if value is greater than 0 destroy the object after destroyTimer time
        if (destroyTimer >= 0f)
        {
            Destroy(gameObject, destroyTimer);
        }
    }

    public abstract void ApplyAmount(Player player);

    private void Start()
    {
        DestroyTimer();
    }

}

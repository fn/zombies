using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Zombies;

public class Interact : MonoBehaviour
{

    public GameObject ExplosiveBarrel;
    public Transform ItemParent;

    // Start is called before the first frame update
    void Start()
    {
        ExplosiveBarrel.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            drop();
        }
    }


        // if player is in range of an item tagged "Pickup"
        //display a prompt to the player that the item is able to be picked up
        //if they press the interact button with the prompt up
        //Attatch the item on the map to the end of the gun.
        //display a drop item prompt with the interact Key.

        //When the user drops the item
        //The item will be removed from the end of the gun and placed a distance away from the player's forward direction.

    void drop()
    {
        ItemParent.DetachChildren();
        ExplosiveBarrel.transform.eulerAngles = new Vector3(ExplosiveBarrel.transform.position.x, ExplosiveBarrel.transform.position.z, ExplosiveBarrel.transform.position.y);
        ExplosiveBarrel.GetComponent<Rigidbody>().isKinematic = false;
        ExplosiveBarrel.GetComponent<MeshCollider>().enabled = true;
    }

    void Pickup()
    {
        ExplosiveBarrel.GetComponent<Rigidbody>().isKinematic = true;

        ExplosiveBarrel.transform.position = ItemParent.transform.position;
        ExplosiveBarrel.transform.rotation = ItemParent.transform.rotation;
        ExplosiveBarrel.GetComponent<MeshCollider>().enabled = false;
        ExplosiveBarrel.transform.SetParent(ItemParent);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Pickup();
            }
        }
    }
}

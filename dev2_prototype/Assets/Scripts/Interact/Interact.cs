using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Interact : MonoBehaviour
{

    public GameObject ExplosiveBarrelOnPlayer;
    public GameObject PickupText;
    public GameObject DropText;
    bool somethingInHand;

    // Start is called before the first frame update
    void Start()
    {
        ExplosiveBarrelOnPlayer.SetActive(false);
        PickupText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // if player is in range of an item tagged "Pickup"
    //display a prompt to the player that the item is able to be picked up
    //if they press the interact button with the prompt up
    //Attatch the item on the map to the end of the gun.
    //display a drop item prompt with the interact Key.

    //When the user drops the item
    //The item will be removed from the end of the gun and placed a distance away from the player's forward direction.

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player" && somethingInHand == false)
        {
            PickupText.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                somethingInHand = true;
                this.gameObject.SetActive(false);
                ExplosiveBarrelOnPlayer.SetActive(true);
                PickupText.SetActive(false);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        PickupText.SetActive(false);   
    }

    public void dropItem()
    {
        if(somethingInHand == true)
        {
            DropText.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                somethingInHand= false;
                this.gameObject.SetActive(true);
                ExplosiveBarrelOnPlayer.SetActive(false);
                PickupText.SetActive(false);
            }
        }
    }

}

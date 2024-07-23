using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UIElements;
using Zombies;

public class Interact : MonoBehaviour
{

    public GameObject Barrel;
    public Transform ItemParent;

    

    // Start is called before the first frame update
    void Start()
    {
        Barrel.GetComponent<Rigidbody>().isKinematic = true;
        GameManager.Instance.ItemInHand = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ItemInHand)
        {
            //GameManager.Instance.DropPrompt.SetActive(true);
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText("'Q' To Drop");
            if (Input.GetKey(KeyCode.Q))
            {
                drop();
                GameManager.Instance.PromptBackground.SetActive(false);
            }
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
            //GameManager.Instance.DropPrompt.SetActive(false);
            GameManager.Instance.ItemInHand = false;
            ItemParent.DetachChildren();
            Barrel.transform.eulerAngles = new Vector3(Barrel.transform.position.x, Barrel.transform.position.z, Barrel.transform.position.y);
            Barrel.GetComponent<Rigidbody>().isKinematic = false;
            Barrel.GetComponent<MeshCollider>().enabled = true;
    }

    void Pickup()
    {
        GameManager.Instance.ItemInHand = true;
        Barrel.GetComponent<Rigidbody>().isKinematic = true;

        Barrel.transform.position = ItemParent.transform.position;
        Barrel.transform.rotation = ItemParent.transform.rotation;
        Barrel.GetComponent<MeshCollider>().enabled = false;
        Barrel.transform.SetParent(ItemParent);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !GameManager.Instance.ItemInHand)
        {
            //GameManager.Instance.PickupPrompt.SetActive(true);
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText("'E' Pickup Item");
            if (Input.GetKey(KeyCode.E))
            {
                Pickup();
                //GameManager.Instance.PickupPrompt.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameManager.Instance.PromptBackground.SetActive(false);
            //GameManager.Instance.PickupPrompt.SetActive(false);
        }
    }
}

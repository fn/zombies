using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    // object to spawn
    [SerializeField] GameObject barricadeToSpawn;

    // cost
    [SerializeField] float Cost;
    public float PLACEHOLDERMONEY = 5.0f;

    // checking for range
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // check for player enter collider range
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check for player leaving collider range
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // player in range
        if (other.CompareTag("Player") && playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && PLACEHOLDERMONEY >= Cost)
            {
                // is the child active or not
                if (!barricadeToSpawn.gameObject.activeSelf)
                {
                    // ray extended from camera to mouse
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    // did day hit the collider
                    if (Physics.Raycast(ray, out hit) && hit.collider == GetComponent<Collider>())
                    {
                        // spawn the object
                        SpawnBarricade();

                        // subtract the cost
                        PLACEHOLDERMONEY -= Cost;
                    }
                }
            }
        }
    }

    public void SpawnBarricade()
    {
        // set the object to active
        barricadeToSpawn.SetActive(true);
    }

}

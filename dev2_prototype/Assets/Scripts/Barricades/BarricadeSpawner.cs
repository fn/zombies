using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeSpawner : MonoBehaviour
{
    // object to spawn
    [SerializeField] GameObject barricadeToSpawn;
    // cost
    [SerializeField] int purchaseCost;
    public int PLACEHOLDERMONEY = 5;

    // checking for range
    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        // check for player enter collider range
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // is child active or not
            if (barricadeToSpawn.activeSelf)
            {
                Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
                if (barricade != null)
                {
                    barricade.StartRepair();
                }
                else
                    Debug.Log("barricade = null");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check for player leaving collider range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // is child active or not
            if (barricadeToSpawn.activeSelf)
            {
                Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
                if (barricade != null)
                {
                    barricade.StopRepair();
                }
                else
                    Debug.Log("barricade = null");
            } 
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            PurchaseBarricade();

    }

    public void PurchaseBarricade()
    {
        // player in range
        if (playerInRange)
        {
            // purchase barricade
            if (PLACEHOLDERMONEY >= purchaseCost)
            {
                // is the child active or not
                if (!barricadeToSpawn.activeSelf)
                {
                    // spawn barricade
                    SpawnBarricade();

                    // reduce player money by cost
                    PLACEHOLDERMONEY -= purchaseCost;
                }
                else
                    Debug.Log("barricadeToSpawn is spawned");
            }
            else
                Debug.Log("player cannot afford");
        }
    }

    public void SpawnBarricade()
    {
        // set the object to active
        barricadeToSpawn.SetActive(true);
        Debug.Log("barricade SetActive(true)");

        Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
        if (barricade != null)
        {
            barricade.ResetHealth();
        }
        else
            Debug.Log("barricade = null");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeSpawner : MonoBehaviour
{
    // object to spawn
    [SerializeField] GameObject barricadeToSpawn;
    // cost
    [SerializeField] float purchaseCost;
    public float PLACEHOLDERMONEY = 5.0f;

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
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // player in range
        if (other.CompareTag("Player") && playerInRange)
        {   
            // purchase barricade
            if (Input.GetKeyDown(KeyCode.E) && PLACEHOLDERMONEY >= purchaseCost)
            {
                // is the child active or not
                if (!barricadeToSpawn.activeSelf)
                {
                    // spawn barricade
                    SpawnBarricade();

                    // reduce player money by cost
                    PLACEHOLDERMONEY -= purchaseCost;
                }
            }
        }
    }

    public void SpawnBarricade()
    {
        // set the object to active
        barricadeToSpawn.SetActive(true);
        Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
        if(barricade != null)
        {
            barricade.ResetHealth();
        }
    }

}

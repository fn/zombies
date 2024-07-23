using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeSpawner : MonoBehaviour
{
    // object to spawn
    [SerializeField] GameObject barricadeToSpawn;
    // cost
    [SerializeField] int purchaseCost;

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
                //else
                   // Debug.Log("barricade = null");
            } else
            {
                GameManager.Instance.PromptBackground.SetActive(true);
                GameManager.Instance.PromptText.SetText($"Spawn Barricade 'E' Cost: {purchaseCost}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check for player leaving collider range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            GameManager.Instance.PromptBackground.SetActive(false);

            // is child active or not
            if (barricadeToSpawn.activeSelf)
            {
                Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
                if (barricade != null)
                {
                    barricade.StopRepair();
                }
                //else
                //Debug.Log("barricade = null");
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
            if (GameManager.Instance.LocalPlayer.Money >= purchaseCost)
            {
                // is the child active or not
                if (!barricadeToSpawn.activeSelf)
                {
                    // spawn barricade
                    SpawnBarricade();

                    // reduce player money by cost
                    GameManager.Instance.LocalPlayer.Money -= purchaseCost;
                }
                //else
                    //Debug.Log("barricadeToSpawn is spawned");
            }
            //else
                //Debug.Log("player cannot afford");
        }
    }

    public void SpawnBarricade()
    {
        // set the object to active
        barricadeToSpawn.SetActive(true);
        //Debug.Log("barricade SetActive(true)");

        Barricade barricade = barricadeToSpawn.GetComponent<Barricade>();
        if (barricade != null)
        {
            barricade.ResetHealth();
        }
        //else
            //Debug.Log("barricade = null");
    }

}

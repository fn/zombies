using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    // object to spawn
    [SerializeField] GameObject interactPrefab;

    [SerializeField] Transform spawnLocation;

    // cost
    [SerializeField] float Cost;
    public float PLACEHOLDERMONEY = 5.0f;

    // checking for range
    private bool playerInRange = false;

    // coroutine for interaction
    private Coroutine interactCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // check for player enter collider range
        if(other.CompareTag("Player"))
        {
            playerInRange = true;

            // start coroutine
            if(interactCoroutine == null)
            
               interactCoroutine = StartCoroutine(InteractionCheck());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // check for player leaving collider range
        if(other.CompareTag("Player"))
        {
            playerInRange = false;

            // stop coroutine
            if(interactCoroutine != null)
            {
                StopCoroutine(interactCoroutine);
                interactCoroutine = null;
            }
        }
    }

    private IEnumerator InteractionCheck()
    {
        // while player is in range
        while(playerInRange)
        {
            //   check keycode
            if (Input.GetKeyDown(KeyCode.E))
            {
                // ray extended from camera to mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                // did day hit the collider && can the player afford it && is there already a child other than spawnLocation
                if(Physics.Raycast(ray, out hit) && hit.collider == GetComponent<Collider>()
                    && PLACEHOLDERMONEY >= Cost && transform.childCount < 2)
                {
                    SpawnBarricade();
                    PLACEHOLDERMONEY -= Cost;
                }
            }

            // stop coroutine
            yield return null;
        }
    }

    public void SpawnBarricade()
    {
        // instantiate the object, position, and rotation
        GameObject barricade = Instantiate(interactPrefab, spawnLocation.position, spawnLocation.rotation);

        // set barricade object as child to prevent multiple spawns
        barricade.transform.SetParent(transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

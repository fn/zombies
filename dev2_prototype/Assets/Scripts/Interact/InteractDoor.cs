using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDoor : MonoBehaviour
{

    [Range(0,1000)][SerializeField] int DoorCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.DoorPrompt.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                openDoor();
                GameManager.Instance.DoorPrompt.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.DoorPrompt.SetActive(false);
        }
    }

    void openDoor()
    {
        //GameManager.Instance.money -= doorcost;
        Destroy(gameObject);
    }
}

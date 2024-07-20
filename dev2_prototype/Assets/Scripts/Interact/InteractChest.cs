using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : MonoBehaviour
{

    public GameObject ItemDropped;
    public GameObject InteractPrompt;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player)")
        {
            InteractPrompt.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                Instantiate(ItemDropped, transform.position, transform.rotation);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            InteractPrompt.SetActive(false);
        }
    }
}

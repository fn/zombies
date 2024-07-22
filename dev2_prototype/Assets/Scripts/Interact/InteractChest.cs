using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : MonoBehaviour
{

    public GameObject ItemDropped;
    public GameObject DropLocation;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.ChestPrompt.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                Instantiate(ItemDropped, DropLocation.transform.position, transform.rotation);
                GameManager.Instance.ChestPrompt.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.ChestPrompt.SetActive(false);
        }
    }

}

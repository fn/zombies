using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : MonoBehaviour
{
    public GameObject ItemDropped;
    public GameObject DropLocation;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //GameManager.Instance.ChestPrompt.SetActive(true);
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText("'E' Open Chest");
            if (Input.GetKey(KeyCode.E))
            {
                Instantiate(ItemDropped, DropLocation.transform.position, transform.rotation);
                GameManager.Instance.PromptBackground.SetActive(false);
                //GameManager.Instance.ChestPrompt.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PromptBackground.SetActive(false);
            //GameManager.Instance.ChestPrompt.SetActive(false);
        }
    }
}
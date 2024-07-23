using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractChest : MonoBehaviour
{
    public GameObject ItemDropped;
    public GameObject DropLocation;
    [SerializeField] int chestCost = 0;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText("'E' Open Chest");
            if (Input.GetKey(KeyCode.E))
            {
                Instantiate(ItemDropped, DropLocation.transform.position, transform.rotation);
                GameManager.Instance.PromptBackground.SetActive(false);
                GameManager.Instance.LocalPlayer.Money -= chestCost;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PromptBackground.SetActive(false);
        }
    }
}
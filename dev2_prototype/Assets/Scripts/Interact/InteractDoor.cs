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
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText($"'E' Open Door Cost: {DoorCost}");
            if (Input.GetKey(KeyCode.E) && GameManager.Instance.LocalPlayer.Money >= DoorCost)
            {
                openDoor();
                GameManager.Instance.PromptBackground.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.PromptBackground?.SetActive(false);
        }
    }

    void openDoor()
    {
        GameManager.Instance.LocalPlayer.Money -= DoorCost;
        Destroy(gameObject);
    }
}

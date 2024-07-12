using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombies;

public class ItemDropper : MonoBehaviour
{
    // array to hold droppable items tagged Pickup
    [SerializeField] GameObject[] droppableItems;

    public void DropItem(Vector3 position, Quaternion rotation)
    {
        // make drop based on random range
        GameObject drop = droppableItems[Random.Range(0, droppableItems.Length)];

        // instantiate at position and rotation of object calling DropItem
        Instantiate(drop, position, rotation);
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

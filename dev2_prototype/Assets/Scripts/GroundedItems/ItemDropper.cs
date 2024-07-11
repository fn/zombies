using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDropper : MonoBehaviour
{
    // array to hold droppable items tagged Pickup
    [SerializeField] GameObject[] droppableItems;

    public void DropItem()
    {
        // make drop based on random range
        GameObject drop = droppableItems[Random.Range(0, droppableItems.Length)];

        // instantiate on condition
        Instantiate(drop);
    }


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        // test code
        //if(Input.GetKeyDown(KeyCode.P)) {
        //    DropItem();
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpawnBarricade : MonoBehaviour
{
    // which object to spawn
    [SerializeField] GameObject barricadeToSpawn;

    // how far you can interact
    [SerializeField] float interactRange;

    // new positions y offset
    [SerializeField] float yOffset;

    [SerializeField] int cost;
    public int placeHolderMoney = 10;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            // ray extended from camera to mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, interactRange) && placeHolderMoney >= cost)
            {
                if (hit.collider == GetComponent<Collider>())
                {
                    SpawnBarricade();
                }
            }
        }
    }

    public void SpawnBarricade()
    {
        // y offset for object
        Vector3 offset = new Vector3(0, yOffset, 0);

        // store position in new vector3
        Vector3 newPosition = transform.position + offset;

        // instantiate the object, position, and rotatation
        Instantiate(barricadeToSpawn, newPosition, transform.rotation);
    }
}

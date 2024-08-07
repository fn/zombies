using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampartMove : BaseAI
{
    [SerializeField] bool placeFound;
    
    void Update(){
        if (!placeFound){
            FindPosition();
            return;
        }
    }
    void FindPosition(){
        if (!seesPlayer){
            Move();
        }

        VisibilityCheck();

        if (seesPlayer){
            placeFound = true;
            EstablishPoint();
        }
    }

    void EstablishPoint(){
        UpdatePlayerDir();
        Vector3 newPos = transform.position - playerDir;
        agent.SetDestination(newPos);
        ///NavMesh.FindClosestEdge() learn about this <----
    }
}

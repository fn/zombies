using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomPathFinding : MonoBehaviour
{
    public GameObject Origin;
    public GameObject Target;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("No NavMesh Component detected. Add one in dummy");
    }

    //public void UpdatePath()
    //{
    //    Vector3 pointA = FindPointTo(Origin.transform.position, pointA, pointB, )
    //}

    //Vector3 FindPointTo()
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlankBehavior : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float flankDistance = 10f; // Distance from the player to flank
    public float flankOffset = 5f; // Offset distance for flanking
    private UnityEngine.AI.NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        FlankPlayer();
    }

    void FlankPlayer()
    {
        if (player == null) return;

        Vector3 directionToPlayer = player.position - transform.position;
        Vector3 rightFlank = Vector3.Cross(Vector3.up, directionToPlayer).normalized;

        Vector3 flankPosition = player.position + rightFlank * flankOffset;

        agent.SetDestination(flankPosition);
    }
}

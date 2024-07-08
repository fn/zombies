using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected float origStoppingDistance;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesPlayer;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;

    protected Vector3 playerDir;

    [SerializeField] protected GameObject player;


    void Start(){
        player = HordeManager.instance.Player();
    }

    protected void UpdatePlayerDir(){
        playerDir = player.transform.position - transform.position;
    }

    protected void VisibilityCheck(){
        UpdatePlayerDir();
        RaycastHit vis;
        if (Physics.Raycast(transform.position, playerDir, out vis, detectionRange)){
            Debug.DrawRay(transform.position, playerDir, Color.green);
            if (vis.collider.CompareTag("Player") && !vis.collider.CompareTag("Obstacle")){
                seesPlayer = true;
            }
            else{
            seesPlayer = false;
            }
        }
    }

    protected IEnumerator TargetCheck(float delay = 0.1f){
    yield return new WaitForSeconds(delay);

    if (agent.remainingDistance < agent.stoppingDistance) {
        nearPlayer = true;
    }
    else {
        nearPlayer = false;
    }
    }
    
    protected void Move(){
        agent.stoppingDistance = origStoppingDistance;
        agent.SetDestination(player.transform.position);
    }
}



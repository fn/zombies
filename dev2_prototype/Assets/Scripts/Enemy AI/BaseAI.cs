using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{

    [SerializeField] int faceTargetSpeed;
    protected float origStoppingDistance;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesPlayer;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;

    protected Vector3 playerDir;

    protected Vector3 movePosition;

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

        if (Distance() <=  origStoppingDistance) {
            nearPlayer = true;
        }
        else {
            nearPlayer = false;
        }

        yield return new WaitForSeconds(delay);
    }
    
    protected void FaceTarget(float faceSpeed = 1){
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    protected float Distance(){
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist;
    }
    protected void Move(){
        agent.stoppingDistance = origStoppingDistance;
        agent.SetDestination(movePosition);
    }
}



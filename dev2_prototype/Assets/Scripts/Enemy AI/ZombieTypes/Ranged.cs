using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : BaseZombie
{
    [SerializeField] float fleeingDist;
    [SerializeField] int faceTargetSpeed;

    [SerializeField] bool fleeing;
    public override void Seek(){
        agent.speed = movementSpeed;
        Move();
        VisibilityCheck();
        StartCoroutine(TargetCheck());
        if (nearPlayer){
            State(enemyState.ATTACK);
        }
    }
    

    float Distance(){
        float dist = Vector3.Distance(transform.position, player.transform.position);
        return dist;
    }
    
    void FaceTarget(){
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }
    public override void Attack(){
        if (Distance() < fleeingDist){
            fleeing = true;
            
        }

        if (fleeing){
            State(enemyState.FLEE);
            return;
        }
        
        VisibilityCheck();
        
        if (!seesPlayer && !fleeing){
            State(enemyState.SEEK);
            return;
        }
        
        

        FaceTarget();

        if (!attacking && !fleeing){
            StartCoroutine(Attacking());
            Debug.Log("Attack");
        }

    }

    public override void Flee(){
        agent.speed = 2 * movementSpeed;
        UpdatePlayerDir();
        Vector3 newPos = (transform.position - playerDir);
        //Debug.Log("New:" + newPos);
        //Debug.Log("Old: " + transform.position);
        agent.stoppingDistance = 0;
        agent.SetDestination(newPos);

        Debug.Log("Distance: " + Distance());
        if (Distance() >= detectionRange){
            fleeing = false;
            State(enemyState.SEEK);
        }

    }
}

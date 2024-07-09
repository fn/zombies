using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : BaseZombie
{
    [SerializeField] float fleeingDist;
    

    [SerializeField] bool fleeing;
    public override void Seek(){
        if (attacking){
            return;
        }
        agent.speed = movementSpeed;
        Move();
        VisibilityCheck();
        StartCoroutine(TargetCheck());
        if (nearPlayer){
            State(enemyState.ATTACK);
        }
    }
    

    
    
    
    public override void Attack(){
        FaceTarget();
        if (attacking){
            return;
        }
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
        
        

        

        if (!attacking && !fleeing){
            StartCoroutine(Attacking());
            State(enemyState.SEEK);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : BaseZombie
{

    public override void Seek(){
        
        int tempSpeed = movementSpeed;
        if (agent.remainingDistance < detectionRange){
            VisibilityCheck();
            if (SeesPlayer()){
                tempSpeed = movementSpeed * 2;
            }
            
        }
            
        
        agent.speed = tempSpeed;

        Move();
        
        StartCoroutine(TargetCheck(0.1f));
        if (nearPlayer){
            State(enemyState.ATTACK);
        }
    }

    
    public override void Attack(){
        if (!attacking){
            StartCoroutine(Attacking());
            Debug.Log("Attack");
        }
        else{
            State(enemyState.SEEK);
        }
    }
}

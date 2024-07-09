using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : BaseZombie
{

    public override void Seek(){
        Move();
        
        StartCoroutine(TargetCheck(0.1f));
        VisibilityCheck();
        
        int tempSpeed = movementSpeed;
        if (agent.remainingDistance < detectionRange){
            if (SeesPlayer()){
                tempSpeed = movementSpeed * 2;
            }
            
        }
        
        agent.speed = tempSpeed;

        if (attacking){
            return;
        }
        if (nearPlayer){
            State(enemyState.ATTACK);
        }
    }

    
    public override void Attack(){
        Attacking();
       
    }

    protected override void AttackLogic(){
        if (nearPlayer)
        {
            IDamage dmg = player.GetComponent<IDamage>();

            dmg.takeDamage(AttackDMG());
        }
    }
}

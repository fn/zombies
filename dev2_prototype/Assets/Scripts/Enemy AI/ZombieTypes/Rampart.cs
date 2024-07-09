using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rampart : BaseZombie
{
    List<GameObject> affected = new List<GameObject>();
    public override void  Seek(){
        StartCoroutine(TargetCheck());
        VisibilityCheck();
        if (attacking){
            return;
        }
        if (nearPlayer){
            FaceTarget();
        }
        
        if (nearPlayer && seesPlayer){
            State(enemyState.ATTACK);
            return;
        }
        agent.SetDestination(player.transform.position);
        
    }

    public override void Attack(){
        if (!attacking){
            
            attacking = true;
            StartCoroutine(Attacking());
            State(enemyState.SEEK);
            return;
        }

    }


    private void OnTriggerEnter(Collider other) {
        affected.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other) {
        affected.Remove(other.gameObject);
    }
    protected override void AttackLogic(){
        Debug.Log("Rampart Attack");

        if (affected.Count == 0){
            return;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rampart : BaseZombie
{
    [SerializeField] public List<GameObject> affected = new List<GameObject>();
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
         Attacking();
         State(enemyState.SEEK);

    }


    
    protected override void AttackLogic(){
        Debug.Log("Rampart Attack");

        if (affected.Count == 0){
            return;
        }

        foreach (GameObject obj in affected){
            IDamage dmg = obj.GetComponent<IDamage>();
            if (dmg == null){
                continue;
            }
            dmg.takeDamage(AttackDMG());
            Debug.Log("Attacking " + obj.name);
        }
    }
}

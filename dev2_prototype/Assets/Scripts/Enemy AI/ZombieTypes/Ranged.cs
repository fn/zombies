using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : BaseZombie
{
    [SerializeField] float fireRate;

    void Start(){
        weapon.damage = AttackDMG();
        weapon.rateOfFire = fireRate;
        agent.speed = movementSpeed;
        player = GameObject.FindWithTag("Player"); // HordeManager.instance.Player();
        render = GetComponent<Renderer>();
        colorOrig = render.material.color;
        origStoppingDistance = agent.stoppingDistance;
    }
    [SerializeField] float fleeingDist;
    
    [SerializeField] WeaponComponent weapon;

    [SerializeField] bool fleeing;
    public override void Seek(){
        
        
        agent.speed = movementSpeed;
        Move();
        VisibilityCheck();
        StartCoroutine(TargetCheck());
        if (nearPlayer && seesPlayer){
            State(enemyState.ATTACK);
        }
    }
    

    
    
    
    public override void Attack(){
        UpdatePlayerDir();
        FaceTarget();

        if (attacking)
        {
            Attacking();
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


        Attacking();
    }

    public override void Flee(){
        agent.speed = 2 * movementSpeed;
        UpdatePlayerDir();
        Vector3 newPos = (transform.position - playerDir);
        //Debug.Log("New:" + newPos);
        //Debug.Log("Old: " + transform.position);
        agent.stoppingDistance = 0;
        agent.SetDestination(newPos);
        if (Distance() >= detectionRange){
            fleeing = false;
            State(enemyState.SEEK);
        }

    }

    protected override void AttackLogic(){
        
        UpdatePlayerDir();
        weapon.Shoot(transform.position, playerDir);
    }
}

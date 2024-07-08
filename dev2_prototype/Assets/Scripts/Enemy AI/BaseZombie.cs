using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseZombie : MonoBehaviour, ZombieStates
{
//variables


public enum enemyState {NORMAL, SEEK, ATTACK, FLEE};
[SerializeField] protected int hp;
[SerializeField] protected int attackDamage;
[SerializeField] protected float attackSpeed;
[SerializeField] protected float attackCooldown;
[SerializeField] protected int movementSpeed;
[SerializeField] protected int destructionPower;
[SerializeField] protected int cost;
[SerializeField] protected enemyState state;
[SerializeField] protected int detectionRange;

[SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
protected Color colorOrig;
[SerializeField] protected Color colorPrimed;

protected Renderer render;

float origStoppingDistance;

protected Vector3 playerDir;

[SerializeField] protected bool attacking;

[SerializeField] protected bool seesPlayer;
[SerializeField] protected bool nearPlayer;

protected GameObject player;



int layerMask;

void Start(){
    render = GetComponent<Renderer>();
    colorOrig = render.material.color;
    player = HordeManager.instance.Player();
    origStoppingDistance = agent.stoppingDistance;
}
void Update(){
    switch (state){
        case enemyState.NORMAL:
        Normal();
        break;
        case enemyState.SEEK:
        Seek();
        break;
        case enemyState.ATTACK:
        Attack();
        break;
        case enemyState.FLEE:
        Flee();
        break;
    }
}

//get and set
int HP(int hp_ = 0){
    hp += hp_;
    return hp;
}

int AttackDMG(int input = 0){
    attackDamage += input;
    return attackDamage;
}

public float AttackSPD(float i = 0){
    attackSpeed += i;
    return attackSpeed;
}

int MoveSPD(int i = 0){
    movementSpeed += i;
    return movementSpeed;
}

int DestructionPWR(int i = 0){
    destructionPower += 0;
    return destructionPower;
}

int Cost(int i = 0){
    cost += i;
    return cost;
}

public bool SeesPlayer(){
    return seesPlayer;
}
public bool SeesPlayer(bool input){
    seesPlayer = input;
    return seesPlayer;
}
public enemyState State(){
    return state;
}
public enemyState State(int i){
    state = (enemyState)i;
    return state;
}
public enemyState State(enemyState i){
    state = i;
    return state;
}



virtual public void Normal(){}
virtual public void Seek(){}
virtual public void Attack(){}
virtual public void Flee(){}

protected void UpdatePlayerDir(){
    playerDir = player.transform.position - transform.position;
}
public void Move(){
    agent.stoppingDistance = origStoppingDistance;
    UpdatePlayerDir();
    agent.SetDestination(player.transform.position);
}





protected IEnumerator Attacking(){
    attacking = true;
    render.material.color = colorPrimed;
    yield return new WaitForSeconds(AttackSPD());
    attacking = false;
    render.material.color = colorOrig;
    state = enemyState.SEEK;
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
}


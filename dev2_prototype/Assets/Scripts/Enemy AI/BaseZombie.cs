using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseZombie : MonoBehaviour, ZombieStates
{
public enum enemyState {NORMAL, SEEK, ATTACK, FLEE};
//get and set
public int HP(){
    return hp;
}
public void HP(int i){
    hp = i;
}

public int AttackDMG(){
    return attackDamage;
}
public void AttackDMG(int i){
    attackDamage = i;
}

public float AttackSPD(){
    return attackSpeed;
}

public void AttackSPD(int i){
    attackSpeed = i;
}

public int MoveSPD(){
    return movementSpeed;
}
public void MoveSPD(int i){
    movementSpeed += i;
}

public int DestructionPWR(){
    return destructionPower;
}
public void DestructionPWR(int i){
    destructionPower = i;
}

public int Cost(){
    return cost;
}
public void Cost(int i){
    cost = i;
}

public bool IsAttacking(){
    return attacking;
}
void IsAttacking(bool i){
    attacking = i;
}
void IsAttackingToggle(){
    attacking = !attacking;
}

public bool SeesPlayer(){
    return seesPlayer;
}
public void SeesPlayer(bool input){
    seesPlayer = input;
}
public enemyState State(){
    return state;
}
public void State(int i){
    state = (enemyState)i;
}
public void State(enemyState i){
    state = i;
}


//states DO NOT TOUCH THESE ISTFG
virtual public void Normal(){}
virtual public void Seek(){}
virtual public void Attack(){}
virtual public void Flee(){}


//everything below this is protected or privated by the class and wont be able to accessed by other classes



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

protected void UpdatePlayerDir(){
    playerDir = player.transform.position - transform.position;
}
protected void Move(){
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


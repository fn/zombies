using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseZombie : BaseAI, ZombieStates
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
    movementSpeed = i;
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



protected Color colorOrig;
[SerializeField] protected Color colorPrimed;

protected Renderer render;

[SerializeField] protected bool attacking;

void Start(){
    player = HordeManager.instance.Player();
    render = GetComponent<Renderer>();
    colorOrig = render.material.color;
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



protected IEnumerator Attacking(){
    attacking = true;
    render.material.color = colorPrimed;
    yield return new WaitForSeconds(AttackSPD());
    attacking = false;
    render.material.color = colorOrig;
    state = enemyState.SEEK;
}


}


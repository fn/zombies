using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseZombie : BaseAI, ZombieStates, IDamage
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

public float AttackDelay(){
    return attackDelay;
}

public void AttackDelay(float i){
    attackDelay = i;
}

public float AttackCD(){
    return attackCooldown;
}
public void AttackCD(float i){
    attackCooldown = i;
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
[SerializeField] protected float attackDelay;
[SerializeField] protected float attackCooldown;
[SerializeField] protected int movementSpeed;
[SerializeField] protected int destructionPower;
[SerializeField] protected int cost;
[SerializeField] protected enemyState state;
    protected float lastAttackTime;






protected Color colorOrig;
[SerializeField] protected Color colorPrimed;

protected Renderer render;

[SerializeField] protected bool attacking;

void Start(){
    agent.speed = movementSpeed;
    player = HordeManager.instance.Player();
    render = GetComponent<Renderer>();
    colorOrig = render.material.color;
    origStoppingDistance = agent.stoppingDistance;
}
void Update(){
    
    if (!attacking){
        movePosition = player.transform.position;
    }
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
    FaceTarget();
}

public void takeDamage(int amount){
    hp -= amount;
    if (hp <= 0){
        Destroy(gameObject);
    }
    StartCoroutine(FlashDamage() ); 
}


private IEnumerator FlashDamage(){
    render.material.color = Color.red;
    yield return new WaitForSeconds(.5f);
    render.material.color = colorOrig;
}


protected void Attacking(){
     if (!attacking)
     {
         lastAttackTime = Time.time;
     }
     FaceTarget();
     attacking = true;
      render.material.color = colorPrimed;

        //new WaitForSeconds(AttackDelay());
    if (!Wait(AttackDelay(), ref lastAttackTime))
        return;
    
        
    
    AttackLogic();
    render.material.color = colorOrig;
   // return new WaitForSeconds(AttackCD());
   if (!Wait(AttackCD(), ref lastAttackTime))
        return;
    attacking = false;
  
}

protected virtual void AttackLogic(){}
    

}


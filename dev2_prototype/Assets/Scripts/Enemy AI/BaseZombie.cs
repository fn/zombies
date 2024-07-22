using System.Collections;
using UnityEngine;

public class BaseZombie : BaseAI, ZombieStates, IDamageable
{
    public enum enemyState { NORMAL, SEEK, ATTACK, FLEE, DEMOLITION, GATHER };

    public int HP { get => hp; set => hp = value; }
    public int AttackDMG { get => attackDamage; set => attackDamage = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }
    public float AttackCD { get => attackCooldown; set => attackCooldown = value; }
    public float MoveSPD { get => movementSpeed; set => movementSpeed = (int)value; }
    public int DestructionPWR { get => destructionPower; set => destructionPower = value; }
    public int Cost { get => cost; set => cost = value; }
    public bool IsAttacking { get => attacking; set => attacking = value; }
    public bool CommandComplete { get => commandComplete; set => commandComplete = value; }

    void IsAttackingToggle()
    {
        attacking = !attacking;
    }

    public bool SeesPlayer { get => seesPlayer; set => seesPlayer = value; }
    public enemyState State { get => state; set => state = value; }
    public Commander commander;
    public GameObject currentTarget;



    virtual public void Normal() { }
    virtual public void Seek() { }
    virtual public void Attack() { }
    virtual public void Flee()
    {
        agent.speed = 2 * movementSpeed;
        UpdatePlayerDir();
        Vector3 newPos = (transform.position - playerDir);
        //Debug.Log("New:" + newPos);
        //Debug.Log("Old: " + transform.position);
        agent.stoppingDistance = 0;
        agent.SetDestination(newPos);
        if (GetDistanceToPlayer() >= detectionRange)
        {
            fleeing = false;
            State = enemyState.SEEK;
        }
    }
    virtual public void Gather()
    {
        agent.speed = movementSpeed;
        agent.SetDestination(commander.transform.position);
    }

    //everything below this is protected or privated by the class and wont be able to accessed by other classes

    [SerializeField] protected int hp;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int destructionPower;
    [SerializeField] protected int cost;
    [SerializeField] protected enemyState state;
    protected bool fleeing;
    protected float attackTimer;
    protected bool commandComplete;

    protected enum attackPhase { IDLE, PRIMED, ATTACK, RECOVERY };

    [SerializeField] protected attackPhase phase;

    protected Color origColor;
    [SerializeField] protected Color colorPrimed;

    protected Renderer model;

    [SerializeField] protected bool attacking;
    [SerializeField] protected Vector3 maxDistance;


    void Start()
    {
        agent.speed = movementSpeed;

        targetPlayer = GameManager.Instance.LocalPlayer;
        currentTarget = targetPlayer.gameObject;

        model = GetComponent<Renderer>();
        origColor = GetComponent<Renderer>().material.color;
        origStoppingDistance = agent.stoppingDistance;
    }

    void Update()
    {

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (!attacking)
        {
            movePosition = targetPlayer.transform.position;
        }
        switch (state)
        {
            case enemyState.NORMAL:
                Normal();
                break;
            case enemyState.SEEK:
                currentTarget = targetPlayer.gameObject;
                FaceTarget();
                Seek();
                break;
            case enemyState.ATTACK:
                Attack();
                break;
            case enemyState.FLEE:
                Flee();
                break;
            case enemyState.DEMOLITION:
                Attacking();
                break;
            case enemyState.GATHER:
                Gather();
                break;
        }

    }

    public void DestinationCommand(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
    public void TakeDamage(int amount)
    {
        //check originally to snap zombies out of breaking barricades if attacked, but that just lets them phase right through the barricade (plus defeats the point of barricades anyway)
        //if (State == enemyState.DEMOLITION || State == enemyState.NORMAL)
        //    State = enemyState.SEEK;
        hp -= amount;
        if (hp <= 0)
        {
            // 25% drop chance
            if (Random.Range(0, 101) <= 25)
            {
                // check if this object does have ItemDropper
                if (TryGetComponent<ItemDropper>(out ItemDropper item))
                {
                    // pass this items location and rotation to DropItem
                    item.DropItem(transform.position, transform.rotation);
                }
            }
            Destroy(gameObject);
        }

        StartCoroutine(FlashDamage());
    }

    private IEnumerator FlashDamage()
    {
        GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        GetComponent<Renderer>().material.color = origColor;
    }

    protected void Attacking()
    {
        seesPlayer = false;
        agent.ResetPath();
        switch (phase)
        {
            case attackPhase.IDLE:
                phase++;
                return;
            case attackPhase.PRIMED:
                attacking = true;
                attackTimer = AttackDelay;
                phase++;

                break;
            case attackPhase.ATTACK:
                if (attackTimer < 0)
                {
                    phase++;

                    if (currentTarget.tag.Contains("Barricade"))
                    {
                        GameObject barrChild = currentTarget.gameObject.transform.GetChild(0).gameObject;

                        if (barrChild.TryGetComponent(out IDamageable dmg))
                            dmg.TakeDamage(destructionPower);
                        if (!barrChild.activeSelf)
                            State = enemyState.ATTACK;
                    }
                    else
                        AttackLogic();

                    attackTimer = AttackCD;
                }
                break;
            case attackPhase.RECOVERY:
                if (attackTimer < 0)
                {
                    attacking = false;
                    phase = attackPhase.IDLE;
                    if (State != enemyState.DEMOLITION)
                        state = enemyState.SEEK;
                }
                break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Barricade"))
        {
            //checks if barricadeSpawner's child is active

            if (other.gameObject.TryGetComponent(out Barricade barricade) && barricade.IsBroken)
                return;

            currentTarget = other.gameObject;
            State = enemyState.DEMOLITION;
        }
    }

    protected virtual void AttackLogic() { }
}
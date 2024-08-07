using System.Collections;
using UnityEngine;

public class BaseZombie : BaseAI, ZombieStates, IDamageable
{
    public enum enemyState { NORMAL, SEEK, ATTACK, FLEE, DEMOLITION, GATHER, FLANK, DEAD };

    public int HP { get => hp; set => hp = value; }
    public int AttackDMG { get => attackDamage; set => attackDamage = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }
    public float AttackCD { get => attackCooldown; set => attackCooldown = value; }
    public float MoveSPD { get => movementSpeed; set => movementSpeed = (int)value; }
    public int DestructionPWR { get => destructionPower; set => destructionPower = value; }
    public int Cost { get => cost; set => cost = value; }
    public bool IsAttacking { get => attacking; set => attacking = value; }
    public bool IsInMain { get => inMainGroup; set => inMainGroup = value; }
    public bool Free { get => free; set => free = value; }

    void IsAttackingToggle()
    {
        attacking = !attacking;
    }

    
    public enemyState State { get => state; set => state = value; }
    public Commander commander;
    



    virtual public void Normal() { }
    virtual public void Seek() { }
    virtual public void Attack() { }
    virtual public void Flee()
    {
        agent.speed = 2 * movementSpeed;
        UpdateTargetDir();
        Vector3 newPos = (transform.position - playerDir);
        //Debug.Log("New:" + newPos);
        //Debug.Log("Old: " + transform.position);
        agent.stoppingDistance = 0;
        agent.SetDestination(newPos);
        if (GetDistanceToTarget() >= detectionRange)
        {
            fleeing = false;
            State = enemyState.SEEK;
        }
    }
    virtual public void Gather()
    {
        agent.speed = movementSpeed;
        if (inMainGroup)
            agent.SetDestination(commander.transform.position);
        else
            agent.SetDestination(commander.movePos.transform.position);

        if (Vector3.Distance(transform.position, commander.transform.position) <= agent.stoppingDistance + 1)
        {
            State = enemyState.NORMAL;
            commander.readyZombies++;
            free = true;
        }
    }
    virtual public void Flank()
    {
        agent.speed = 2 * movementSpeed;
        FlankTarget(currentTarget.transform, commander.flankingDeviation);
        if (Vector3.Distance(transform.position, currentTarget.transform.position) <= agent.stoppingDistance + commander.flankingDeviation)
        {
            State = enemyState.SEEK;
        }
    }

    virtual public void Dead()
    {
        free = false;
        agent.ResetPath();

        if (animator != null)
            animator.SetBool("Dead", true);
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
    protected bool inMainGroup;
    protected bool free;

    protected enum attackPhase { IDLE, PRIMED, ATTACK, RECOVERY };

    [SerializeField] protected attackPhase phase;

    protected Color origColor;
    [SerializeField] protected Color colorPrimed;

    // protected Renderer model;

    [SerializeField] protected bool attacking;
    [SerializeField] protected Vector3 maxDistance;

    public Animator animator;
    int hpOriginal;
    Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
        hpOriginal = hp;
        free = true;
        agent.speed = movementSpeed;

        currentTarget = GameManager.Instance.LocalPlayer.gameObject;

        // model = GetComponent<Renderer>();
        origColor = Color.white; //GetComponent<Renderer>().material.color;
        origStoppingDistance = agent.stoppingDistance;

        AddDetLayer("Player");
        AddDetLayer("Default");
        
    }

    void Update()
    {
        // if (attackTimer > 0)
        // {
        //     attackTimer -= Time.deltaTime;
        // }

        // if (!attacking)
        // {
        //     movePosition = currentTarget.transform.position;
        // }
        // if (seesTarget)
        // {
        //     if (commander != null)
        //         commander.PlayerVisible();
        // }

        // if (animator != null)
        //     animator.SetFloat("Speed", agent.velocity.normalized.magnitude);

        // if (TryGetComponent(out CommanderLine commanderLine))
        // {
        //     commanderLine.enabled = commander != null;
        //     if (commanderLine.enabled)
        //         commanderLine.commanderPoint = commander.transform;
        // }

        // Temp dead check.
        if (hp <= 0)
            State = enemyState.DEAD;

        switch (state)
        {
            case enemyState.NORMAL:
                Normal();
                break;
            case enemyState.SEEK:
                free = false;
                FaceTarget();
                currentTarget = GameManager.Instance.LocalPlayer.gameObject;
                Seek();
                break;
            case enemyState.ATTACK:
                free = false;
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
            case enemyState.FLANK:
                Flank();
                break;
            case enemyState.DEAD:
                attacking = false;
                phase = attackPhase.IDLE;
                Dead();
                return;
        }

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (!attacking)
        {
            movePosition = currentTarget.transform.position;
        }
        if (seesTarget)
        {
            if (commander != null)
                commander.PlayerVisible();
        }

        if (gameObject.TryGetComponent(out CommanderLine commanderLine))
        {
            if (commander != null && commander.State != enemyState.DEAD)
            {
                commanderLine.enabled = true;
                commanderLine.commanderPoint = commander.transform;
            }
            else
                commanderLine.enabled = false;
        }

        if (animator != null)
            animator.SetFloat("Speed", agent.velocity.normalized.magnitude);
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

        if (amount > 0)
        {
            StartCoroutine(FlashDamage(false));
            if (phase == attackPhase.RECOVERY)
                hp -= amount;
        }
        else
            StartCoroutine(FlashDamage(true));


        if (hp > hpOriginal * 2)
            hp = hpOriginal * 2;
        if (hp <= 0 && State != enemyState.DEAD)
        {
            GameManager.Instance.zombieDead.Add(this); //adds to the dead pile

            //if (commander != null)
            //{
            //    if (inMainGroup)
            //        commander.mainGroup.Remove(this);
            //    else
            //        commander.flankGroup.Remove(this);
            //}

            // Give player money
            GameManager.Instance.LocalPlayer.Money += 50;

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
            //Destroy(gameObject);
            col.isTrigger = true;
            State = enemyState.DEAD;
            return;
        }
        if (hp > 0 && State == enemyState.DEAD)
        {
            
            agent.ResetPath();
            col.isTrigger = false;

            if (animator != null)
            {
                animator.SetBool("Dead", false);
            }

            GameManager.Instance.zombieDead.Remove(this);
            State = enemyState.SEEK;
            free = true;
        }
        
    }

    private IEnumerator FlashDamage(bool heal)
    {
        yield return new WaitForSeconds(.5f);
    }

    protected void Attacking()
    {
        if (State == enemyState.DEAD)
            return;

        seesTarget = false;
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
                attackTimer = AttackCD;
                phase++;

                if (animator != null)
                {
                    if (currentTarget.tag.Contains("Barricade"))
                        animator.SetTrigger("BarricadeAttack");
                    else
                        animator.SetTrigger("Attack");
                    return;
                }

                AttackLogic();

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
        if (State == enemyState.DEAD)
            return;

        if (other.tag.Contains("Barricade"))
        {
            if (other.gameObject.TryGetComponent(out Barricade barricade) && barricade.IsBroken)
            {
                return;
            }

            currentTarget = other.gameObject;
            State = enemyState.DEMOLITION;
        }
    }
 
    protected virtual void AttackLogic() { }
}
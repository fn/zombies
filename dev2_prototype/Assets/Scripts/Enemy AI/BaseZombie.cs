using System.Collections;
using UnityEngine;
using Zombies.AI;
using Zombies.AI.States;

public abstract class BaseZombie : BaseAI, /*ZombieStates,*/ IDamageable
{
    public int Health { get => health; set => health = value; }
    public int AttackDamage { get => attackDamage; set => attackDamage = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = (int)value; }
    public int DestructionPower { get => destructionPower; set => destructionPower = value; }
    public int Cost { get => cost; set => cost = value; }
    public bool IsAttacking { get => attacking; set => attacking = value; }
    public bool IsInMainGroup { get => inMainGroup; set => inMainGroup = value; }
    public bool Free { get => free; set => free = value; }

    // public enemyState State { get => state; set => state = value; }
    public Commander commander;

    public BaseAIState CurrentState;
    public EnemyState CurrentStateName;
    // virtual public void Normal() { }
    // virtual public void Seek() { }
    // virtual public void Attack() { }

    // These should ideally just be temporary.
    public abstract BaseAIState GetNormalState();
    public abstract BaseAIState GetAttackState();
    public abstract BaseAIState GetSeekState();

    virtual public void Flee()
    {
        agent.speed = 2 * movementSpeed;
        UpdateTargetDir();
        Vector3 newPos = (transform.position - targetDir);
        //Debug.Log("New:" + newPos);
        //Debug.Log("Old: " + transform.position);
        agent.stoppingDistance = 0;
        agent.SetDestination(newPos);
        if (GetDistanceToTarget() >= detectionRange)
        {
            fleeing = false;

            UpdateState(GetSeekState());
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
            // Set normal state.
            UpdateState(GetNormalState());

            // State = enemyState.NORMAL;
            commander.readyZombies++;
            free = true;
        }
    }

    virtual public void Flank()
    {
        agent.speed = 2 * movementSpeed;
        FlankTarget(CurrentTarget.transform, commander.flankingDeviation);
        if (Vector3.Distance(transform.position, CurrentTarget.transform.position) <= agent.stoppingDistance + commander.flankingDeviation)
        {
            // Set seek state.
            UpdateState(GetSeekState());
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

    [SerializeField] protected int health;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int destructionPower;
    [SerializeField] protected int cost;
    
    protected bool fleeing;
    protected float attackTimer;
    protected bool inMainGroup;
    protected bool free;
    
    public bool IsFleeing { get => fleeing; set => fleeing = value; }

    protected enum AttackPhases { IDLE, PRIMED, ATTACK, RECOVERY };

    [SerializeField] protected AttackPhases AttackPhase;

    // We will have a shader for this stuff.
    // protected Color origColor;
    // [SerializeField] protected Color colorPrimed;

    [SerializeField] protected bool attacking;
    [SerializeField] protected Vector3 maxDistance;

    public Animator animator;
    int origHP;
    Collider col;
    void Start()
    {
        col = GetComponent<Collider>();
        origHP = health;
        free = true;
        agent.speed = movementSpeed;

        CurrentTarget = GameManager.Instance.LocalPlayer.gameObject;

        origStoppingDistance = agent.stoppingDistance;

        // AddDetLayer("Player");
        // AddDetLayer("Default");
        AddDetLayers(new string[] { "Player", "Default" });

        UpdateState(GetSeekState());
    }

    public void UpdateState(BaseAIState aiState)
    {
        CurrentState = aiState;
    }

    public void ResetAttack()
    {
        IsAttacking = false;
        AttackPhase = AttackPhases.IDLE;
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
        if (health <= 0)
            UpdateState(new DeadState(this));

        if (CurrentState != null)
        {
            CurrentStateName = CurrentState.Name;
            CurrentState.Run();
        }

        //switch (state)
        //{
        //    case enemyState.NORMAL:
        //        Normal();
        //        break;
        //    case enemyState.SEEK:
        //        free = false;
        //        FaceTarget();
        //        currentTarget = GameManager.Instance.LocalPlayer.gameObject;
        //        Seek();
        //        break;
        //    case enemyState.ATTACK:
        //        free = false;
        //        Attack();
        //        break;
        //    case enemyState.FLEE:
        //        Flee();
        //        break;
        //    case enemyState.DEMOLITION:
        //        Attacking();
        //        break;
        //    case enemyState.GATHER:
        //        Gather();
        //        break;
        //    case enemyState.FLANK:
        //        Flank();
        //        break;
        //    //case enemyState.DEAD:
        //    //    attacking = false;
        //    //    AttackPhase = AttackPhases.IDLE;
        //    //    Dead();

        //        return;
        //}

        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if (!attacking)
        {
            movePosition = CurrentTarget.transform.position;
        }
        if (seesTarget)
        {
            if (commander != null)
                commander.PlayerVisible();
        }

        if (gameObject.TryGetComponent(out CommanderLine commanderLine))
        {
            if (commander != null && commander.CurrentState != null && commander.CurrentState.Name != EnemyState.DEAD)
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
        health -= amount;

        if (amount > 0)
        {
            if (AttackPhase == AttackPhases.RECOVERY)
                health -= amount;
        }

        if (health > origHP * 2)
            health = origHP * 2;
        if (health <= 0 && CurrentState != null && CurrentState.Name != EnemyState.DEAD)
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
                if (TryGetComponent(out ItemDropper item))
                {
                    // pass this items location and rotation to DropItem
                    item.DropItem(transform.position, transform.rotation);
                }
            }
            //Destroy(gameObject);
            col.isTrigger = true;

            // Set the current state to dead.
            UpdateState(new DeadState(this));
            return;
        }
        if (health > 0 && CurrentState != null && CurrentState.Name == EnemyState.DEAD)
        {
            agent.ResetPath();
            col.isTrigger = false;

            if (animator != null)
            {
                animator.SetBool("Dead", false);
            }

            GameManager.Instance.zombieDead.Remove(this);
            
            // We have been revived. Set state back to seek state.
            UpdateState(GetSeekState());
            free = true;
        }
    }

    public void DoPhasedAttack()
    {
        seesTarget = false;
        agent.ResetPath();
        switch (AttackPhase)
        {
            case AttackPhases.IDLE:
                AttackPhase++;
                return;
            case AttackPhases.PRIMED:
                attacking = true;
                attackTimer = AttackDelay;
                AttackPhase++;
                break;
            case AttackPhases.ATTACK:
                attackTimer = AttackCooldown;
                AttackPhase++;

                if (animator != null)
                {
                    animator.SetTrigger(CurrentTarget.tag.Contains("Barricade") ? "BarricadeAttack" : "Attack");
                    return;
                }

                AttackLogic();

                break;
            case AttackPhases.RECOVERY:
                if (attackTimer < 0)
                {
                    attacking = false;
                    AttackPhase = AttackPhases.IDLE;
                    if (CurrentState != null && CurrentState.Name != EnemyState.DEMOLITION)
                        UpdateState(GetSeekState());
                }
                break;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (CurrentState != null && CurrentState.Name == EnemyState.DEAD)
            return;

        if (other.tag.Contains("Barricade"))
        {
            if (other.gameObject.TryGetComponent(out Barricade barricade) && barricade.IsBroken)
            {
                return;
            }

            CurrentTarget = other.gameObject;
            
            // Set current state to demolition.
            UpdateState(new DemolitionState(this));
        }
    }
 
    protected virtual void AttackLogic() { }
}
using System.Collections;
using UnityEngine;

public class BaseZombie : BaseAI, ZombieStates, IDamage
{
    public enum enemyState { NORMAL, SEEK, ATTACK, FLEE };

    public int HP { get => hp; set => hp = value; }
    public int AttackDMG { get => attackDamage; set => attackDamage = value; }
    public float AttackDelay { get => attackDelay; set => attackDelay = value; }
    public float AttackCD { get => attackCooldown; set => attackCooldown = value; }
    public float MoveSPD { get => movementSpeed; set => movementSpeed = (int)value; }
    public int DestructionPWR { get => destructionPower; set => destructionPower = value; }
    public int Cost { get => cost; set => cost = value; }
    public bool IsAttacking { get => attacking; set => attacking = value; }

    void IsAttackingToggle()
    {
        attacking = !attacking;
    }

    public bool SeesPlayer { get => seesPlayer; set => seesPlayer = value; }
    public enemyState State { get => state; set => state = value; }

    //states DO NOT TOUCH THESE ISTFG
    virtual public void Normal() { }
    virtual public void Seek() { }
    virtual public void Attack() { }
    virtual public void Flee() { }

    //everything below this is protected or privated by the class and wont be able to accessed by other classes

    [SerializeField] protected int hp;
    [SerializeField] protected int attackDamage;
    [SerializeField] protected float attackDelay;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected int movementSpeed;
    [SerializeField] protected int destructionPower;
    [SerializeField] protected int cost;
    [SerializeField] protected enemyState state;
    protected float attackTimer;
    protected enum attackPhase { IDLE, PRIMED, ATTACK, RECOVERY };

    [SerializeField] protected attackPhase phase;

    protected Color origColor;
    [SerializeField] protected Color colorPrimed;

    protected Renderer renderer;

    [SerializeField] protected bool attacking;

    void Start()
    {
        agent.speed = movementSpeed;

        targetPlayer = GameManager.Instance.LocalPlayer;

        renderer = GetComponent<Renderer>();
        origColor = renderer.material.color;
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
                Seek();
                break;
            case enemyState.ATTACK:
                Attack();
                break;
            case enemyState.FLEE:
                Flee();
                break;
        }
        if (State == enemyState.SEEK)
            FaceTarget();
    }

    public void takeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            ItemDropper itemToDrop = GetComponent<ItemDropper>();
            itemToDrop.DropItem();
            Destroy(gameObject);
        }

        StartCoroutine(FlashDamage());
    }

    private IEnumerator FlashDamage()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(.5f);
        renderer.material.color = origColor;
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
                    AttackLogic();
                    attackTimer = AttackCD;
                }
                break;
            case attackPhase.RECOVERY:
                if (attackTimer < 0)
                {
                    attacking = false;
                    phase = attackPhase.IDLE;
                    state = enemyState.SEEK;
                }
                break;
        }
    }

    protected virtual void AttackLogic() { }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Commander : BaseZombie
{
    
    [Tooltip("Percentage of zeds being sent to flanking")]
    [Range(0, 50)]
    [SerializeField] public int flankPercent;
    [Tooltip("the bigger the number, the further the flanking zeds will be from the player")]
    public int flankingDeviation;
    public GameObject movePos;
    public List<BaseZombie> mainGroup;
    public List<BaseZombie> flankGroup;
    public int readyZombies;
    bool commandSent;
    bool flanking;

    
    
    public override void Seek()
    {
        agent.speed = MoveSPD;
        Move();
    }

    public override void Gather()
    {
        SendCommand(mainGroup, enemyState.GATHER);
        SendCommand(flankGroup, enemyState.GATHER);
        State = enemyState.NORMAL;
    }

    public override void Normal()
    {
       
            
        UpdateTargetDir();
        FaceTarget();
        TargetVisibilityCheck();
        if (seesTarget)
        {
            State = enemyState.ATTACK;
            return;
        }

        if (readyZombies >= mainGroup.Count / 2)
        {
            State = enemyState.FLANK;
            readyZombies = 0;
        }
    }

    public override void Attack()
    {
        UpdateTargetDir();
        FaceTarget();
        SendCommand(mainGroup, enemyState.SEEK);
        SendCommand(mainGroup, currentTarget);
        SendCommand(flankGroup, enemyState.SEEK);
        SendCommand(flankGroup, currentTarget);
        if (!seesTarget) State = enemyState.NORMAL;
        if (attackTimer <= 0)
        {
            attackTimer = attackCooldown;
            HealZeds();
        }

    }

    public override void Flank()
    {
        SendCommand(mainGroup, enemyState.SEEK);
        SendCommand(flankGroup, enemyState.FLANK);
        State = enemyState.NORMAL;
    }

    public override void Dead()
    {
        Destroy(gameObject);
    }

    public void HealZeds()
    {
        foreach (var z in mainGroup)
        {
            IDamageable dmg = z.GetComponent<IDamageable>();
            if (dmg == null)
                continue;
            dmg.TakeDamage(-attackDamage);
        }
        foreach (var z in flankGroup)
        {
            IDamageable dmg = z.GetComponent<IDamageable>();
            if (dmg == null)
                continue;
            dmg.TakeDamage(-attackDamage);
        }

    }

    public void PlayerVisible()
    {
        seesTarget = true;
        State = enemyState.ATTACK;
    }

    //change state
    void SendCommand(List<BaseZombie> horde, enemyState state)
    {
        readyZombies = 0;
        foreach (var z in horde)
        {
            if (!z.Free)
                continue;
            z.State = state;
            z.Free = false;
        }
    }
    //gives info about player/location
    void SendCommand(List<BaseZombie> horde, GameObject target)
    {
        readyZombies = 0;
        foreach (var z in horde)
        {
            if (!z.Free)
                continue;
            z.currentTarget = target;
            if (!z.SeesTarget)
                z.SeesTarget = seesTarget;
            z.DestinationCommand(target.transform.position);
            z.Free = false;
        }
            
    }

    


    ////gives a spot to move to/gather around
    //void SendCommand(Vector3 destination)
    //{
    //    readyZombies = 0;
    //    foreach (var z in mainGroup)
    //    {
    //        z.DestinationCommand(destination);
    //        z.CommandComplete = false;
    //    }
    //}
}

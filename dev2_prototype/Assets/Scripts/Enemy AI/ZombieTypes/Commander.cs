using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using Zombies.AI;
using Zombies.AI.States;

public class Commander : BaseZombie
{
    [Tooltip("Percentage of zeds being sent to flanking")]
    [Range(0, 50)]
    [SerializeField] public int flankPercent;
    [Tooltip("the bigger the number, the further the flanking zeds will be from the player")]
    public int flankingDeviation;
    public GameObject movePos;
    public List<BaseZombie> MainGroup;
    public List<BaseZombie> FlankGroup;
    public int readyZombies;
    float commandTimer;
    bool commandSent;
    bool flanking;

    //public override void Seek()
    //{
    //    agent.speed = MovementSpeed;
    //    Move();
    //}

    public override void Gather()
    {
        // SendCommand(mainGroup, enemyState.GATHER);
        // SendCommand(flankGroup, enemyState.GATHER);
        // State = enemyState.NORMAL;
        // 
        // UpdateState(GetNormalState());
    }

    //public override void Normal()
    //{
    //    //UpdateTargetDir();
    //    //FaceTarget();
    //    //TargetVisibilityCheck();
    //    //if (seesTarget)
    //    //{
    //    //    UpdateState(GetAttackState());
    //    //    return;
    //    //}

    //    //if (readyZombies >= mainGroup.Count / 2)
    //    //{
    //    //    UpdateState(new FlankState(this));
    //    //    readyZombies = 0;
    //    //}
    //}

    //public override void Attack()
    //{
    //    //UpdateTargetDir();
    //    //StartCoroutine(TargetProximityCheck());
    //    //FaceTarget();
        
    //    //if (!seesTarget) State = enemyState.NORMAL;
    //    //if (attackTimer <= 0)
    //    //{
    //    //    attackTimer = attackCooldown;
    //    //    HealZeds();
    //    //}
    //    //commandTimer -= Time.deltaTime;

    //    //if (commandTimer < 0)
    //    //{
    //    //    commandTimer = 5;
    //    //    commandSent = false;
    //    //}
    //    //if (nearPlayer) 
    //    //    State = enemyState.FLEE;


    //    //if (commandSent)
    //    //    return;


    //    //SendCommand(mainGroup, nameof(GetSeekState));
    //    //SendCommand(mainGroup, currentTarget);
    //    //SendCommand(flankGroup, nameof(GetSeekState));
    //    //SendCommand(flankGroup, currentTarget);
    //    //commandSent = true;

    //}

    public override void Flank()
    {
        //SendCommand(mainGroup, enemyState.SEEK);
        //SendCommand(flankGroup, enemyState.FLANK);
        //State = enemyState.NORMAL;
    }

    public override void Dead()
    {
        Destroy(gameObject);
    }

    public void HealZeds()
    {
        // Loop over the main group and flank group.
        foreach (var z in MainGroup.Concat(FlankGroup))
        {
            if (z.TryGetComponent(out IDamageable dmg))
                dmg.TakeDamage(-attackDamage);
        }

        // foreach (var z in FlankGroup)
        // {
        //     IDamageable dmg = z.GetComponent<IDamageable>();
        //     if (dmg == null)
        //         continue;
        //     dmg.TakeDamage(-attackDamage);
        // }

    }

    public void PlayerVisible()
    {
        seesTarget = true;
        
        // Set our attack state.
        UpdateState(GetAttackState());
    }

    //change state
    void SendCommand(List<BaseZombie> horde, string stateName)
    {
        readyZombies = 0;
        foreach (var z in horde)
        {
            if (!z.Free)
                continue;

            var state = z.GetType().GetMethod(stateName).Invoke(z, null);

            z.UpdateState((BaseAIState)state);
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
            z.CurrentTarget = target;
            if (!z.SeesTarget)
                z.SeesTarget = seesTarget;

            z.DestinationCommand(target.transform.position);
            z.Free = false;
        }
            
    }

    public override BaseAIState GetNormalState()
    {
        return new CommanderNormalState(this);
    }

    public override BaseAIState GetAttackState()
    {
        throw new System.NotImplementedException();
    }

    public override BaseAIState GetSeekState()
    {
        throw new System.NotImplementedException();
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

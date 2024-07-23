using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Commander : BaseZombie
{
    
    [Tooltip("Percentage of zeds being sent to flanking")]
    [Range(0, 50)]
    [SerializeField] int flankPercent;
    [Tooltip("the bigger the number, the further the flanking zeds will be from the player")] 
    public int flankingDeviation;
    public GameObject movePos;
    public List<BaseZombie> mainGroup;
    public List<BaseZombie> flankGroup;
    public int readyZombies;
    bool commandSent;
    bool flanking;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            BaseZombie z = other.gameObject.GetComponent<BaseZombie>();
            if (z.commander != null)
                return;
            if (z.State != enemyState.SEEK)
                return;

            z.commander = this;
            z.State = enemyState.GATHER;

            int total = mainGroup.Count + flankGroup.Count;
            float per = (float)flankPercent / 100;
            float totPer = total * per;

            if (flankGroup.Count >= totPer)
            {
                mainGroup.Add(z);
                z.IsInMain = true;
            }
            else
            {
                z.IsInMain = false;
                flankGroup.Add(z);
            }
                
        }

        if (other.gameObject.tag == "Player")
        {
            agent.ResetPath();
            State = enemyState.GATHER;
        }
        
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            BaseZombie z = other.gameObject.GetComponent<BaseZombie>();
            if (z.commander != this)
                return;

            z.commander = null;
        }

        if (other.gameObject.tag == "Player")
        {
            State = enemyState.SEEK;
        }
    }
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
       
            
        UpdatePlayerDir();
        FaceTarget();
        VisibilityCheck();
        if (seesPlayer)
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
        SendCommand(mainGroup, enemyState.SEEK);
        SendCommand(mainGroup, currentTarget);
        SendCommand(flankGroup, enemyState.SEEK);
        SendCommand(flankGroup, currentTarget);
        if (!seesPlayer) State = enemyState.NORMAL;
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
        seesPlayer = true;
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
            if (!z.SeesPlayer)
                z.SeesPlayer = SeesPlayer;
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

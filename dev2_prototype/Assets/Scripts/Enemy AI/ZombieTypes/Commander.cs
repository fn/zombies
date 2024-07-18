using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Commander : BaseZombie
{
    List<BaseZombie> commandedZombies = new List<BaseZombie>();
    bool commandSent;
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
            commandedZombies.Add(z);
        }

        if (other.gameObject.tag == "Player")
        {
            agent.ResetPath();
            State = enemyState.GATHER;
            commandSent = true;
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
        SendCommand(enemyState.GATHER);
        State = enemyState.NORMAL;
    }

    public override void Normal()
    {
        UpdatePlayerDir();
        FaceTarget();
        VisibilityCheck();
        if (seesPlayer) State = enemyState.ATTACK;
    }

    public override void Attack()
    {
        SendCommand(enemyState.SEEK);
        SendCommand(currentTarget);
        if (seesPlayer) State = enemyState.NORMAL;
    }
    //change state
    void SendCommand(enemyState state)
    {
        foreach (var z in commandedZombies)
        {
            z.State = state;
        }
    }
    //gives info about player
    void SendCommand(GameObject target)
    {
        foreach (var z in commandedZombies)
        {
            z.currentTarget = target;
            z.SeesPlayer = true;
            z.DestinationCommand(target.transform.position);
        }
            
    }
}

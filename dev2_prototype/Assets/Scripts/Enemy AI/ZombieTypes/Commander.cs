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
    [Range(0f, 1f)]
    [SerializeField] float flankingDeviation;
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
            
        if (flanking)
            return;

        if (readyZombies >= mainGroup.Count / 2)
        {
            flanking = true;
            Flanking();
        }
    }

    public override void Attack()
    {
        SendCommand(mainGroup, enemyState.SEEK);
        SendCommand(mainGroup, currentTarget);
        if (!seesPlayer) State = enemyState.NORMAL;

    }

    void Flanking()
    {
        float dev = 100f * flankingDeviation; 
        movePos.transform.position = 
            new Vector3(Random.Range(targetPlayer.transform.position.x - dev, targetPlayer.transform.position.x + dev),
            targetPlayer.transform.position.y,
            Random.Range(targetPlayer.transform.position.z - dev, targetPlayer.transform.position.z + dev));

        SendCommand(flankGroup, enemyState.GATHER);
        SendCommand(flankGroup, movePos);
    }

    //change state
    void SendCommand(List<BaseZombie> horde, enemyState state)
    {
        readyZombies = 0;
        foreach (var z in horde)
        {
            z.State = state;
        }
    }
    //gives info about player/location
    void SendCommand(List<BaseZombie> horde, GameObject target)
    {
        readyZombies = 0;
        foreach (var z in horde)
        {
            z.currentTarget = target;
            if (!z.SeesPlayer)
                z.SeesPlayer = SeesPlayer;
            z.DestinationCommand(target.transform.position);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rampart : BaseZombie
{
    [SerializeField] SphereCollider findSpots;
    [SerializeField] LayerMask HideLayers;
    [Range(-1f, 1f)]
    [SerializeField] float hideFactor;
    [SerializeField] public List<GameObject> affected = new List<GameObject>();
    [SerializeField] Collider colli;
    [SerializeField] private Collider[] hideSpots = new Collider[10];
    [SerializeField] bool rushing;
    [SerializeField] Vector3 oRotation;
    public override void Seek()
    {
        agent.stoppingDistance = origStoppingDistance;
        StartCoroutine(TargetCheck());
        VisibilityCheck();
        if (attacking)
        {
            return;
        }
        if (nearPlayer)
        {
            FaceTarget();
        }
        else
        {
            colli.enabled = false;
        }

        if (nearPlayer && seesPlayer)
        {
            State = enemyState.ATTACK;
            return;
        }
        //agent.SetDestination(targetPlayer.transform.position);
        MoveLogic();
    }

    public override void Attack()
    {
        Debug.Log((transform.forward - oRotation).sqrMagnitude);
        colli.enabled = agent.velocity.sqrMagnitude >= 25;
        rushing = agent.velocity.sqrMagnitude >= 25;
        if (rushing) {
            if (agent.velocity.sqrMagnitude < 25 || (transform.forward - oRotation).sqrMagnitude > 1)
            {
                Attacking();
                return;
            }
            AttackLogic();
        }
        else
        {
            FaceTarget();
        }
        
        
        

        if (phase != attackPhase.IDLE)
        {
            rushing = false;
            Attacking();
            return;
        }
            

        agent.SetDestination(targetPlayer.transform.position);
    }

    protected override void AttackLogic(){
        if (affected.Count == 0){
            return;
        }

        foreach (GameObject obj in affected){
            IDamage dmg = obj.GetComponent<IDamage>();
            if (dmg == null){
                continue;
            }
            dmg.takeDamage(AttackDMG);
        }

        affected.Clear();
    }

    void MoveLogic()
    {
        if (!seesPlayer)
        {
            agent.speed = movementSpeed;
            Hide();
            
        }
        else
        {
            oRotation = transform.forward;
            agent.speed = movementSpeed * 10;
            
            State = enemyState.ATTACK;
        }
    }

    void Hide()
    {
        while (true)
        {
            //gives a number of objects within a certain distance of the object that one can hide behind
            int hits = Physics.OverlapSphereNonAlloc(transform.position, findSpots.radius, hideSpots, HideLayers);

            for (int i = 0; i < hits; i++)
            {
                //finds nearest navmesh point within range
                if (NavMesh.SamplePosition(hideSpots[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
                {
                    //error check
                    if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                    {
                        Debug.LogError($"Cant find hide spot at {hit.position}");
                    }
                    //if diff between hiding spot's "Dot" variable (a part of the hit data) and player is within hideFactor range, do hiding
                    if (Vector3.Dot(hit.normal, (targetPlayer.transform.position - hit.position).normalized) < hideFactor)
                    {
                        agent.SetDestination(hit.position);
                        break;

                    }
                    else
                    {//documentation recommends doing this twice for random results. changing the code a bit we can also use this to check the other side of the obstacle near the hiding spot instead.
                        if (NavMesh.SamplePosition(hideSpots[i].transform.position - (targetPlayer.transform.position - hit.position).normalized * 2, out NavMeshHit hit2, 2f, agent.areaMask))
                        {
                            if (!NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask))
                            {
                                Debug.LogError($"Cant find hide spot at {hit2.position}");
                            }
                            
                            if (Vector3.Dot(hit2.normal, (targetPlayer.transform.position - hit2.position).normalized) < hideFactor)
                            {
                                agent.SetDestination(hit2.position);
                                break;

                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Unable to find NavMesh near {hideSpots[i].name} at {hideSpots[i].transform.position}");
                }
            }
            return;
        }
    }
}

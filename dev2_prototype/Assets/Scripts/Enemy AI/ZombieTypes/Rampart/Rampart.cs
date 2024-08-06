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
    [SerializeField] protected AttackArea attackArea;
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
            
            //if (seesPlayer)
            //{
            //    currentTarget = targetPlayer.gameObject;
            //    State = enemyState.ATTACK;
            //    return;
            //}
        }
        else
        {
            colli.enabled = false;
        }
        //agent.SetDestination(targetPlayer.transform.position);
        MoveLogic();
    }

    public override void Attack()
    {
        agent.stoppingDistance = 1;
        StartCoroutine(TargetCheck());
        if (nearPlayer && phase != attackPhase.RECOVERY)
            colli.enabled = true;
        else
            colli.enabled = false;
        rushing = agent.velocity.sqrMagnitude >= 25;
        if (rushing)
        {
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
            VisibilityCheck();
            

            if (nearPlayer & seesPlayer)
            {
                Attacking();
            }
        }




        if (phase != attackPhase.IDLE)
        {
            rushing = false;
            Attacking();
            return;
        }


        agent.SetDestination(targetPlayer.transform.position);
    }

    protected override void AttackLogic()
    {
        if (attackArea.affected.Count == 0)
        {
            return;
        }

        foreach (GameObject obj in attackArea.affected)
        {
            if (obj.TryGetComponent(out IDamageable dmg))
            {
                dmg.TakeDamage(AttackDMG);
            }
        }

        attackArea.affected.Clear();
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
        int hits = Physics.OverlapSphereNonAlloc(transform.position, findSpots.radius, hideSpots, HideLayers);
        float scoreBest = float.MinValue;
        Vector3 spotBest = transform.position;

        for (int i = 0; i < hits; i++)
        {
            float dist = Vector3.Distance(transform.position, hideSpots[i].transform.position);
            Vector3 dir = (targetPlayer.transform.position - hideSpots[i].transform.position).normalized;

            if (NavMesh.SamplePosition(hideSpots[i].transform.position, out NavMeshHit hit, dist, agent.areaMask))
            {
                NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask);
                float vis = Vector3.Dot(hit.normal, dir);

                if (vis < hideFactor)
                {
                    float score = -vis / dist;
                    if (score > scoreBest)
                    {
                        scoreBest = score;
                        spotBest = hit.position;
                    }
                }
                else
                {
                    Vector3 pos2 = hideSpots[i].transform.position - dir * 2;
                    if (NavMesh.SamplePosition(pos2, out NavMeshHit hit2, dist, agent.areaMask))
                    {
                        NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask);
                        vis = Vector3.Dot(hit2.normal, dir);

                        if (vis < hideFactor)
                        {
                            float score = -vis / dist;
                            if (score > scoreBest)
                            {
                                scoreBest = score;
                                spotBest = hit2.position;
                            }
                        }
                    }
                }
            }
        }

        agent.SetDestination(spotBest);
    }
}








//if (Vector3.Dot(hit.normal, (targetPlayer.transform.position - hit.position).normalized) < hideFactor)
//{
//    agent.SetDestination(hit.position);
//    break;

//}
//else
//{
//    if (NavMesh.SamplePosition(hideSpots[i].transform.position - (targetPlayer.transform.position - hit.position).normalized * 2, out NavMeshHit hit2, dist * 2, agent.areaMask))
//    {


//        NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask);
//        if (Vector3.Dot(hit2.normal, (targetPlayer.transform.position - hit2.position).normalized) < hideFactor)
//        {
//            agent.SetDestination(hit2.position);
//            break;

//        }
//    }
//}
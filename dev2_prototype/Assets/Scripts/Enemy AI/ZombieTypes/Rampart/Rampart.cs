using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombies.AI;

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
    //public override void Seek()
    //{
    //    agent.stoppingDistance = origStoppingDistance;
    //    TargetProximityCheck();
    //    TargetVisibilityCheck();
    //    if (attacking)
    //    {
    //        return;
    //    }
    //    if (nearPlayer)
    //    {
    //        FaceTarget();
            
    //        //if (seesTarget)
    //        //{
    //        //    currentTarget = currentTarget.gameObject;
    //        //    State = enemyState.ATTACK;
    //        //    return;
    //        //}
    //    }
    //    //agent.SetDestination(currentTarget.transform.position);
    //    MoveLogic();
    //}

    //public override void Attack()
    //{
    //    agent.stoppingDistance = 1;
    //    TargetProximityCheck();
    //    rushing = agent.velocity.sqrMagnitude >= 25;
    //    if (rushing)
    //    {
    //        if (agent.velocity.sqrMagnitude < 25 || (transform.forward - oRotation).sqrMagnitude > 1)
    //        {
    //            DoPhasedAttack();
    //            return;
    //        }
    //        AttackLogic();
    //    }
    //    else
    //    {
    //        FaceTarget();
    //        TargetVisibilityCheck();
            

    //        if (nearPlayer & seesTarget)
    //        {
    //            DoPhasedAttack();
    //        }
    //    }

    //    if (AttackPhase != AttackPhases.IDLE)
    //    {
    //        rushing = false;
    //        DoPhasedAttack();
    //        return;
    //    }


    //    agent.SetDestination(CurrentTarget.transform.position);
    //}

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
                dmg.TakeDamage(AttackDamage);
            }
        }

        attackArea.affected.Clear();
    }

    void MoveLogic()
    {
        if (!seesTarget)
        {
            agent.speed = movementSpeed;
            Hide();

        }
        else
        {
            oRotation = transform.forward;
            agent.speed = movementSpeed * 10;

            // State = enemyState.ATTACK;
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
            Vector3 dir = (CurrentTarget.transform.position - hideSpots[i].transform.position).normalized;

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


    protected override void OnTriggerEnter(Collider other)
    {
        if (CurrentState != null && CurrentState.Name == EnemyState.DEAD)
            return;

        if (other.tag.Contains("Barricade"))
        {

            Barricade barricade = other.GetComponentInChildren<Barricade>();
            if (barricade == null)
                return;

            IDamageable dmg = barricade.GetComponent<IDamageable>();
            if (dmg == null)
                return;

            dmg.TakeDamage(barricade.Health);
            

        }
    }

    public override BaseAIState GetNormalState()
    {
        throw new System.NotImplementedException();
    }

    public override BaseAIState GetAttackState()
    {
        throw new System.NotImplementedException();
    }

    public override BaseAIState GetSeekState()
    {
        throw new System.NotImplementedException();
    }
}








//if (Vector3.Dot(hit.normal, (currentTarget.transform.position - hit.position).normalized) < hideFactor)
//{
//    agent.SetDestination(hit.position);
//    break;

//}
//else
//{
//    if (NavMesh.SamplePosition(hideSpots[i].transform.position - (currentTarget.transform.position - hit.position).normalized * 2, out NavMeshHit hit2, dist * 2, agent.areaMask))
//    {


//        NavMesh.FindClosestEdge(hit2.position, out hit2, agent.areaMask);
//        if (Vector3.Dot(hit2.normal, (currentTarget.transform.position - hit2.position).normalized) < hideFactor)
//        {
//            agent.SetDestination(hit2.position);
//            break;

//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombies;

public class BaseAI : MonoBehaviour
{
    public GameObject currentTarget;

    [SerializeField] int faceTargetSpeed;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesTarget;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;
    [SerializeField] protected LayerMask detectionLayers;

    protected Vector3 playerDir;
    protected Vector3 movePosition;
    protected float origStoppingDistance;


    public NavMeshAgent Agent()
    {
        return agent;
    }

    public bool SeesTarget { get => seesTarget; set => seesTarget = value; }

    void Start()
    {
    }

    protected void UpdateTargetDir()
    {
        playerDir = currentTarget.transform.position - transform.position;
    }
    protected void TargetVisibilityCheck()
    {
        UpdateTargetDir();
        if (Physics.Raycast(transform.position, playerDir, out RaycastHit vis, detectionRange, detectionLayers))
        {
            Debug.DrawRay(transform.position, playerDir, Color.green);

            seesTarget = vis.collider.gameObject.CompareTag("Player");
        }
    }

    protected IEnumerator TargetCheck(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        nearPlayer = GetDistanceToTarget() <= origStoppingDistance;
        yield return new WaitForSeconds(delay);
    }

    protected void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    protected float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, currentTarget.transform.position);
    }

    protected void Move()
    {
        agent.stoppingDistance = origStoppingDistance;
        agent.SetDestination(movePosition);
    }

    protected void FlankTarget(Transform target, float offset)
    {

         Vector3 dir = target.position - transform.position;
         Vector3 flank = Vector3.Cross(Vector3.up, dir).normalized;

         Vector3 flankPosition = target.position + flank * offset;

         agent.SetDestination(flankPosition);
    }


    public void AddDetLayer(string  layer)
    {
        detectionLayers |= (1 << LayerMask.NameToLayer(layer));
    }

    
}
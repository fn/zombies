using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombies;

public enum EnemyState { 
    NORMAL, 
    SEEK, 
    ATTACK, 
    FLEE, 
    DEMOLITION, 
    GATHER, 
    FLANK, 
    DEAD 
};

public class BaseAI : MonoBehaviour
{
    public GameObject currentTarget;

    [SerializeField] int faceTargetSpeed;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesTarget;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;
    [SerializeField] protected LayerMask detectionLayers;
    [SerializeField] protected float proximityRange;

    protected Vector3 targetDir;
    protected Vector3 movePosition;
    protected float origStoppingDistance;

    public bool SeesTarget { get => seesTarget; set => seesTarget = value; }
    public bool NearTarget { get => nearPlayer; set => nearPlayer = value; }

    public NavMeshAgent Agent { get { return agent; } }

    void Start()
    {
    }

    public void UpdateTargetDir()
    {
        targetDir = currentTarget.transform.position - transform.position;
    }
    public void TargetVisibilityCheck()
    {
        UpdateTargetDir();
        if (Physics.Raycast(transform.position, targetDir, out RaycastHit vis, detectionRange, detectionLayers))
        {
            Debug.DrawRay(transform.position, targetDir, Color.green);

            seesTarget = vis.collider.gameObject.CompareTag("Player");
        }
    }

    public IEnumerator TargetProximityCheck(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        nearPlayer = GetDistanceToTarget() <= proximityRange;
        yield return new WaitForSeconds(delay);
    }

    public void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, currentTarget.transform.position);
    }

    public void Move()
    {
        agent.stoppingDistance = origStoppingDistance;
        agent.SetDestination(movePosition);
    }

    public void FlankTarget(Transform target, float offset)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public GameObject CurrentTarget;

    [SerializeField] int faceTargetSpeed;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected bool seesTarget;
    [SerializeField] protected bool nearPlayer;
    public /*[SerializeField] protected*/ int detectionRange;
    [SerializeField] protected LayerMask detectionLayers;
    [SerializeField] protected float proximityRange;

    public Vector3 targetDir;
    protected Vector3 movePosition;
    protected float origStoppingDistance;

    public bool SeesTarget { get => seesTarget; set => seesTarget = value; }
    public bool NearTarget { get => nearPlayer; set => nearPlayer = value; }

    public NavMeshAgent Agent { get => agent; }

    void Start()
    {
    }

    public void UpdateTargetDir()
    {
        targetDir = CurrentTarget.transform.position - transform.position;
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

    private float lastProximityCheckTime;

    public void TargetProximityCheck(float delay = 0.1f)
    {
        if (Time.time - lastProximityCheckTime > delay)
        {
            nearPlayer = GetDistanceToTarget() <= proximityRange;
            lastProximityCheckTime = Time.time;
        }
    }

    public void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, CurrentTarget.transform.position);
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

    public void AddDetLayer(string layer)
    {
        detectionLayers |= (1 << LayerMask.NameToLayer(layer));
    }

    public void AddDetLayers(string[] layers)
    {
        foreach (var l in layers)
            detectionLayers |= 1 << LayerMask.NameToLayer(l);
    }
}
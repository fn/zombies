using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zombies;

public class BaseAI : MonoBehaviour
{
    [SerializeField] int faceTargetSpeed;
    protected float origStoppingDistance;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesPlayer;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;
    [SerializeField] protected LayerMask detectionLayers;

    protected Vector3 playerDir;
    protected Vector3 movePosition;
    protected Player targetPlayer;

    public NavMeshAgent Agent()
    {
        return agent;
    }

    void Start()
    {
        targetPlayer = GameManager.Instance.LocalPlayer;
    }

    protected void UpdatePlayerDir()
    {
        playerDir = targetPlayer.View.transform.position - transform.position;
    }

    protected void VisibilityCheck()
    {
        UpdatePlayerDir();
        if (Physics.Raycast(transform.position, playerDir, out RaycastHit vis, detectionRange, detectionLayers))
        {
            Debug.DrawRay(transform.position, playerDir, Color.green);

            seesPlayer = vis.collider.gameObject.CompareTag("Player");
        }
    }

    protected IEnumerator TargetCheck(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay);
        nearPlayer = GetDistanceToPlayer() <= origStoppingDistance;
        yield return new WaitForSeconds(delay);
    }

    protected void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    protected float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, targetPlayer.transform.position);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [SerializeField] int faceTargetSpeed;
    protected float origStoppingDistance;
    [SerializeField] protected UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] protected bool seesPlayer;
    [SerializeField] protected bool nearPlayer;
    [SerializeField] protected int detectionRange;

    protected Vector3 playerDir;

    protected Vector3 movePosition;

    protected Zombies.Player targetPlayer;

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
        RaycastHit vis;
        if (Physics.Raycast(transform.position, playerDir, out vis, detectionRange))
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

    
}
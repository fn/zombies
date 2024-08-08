using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderLine : MonoBehaviour
{

    [SerializeField] LineRenderer commanderLine;

    public Transform commanderPoint;

    [SerializeField] Material model;
    [SerializeField] Color color;

    // Start is called before the first frame update
    void Start()
    {
        commanderLine.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {
        BaseZombie z = this.transform.parent.GetComponent<BaseZombie>();

        if (z.commander == null || z.CurrentState != null && z.CurrentState.Name == EnemyState.DEAD)
        {
            commanderLine.enabled = false;
            return;
        }

        commanderPoint = z.commander.transform;
        commanderLine.enabled = true;
        // line renderer position
        commanderLine.SetPosition(0, transform.position);
        commanderLine.SetPosition(1, commanderPoint.position);
        // assigning color
        model.SetColor("_EmissionColor", color);
        commanderLine.material = model;
    }
}
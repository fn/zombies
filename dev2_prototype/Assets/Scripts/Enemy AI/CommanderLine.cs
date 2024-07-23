using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderLine : MonoBehaviour
{

    [SerializeField] LineRenderer commanderLine;

    [SerializeField] Transform commanderPoint;

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
        // line renderer position
        commanderLine.SetPosition(0, transform.position);
        commanderLine.SetPosition(1, commanderPoint.position);
        // assigning color
        model.SetColor("_EmissionColor", color);
        commanderLine.material = model;
    }
}
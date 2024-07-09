using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] Rampart rampart;

    private void OnTriggerEnter(Collider other) {
        rampart.affected.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other) {
        rampart.affected.Remove(other.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] public List<GameObject> affected = new List<GameObject>();

    private void OnTriggerEnter(Collider other) {
        affected.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other) {
        affected.Remove(other.gameObject);
    }
}

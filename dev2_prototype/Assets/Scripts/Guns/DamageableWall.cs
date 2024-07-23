using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class DamageableWall : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] GameObject Wall;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 14)
        {
            TakeDamage(100);
        }
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            GameObject.Destroy(Wall);
        }
    }
}

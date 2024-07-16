using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    public List<GameObject> zombies = new List<GameObject>();

    public void OnZombieDeath(GameObject zomb)
    {
        zombies.Remove(zomb);
        GameManager.Instance.Horde.zombieDead.Add(zomb);
    }
}

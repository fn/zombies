using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] Color EnemyFlashColor;
    [SerializeField] int HP;

    Color baseEnemyColor;
    // Start is called before the first frame update
    void Start()
    {
        baseEnemyColor = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        StartCoroutine(flashDamage());
    }

    IEnumerator flashDamage()
    {
        model.material.color = EnemyFlashColor; //Turn from gray to red
        yield return new WaitForSeconds(0.1f); //wait a bit
        model.material.color = baseEnemyColor; //go back to gray
    }
}


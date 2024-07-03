using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    // health of barricade
    [SerializeField] float Health;

    // its model
    [SerializeField] Renderer model;

    // what damage it should flash
    [SerializeField] Color damageFlash;


    // default color
    Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {

        // set and store default color
        defaultColor = model.material.color;
    }

    IEnumerator FlashDamage()
    {
        model.material.color = damageFlash;

        yield return new WaitForSeconds(0.1f);

        model.material.color = defaultColor;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        StartCoroutine(FlashDamage());
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

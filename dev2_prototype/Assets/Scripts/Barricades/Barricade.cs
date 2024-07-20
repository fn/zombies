using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour, IDamageable
{
    // repair logic called every Invoke
    [SerializeField] int repairHealthAmount;
    [SerializeField] float repairRateInterval;
    [SerializeField] int repairRateCost;
    private bool isRepairing = false;

    public int PLACEHOLDERMONEY = 10;

    // health logic
    [SerializeField] int maxHealth;
    private int currentHealth;

    // Health variable is the point of interaction for all other game components
    public int Health
    {
        get { return currentHealth; }
        set {
            // ensure health doesnt exceed max health
            if (value > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = value;
            }
        }
    }
    

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

        // initialize currentHealth to maxHealth on spawn
        currentHealth = maxHealth;
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
        Debug.Log("TakeDamage barricade");

        StartCoroutine(FlashDamage());
        if (Health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("barricade SetActive(false)");
        }
    }

    public void Repair(int amount)
    {
        Health += amount;
        
    }

    private void RepairBarricade()
    {
        // barricade active && can be afforded && reason to repair
        if(gameObject.activeSelf && PLACEHOLDERMONEY >= repairRateCost && currentHealth < maxHealth)
        {
            Repair(repairHealthAmount);
            PLACEHOLDERMONEY -= repairRateCost;
        }
    }

    public void StartRepair()
    {
        if (!isRepairing)
        {
            isRepairing = true;

            // how often the barricade is repaired, based on Time.timeScale
            InvokeRepeating(nameof(RepairBarricade), repairRateInterval, repairRateInterval);
            Debug.Log("RepairBarricade InvokeRepeating");
        }
    }

    public void StopRepair()
    {
        if (isRepairing)
        {
            isRepairing = false;
            CancelInvoke(nameof(RepairBarricade));
            Debug.Log("RepairBarricade CancelInvoke");
        }
    }

    public void ResetHealth()
    {
        Health = maxHealth;
        Debug.Log("ResetHealth barricade");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
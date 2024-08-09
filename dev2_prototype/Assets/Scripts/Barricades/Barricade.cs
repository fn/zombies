using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Barricade : MonoBehaviour, IDamageable
{
    // repair logic called every Invoke
    [SerializeField] int repairHealthAmount;
    [SerializeField] float repairRateInterval;
    [SerializeField] int repairRateCost = 10;
    private bool isRepairing = false;

    public bool IsBroken { get; private set; }

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
        // Debug.Log("TakeDamage barricade");

        if (gameObject.activeSelf)
            StartCoroutine(FlashDamage());

        if (Health <= 0)
        {
            gameObject.SetActive(false);
            IsBroken = true;
        }
    }

    public void Repair(int amount)
    {
        Health += amount;
        IsBroken = false;
    }

    private void RepairBarricade()
    {
        // barricade active && can be afforded && reason to repair
        if(gameObject.activeSelf && GameManager.Instance.LocalPlayer.Money >= repairRateCost && currentHealth < maxHealth)
        {
            Repair(repairHealthAmount);
            GameManager.Instance.LocalPlayer.Money -= repairRateCost;
        }
    }

    public void StartRepair()
    {
        if (!isRepairing)
        {
            isRepairing = true;

            // how often the barricade is repaired, based on Time.timeScale
            InvokeRepeating(nameof(RepairBarricade), repairRateInterval, repairRateInterval);

            //Debug.Log("RepairBarricade InvokeRepeating");
        }
    }

    public void StopRepair()
    {
        if (isRepairing)
        {
            isRepairing = false;
            CancelInvoke(nameof(RepairBarricade));
            GameManager.Instance.PromptBackground.SetActive(false);
            //Debug.Log("RepairBarricade CancelInvoke");
        }
    }

    public void ResetHealth()
    {
        Health = maxHealth;
        //Debug.Log("ResetHealth barricade");
    }

}
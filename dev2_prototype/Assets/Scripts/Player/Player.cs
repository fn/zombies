using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    public class Player : MonoBehaviour, IDamageable
    {
        public MovementComponent Movement;
        public ViewComponent View;
        
        [SerializeField] List<WeaponComponent> Weapons;

        [SerializeField] Transform WeaponPivot;
        [SerializeField] Animator ViewModelAnimator;

        int HeldWeaponIndex;

        public int Health, MaxHealth;
        float attackTime;

        public WeaponComponent HeldWeapon { get => Weapons[HeldWeaponIndex]; }

        public bool IsAiming { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            HeldWeaponIndex = 0;

            GameManager.Instance.HurtScreen.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateWeapons();
            passiveHealthRegen();
            HealthDisplay();

            UpdateViewModel();
        }

        void UpdateViewModel()
        {
            UpdateAiming();

            ViewModelAnimator.SetBool("Sprinting", Movement.IsSprinting);
        }

        void UpdateWeapons()
        {
            if (HeldWeapon == null)
                return;

            GameManager.Instance.AmmoHudText.SetText($"{HeldWeapon.currentAmmo}/{HeldWeapon.remainingAmmo}");

            if (Input.GetButtonDown("Fire1") && HeldWeapon.HasAmmo)
            {
                HeldWeapon.Shoot(WeaponPivot.transform.position, View.viewCamera.transform.forward);
                ViewModelAnimator.SetTrigger("Shoot");
            }

            if (Input.GetButtonDown("Reload") && HeldWeapon.CanReload)
            {
                ViewModelAnimator.SetTrigger("Reload");
            }
        }

        void UpdateAiming()
        {
            // We can only aim if we are not sprinting.
            IsAiming = Input.GetButton("Fire2") && !Movement.IsSprinting;

            // This code below is kind of trash.
            float fovOffset = IsAiming ? 25f : 0f;
            View.SetFov(90f - fovOffset, Time.deltaTime * 10f);

            ViewModelAnimator.SetBool("Aiming", IsAiming);
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            attackTime = 3;
            if (Health <= 0)
            {
                GameManager.Instance.StateLose();
            }
        }

        public void passiveHealthRegen()
        {
            //If 3 seconds have passed
            if (attackTime < 0)
            {
                //If the player has taken damage
                if (Health < MaxHealth)
                {
                    //increases health by percentage of max health in case we want to add health gain options.
                    Health += MaxHealth / 20;
                    //then reduces the health to be at a maximum of 100;
                    Health = Mathf.Clamp(Health, 0, MaxHealth);
                }
                //Increases the time again to not be gaining health every frame.
                attackTime++;
            }
            else
                //Lowers the time until the next health regen.
                attackTime -= Time.deltaTime;
        }

        void HealthDisplay()
        {
            float healthPercentage = 1 - (float)Health / MaxHealth;
            if (healthPercentage > .10)
            {
                GameManager.Instance.HurtScreen.enabled = true;
                Color hpAlpha = GameManager.Instance.HurtScreen.color;
                hpAlpha.a = healthPercentage;
                GameManager.Instance.HurtScreen.color = hpAlpha;
            }
            else
                GameManager.Instance.HurtScreen.enabled= false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    public class Player : MonoBehaviour, IDamage
    {
        public MovementComponent Movement;
        public ViewComponent View;
        [SerializeField] List<WeaponComponent> Weapons;

        [SerializeField] Transform WeaponPivot;

        int HeldWeaponIndex;

        public int Health, MaxHealth;
        float attackTime;

        public WeaponComponent HeldWeapon { get => Weapons[HeldWeaponIndex]; set => value = Weapons[HeldWeaponIndex]; }

        Vector3 origAimPosition;

        bool IsAiming;

        // Start is called before the first frame update
        void Start()
        {
            //Weapons = new List<WeaponComponent>();
            HeldWeaponIndex = 0;

            origAimPosition = WeaponPivot.localPosition;

            GameManager.Instance.HurtScreen.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateWeapons();
            passiveHealthRegen();
            HealthDisplay();
        }

        void UpdateWeapons()
        {
            if (HeldWeapon == null)
                return;

            GameManager.Instance.AmmoHudText.SetText($"{HeldWeapon.currentAmmo}/{HeldWeapon.remainingAmmo}");

            if (Input.GetButtonDown("Fire1"))
            {
                var shootTransform = View.viewCamera.transform;
                HeldWeapon.Shoot(shootTransform.position, shootTransform.forward);
            }

            if (Input.GetButtonDown("Reload"))
            {
                HeldWeapon.Reload();
            }

            UpdateAiming();
        }

        void UpdateAiming()
        {
            IsAiming = Input.GetButton("Fire2");

            // This code below is kind of trash.
            float fovOffset = IsAiming ? 25f : 0f;
            View.SetFov(90f - fovOffset, Time.deltaTime * 2f);

            if (IsAiming)
                WeaponPivot.localPosition = Vector3.Lerp(WeaponPivot.localPosition, new Vector3(0f, -0.2f, WeaponPivot.localPosition.z), Time.deltaTime * 10f);
            else
                WeaponPivot.localPosition = Vector3.Lerp(WeaponPivot.localPosition, origAimPosition, Time.deltaTime * 10f);
        }

        public void takeDamage(int damage)
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
            if (attackTime <0)
            {
                //If the player has taken damage
                if (Health < MaxHealth)
                {
                    //increases health by percentage of max health in case we want to add health gain options.
                    Health += MaxHealth / 20;
                    //then reduces the health to be at a maximum of 100;
                    Health= Mathf.Clamp(Health, 0 , MaxHealth);
                }
                //Increases the time again to not be gaining health every frame.
                attackTime++;
            }
            else
                //Lowers the time until the next health regen.
                    attackTime -= Time.deltaTime;
        }

        IEnumerator FlashHurtScreen()
        {
            GameManager.Instance.HurtScreen.enabled = true;
            float waitTime = 10f * (1- (float)Health / MaxHealth);
            yield return new WaitForSeconds(waitTime);
            GameManager.Instance.HurtScreen.enabled = false;
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
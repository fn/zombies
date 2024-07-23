using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    public class Player : MonoBehaviour, IDamageable
    {
        public MovementComponent Movement;
        public ViewComponent View;
        
        
        [SerializeField] Transform ViewModel;
        [SerializeField] Animator ViewModelAnimator;

        // The starting weapon we want the player to have.
        [SerializeField] GameObject StartingWeapon;

        public int Money;

        public int Health, MaxHealth;
        float attackTime;

        public int HeldWeaponIndex;
        public List<WeaponComponent> Weapons;
        public WeaponComponent HeldWeapon { get => Weapons[HeldWeaponIndex]; }

        public bool IsAiming { get; private set; }

        private Transform ShotOrigin;

        // Start is called before the first frame update
        void Start()
        {
            HeldWeaponIndex = 0;
            Money = 0;

            GameManager.Instance.HurtScreen.enabled = false;

            if (StartingWeapon != null)
            {
                // Add our starting weapon and load the active view model.
                Weapons.Add(StartingWeapon.GetComponent<WeaponComponent>());
                Weapons[Weapons.Count - 1].ResetWeapon();
                LoadViewModel();
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateWeapons();
            PassiveHealthRegen();
            HealthDisplay();

            UpdateViewModel();
        }

        public void LoadViewModel()
        {
            // Destory current view model.
            if (ViewModel.childCount > 0)
            {
                for (int i = 0; i < ViewModel.childCount; i++)
                    Destroy(ViewModel.GetChild(i).gameObject);
            }

            var viewModel = Instantiate(HeldWeapon.Model, ViewModel);

            // I don't really like this... But with the way we have done our bullets it makes it look bad for them to come from the camera.
            ShotOrigin = viewModel.transform.Find("Model/ShootPos");

            //HeldWeapon.ResetWeapon();
        }

        void UpdateViewModel()
        {
            UpdateAiming();

            ViewModelAnimator.SetBool("Sprinting", Movement.IsSprinting);
        }

        void PickupWeapon(WeaponComponent weapon)
        {
            Weapons.Add(weapon);
            HeldWeaponIndex = Weapons.Count - 1;
        }

        void SwapWeapon(  )
        {
            LoadViewModel();
        }

        void UpdateWeapons()
        {
            if (HeldWeapon == null)
                return;

            GameManager.Instance.AmmoHudText.SetText($"{HeldWeapon.CurrentAmmo}/{HeldWeapon.RemainingAmmo}");

            if (Input.GetButtonDown("Fire1") && HeldWeapon.HasAmmo && !HeldWeapon.IsReloading)
            {
                HeldWeapon.Shoot(ShotOrigin.transform.position, View.viewCamera.transform.forward);
                ViewModelAnimator.SetTrigger("Shoot");
            }

            if (Input.GetButtonDown("Reload") && HeldWeapon.CanReload)
            {
                ViewModelAnimator.SetTrigger("Reload");
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f && HeldWeaponIndex < Weapons.Count - 1)
            {
                HeldWeaponIndex++;
                SwapWeapon();
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && HeldWeaponIndex > 0)
            {
                HeldWeaponIndex--;
                SwapWeapon();
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

        public void PassiveHealthRegen()
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
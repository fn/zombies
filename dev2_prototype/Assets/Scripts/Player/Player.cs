using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zombies
{
    public class Player : MonoBehaviour
    {
        [SerializeField] MovementComponent Movement;
        [SerializeField] ViewComponent View;
        [SerializeField] List<WeaponComponent> Weapons;

        [SerializeField] Transform WeaponPivot;

        int HeldWeaponIndex;
        public WeaponComponent HeldWeapon { get => Weapons[HeldWeaponIndex]; set => value = Weapons[HeldWeaponIndex]; }

        Vector3 origAimPosition;

        bool IsAiming;

        // Start is called before the first frame update
        void Start()
        {
            //Weapons = new List<WeaponComponent>();
            HeldWeaponIndex = 0;

            origAimPosition = WeaponPivot.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateWeapons();
        }

        void UpdateWeapons()
        {
            if (HeldWeapon == null)
                return;

            GameManager.Instance.AmmoHudText.SetText($"{HeldWeapon.currentAmmo}/{HeldWeapon.ammoCapacity}");

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
    }
}
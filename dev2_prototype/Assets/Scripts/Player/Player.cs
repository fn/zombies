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
        WeaponComponent HeldWeapon { get => Weapons[HeldWeaponIndex]; set => value = Weapons[HeldWeaponIndex]; }

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
            // Update held weapon index.
            //for (int i = 0; i < Weapons.Count; i++)
            //    if (Input.GetButtonDown($"Slot{i + 1}"))
            //        HeldWeaponIndex = i;

            if (HeldWeapon == null)
                return;

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
            IsAiming = false;
            if (Input.GetButton("Fire2"))
                IsAiming = true;

            float fovOffset = IsAiming ? 25f : 0f;
            View.SetFov(90f - fovOffset, Time.deltaTime * 2f);

            if (IsAiming)
                WeaponPivot.localPosition = Vector3.Lerp(WeaponPivot.localPosition, new Vector3(0f, -0.2f, WeaponPivot.localPosition.z), Time.deltaTime * 10f);
            else
                WeaponPivot.localPosition = Vector3.Lerp(WeaponPivot.localPosition, origAimPosition, Time.deltaTime * 10f);
        }
    }
}
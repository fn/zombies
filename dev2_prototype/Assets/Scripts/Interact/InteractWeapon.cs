using UnityEngine;

public class InteractWeapon : MonoBehaviour
{
    [SerializeField] GameObject Weapon;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //GameManager.Instance.DoorPrompt.SetActive(true);
            GameManager.Instance.PromptBackground.SetActive(true);
            GameManager.Instance.PromptText.SetText("'E' To Purchase");
            if (Input.GetKey(KeyCode.E))
            {
                if (Weapon.TryGetComponent(out WeaponComponent weaponComp))
                {
                    GameManager.Instance.LocalPlayer.Weapons.Add(weaponComp);
                    GameManager.Instance.LocalPlayer.HeldWeaponIndex = GameManager.Instance.LocalPlayer.Weapons.Count - 1;
                    GameManager.Instance.LocalPlayer.HeldWeapon.ResetWeapon();
                    GameManager.Instance.LocalPlayer.LoadViewModel();
                }

                //GameManager.Instance.DoorPrompt.SetActive(false);
                GameManager.Instance.PromptBackground.SetActive(false); ;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            GameManager.Instance.PromptBackground.SetActive(false);
            //GameManager.Instance.DoorPrompt.SetActive(false);
    }
}
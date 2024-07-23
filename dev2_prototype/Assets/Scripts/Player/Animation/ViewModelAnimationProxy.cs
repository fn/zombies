using UnityEngine;

namespace Zombies
{
    public class ViewModelAnimationProxy : MonoBehaviour
    {
        [SerializeField] Player player;

        void StartReload()
        {
            if (player.HeldWeapon.CanReload)
                player.HeldWeapon.IsReloading = true;
        }

        // This is an animation event.
        void EndReload()
        {
            player.HeldWeapon.Reload();
        }
    }
}
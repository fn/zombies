using UnityEngine;

namespace Zombies
{
    public class ViewModelAnimationProxy : MonoBehaviour
    {
        [SerializeField] Player player;

        // This is an animation event.
        void DoReload()
        {
            player.HeldWeapon.Reload();
        }
    }
}
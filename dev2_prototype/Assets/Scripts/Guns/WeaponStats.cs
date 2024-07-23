using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    public float Damage;

    // Prevent division by zero.
    [Range(1f, 10000f)]
    public float FireRate;
    public int AmmoCapacity;
    public int MagSize;
    public bool IsSpecial;
    public bool InfiniteAmmo;
    public int ProjectilesPerShot;
    public float SpreadFactor;
    public float ReloadSpeed;
}
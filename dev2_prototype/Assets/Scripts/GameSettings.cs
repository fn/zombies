using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float effectVolume;
    public float musicVolume;

}

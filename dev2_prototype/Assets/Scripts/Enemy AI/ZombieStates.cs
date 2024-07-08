using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ZombieStates //reading up on interfaces. will probably switch to that later
{
    void Normal();
    void Seek();
    void Attack();
    void Flee();
}

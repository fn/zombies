using UnityEngine;

public class RunnerAnimationProxy : MonoBehaviour
{
    [SerializeField] Runner Runner;

    void OnAttackHit()
    {
        Runner.OnAttackHit();
    }
}

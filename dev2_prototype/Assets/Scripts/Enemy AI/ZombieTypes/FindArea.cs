using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseZombie;

public class FindArea : MonoBehaviour
{
    [SerializeField] Commander comm;

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            BaseZombie z = other.gameObject.GetComponent<BaseZombie>();
            if (z.commander != null)
                return;
            if (z.State != enemyState.SEEK)
                return;

            z.commander = comm;
            z.State = enemyState.GATHER;

            int total = comm.mainGroup.Count + comm.flankGroup.Count;
            float per = (float)comm.flankPercent / 100;
            float totPer = total * per;

            if (comm.flankGroup.Count >= totPer)
            {
                comm.mainGroup.Add(z);
                z.IsInMain = true;
            }
            else
            {
                z.IsInMain = false;
                comm.flankGroup.Add(z);
            }

        }

        if (other.gameObject.tag == "Player")
        {
            comm.Agent().ResetPath();
            comm.State = enemyState.GATHER;
        }

    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            BaseZombie z = other.gameObject.GetComponent<BaseZombie>();
            if (z.commander != this)
                return;

            z.commander = null;
        }

        if (other.gameObject.tag == "Player")
        {
            comm.State = enemyState.SEEK;
        }
    }
}

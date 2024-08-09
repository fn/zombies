using UnityEngine;

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
            // if (z.State != EnemyState.SEEK)
            //     return;

            z.commander = comm;
            // z.State = EnemyState.GATHER;

            int total = comm.MainGroup.Count + comm.FlankGroup.Count;
            float per = (float)comm.flankPercent / 100;
            float totPer = total * per;

            if (comm.FlankGroup.Count >= totPer)
            {
                comm.MainGroup.Add(z);
                z.IsInMainGroup = true;
            }
            else
            {
                z.IsInMainGroup = false;
                comm.FlankGroup.Add(z);
            }
        }

        if (other.gameObject.tag == "Player")
        {

            // comm.Agent().ResetPath();
            // comm.State = EnemyState.GATHER;
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
            // comm.CurrentState = EnemyState.SEEK;
        }
    }
}

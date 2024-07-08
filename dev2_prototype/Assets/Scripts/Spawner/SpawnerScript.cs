using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public enum StateOfSpawn { SPAWNING, WAITING, COUNTING };

    [System.Serializable] // allows to change values of instances of
                          // class in Unity Inspector
    public class Wave
    {
        public string name;
        public Transform capsulePrefab;
        public int enemyCount;
        public float spawnRate;
    }

    public Wave[] allWaves;
    private int nextWave = 0;

    public Transform[] points;

    public float waitTime = 5f;
    private float countDown;

    private float findTagCountDown = 1;

    public StateOfSpawn spawnState = StateOfSpawn.COUNTING;

    void Start()
    {

        if (points.Length == 0)
        {
            Debug.LogError("No Spawn Points");
        }

        countDown = waitTime;
    }

    // Update is called once per frame
    void Update()
    {   // make sure spawner spawns capsules
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Instantiate(capsulePrefab, transform.position, Quaternion.identity);
        //}

        if (spawnState == StateOfSpawn.WAITING)
        {
            //  check if enemies are still alive
            if (EnemyIsAlive())
            {
                NewRound();
            }
            else
            {
                return; // let player finish killing enemies so return
            }

        }

        if (countDown <= 0) // not at zero yet so subtracting one second til zero from 5 then checking if a wave is spawned
        {
            countDown = 1f;
            if (spawnState != StateOfSpawn.SPAWNING)
            {
                // start spawning wave
                StartCoroutine(SpawnAWave(allWaves[nextWave]));
            }

        }
        else
        {
            // timer relivant to time not frames
            countDown -= Time.deltaTime;
        }

    }

    void NewRound()
    {
        // begin a new round
        Debug.Log("Wave Completed");

        spawnState = StateOfSpawn.COUNTING;
        countDown = waitTime;

        if (nextWave + 1 > allWaves.Length - 1) // if next wave spawn is bigger than the number of waves
        {
            nextWave = 0;
            Debug.Log("Waves Complete Looping...");
            // Add win screen here
        }
        else
        {
            nextWave++; // if not complete then continue to next wave
        }
    }

    bool EnemyIsAlive()
    {
        findTagCountDown -= Time.deltaTime;
        if (findTagCountDown <= 0)
        {
            if (GameObject.FindGameObjectWithTag("Enemy") == null) // check if there is no enemy left
            {
                return false; // if not
            }
        }
        return true; // if so
    }

    IEnumerator SpawnAWave(Wave _wave)
    {
        Debug.Log("Spawning A Wave: " + _wave.name);

        spawnState = StateOfSpawn.SPAWNING;

        // spawn a bunch of things

        for (int i = 0; i < _wave.enemyCount; i++) // loop through how many enemies we want to spawn
        {
            SpawnEnemy(_wave.capsulePrefab); // call spawn method
            yield return new WaitForSeconds(1f / _wave.spawnRate); // wait time before spawn
        }

        spawnState = StateOfSpawn.WAITING;


        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy:" + _enemy.name); // write to console spawning enemy
        Transform randomPoint = points[Random.Range(0, points.Length)]; // 

        Instantiate(_enemy, randomPoint.position, randomPoint.rotation);
    }

}

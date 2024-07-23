using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public enum StateOfSpawn { SPAWNING, WAITING, COUNTING };

    [System.Serializable] // allows to change values of instances of class in Unity Inspector
    public class WaveConfiguration
    {
        public int setupWaves; // Waves that open up the map and such.
        public float spawnRate;
        public int waveNumber;
        public int enemyCount;
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform Position;
        public GameObject[] Entities;
    }

    [SerializeField] int MaxWaveNumber;
    public WaveConfiguration waveConfig;

    public SpawnPoint[] spawnPointLocations;
    public float waitTime = 5f;
    public StateOfSpawn spawnState = StateOfSpawn.COUNTING;
    public bool isActive;
    public float spawnerRange;

    private int currentWaveNumber = 1;
    private float countDown;
    private int enemiesLeftToSpawn;
    private float lastSpawnTime;

    void Start()
    {
        if (spawnPointLocations.Length == 0)
        {
            Debug.LogError("No Spawn Points");
        }

        countDown = waitTime;
        currentWaveNumber = 1;
    }

    void NewRound()
    {
        waveConfig.waveNumber = currentWaveNumber++;
        waveConfig.enemyCount = 7 * waveConfig.waveNumber + (5 / waveConfig.waveNumber);
        enemiesLeftToSpawn = waveConfig.enemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        bool noEnemies = NoEnemiesAlive();

        if (currentWaveNumber <= MaxWaveNumber)
        {
            GameManager.Instance.WaveHudText.text = $"Wave {waveConfig.waveNumber}";

            if (spawnState == StateOfSpawn.COUNTING)
            {
                if (countDown <= 0) // not at zero yet so subtracting one second til zero from 5 then checking if a wave is spawned
                {
                    countDown = 1f;
                    spawnState = StateOfSpawn.WAITING;
                }
                else
                {
                    // timer relivant to time not frames
                    countDown -= Time.deltaTime;
                }
            }

            if (spawnState == StateOfSpawn.WAITING)
            {
                if (noEnemies)
                {
                    NewRound();
                    // Spawn the wave.
                    // SpawnWave(waveConfig);

                    spawnState = StateOfSpawn.SPAWNING;
                }
            }

            if (spawnState == StateOfSpawn.SPAWNING)
            {
                Debug.Log($"Spawning wave: {waveConfig.waveNumber}");
                if (enemiesLeftToSpawn > 0)
                {
                    float spawnTime = 1f / waveConfig.spawnRate;

                    if (Time.time - lastSpawnTime > spawnTime)
                    {
                        SpawnEnemy(waveConfig);

                        lastSpawnTime = Time.time;
                        enemiesLeftToSpawn--;
                    }
                }
                else
                {
                    spawnState = StateOfSpawn.COUNTING;
                }
            }
        }
        else
        {
            //GameManager.Instance.StateWin();
            Debug.Log("All Waves Completed, You win!");
        }
    }

    bool NoEnemiesAlive()
    {
        // This is a bad check honestly. The spawner should put entities in a list and then we remove them from when they die.
        return GameObject.FindGameObjectWithTag("Enemy") == null;
    }

    void SpawnEnemy(WaveConfiguration waveCfg)
    {
        var availableSpawns = new List<SpawnPoint>();

        // These initial points are for opening up the map.
        if (waveCfg.waveNumber < waveCfg.setupWaves)
        {
            for (int i = 0; i < spawnPointLocations.Length; i++)
            {
                var location = spawnPointLocations[i].Position;
                if (location.gameObject.CompareTag($"Wave{waveCfg.waveNumber}"))
                    availableSpawns.Add(spawnPointLocations[i]);
            }
        }
        else // If we are past that we can spawn from any nearby spawn point.
        {
            foreach (SpawnPoint sp in spawnPointLocations)
            {
                if (Vector3.Distance(sp.Position.transform.position, GameManager.Instance.LocalPlayer.transform.position) <= spawnerRange)
                    availableSpawns.Add(sp);
            }
        }

        var curSpawner = availableSpawns[Random.Range(0, availableSpawns.Count)];
        var curPoint = curSpawner.Position;
        var curEntities = curSpawner.Entities;
        Instantiate(curEntities[Random.Range(0, curEntities.Length)], curPoint.position, curPoint.rotation);
    }
}
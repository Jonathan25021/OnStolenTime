using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    #region spawn_vars
    [Tooltip("The enemy that is spawned by this SpawnManager. All enemy types should have their own SpawnManager.")]
    public GameObject enemy;
    [Tooltip("A list of positions where enemies are spawned.")]
    public Transform[] spawnPositions;
    [Tooltip("Enemies level up after this many enemies have spawned.")]
    public int level_band_size;
    int enemy_spawn_counter;
    #endregion

    #region timing_vars
    [Tooltip("Time, in seconds, before first spawn")]
    public float timeUntilFirstSpawn;
    [Tooltip("Mean delay between spawn times")]
    public float meanSpawnDelay;
    [Tooltip("Standard deviation in delay between spawn times")]
    public float devSpawnDelay;
    #endregion

    #region unity_funcs
    // Start is called before the first frame update
    void Start()
    {
        enemy_spawn_counter = 0;
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        //Unused
    }
    #endregion

    #region spawn_coroutine
    //Spawns enemies after predetermined times.
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(timeUntilFirstSpawn);
        while (true)
        {

            // Spawn the enemy
            int rand = Random.Range(0, spawnPositions.Length);
            GameObject newEnemy = Instantiate(enemy, spawnPositions[rand]);

            // A bunch of math to get a normal distribution
            // Source: https://stackoverflow.com/questions/218060/random-gaussian-variables
            float u1 = Random.Range(Mathf.Epsilon, 1f);
            float u2 = Random.Range(Mathf.Epsilon, 1f);
            float norm = Mathf.Sqrt(-2 * Mathf.Log(u1)) *
                         Mathf.Sin(2 * Mathf.PI * u2); //random normal(0,1)
            //Truncating distribution tails
            if (norm < -3)
            {
                norm = -3;
            }
            if (norm > 3)
            {
                norm = 3;
            }
            float delay =
                         meanSpawnDelay + devSpawnDelay * norm; //random normal(mean,stdDev^2)
            yield return new WaitForSeconds(delay);
        }
    }
    #endregion
}
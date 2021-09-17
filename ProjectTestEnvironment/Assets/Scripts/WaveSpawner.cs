using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting }; //store the possible states that the wavespawner can be in.

   [System.Serializable] //allows us to change the values of instances of this class in the unity inspector.
    public class Wave
    {
        public string _name;
        public Transform _enemy; //reference to the prefab that we want to instantiate
        public int _amount;
        public float _spawnRate;
    }

    public Wave[] waves;
    private int _nextWave = 0;  //store index of the wave
    private float _searchCountdown = 1f;

    public float _timeBetweenWaves = 5f; //store time between waves
    public float _waveCountdown;

   

    private SpawnState _state = SpawnState.Counting;

    private void Start()
    {
        _waveCountdown = _timeBetweenWaves;
    }

    private void Update()
    {
        if (_state == SpawnState.Waiting) //checks if the player has killed all the enemis.
        {
            if(_EnemyIsAlive() == false)
            {
                Debug.Log("Wave completed!");
                return;
                //Begin a new round
            }
            else
            {
                return; //if there as still enemies alive, wait for the player till kill them
            }
            //checks if enemies are still alive
        }

        if(_waveCountdown <= 0) // if its time to spawning a wave
        {
            if(_state != SpawnState.Spawning) //checks if we already are spawning a wave
            {
                StartCoroutine(SpawnWave(waves[_nextWave]));  // start spawning wave here by calling the IEnumerator with startcoroutine.

            }
        }
        else // if wawecountdown is not zero
        {
            _waveCountdown -= Time.deltaTime; //make sure that the actual countdowntimer is relevent to time and not the amount of frames/s
        }
    }

    bool _EnemyIsAlive()
    {
        _searchCountdown -= Time.deltaTime;
        if (_searchCountdown <= 0f) //for performance, checks every second if there are any enemies alive, instead of every frame.
        {
            _searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) //checks if there are still any enemies alive
            {
                return false; //returns false if no enemies are found alive
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave) //we want to be able to wait x seconds inside the method
    {
        Debug.Log("Spawning Wave: " + _wave._name);
        _state = SpawnState.Spawning; // now we are spawning a wave

        for (int i = 0; i <_wave._amount; i++) //loop through the enemies that we want to spawn
        {
            SpawnEnemy(_wave._enemy); // for each enemies we want to spawn we call spawnenemy method
            yield return new WaitForSeconds(1f / _wave._spawnRate); //wait x seconds before next loop through

        }

        _state = SpawnState.Waiting; // when we are done spawning a wave, we want to wait for the next round (player kills all the enemies)
        yield break;
    }
        
    void SpawnEnemy(Transform _enemy)
    {
       
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Instantiate(_enemy, transform.position, transform.rotation);  //spawn enemy

    }

}

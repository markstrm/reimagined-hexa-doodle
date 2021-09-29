using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { Spawning, Waiting, Counting }; //store the possible states that the wavespawner can be in.

    public GameObject _EnemyHolder;
    public GameSession gameSession;

    //public Transform _enemy2;

    [System.Serializable] //allows us to change the values of instances of this class in the unity inspector.
    public class Wave //to define what a wave is in our game
    {
        public string _name;
        public Transform _enemy; //reference to the prefab that we want to instantiate
        
        public int _amount;
        public float _spawnRate;
       
    }

    public Wave[] waves;
    public Transform[] spawnPoint; //array with all the possible spawn locations

    public int _nextWave = 0;  //store index of the wave
    private float _searchCountdown = 1f;

    public float _timeBetweenWaves = 5f; //store time between waves
    private float _waveCountdown;

   

    private SpawnState _state = SpawnState.Counting;

    private void Start()
    {
        if(spawnPoint.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }


        _waveCountdown = _timeBetweenWaves;
    }

    private void Update()
    {
        //SpawnEnemy2(Transform _enemy2);


        if (_state == SpawnState.Waiting) //checks if the player has killed all the enemis.
        {
            if(_EnemyIsAlive() == false) //is any enemy still alive
            {
                WaveCompleted();
                //Begin a new round
                
                //Give the player a reward/more points for clearing a round. "round bonus" can be put here. ---------------HIGHSCORE--------------
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

    void WaveCompleted()
    {
        Debug.Log("Wave Comleted!");
        
        _state = SpawnState.Counting;
        _waveCountdown = _timeBetweenWaves;

        if(_nextWave + 1 > waves.Length -1) //if next wave is bigger than number of waves that we have
        {
            _nextWave = waves.Length;      //when all waves are completed, reset to x wave. 
        }
        else
        {
            _nextWave++;
        }
        
        //Debug.Log("ALL WAVES COMPLETE! Looping...");
    }

    bool _EnemyIsAlive()
    {
        _searchCountdown -= Time.deltaTime;
        if (_searchCountdown <= 0f) //for performance, checks every second if there are any enemies alive, instead of every frame.
        {
            _searchCountdown = 1f; //if searchCountdown reaches 0 and there still is enemies alive
            if (GameObject.FindGameObjectWithTag("Enemy") == null) //checks if there are still any enemies alive
            {
                return false; //returns false if no enemies are found alive
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave) //we want to be able to wait x seconds inside the method | need to use systems.collections for IEnumerator
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

        
        Transform _sp = spawnPoint[Random.Range(0, spawnPoint.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy
        
    }


}

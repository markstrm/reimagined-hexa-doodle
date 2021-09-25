using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public Animator animator;
    public GameOverScreen gameOverScreen;
    public WinScreen winScreen;

    int score = 0;
    public int _lives = 3;
    public bool isAlive = true;
    public float _respawnTime = 2.0f;
    public float _respawnInvulnerabilityTime = 3.0f;

    public PlayerMovement player;
    public EnemyTwoFollowPlayer enemy;
    public Transform _enemy; //reference to the prefab that we want to instantiate
    public Transform[] spawnPoint; //array with all the possible spawn locations

    public GameObject _EnemyHolder;
    private WaveSpawner EnemySpawner;
    public LifeCounter lifeCounter;
    void Start()
    {
    GetComponent<Animator>();
     animator.SetBool("Respawn",true);
    }

    private void Awake()
    {
        EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<WaveSpawner>();
        SetUpSingleton();
    }

    public void PlayerDied()
    {
        _lives--;
        if(_lives <= 0) //check lives left
        {
            GameOver();
        }
        else
        {
            isAlive = false;
            Invoke(nameof(Respawn), _respawnTime);
        }
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero; //spawns the player at the center of the board
        player._health = 300;

        player.GetComponent<PolygonCollider2D>().enabled = false;
        //this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); //when player respawns, temporarily change layer to ignore all collisions
        player.gameObject.SetActive(true);

        player.canShoot = true;
        player.canShootL = true;
        animator.SetBool("Respawn", false);

        Invoke(nameof(TurnOnCollisions), _respawnInvulnerabilityTime); //3s after spawning, set the layer back to player to enable collisions
        isAlive = true;
    }

    private void TurnOnCollisions()
    {
        player.GetComponent<PolygonCollider2D>().enabled = true;

        animator.SetBool("Respawn", true);
    }

    private void GameOver()
    {

        Time.timeScale = 0f;
        gameOverScreen.Setup(score);
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
        if(score == 1000 || score == 2500 || score == 5000 || score == 10000)
        {
            SpawnEnemy(_enemy);
        }
        if(score == 15000 || score == 20000 || score == 25000)
        {
            SpawnEnemy(_enemy);
            SpawnEnemy(_enemy);
        }
        if (score == 35000 || score == 40000 || score == 50000)
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnEnemy(_enemy);
            }
        }
        if (score == 75000)
        {
            for (int i = 0; i < 9; i++)
            {
                SpawnEnemy(_enemy);
            }

        }
        if (score == 100000)
        {
            Time.timeScale = 0f;
            winScreen.Setup(score);
        }
    }

    void SpawnEnemy(Transform _enemy) 
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        Transform _sp = spawnPoint[Random.Range(0, spawnPoint.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy
    } 

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    
    public Animator animator;
    public GameOverScreen gameOverScreen;

    int score = 0;
    public int _lives = 3;
    public bool isAlive = true;
    public float _respawnTime = 2.0f;
    public float _respawnInvulnerabilityTime = 3.0f;

    public PlayerMovement player;
    //public EnemyTwoFollowPlayer enemy;
    public Transform _enemy; //reference to the prefab that we want to instantiate
    public Transform[] spawnPoint; //array with all the possible spawn locations

    public GameObject _EnemyHolder;
    private WaveSpawner EnemySpawner;
    public LifeCounter lifeCounter;

    //Animation States


    void Start()
    {
    GetComponent<Animator>();

     animator.SetBool("Respawn",true);

        if (spawnPoint.Length == 0)
        {
            Debug.LogError("No spawn points referenced");
        }

    }



    private void Awake()
    {
        EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<WaveSpawner>();
        SetUpSingleton();
    }

    public void PlayerDied()
    {
        _lives--;
       // EnemySpawner.ResetWave();

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


        // call method to disable collider

        player._health = 300;

        player.GetComponent<PolygonCollider2D>().enabled = false;
        //this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); //when player respawns, temporarily change layer to ignore all collisions
        player.gameObject.SetActive(true);

        player.canShoot = true;
        player.canShootL = true;

        // animation?
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

        gameOverScreen.Setup(score);

        //here we can place gameover text
        //score text etc - press X to play again
        //destroy all other game objects??
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // this._lives = 3;
        // this.score = 0;

        //player._health = 300;
        //EnemySpawner._nextWave = 0;
        //Invoke(nameof(Respawn), this._respawnTime);
        //later
    }



    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            //DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
        if(score == 500 || score == 1500 || score == 2500 || score == 3000 || score == 3800 || score == 4500 || score == 5500 || score == 6000 || score == 6500 || score == 8000)
        {
            SpawnEnemy(_enemy);
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

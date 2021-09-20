using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    int score = 0;
    public int _lives = 3;
    public bool isAlive = true;
    public float _respawnTime = 2.0f;
    public float _respawnInvulnerabilityTime = 3.0f;

    public PlayerMovement player;
    private WaveSpawner EnemySpawner;

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

        player.GetComponent<PolygonCollider2D>().enabled = false;
        //this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); //when player respawns, temporarily change layer to ignore all collisions
        player.gameObject.SetActive(true);
        
        Invoke(nameof(TurnOnCollisions), _respawnInvulnerabilityTime); //3s after spawning, set the layer back to player to enable collisions
        isAlive = true;
    }

    private void TurnOnCollisions()
    {
        player.GetComponent<PolygonCollider2D>().enabled = true;
    }

    private void GameOver()
    {
        //here we can place gameover text
        //score text etc - press X to play again
        //destroy all other game objects??
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        this._lives = 3;
        this.score = 0;

        player._health = 300;
        EnemySpawner._nextWave = 0;
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
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}

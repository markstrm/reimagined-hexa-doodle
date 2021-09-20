using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    int score = 0;
    public int _lives = 3;
    public float _respawnTime = 2.0f;
    public float _respawnInvulnerabilityTime = 3.0f;

    public PlayerMovement player;

    public void PlayerDied()
    {
        this._lives--;

        if(this._lives <= 0) //check lives left
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this._respawnTime);
        }
      
    }

    private void Respawn()
    {

        this.player.transform.position = Vector3.zero; //spawns the player at the center of the board
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions"); //when player respawns, temporarily change layer to ignore all collisions
        this.player.gameObject.SetActive(true);
        
        Invoke(nameof(TurnOnCollisions), this._respawnInvulnerabilityTime); //3s after spawning, set the layer back to player to enable collisions
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver()
    {
        //here we can place gameover text
        //score text etc - press X to play again
        //destroy all other game objects??
        this._lives = 3;
        this.score = 0;

        Invoke(nameof(Respawn), this._respawnTime);
        //later
    }

    private void Awake()
    {
        SetUpSingleton();
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
            DontDestroyOnLoad(gameObject);
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

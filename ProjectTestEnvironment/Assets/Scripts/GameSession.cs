using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{

    int score = 0;
    public int _lives = 3;
    public float respawnTime = 2.0f;

    public GameObject player;

    public void PlayerDied()
    {
        this._lives--;

        if(this._lives <= 0) //check lives left
        {
            GameOver();
        }
        else
        {
            Invoke(nameof(Respawn), this.respawnTime);
        }
      
    }

    private void Respawn()
    {
        this.player.transform.position = Vector3.zero; //spawns the player at the center of the board
        this.player.gameObject.SetActive(true);
    }

    private void GameOver()
    {
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

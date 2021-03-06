using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text PointsText;
  
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        PointsText.text = score.ToString() + " POINTS";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;

    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

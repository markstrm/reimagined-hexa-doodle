using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    public TMP_Text pointsText;


    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";
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

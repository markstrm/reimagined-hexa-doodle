using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{

    TMP_Text ScoreText;
    GameSession GameSession;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<TMP_Text>();
        GameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = GameSession.GetScore().ToString();
    }
}
